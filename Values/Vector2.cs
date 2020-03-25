using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorGetElementByIdBug.Values
{
    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
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

            Vector2 v = (Vector2)obj;
            return this == v;
        }

        public bool Equals(Vector2 v)
        {
            return this == v;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = X.GetHashCode();
                hash = (hash * 397) ^ Y.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"{X}, {Y}";
        }

        public static bool operator==(Vector2 a, Vector2 b)
        {
            return (a.X == b.X) && (a.Y == b.Y);
        }

        public static bool operator!=(Vector2 a, Vector2 b)
        {
            return (a.X != b.X) || (a.Y != b.Y);
        }
    }
}
