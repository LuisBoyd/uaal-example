using System;
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
        
        private void PaintOnMap(Vector2 WorldSpaceLocation)
        {
            //Convert WorldSpacePos To tilemap Pos
            Vector3Int CellPos = VisualTileMap.WorldToCell(WorldSpaceLocation);
            if (TilePaintingSystem.Instance.IsImmutablePosition(CellPos))
            {
                Debug.Log($"{CellPos} is Immutable position");
                return;
            }
            Debug.Log($"Cell Space of Pointer {CellPos}");
            VisualTileMap.SetTile(CellPos, TilePaintingSystem.Instance.CurrentTilePair.VisualTile);
            LogicTileMap.SetTile(CellPos, TilePaintingSystem.Instance.CurrentTilePair.VisualTile);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            //TODO Send Event off to confirm Selection??? 
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(TilePaintingSystem.Instance.DisablePaintBrush)
                return;
            Vector2 WorldSpace = eventData.enterEventCamera.ScreenToWorldPoint(eventData.position);
            Debug.Log($"World Space of Pointer {WorldSpace}");
            PaintOnMap(WorldSpace);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(TilePaintingSystem.Instance.DisablePaintBrush)
                return;
            Vector2 WorldSpace = eventData.enterEventCamera.ScreenToWorldPoint(eventData.position);
            PaintOnMap(WorldSpace);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(TilePaintingSystem.Instance.DisablePaintBrush)
                return;
        }
    }
}