using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorGetElementByIdBug.Values
{
    public struct Color
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public Color(byte r, byte g, byte b) : this(r, g, b, 255)
        {
        }

        public Color(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public override bool Equals(Object obj)
        {
            // Null?
            if(obj == null)
            {
                return false;
            }

            // Same type?
            if(this.GetType() != obj.GetType())
            {
                return false;
            }

            Color c = (Color)obj;
            return this == c;
        }

        public bool Equals(Color c)
        {
            return this == c;
        }

        public override int GetHashCode()
        {
            return R ^ G ^ B ^ A;
        }

        public override string ToString()
        {
            return $"{R}, {G}, {B}, {A}";
        }

        public static bool operator==(Color a, Color b)
        {
            return (a.R == b.R) && (a.G == b.G) && (a.B == b.B) && (a.A == b.A);
        }

        public static bool operator!=(Color a, Color b)
        {
            return (a.R != b.R) || (a.G != b.G) || (a.B != b.B) || (a.A != b.A);
        }
    }
}
