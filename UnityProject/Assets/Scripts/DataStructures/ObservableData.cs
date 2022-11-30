using RCR.BaseClasses;

namespace RCR.DataStructures
{
    public class ObservableData<T>
    {
        private T m_value;

        public OnObservableValueChanged<T, T> OnValueChanged;

        public T Value
        {
            get => m_value;
            set
            {
                T previous = m_value;
                m_value = value;
                OnValueChanged?.Invoke(previous, m_value);
            }
        }
    }
}