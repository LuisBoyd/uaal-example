using System;

namespace Utilities
{
    public static class Helper
    {
        public static bool AreTypesAssignable(Type a, Type b)
        {
            // x.IsAssignableFrom(y) returns true if:
            //   (1) x and y are the same type
            //   (2) x and y are in the same inheritance hierarchy
            //   (3) y is implemented by x
            //   (4) y is a generic type parameter and one of its constraints is x
            if (a.IsAssignableFrom(b) || b.IsAssignableFrom(a))
                return true;
            return false;
        }
    }
}