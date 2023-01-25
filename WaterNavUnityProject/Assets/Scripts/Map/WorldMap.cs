using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using RCR.BaseClasses;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace RCR.Settings.Map
{
    [RequireComponent(typeof(SpriteMask))]
    [RequireComponent(typeof(PathCreator))]
    [RequireComponent(typeof(Grid))]
    public class WorldMap : Singelton<WorldMap>
    {
        [SerializeField] 
        private WorldSize mWorldSize;

        #region Obscur Properties

        private int GetDeaultTextureSize => mWorldSize switch
        {
            WorldSize.x16 => 16,
            WorldSize.x32 => 32,
            WorldSize.x64 => 64,
            WorldSize.x128 => 128,
            WorldSize.x256 => 256,
            WorldSize.x512 => 512,
            WorldSize.x1024 => 1024,
            WorldSize.x2048 => 2048,
            WorldSize.x4096 => 4096,
            _ => 0
        };

        #endregion

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

        #region BezierPath
        private PathCreator m_PathCreator;
        private VertexPath m_vertexPath => m_PathCreator.path;
        private BezierPath m_bezierPath => m_PathCreator.bezierPath;
        #endregion

        #region Grid Details
        private Grid m_grid;
        private HashSet<Vector3Int> m_VertexOccupiedSpaces; //Vertices positions will be converted to grid space and added to this collection
        private bool[,] IsEdge;
        private PixelCell[,] _pixelCells;
        #endregion

        private List<PixelCell> pCels = new List<PixelCell>();

        protected override void Awake()
        {
            base.Awake();
            m_PathCreator = GetComponent<PathCreator>();
            m_mask = GetComponent<SpriteMask>();
            m_grid = GetComponent<Grid>();
            /*mWorldSize should be Initialized with the value 0 but by using serialized field
             in the unity editor we can set that value before we go into play mode
             */
            InitTilemaps();
            InitMask();
            AlignCurveWithNewTransform();
            InitCurve();
            StartCoroutine(ModifyMask());
        }

        #region Init Methods
        private void InitCurve()
        {
            m_VertexOccupiedSpaces = new HashSet<Vector3Int>();
            for (var i = 0; i < m_vertexPath.localPoints.Length; i++)
            {
                Vector2 localPos = m_vertexPath.localPoints[i];
                Vector3Int GridPos = LocalPosition_GridPosition(localPos.x, localPos.y);
                if (!m_VertexOccupiedSpaces.Contains(GridPos))
                    m_VertexOccupiedSpaces.Add(GridPos);
            }
            
            _pixelCells = new PixelCell[GetDeaultTextureSize, GetDeaultTextureSize];
            for (int x = 0; x < GetDeaultTextureSize; x++)
            {
                for (int y = 0; y < GetDeaultTextureSize; y++)
                {
                    _pixelCells[x, y] = new PixelCell(new Vector2Int(x, y),
                        m_VertexOccupiedSpaces.Contains(new Vector3Int(x, y)));
                }
            }
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

        private void InitTilemaps()
        {
            #region Create exploredTilemap
            GameObject ExploredTilemap = new GameObject("Explored_Tilemap");
            ExploredTilemap.transform.SetParent(this.transform);
            TilemapRenderer ExploredTilemapTilemapRenderer = ExploredTilemap.AddComponent<TilemapRenderer>();
            ExploredTilemap.AddComponent<Tilemap>();
            m_ExploredTilemap = ExploredTilemap.GetComponent<Tilemap>();
            ExploredTilemapTilemapRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            #endregion
            #region Create UnexploredTilemap
            GameObject UnexploredTilemap = new GameObject("Unexplored_Tilemap");
            UnexploredTilemap.transform.SetParent(this.transform);
            TilemapRenderer UnexploredTilemapRenderer = UnexploredTilemap.AddComponent<TilemapRenderer>();
            UnexploredTilemapRenderer.sortingOrder = ExploredTilemapTilemapRenderer.sortingOrder + 1;
            UnexploredTilemap.AddComponent<Tilemap>();
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
        
        private void AlignCurveWithNewTransform()
        {
            Vector3 center = m_ExploredTilemap.localBounds.min;
            center.x += m_ExploredTilemap.localBounds.max.x / 2;
            for (int i = 0; i < m_bezierPath.NumPoints; i++)
            {
                m_bezierPath.SetPoint(i, center + m_bezierPath.GetPoint(i));
            }
        }
        #endregion
        private IEnumerator ModifyMask()
        {
            if (!m_bezierPath.IsClosed)
            {
                Debug.LogWarning($"The path is not closed therefore We can not modify the mask");
               yield return null;
            }

            List<PixelCell> Reached = new List<PixelCell>();
            foreach (PixelCell pixelCell in _pixelCells)
            {
                pixelCell.SetRValue(byte.MinValue);
                if (pixelCell.IsEdge)
                {
                    Reached.Add(pixelCell);
                    pCels.Add(pixelCell);
                }
                
                //If this gets to much offload it to a worker thread
            }
            
            Stack<PixelCell> pixelCells = new Stack<PixelCell>();
            Vector2 center = m_bezierPath.PathBounds.center;
            Vector3Int StartingPoint = LocalPosition_GridPosition(center.x, center.y);
            if (!Reached.Contains(_pixelCells[StartingPoint.x, StartingPoint.y]))
            {
                _pixelCells[StartingPoint.x, StartingPoint.y].SetRValue(Byte.MaxValue);
                Reached.Add(_pixelCells[StartingPoint.x, StartingPoint.y]);
            }
            else
            {
                yield return null;
            }
            pixelCells.Push(_pixelCells[StartingPoint.x, StartingPoint.y]);

            while (pixelCells.Count > 0)
            {
                PixelCell current = pixelCells.Pop();
                foreach (var cell in GetNeighbors(current))
                {
                    if (!Reached.Contains(cell))
                    {
                        Debug.DrawLine(new Vector3(current.Location.x, current.Location.y), new Vector3(cell.Location.x, cell.Location.y), Random.ColorHSV(), 15f);
                        yield return new WaitForSecondsRealtime(0.2f);
                        pixelCells.Push(cell);
                        Reached.Add(cell);
                    }
                    else
                    {
                        yield return new WaitForSecondsRealtime(0.2f);
                        Debug.Log(cell.Location);
                    }
                }
            }
            
            //WriteToMask();
        }

        private void WriteToMask()
        {
            byte[] rawdata = new byte[_pixelCells.GetLength(0) * _pixelCells.GetLength(1)];
            for (int x = 0; x < _pixelCells.GetLength(0); x++)
            {
                for (int y = 0; y < _pixelCells.GetLength(1); y++)
                {
                    rawdata[(x * _pixelCells.GetLength(0)) + y] = Convert.ToByte(_pixelCells[x,y].R);
                }
            }
            
            m_mask_texture.LoadRawTextureData(rawdata);
            m_mask_texture.Apply();
        }

        #region Utility

        private PixelCell[] GetNeighbors(PixelCell cell)
        {
            List<PixelCell> pixelCells = new List<PixelCell>();
            if (cell.Location.x > 0)
            {
                pixelCells.Add(_pixelCells[cell.Location.x - 1, cell.Location.y]); //Get Mid Left
                if(cell.Location.y > 0)
                    pixelCells.Add(_pixelCells[cell.Location.x - 1, cell.Location.y - 1]); //Get Bottom left
                if(cell.Location.y < _pixelCells.GetLength(0) - 1)
                    pixelCells.Add(_pixelCells[cell.Location.x - 1, cell.Location.y + 1]); //Get Top Left
            }

            if (cell.Location.x < _pixelCells.GetLength(0) - 1)
            {
                pixelCells.Add(_pixelCells[cell.Location.x + 1, cell.Location.y]); //Get Mid Right
                if(cell.Location.y > 0)
                    pixelCells.Add(_pixelCells[cell.Location.x + 1, cell.Location.y - 1]); //Get Bottom Right
                if(cell.Location.y < _pixelCells.GetLength(0) - 1)
                    pixelCells.Add(_pixelCells[cell.Location.x + 1, cell.Location.y + 1]); //Get Top Right
            }
            
            if(cell.Location.y > 0)
                pixelCells.Add(_pixelCells[cell.Location.x, cell.Location.y - 1]);
            if(cell.Location.y < _pixelCells.GetLength(0) - 1)
                pixelCells.Add(_pixelCells[cell.Location.x, cell.Location.y + 1]);

            return pixelCells.ToArray();
        }
        
        private Vector2 GridPosition_LocalPosition(int x, int y)
        {
            return m_grid.CellToLocal(new Vector3Int(x, y));
        }
        private Vector2 GridPosition_WorldPosition(int x, int y)
        {
            return m_grid.CellToWorld(new Vector3Int(x, y));
        }
        private Vector3Int WorldPosition_GridPosition(float x, float y)
        {
            return m_grid.WorldToCell(new Vector3(x, y));
        }
        private Vector3Int LocalPosition_GridPosition(float x, float y)
        {
            return m_grid.LocalToCell(new Vector3(x, y));
        }
        
        private bool LineIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            Vector2 cmp = new Vector2(c.x - a.x, c.y - a.y);
            Vector2 r = new Vector2(b.x - a.x, b.y - a.y); //  r is the end point of
            Vector2 s = new Vector2(d.x - c.x, d.y - c.y); // s end point

            float cmPxr = cmp.x * r.y - cmp.y * r.x;
            float cmPxs = cmp.x * s.y - cmp.y * s.x;
            float rxs = r.x * s.y - r.y * s.x;

            if (cmPxr == 0f)
            {
                //Lines are collinear and so intersect if the overlap
                return ((c.x - a.x < 0f) != (c.x - b.x < 0f)) ||
                       ((c.y - a.y < 0f) != (c.y - b.y < 0f));
            }

            if (rxs == 0f)
                return false;  //Lines are parallel and do not intersect

            float rxsr = 1f / rxs;
            float t = cmPxs * rxsr;
            float u = cmPxr * rxsr;

            return (t >= 0f) && (t <= 1f) && (u >= 0f) && (u <= 1f);
        }
        #endregion

        private void OnDrawGizmos()
        {
            foreach (PixelCell cel in pCels)
            {
                Gizmos.color = Random.ColorHSV();
                Gizmos.DrawSphere(new Vector3(cel.Location.x, cel.Location.y), 0.05f);
            }
        }
    }

    /*
     * TODO currently Un-optimized tile count such as x4096 is taking way to much time to load
     * I will look into segmenting theis into chunks and then only setting the chunks
     */
}