using System;
using System.Text;
using UnityEngine;

namespace RCR.Settings.SuperNewScripts.Converters
{
    public static class NativeConvertExtension 
    {
        public static int ToHex32(string value)
        {
            string hexString = ToHex(value);
            Debug.Log(value);
            return Convert.ToInt32(hexString, 16);
        }

        public static string ToHex(string value)
        {
            byte[] ba = Encoding.Default.GetBytes(value);
            string hexString = BitConverter.ToString(ba);
            hexString = hexString.Replace("-", "");
            return hexString;
        }
    }
}