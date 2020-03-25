using BlazorGetElementByIdBug.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGetElementByIdBug.Utils
{
    public class ValueUtils
    {
        public static byte HexByteToByte(string hex)
        {
            const string hex_chars = "0123456789abcdef";
            hex = hex.ToLowerInvariant();
            int result = (hex_chars.IndexOf(hex[0]) * 16) + hex_chars.IndexOf(hex[1]);
            return (byte)result;
        }

        public static string ColorToHexColor(Color c)
        {
            string hex = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2") + c.A.ToString("X2");
            return hex;
        }
    }
}
