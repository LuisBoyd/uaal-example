using System;
using Events.Library.Models.WorldEvents;
using NewManagers;
using RCR.Settings.NewScripts.Tilesets;
using UnityEngine;

namespace RCR.Settings.NewScripts.AI
{
    public class AILayer
    {
        public class AITile
        {
            public LogicDecorations Functinailty;
            public Vector2Int location;

            public AITile(Vector2Int point, LogicDecorations functinailty)
            {
                location = point;
                Functinailty = functinailty;
            }
        }
        
        private AITile[,] AiTiles; //TileType is an enum that represents int32 values
        private int Width;
        private int Height;
        public AILayer(int width, int height)
        {
            AiTiles = new AITile[width, height];
            Width = width;
            Height = height;
        }
        
        private void ChangeAllTiles(int value)
        {
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                AiTiles[x, y] = default;
        }

        public void ChangeAITiles(PureLogicTile[] tiles)
        {
            foreach (PureLogicTile tile in tiles)
            {
                AiTiles[tile.location.x % Width, tile.location.y % Height]
                    = new AITile(new Vector2Int(tile.location.x, tile.location.y),
                        tile.LogicDecorations);
            }
            AILayerChanged();
        }
        public bool ValidateAreaInAiLayer(BoundsInt bounds)
        {
            bounds.position = Vector3Int.zero;
            return (bounds.position.x < Width) &&
                   (bounds.position.y <Height) &&
                   (bounds.position.y >= 0) && (bounds.position.x >= 0);
        }
        public bool ValidatePointInAiLayer(Vector2Int Position)
        {
            return Position.x < Width && Position.y < Height 
                                      && Position.x >= 0 && Position.y >= 0;
        }

        private void AILayerChanged()
        {
            foreach (AITile LogicTileValue in AiTiles)
            {
                if(LogicTileValue == null)
                    continue;
                if(LogicTileValue.Functinailty.HasFlag(LogicDecorations.Debugger))
                    Debug.Log("");
                if(LogicTileValue.Functinailty.HasFlag(LogicDecorations.Path))
                    Debug.Log("");
                if(LogicTileValue.Functinailty.HasFlag(LogicDecorations.BoatSpawner) ||LogicTileValue.Functinailty.HasFlag(LogicDecorations.CustomerSpawner))
                   GameManager_2_0.Instance.EventBus.Publish(new OnSpawnerPlaced(
                       new OnLogicChangedArgs(LogicTileValue.location, LogicTileValue.Functinailty)),EventArgs.Empty);
            }
        }
        
    }
}