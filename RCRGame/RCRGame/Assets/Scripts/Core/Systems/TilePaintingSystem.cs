using System;
using System.Collections.Generic;
using RCRCoreLib.Core.Systems.Tiles;
using RCRCoreLib.MapModification;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.Systems
{
    public class TilePaintingSystem : Singelton<TilePaintingSystem>
    {
        private bool isOpened;
        private const string KEYWORD_SHADER = "_OUTLINE_ENABLED";

        [SerializeField] 
        private TilemapRenderer BackGroundTilemapRenderer;
        private LocalKeyword OutlineEnabledKeyWord;

        [SerializeField] 
        private RectTransform TilePlacementMenu;
        [SerializeField] 
        private RectTransform ConstructionTab;
        

        [SerializeField] 
        private TilePairCollection Collection;

        [SerializeField] private List<TileBase> ImmutableTiles
            = new List<TileBase>();

        private HashSet<Vector3Int> ImmutablePositions = new HashSet<Vector3Int>();

        [SerializeField] 
        private Tilemap LogicTilemap;

        public TilePair CurrentTilePair { get; private set; }

        public bool DisablePaintBrush { get; private set; }

        private Material GridOutlineMatireal
        {
            get
            {
                //Returns back the Copy Material so if any other objects use the same
                //Matireal any Changes I make here will not happen to all gameObjects with
                //This Matireal only the gameObject this renderer is attached to.
                return BackGroundTilemapRenderer.sharedMaterial; 
            }
        }

        protected override void Awake()
        {
            base.Awake();
            OutlineEnabledKeyWord = new LocalKeyword(GridOutlineMatireal.shader, KEYWORD_SHADER);
        }

        private void Start()
        {
            EventManager.Instance.AddListener<NewTilePaintBrushSelected>(On_NewtileBrushSelected);
            DisablePaintBrush = true;
            Collection.Init();
            CompileImmutablePositions();
        }

        private void OnDisable()
        {
            GridOutlineMatireal.SetKeyword(OutlineEnabledKeyWord, false);
            EventManager.Instance.RemoveListener<NewTilePaintBrushSelected>(On_NewtileBrushSelected);
        }

        private void CompileImmutablePositions()
        {
            foreach (Vector3Int vector3Int in LogicTilemap.cellBounds.allPositionsWithin)
            {
                TileBase cell = LogicTilemap.GetTile(vector3Int);
                if(cell == null)
                    continue;
                if(!ImmutableTiles.Contains(cell))
                    continue;
                ImmutablePositions.Add(vector3Int);
            }
        }

        public bool IsImmutablePosition(Vector3Int pos) => ImmutablePositions.Contains(pos);

        private void On_NewtileBrushSelected(NewTilePaintBrushSelected evnt)
        {
            Debug.Log($"New Tile Brush {evnt.option.ToString()}");
            DisablePaintBrush = false;
            CurrentTilePair = Collection.LookUp(evnt.option);
            if (CurrentTilePair == null)
            {
                Debug.Log("CurrentTilePair Was Null");
                DisablePaintBrush = true;
                return;
            }
        }

        public void OnTilePaint_Btn_Clicked()
        {
            if (!isOpened)
            {
                DisablePaintBrush = false;
                isOpened = true;
                GridOutlineMatireal.SetKeyword(OutlineEnabledKeyWord, isOpened);
                Debug.Log("Open");
                TilePlacementMenu.gameObject.SetActive(true);
                ConstructionTab.gameObject.SetActive(false);
                //TODO in case object is not enabled at start compensate for this.
            }
            else
            {
                DisablePaintBrush = true;
                isOpened = false;
                Debug.Log("Close");
                GridOutlineMatireal.SetKeyword(OutlineEnabledKeyWord, isOpened);
                TilePlacementMenu.gameObject.SetActive(false);
                ConstructionTab.gameObject.SetActive(true);
            }
        }
    }
}