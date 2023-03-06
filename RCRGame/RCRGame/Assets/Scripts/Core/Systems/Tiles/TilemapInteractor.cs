﻿using System;
using System.Collections.Generic;
using RCRCoreLib.Core.Optimisation.Patterns.Command;
using RCRCoreLib.TilePaintingSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.Systems.Tiles
{
    [RequireComponent(typeof(Tilemap))]
    public class TilemapInteractor : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler, ITileCommandHandler
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
            EventManager.Instance.AddListener<PainterClosedEvent>(On_Cancelled_Paint);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<PainterActiveStateSwitchEvent>(On_PainterSwitchActiveState);
            EventManager.Instance.RemoveListener<PainterClosedEvent>(On_Cancelled_Paint);
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
            TilePaintingSystem.Instance.RecordTile(CellPos);
            VisualTileMap.SetTile(CellPos, TilePaintingSystem.Instance.CurrentTilePair.VisualTile);
            LogicTileMap.SetTile(CellPos, TilePaintingSystem.Instance.CurrentTilePair.LogicTile);
        }

        private void On_Cancelled_Paint(PainterClosedEvent evnt)
        {
            foreach (Vector3Int pos in evnt.positions)
            {
                VisualTileMap.SetTile(pos, null);
                LogicTileMap.SetTile(pos,null);
            }
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

        #region CommandHandler
        
        private LinkedList<TileCommand> m_commandBuffer = new LinkedList<TileCommand>();
        public LinkedList<TileCommand> CommandBuffer
        {
            get => m_commandBuffer;
        }

        private LinkedListNode<TileCommand> m_head;
        public LinkedListNode<TileCommand> Head
        {
            get => m_head;
            set => m_head = value;
        }

        public void Record(TileCommand command)
        {
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
            VisualTileMap.SetTile(cellPos, tile);
            LogicTileMap.SetTile(cellPos, tile);
        }

        public void RemoveTile(Vector3Int cellPos)
        {
            VisualTileMap.SetTile(cellPos, null);
            LogicTileMap.SetTile(cellPos, null);
        }

        public TileBase CheckTile(Vector3Int cellPos)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}