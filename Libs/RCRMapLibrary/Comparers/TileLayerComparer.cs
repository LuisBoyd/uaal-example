using System.Collections.Generic;
using System.Net.Mail;

namespace RCRMapLibrary.Comparers
{
    public class TileLayerComparer : IComparer<TileLayer>
    {
        public int Compare(TileLayer x, TileLayer y)
        {
            if (x.SortingOrder > y.SortingOrder)
                return 1;

            if (x.SortingOrder < y.SortingOrder)
                return -1;

            return 0;
        }
    }
}