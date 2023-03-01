using System;
using System.Collections.Generic;
using RCRCoreLib.Core.Systems.Tiles;
using RCRCoreLib.MapModification;
using RCRCoreLib.TilePaintingSystem;
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

        // [SerializeField] 
        // private List<TileBase> NoN_EraseableTiles
        //     = new List<TileBase>();

        private HashSet<Vector3Int> ImmutablePositions = new HashSet<Vector3Int>();

        [SerializeField] 
        private Tilemap LogicTilemap;

        public TilePair CurrentTilePair { get; private set; }
        public bool HasCurrentTileSet
        {
            get
            {
                if (CurrentTilePair != null)
                    return true;
                return false;
            }
        }
        
        public bool EarserModeActive { get; private set; }
        public bool CameraModeActive { get; private set; }

        private bool _SystemEnabled;
        public bool SystemEnabled
        {
            get => _SystemEnabled;
            private set
            {
                _SystemEnabled = value;
                var evnt = new PainterActiveStateSwitchEvent(value);
                EventManager.Instance.QueueEvent(evnt);
            }
        }

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
            SystemEnabled = false;
            Collection.Init();
            CompileImmutablePositions();
        }

        private void OnDisable()
        {
            //GridOutlineMatireal.SetKeyword(OutlineEnabledKeyWord, false); //TODO This can't get called at destroy time
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

        // public bool CanEraseTile(Vector3Int pos)
        // {
        //     TileBase CellTile = LogicTilemap.GetTile(pos);
        //     if (NoN_EraseableTiles.Contains(CellTile))
        //         return false;
        //     return true;
        // }

        private void On_NewtileBrushSelected(NewTilePaintBrushSelected evnt)
        {
            //Debug.Log($"New Tile Brush {evnt.option.ToString()}");
            EarserModeActive = false;
            CameraModeActive = false;
            switch (evnt.option)
            {
                case TileSelectionOptions.Camera:
                    SystemEnabled = false;
                    CameraModeActive = true;
                    return;
                    break;
                case TileSelectionOptions.Eraser:
                    EarserModeActive = true;
                    break;
            }
            CurrentTilePair = Collection.LookUp(evnt.option);
            if (CurrentTilePair == null)
            {
                //Debug.Log("CurrentTilePair Was Null");
                SystemEnabled = false;
                return;
            }
            SystemEnabled = true;
        }

        public void OnTilePaint_Btn_Clicked()
        {
            if (!isOpened)
            {
                SystemEnabled = true;
                isOpened = true;
                GridOutlineMatireal.SetKeyword(OutlineEnabledKeyWord, isOpened);
                Debug.Log("Open");
                TilePlacementMenu.gameObject.SetActive(true);
                ConstructionTab.gameObject.SetActive(false);
                //TODO in case object is not enabled at start compensate for this.
            }
            else
            {
                SystemEnabled = false;
                isOpened = false;
                Debug.Log("Close");
                GridOutlineMatireal.SetKeyword(OutlineEnabledKeyWord, isOpened);
                TilePlacementMenu.gameObject.SetActive(false);
                ConstructionTab.gameObject.SetActive(true);
            }
        }
    }
}