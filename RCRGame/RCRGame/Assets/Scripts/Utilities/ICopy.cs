using System;

namespace Utilities
{
    //Creates a deep copy of an object.
    public interface ICopy<T> : ICloneable
    {
        T DeepCopy();
    }
}