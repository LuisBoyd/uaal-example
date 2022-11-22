namespace DataStructures
{
    /// <summary>
    /// A Wrapper class to store the bytes for the map
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MapArray<T>
    {
        private T[] m_data;
        private int MaxmapSectionCount;
        private int mapRowCount;
        private int maxRowEntries;

        public MapArray(T[] input, int maxMapSectionCount, int mapRowCount, int maxRowEntries)
        {
            m_data = input;
            this.MaxmapSectionCount = maxMapSectionCount;
            this.mapRowCount = mapRowCount;
            this.maxRowEntries = maxRowEntries;
        }

        public T this[int index0, int index1, int index2]
        {
            get{return CheckNotOutOfrangeException(index0, index1, index2) ? m_data[ (index0 * MaxmapSectionCount) + (index1 * mapRowCount) + index2] : default;}
            set
            {
                if (CheckNotOutOfrangeException(index0, index1, index2))
                    m_data[(index0 * MaxmapSectionCount) + (index1 * mapRowCount) + index2] = value;
                else
                {
                    m_data[(index0 * MaxmapSectionCount) + (index1 * mapRowCount) + index2] = default;
                }
            }
        }

        private bool CheckNotOutOfrangeException(int index0, int index1, int index2)
        {
            if (index0 < MaxmapSectionCount && index1 < mapRowCount && index2 < maxRowEntries)
                return true;
            else
                return false;
        }
        
        
    }
}