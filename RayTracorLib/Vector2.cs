using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracor.RayTracorLib
{
    public class Vector2
    {
        public static readonly Vector2 Zero = new Vector2(0, 0);
        public static readonly Vector2 UnitX = new Vector2(1, 0);
        public static readonly Vector2 UnitY = new Vector2(0, 1);

        public double X { get; set; }
        public double Y { get; set; }

        public double Length { get { return Math.Sqrt(X * X + Y * Y); } }
        public Vector2 Normalized { get { double len = Length; return new Vector2(X / len, Y / len); } }

        public Vector2() { X = 0; Y = 0; }

        public Vector2(double x, double y) { X = x; Y = y; }

        public void Normalize()
        {
            double len = Length;
            X /= len;
            Y /= len;
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", X, Y);
        }

        public static double DotProduct(Vector u, Vector v)
        {
            return u.X * v.X + u.Y * v.Y;
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 operator *(Vector2 v1, double k)
        {
            return new Vector2(v1.X * k, v1.Y * k);
        }

        public static Vector2 operator *(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X * v2.X, v1.Y * v2.Y);
        }

        public static Vector2 operator /(Vector2 v1, double k)
        {
            return new Vector2(v1.X / k, v1.Y / k);
        }

        public static Vector2 operator /(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X / v2.X, v1.Y / v2.Y);
        }
    }
}