using System;
using System.Collections.Generic;
using System.Linq;
using RCRCoreLib.Core.AI;
using RCRCoreLib.Core.Events;
using RCRCoreLib.Core.Events.MapModification;
using RCRCoreLib.Core.Events.System;
using RCRCoreLib.Core.Events.TilePaintingSystem;
using RCRCoreLib.Core.Optimisation.Patterns.Command;
using RCRCoreLib.Core.Optimisation.Patterns.Command.TileCommands;
using RCRCoreLib.Core.Systems.Tiles;
using RCRCoreLib.Core.Tiles;
using RCRCoreLib.Core.Tiles.TilemapSystem;
using RCRCoreLib.Core.Utilities.SerializableDictionary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.Systems
{
    [RequireComponent(typeof(Tilemap), typeof(TilemapCollider2D), typeof(TilemapRenderer))]
    public class TileSystem : Singelton<TileSystem>, ITileCommandHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, ISystem
    {
        [SerializeField] 
        private WorldTileDictionary m_worldTileDictionary = null; //Serialized Dictionary for runtime-Lookup
        public IDictionary<TileType, WorldTile> WorldTileDictionary
        {
            get { return m_worldTileDictionary; }
            set{m_worldTileDictionary.CopyFrom(value);}
        }

        private TilemapRenderer TilemapRenderer;
        private TilemapCollider2D TilemapCollider2D;
        private Tilemap Tilemap;
        private LocalKeyword OutlineEnabledKeyWord;
        private const string KEYWORD_SHADER = "_OUTLINE_ENABLED";
        
        private bool m_systemActive = false;//Turn On The System Or Off.
        
        public WorldTile Currentbrush { get; private set; }

        public bool CurrentbrushSet
        {
            get
            {
                if (Currentbrush != null)
                    return true;
                return false;
            }
        }
        public bool EarserModeActive { get; private set; }
        public bool CameraModeActive { get; private set; }
        public bool TilemapDirty { get; private set; }
        private Material GridOutlineMatireal
        {
            get
            {
                //Returns back the Copy Material so if any other objects use the same
                //Matireal any Changes I make here will not happen to all gameObjects with
                //This Matireal only the gameObject this renderer is attached to.
                return TilemapRenderer.material;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            TilemapRenderer = GetComponent<TilemapRenderer>();
            Tilemap = GetComponent<Tilemap>();
            TilemapCollider2D = GetComponent<TilemapCollider2D>();
            OutlineEnabledKeyWord = new LocalKeyword(GridOutlineMatireal.shader, KEYWORD_SHADER);
            LandWalkableAffectors = new HashSet<Vector2Int>();
            WaterWalkableAffectors = new HashSet<Vector2Int>();
        }

        private void Start()
        {
            GameManager.Instance.RegisterSystem(SystemType.TileEditSystem, this);
            EventManager.Instance.AddListener<NewTilePaintBrushSelected>(On_NewTileBrushSelected);
            EventManager.Instance.AddListener<TileSessionEndEvent>(ConfirmDirtyTiles);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<NewTilePaintBrushSelected>(On_NewTileBrushSelected);
            EventManager.Instance.RemoveListener<TileSessionEndEvent>(ConfirmDirtyTiles);
        }

        // public void On_TileSystem_Btn_Clicked()
        // {
        //   
        // }
        //
        // public void ActivateSystem()
        // {
        //     m_systemActive = true;
        //     SystemEnabled = true;
        //     GridOutlineMatireal.SetKeyword(OutlineEnabledKeyWord, m_systemActive);
        //     //TODO set TileConstruction UI active
        //     //TODO set Normal UI Construction Button's off
        //     //TODO make sure the Current Buffer is clear
        //    
        // }
        //
        // public void DeActivateSystem()
        // {
        //     m_systemActive = false;
        //     SystemEnabled = false;
        //     GridOutlineMatireal.SetKeyword(OutlineEnabledKeyWord, m_systemActive);
        //     //TODO set TileConstruction UI active
        //     //TODO set Normal UI Construction Button's off
        //     //TODO Check The Current Buffer and either save or Undo
        // }

        #region TilePainting

        //private IList<TileCommand> SessionCmds = new List<TileCommand>();
        private bool SessionActive;

        private TileBase[] oldTiles;
        private BoundsInt oldBounds;

        private HashSet<Vector2Int> LandWalkableAffectors;
        private HashSet<Vector2Int> WaterWalkableAffectors;

        private void PaintOnMap(Vector2 worldSpaceLocation)
        {
            if(!CurrentbrushSet)
                return;
            Vector3Int CellPos = Tilemap.WorldToCell(worldSpaceLocation);
            var prevTile = CheckTile(CellPos);
            if (prevTile != null)
            {
                if(prevTile.lockFlag == WorldTileLockFlag.Immutable)
                    return;
            }
            PlaceTile(CellPos, Currentbrush);
            
            //If any of the placed tiles for AI movement...
            
            if ((Currentbrush.pathAffector & WorldTilePathAffector.WaterPathFindable) ==
                WorldTilePathAffector.WaterPathFindable)
            {
                WaterWalkableAffectors.Add(new Vector2Int(CellPos.x, CellPos.y));
            }
            
            if ((Currentbrush.pathAffector & WorldTilePathAffector.LandPathFindable) ==
                     WorldTilePathAffector.LandPathFindable)
            {
                LandWalkableAffectors.Add(new Vector2Int(CellPos.x, CellPos.y));
            }
        }

        private void BeginPaintSession()
        {
            //SessionActive = true;
            oldTiles = Tilemap.GetTilesBlock(Tilemap.cellBounds);
            oldBounds = Tilemap.cellBounds;
        }

        private void EndPaintSession()
        {
            //SessionActive = false;
            Record(new TileSessionCommand(Tilemap.cellBounds, Tilemap.GetTilesBlock(Tilemap.cellBounds), oldBounds, oldTiles));
            //SessionCmds.Clear();
        }

        public void ConfirmDirtyTiles(TileSessionEndEvent e)
        {
            TilemapDirty = false;
            if (!e.save)
            {
                Head = CommandBuffer.Last;
                while (Head != null)
                {
                    Head.Value.Undo(this); //Head Never == null for some reason
                    Head = Head.Previous;
                }
            }
            else
            {
                PathFindingSystem.Instance.SetLandTilePositions(LandWalkableAffectors);
                PathFindingSystem.Instance.SetWaterTilePositions(WaterWalkableAffectors);
            }
            CommandBuffer.Clear();
            //TODO set the ConfirmMenu off here or something.
        }

        public WorldTile ParseOptions(TileSelectionOptions option)
        {
            WorldTile instancedTile = null;
            switch (option)
            {
                case TileSelectionOptions.Path:
                    instancedTile = Instantiate(WorldTileDictionary[TileType.Path]);
                    break;
                case TileSelectionOptions.PathStone:
                    instancedTile = Instantiate(WorldTileDictionary[TileType.PathStone]);
                    break;
                case TileSelectionOptions.PathGrassy:
                    instancedTile = Instantiate(WorldTileDictionary[TileType.PathGrassy]);
                    break;
                case TileSelectionOptions.PathCobbleStone:
                    instancedTile = Instantiate(WorldTileDictionary[TileType.PathCobbleStone]);
                    break;
                case TileSelectionOptions.Water:
                    instancedTile = Instantiate(WorldTileDictionary[TileType.Water]);
                    break;
            }
            instancedTile.lockFlag = WorldTileLockFlag.Mutable;
            return instancedTile;
        }
        
        #endregion

        #region TileCommandHandler

        public int commandBufferSize = 7; //Size of the command buffer when a new one is added we compare the size and pop the first/ oldest cmd

        public int CommandBufferSize
        {
            get => commandBufferSize;
        }

        private LinkedList<TileCommand> m_commandBuffer = new LinkedList<TileCommand>();
        public LinkedList<TileCommand> CommandBuffer
        {
            get => m_commandBuffer;
        }

        private LinkedListNode<TileCommand> m_head;

        public LinkedListNode<TileCommand> Head
        {
            get
            {
                return m_head;
            }
            set
            {
                m_head = value;
            }
        }
        public void Record(TileCommand command)
        {
            // if (SessionActive) //If there is a current Active Tile Painting Session Route all cmds to the "session buffer"
            // {
            //     SessionCmds.Add(command);
            //     return;
            // }
            CommandBuffer.AddLast(command);
        }

        public void Undo(int steps = 1)
        {
            for (int i = 0; i < steps; i++)
            {
                Head.Value.Undo(this);
                Head = Head.Previous;
            }
        }

        public void Redo(int steps = 1)
        {
            for (int i = 0; i < steps; i++)
            {
                Head = Head.Next;
                Head.Value.Execute(this);
            }
        }

        public void PlaceTile(Vector3Int cellPos, TileBase tile)
        {
            Tilemap.SetTile(cellPos, tile);
        }

        public void PlaceTiles(BoundsInt bounds ,TileBase[] tilesBases)
        {
            Tilemap.SetTilesBlock(bounds, tilesBases);
        }

        public void RemoveTile(Vector3Int cellPos)
        {
            Tilemap.SetTile(cellPos, null);
        }
        public WorldTile CheckTile(Vector3Int cellPos)
        {
           return Tilemap.GetTile<WorldTile>(cellPos);
        }
        #endregion
        #region Dragging
        public void OnBeginDrag(PointerEventData eventData)
        {
           if(!m_systemActive)
               return;
           BeginPaintSession();
           Vector2 worldSpace = eventData.enterEventCamera.ScreenToWorldPoint(eventData.position);
           PaintOnMap(worldSpace);
           //Any Errors to do with out of screen bounds has to do with the cursor on mobile platform.
           
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(!m_systemActive)
                return;
            Vector2 worldSpace = eventData.enterEventCamera.ScreenToWorldPoint(eventData.position);
            PaintOnMap(worldSpace);
            //Any Errors to do with out of screen bounds has to do with the cursor on mobile platform.
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(!m_systemActive)
                return;
            EndPaintSession();
            //TODO record a collection event??
        }
        #endregion

        #region eventHandlers
        private void On_NewTileBrushSelected(NewTilePaintBrushSelected evnt)
        {
            Currentbrush = null;
            CameraModeActive = false;
            EarserModeActive = false;
            switch (evnt.option)
            {
                case TileSelectionOptions.Camera:
                    CameraModeActive = true;
                    break;
                case TileSelectionOptions.Eraser:
                    EarserModeActive = true;
                    break;
                default:
                    Currentbrush = ParseOptions(evnt.option);
                    break;
            }

            if (EarserModeActive)
            {
                Currentbrush = WorldTileDictionary[TileType.EraserTile];
            }
            
            if (CameraModeActive)
            {
                m_systemActive = false;
                TilemapCollider2D.enabled = false;
                EventManager.Instance.QueueEvent(new SystemActivateEvent(true, SystemType.CameraSystem));
                return;
            }
            else
            {
                m_systemActive = true;
                TilemapCollider2D.enabled = true;
                EventManager.Instance.QueueEvent(new SystemActivateEvent(false, SystemType.CameraSystem));
            }

            if (Currentbrush == null)
            {
                Debug.LogError($"Error Occurred TileSystem");
                m_systemActive = false;
                return;
            }
           
        }
        
        private void On_Cancelled_Paint(PainterClosedEvent evnt)
        {
            
        }
        #endregion

        public void EnableSystem()
        {
            m_systemActive = true;
            GridOutlineMatireal.SetKeyword(OutlineEnabledKeyWord, m_systemActive);
            TilemapCollider2D.enabled = true;
            //TODO set TileConstruction UI active
            //TODO set Normal UI Construction Button's off
            //TODO make sure the Current Buffer is clear
            LandWalkableAffectors.Clear();
            WaterWalkableAffectors.Clear();
            //When Enable this system Disable Camera Movement By Default
            EventManager.Instance.QueueEvent(new SystemActivateEvent(false, SystemType.CameraSystem));
        }

        public void DisableSystem()
        {
            m_systemActive = false;
            GridOutlineMatireal.SetKeyword(OutlineEnabledKeyWord, m_systemActive);
            TilemapCollider2D.enabled = false;
            //TODO set TileConstruction UI active
            //TODO set Normal UI Construction Button's off
            //TODO Check The Current Buffer and either save or Undo
            LandWalkableAffectors.Clear();
            WaterWalkableAffectors.Clear();
            //When Disable this system Enable Camera Movement By Default
            EventManager.Instance.QueueEvent(new SystemActivateEvent(true, SystemType.CameraSystem));
        }
    }
}