namespace DataStructures
{
    public class Array2d<T>
    {
        private T[] m_input;
        private int m_length0;

        public Array2d(T[] input, int length0)
        {
            this.m_input = input;
            this.m_length0 = length0;
        }

        public T this[int index0, int index1]
        {
            get { return m_input[index0 * this.m_length0 + index1]; }
            set { m_input[index0 * this.m_length0 + index1] = value; }
        }

        public T[] FlatArray
        {
            get => m_input;
        }
    }
}