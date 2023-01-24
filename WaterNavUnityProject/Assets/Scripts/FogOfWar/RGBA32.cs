using System;
using Unity.Mathematics;

namespace RCR.Settings.FogOfWar
{
    public struct RGBA32
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public static readonly RGBA32 Zero = new RGBA32()
        {
            R = 0,
            A = 0,
            B = 0,
            G = 0
        };
        public static readonly RGBA32 White = new RGBA32()
        {
            R = 255,
            A = 255,
            B = 255,
            G = 255
        };
        
        public static explicit operator RGBA32(int4 t)
        {
            RGBA32 v = new RGBA32();
            try
            {
                v.R = Convert.ToByte(t.x);
                v.G = Convert.ToByte(t.y);
                v.B = Convert.ToByte(t.z);
                v.A = Convert.ToByte(t.w);
            }
            catch (OverflowException e)
            {
                v.A = Byte.MaxValue;
                v.B = Byte.MaxValue;
                v.G = Byte.MaxValue;
                v.A = Byte.MaxValue;
            }
            return v;
        }
        public static explicit operator RGBA32(int3 t)
        {
            RGBA32 v = new RGBA32();
            try
            {
                v.R = Convert.ToByte(t.x);
                v.G = Convert.ToByte(t.y);
                v.B = Convert.ToByte(t.z);
                v.B = 0;
            }
            catch (OverflowException e)
            {
                v.A = Byte.MaxValue;
                v.B = Byte.MaxValue;
                v.G = Byte.MaxValue;
                v.A = Byte.MaxValue;
            }
            return v;
        }
      
        
    }
}