using System;

namespace RCRCoreLibrary
{
    /// <summary>
    /// Custom Property is a way to apply an effect to an object implementation of these properties can be left 
    /// </summary>
    public  class CustomProperty
    {
        public string Name { get; internal set; }
        public string Type { get; internal set; }
        public string value { get; internal set; }
        private Type _type;

        protected CustomProperty(string name, string type, string value)
        {
            this.Name = name;
            this.Type = type;
            this.value = value;

            _type = System.Type.GetType(type);
        }

        public Type GetPropertyType() => _type;
    }
}