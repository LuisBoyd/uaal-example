using System;

namespace RCR.DataStructures
{
    [System.Serializable]
    public class Area
    {
        public int[,] m_intTiles = new int[50, 50];

        public Area(byte[] bytes)
        {
            if (bytes.Length == (50 * 50))
            {
                // if(BitConverter.IsLittleEndian)
                //     Array.Reverse(bytes);

                int count = 0;
                for (int x = 0; x < 50; x++)
                {
                    for (int y = 0; y < 50; y++)
                    {
                        m_intTiles[x, y] = BitConverter.ToInt32(bytes, count);
                        count += 4;
                    }
                }
            }
        }
        
    }
}