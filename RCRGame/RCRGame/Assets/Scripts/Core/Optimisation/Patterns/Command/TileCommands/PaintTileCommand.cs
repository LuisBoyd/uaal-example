using RCRCoreLib.Core.Tiles;
using RCRCoreLib.Core.Tiles.TilemapSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.Optimisation.Patterns.Command.TileCommands
{
    public class PaintTileCommand : TileCommand
    {
        private Vector3Int m_cellPosition;
        private WorldTile m_tileBase;
        private WorldTile m_previousTileBase;
        
        public PaintTileCommand(Vector3Int cellPos, WorldTile tileBase)
        {
            m_cellPosition = cellPos;
            m_tileBase = tileBase;
        }
        public override void Execute(ITileCommandHandler handler)
        {
            m_previousTileBase = handler.CheckTile(m_cellPosition);
            if (m_previousTileBase != null)
            {
                if(m_previousTileBase.lockFlag == WorldTileLockFlag.Immutable)
                    return;
            }
            handler.PlaceTile(m_cellPosition, m_tileBase);
            handler.Record(this);
        }

        public override void Undo(ITileCommandHandler handler)
        {
            if (m_previousTileBase != null)
            {
                handler.PlaceTile(m_cellPosition, m_previousTileBase);
            }
        }
    }
}