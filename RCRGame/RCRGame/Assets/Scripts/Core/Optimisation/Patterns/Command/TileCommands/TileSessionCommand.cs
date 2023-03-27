using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RCRCoreLib.Core.Optimisation.Patterns.Command.TileCommands
{
    public class TileSessionCommand : TileCommand
    {
        private TileBase[] m_oldTiles;
        private BoundsInt m_oldBounds;
        private TileBase[] m_newTiles;
        private BoundsInt m_newBounds;
        

        public TileSessionCommand(BoundsInt bounds,TileBase[] tileBases, BoundsInt oldBounds, TileBase[] oldTiles)
        {
            m_oldTiles = oldTiles;
            m_oldBounds = oldBounds;
            m_newTiles = tileBases;
            m_newBounds = bounds;
        }
        
        public override void Execute(ITileCommandHandler handler)
        {
            handler.PlaceTiles(m_newBounds, m_newTiles);
        }

        public override void Undo(ITileCommandHandler handler)
        {
            handler.PlaceTiles(m_oldBounds, m_oldTiles);
        }
    }
}