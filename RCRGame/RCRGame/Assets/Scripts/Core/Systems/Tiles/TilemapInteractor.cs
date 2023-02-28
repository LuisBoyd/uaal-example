using System;
using RCRCoreLib.TilePaintingSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.Systems.Tiles
{
    [RequireComponent(typeof(Tilemap))]
    public class TilemapInteractor : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
    {

        [SerializeField]
        private Tilemap LogicTileMap;
        [SerializeField]
        private Tilemap VisualTileMap;

        private TilemapCollider2D interactionInterface;

        private bool EnableInteraction = false;

        private void Awake()
        {
            interactionInterface = GetComponent<TilemapCollider2D>();
        }

        private void Start()
        {
            EventManager.Instance.AddListener<PainterActiveStateSwitchEvent>(On_PainterSwitchActiveState);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<PainterActiveStateSwitchEvent>(On_PainterSwitchActiveState);
        }

        private void On_PainterSwitchActiveState(PainterActiveStateSwitchEvent evnt)
        {
            EnableInteraction = evnt.State;
            interactionInterface.enabled = evnt.State;
        }

        private void PaintOnMap(Vector2 WorldSpaceLocation)
        {
            //Convert WorldSpacePos To tilemap Pos
            if(!TilePaintingSystem.Instance.HasCurrentTileSet)
                return;
            Vector3Int CellPos = VisualTileMap.WorldToCell(WorldSpaceLocation);

            // if (TilePaintingSystem.Instance.EarserModeActive)
            // {
            //     if (!TilePaintingSystem.Instance.CanEraseTile(CellPos))
            //     {
            //         return;
            //     }
            // }
            
            if (TilePaintingSystem.Instance.IsImmutablePosition(CellPos))
            {
                //Debug.Log($"{CellPos} is Immutable position");
                return;
            }
            //Debug.Log($"Cell Space of Pointer {CellPos}");
            VisualTileMap.SetTile(CellPos, TilePaintingSystem.Instance.CurrentTilePair.VisualTile);
            LogicTileMap.SetTile(CellPos, TilePaintingSystem.Instance.CurrentTilePair.VisualTile);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            //TODO Send Event off to confirm Selection??? 
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(!EnableInteraction)
                return;
            try
            {
                Vector2 WorldSpace = eventData.enterEventCamera.ScreenToWorldPoint(eventData.position);
                //Debug.Log($"World Space of Pointer {WorldSpace}");
                PaintOnMap(WorldSpace);
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(!EnableInteraction)
                return;
            try
            {
                Vector2 WorldSpace = eventData.enterEventCamera.ScreenToWorldPoint(eventData.position);
                PaintOnMap(WorldSpace);
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(!EnableInteraction)
                return;
        }
    }
}