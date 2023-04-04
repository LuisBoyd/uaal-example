using System;
using System.Collections.Generic;

namespace Utilities
{
    public static class Hasher
    {
        private const double p = 53; //Used for Hashing String.
        private const double m = 1e9 + 9; //Used for Hashing String. = 1 * 10^9

        private static IDictionary<int, double> p_pow;

        static Hasher()
        {
            //Static Constructor pre-compute the powers of P for a certain amount 64 seems fine.
            p_pow = new Dictionary<int, double>();
            for (int i = 0; i <= 64; i++) //64 being the certain amount will be 65 entries since 0 index
            {
                p_pow.Add(i, Math.Pow(p, i));
            }
        }

        public static double Compute_RollingHash(string s)
        {
            double hashValue = 0;
            for (int i = 0; i < s.Length; i++) //s[0] + s[1] * p + s[2] * p^2 ... [n-1] * p^n-1 % m.
            {
                hashValue = (hashValue + (s[i] - 'a' + 1) * p_pow[i]) % m;
            }
            return hashValue;
        }
    }
}