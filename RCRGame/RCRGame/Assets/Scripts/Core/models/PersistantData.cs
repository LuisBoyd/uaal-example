using System;

namespace Core.models
{
    public class PersistantData<T>
    {
        public bool MostRecentSave { get; set; }
        public DateTime LastSavedTime { get; set; }
        public T Data { get; set; }
        
        public PersistantData(T data)
        {
            MostRecentSave = false;
            this.Data = data;
            LastSavedTime = DateTime.Now;
        }
        
    }
}