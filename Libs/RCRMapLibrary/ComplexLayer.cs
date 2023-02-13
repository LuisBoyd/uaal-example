using System.Collections.Generic;

namespace RCRMapLibrary
{
    public abstract class ComplexLayer<T> : Layer where T : Layer
    {
        public List<T> ChildLayers { get; protected set; }

        public virtual void AddChildLayer(T layer)
        {
            layer.LayerOrder = ChildLayers.Count;
            ChildLayers.Add(layer);
        }

        public virtual void RemoveChildLayer(T layer)
        {
            ChildLayers.Remove(layer);
        }
    }
}