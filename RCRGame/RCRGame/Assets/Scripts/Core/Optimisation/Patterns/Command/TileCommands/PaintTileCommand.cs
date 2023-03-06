using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.Optimisation.Patterns.Command.TileCommands
{
    public class PaintTileCommand : TileCommand
    {
        private Vector3Int m_cellPosition;
        private TileBase m_tileBase;
        private TileBase m_previousTileBase;
        
        public PaintTileCommand(Vector3Int cellPos, TileBase tileBase)
        {
            m_cellPosition = cellPos;
            m_tileBase = tileBase;
        }
        public override void Execute(ITileCommandHandler handler)
        {
            m_previousTileBase = handler.CheckTile(m_cellPosition);
            handler.PlaceTile(m_cellPosition, m_tileBase);
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