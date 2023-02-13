using System;
using System.Collections.Generic;

namespace RCRCoreLibrary
{
    public class FlattenedArray<T> where T : new()
    {
        public T[] Collection { get; set; }
        public int bound0 { get; private set; }
        public int bound1 { get; private set; }

        public FlattenedArray(int width, int height)
        {
            Collection = new T[width * height];
            bound0 = width;
            bound1 = height;
        }

        public FlattenedArray(T[,] ExistingArray)
        {
            Collection = new T[ExistingArray.Length];
            bound0 = ExistingArray.GetUpperBound(0);
            bound1 = ExistingArray.GetUpperBound(1);
            for (int x = 0; x < bound0; x++)
            for (int y = 0; y < bound1; y++)
                Collection[(x * bound1) + y] = ExistingArray[x, y];
        }

        public T GetElement(int x, int y)
        {
            if (!ValidateIdex(x, y))
                return default!;

            return Collection[(x * bound1) + y];
        }

        public void SetElement(int x, int y, T value)
        {
            if (!ValidateIdex(x, y))
                return;

            Collection[(x * bound1) + y] = value;
        }

        public bool ValidateIdex(int x, int y)
        {
            return x * y <= Collection.Length && x * y >= 0;
        }

        public IEnumerable<T> GetEnumarable()
        {
            return Collection;
        }
    }
}