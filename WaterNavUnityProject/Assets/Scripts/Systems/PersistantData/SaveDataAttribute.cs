using System;

namespace Systems.PersistantData
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public class SaveDataAttribute : Attribute
    {
        private PersistantDataType _dataType;
        
        public SaveDataAttribute(PersistantDataType dataType)
        {
            this._dataType = dataType;
        }
    }
}