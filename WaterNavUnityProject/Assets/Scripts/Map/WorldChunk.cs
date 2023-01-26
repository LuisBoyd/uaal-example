using System;
using RCR.Settings.Optimization;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace RCR.Settings.Map
{
    /// <summary>
    /// World chunk that can pop in and out of visual view but logically will be there
    /// </summary>
    [RequireComponent(typeof(WorldChunkCulling))]
    [RequireComponent(typeof(SpriteMask))]
    public class WorldChunk : MonoBehaviour
    {
        //Maybe hold an addressable reference for the gameobject instead?
        
        #region Tilevalues
        [SerializeField]
        private TileBase[] m_UnexploredTiles;
        [SerializeField] 
        private TileBase[] m_ExploredTiles;
        //TODO Water Tiles
        #endregion
        #region Tilemap Values
        private Tilemap m_UnexploredTilemap;
        private Tilemap m_ExploredTilemap;
        #endregion
        #region Maskingvalues
        private SpriteMask m_mask;
        private Sprite m_maskSprite;
        private Texture2D m_mask_texture;
        #endregion

        private Vector2Int _WorldPosition;
        public Vector2Int WorldPosition
        {
            get => _WorldPosition;
            set
            {
                transform.localPosition = new Vector3(value.x, value.y);
                _WorldPosition = value;
            }
        }

        private int GetDeaultTextureSize => WorldMap_Chunks.Instance.GetDeaultTextureSize;

        private void Awake()
        {
            m_mask = GetComponent<SpriteMask>();
            InitTilemaps();
        }

        #region InitMethods
        private void InitTilemaps()
        {
            #region Create exploredTilemap
            GameObject ExploredTilemap = new GameObject("Explored_Tilemap");
            ExploredTilemap.transform.SetParent(this.transform);
            TilemapRenderer ExploredTilemapTilemapRenderer = ExploredTilemap.AddComponent<TilemapRenderer>();
            ExploredTilemap.AddComponent<Tilemap>();
            TilemapCollider2D ExploredTilemapCollider2D = ExploredTilemap.AddComponent<TilemapCollider2D>();
            m_ExploredTilemap = ExploredTilemap.GetComponent<Tilemap>();
            ExploredTilemapTilemapRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            #endregion
            #region Create UnexploredTilemap
            GameObject UnexploredTilemap = new GameObject("Unexplored_Tilemap");
            UnexploredTilemap.transform.SetParent(this.transform);
            TilemapRenderer UnexploredTilemapRenderer = UnexploredTilemap.AddComponent<TilemapRenderer>();
            UnexploredTilemapRenderer.sortingOrder = ExploredTilemapTilemapRenderer.sortingOrder + 1;
            UnexploredTilemap.AddComponent<Tilemap>();
            TilemapCollider2D UnexploredTilemapCollider2D = UnexploredTilemap.AddComponent<TilemapCollider2D>();
            m_UnexploredTilemap = UnexploredTilemap.GetComponent<Tilemap>();
            UnexploredTilemapRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            #endregion

            #region Init UnexploredTilemap
            m_UnexploredTilemap.size = new Vector3Int(GetDeaultTextureSize, GetDeaultTextureSize, 1);
            m_UnexploredTilemap.origin = Vector3Int.zero;
            m_UnexploredTilemap.ResizeBounds();

            TileBase[] U_fillArray = new RuleTile[GetDeaultTextureSize * GetDeaultTextureSize];
            for (var i = 0; i < U_fillArray.Length; i++)
                U_fillArray[i] = m_UnexploredTiles[Random.Range(0, m_UnexploredTiles.Length)];
            m_UnexploredTilemap.SetTilesBlock(m_UnexploredTilemap.cellBounds, U_fillArray);
            #endregion

            #region Init ExploredTilemap
            m_ExploredTilemap.size = new Vector3Int(GetDeaultTextureSize, GetDeaultTextureSize, 1);
            m_ExploredTilemap.origin = Vector3Int.zero;
            m_ExploredTilemap.ResizeBounds();

            TileBase[] E_fillArray = new RuleTile[GetDeaultTextureSize * GetDeaultTextureSize];
            for (var i = 0; i < E_fillArray.Length; i++)
                E_fillArray[i] = m_ExploredTiles[Random.Range(0, m_ExploredTiles.Length)];
            m_ExploredTilemap.SetTilesBlock(m_ExploredTilemap.cellBounds, E_fillArray);
            #endregion
        }
        private void InitMask()
        {
            #region Create Appropriate Texture and sprite
            /*Makes Texture is created with each pixel represented by 4 bits a reduced red channel
             Only 1 channel is needed for just determining on or off*/
            m_mask_texture = new Texture2D(GetDeaultTextureSize, GetDeaultTextureSize
                , TextureFormat.BC4, false);
            
            /*First getting the texture Are which is W * L in this case both are the same since the texture is constricted to power of 2,
             secondly using unity Sprite Utility create a sprite with the defining rect of the sprite covering it's area and the pivot
             being in the center of the sprite 
             */
            Vector2 Texture_area = new Vector2(GetDeaultTextureSize, GetDeaultTextureSize);
            m_maskSprite = Sprite.Create(m_mask_texture, new Rect(Vector2.zero,
                Texture_area),Texture_area/2);
            #endregion
            
            //Assign the Made sprite to the mask
            m_mask.sprite = m_maskSprite;
        }
        #endregion
    }
}