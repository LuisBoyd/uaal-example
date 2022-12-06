namespace DataStructures
{
    public class MapSection
    {
        public MapSection(byte[] sectionData, int length)
        {
            this.m_mapSectionData = sectionData;
            this.m_length = length;
        }

        private byte[] m_mapSectionData;
        private int m_length;
        
        public byte this[int index0, int index1]
        {
            get { return m_mapSectionData[index0 * m_length + index1]; }
            set { m_mapSectionData[index0 * m_length + index1] = value; }
        }

    }
}