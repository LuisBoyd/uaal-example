using System.Collections.Generic;
using UnityEngine;

namespace RCRCoreLib.TilePaintingSystem
{
    public class PainterClosedEvent : GameEvent
    {
        public IEnumerable<Vector3Int> positions;

        public PainterClosedEvent(IEnumerable<Vector3Int> pos)
        {
            positions = pos;
        }
    }
}