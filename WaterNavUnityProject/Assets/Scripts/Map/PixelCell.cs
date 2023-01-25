using UnityEngine;

namespace RCR.Settings.Map
{
    public struct PixelCell
    {
        public Vector2Int Location;

        public int R
        {
            get => Nibble & 0x0F;
            set => Nibble = value & 0x0F;
        }
        private int Nibble;
        
        public bool IsEdge { get; private set; }

        public PixelCell(Vector2Int location, bool edge)
        {
            Location = location;
            IsEdge = edge;
            Nibble = 0;
        }

        public void SetRValue(byte v) => R = v;

    }
}