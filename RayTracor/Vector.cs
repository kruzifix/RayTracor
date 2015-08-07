using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracor.RayTracor
{
    public class Vector
    {
        public static readonly Vector Zero = new Vector(0, 0, 0);
        public static readonly Vector UnitX = new Vector(1, 0, 0);
        public static readonly Vector UnitY = new Vector(0, 1, 0);
        public static readonly Vector UnitZ = new Vector(0, 0, 1);

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double Length { get { return Math.Sqrt(X * X + Y * Y + Z * Z); } }
        public Vector Normalized { get { return new Vector(X / Length, Y / Length, Z / Length); } }

        public Vector() { X = 0; Y = 0; Z = 0; }

        public Vector(double x, double y, double z) { X = x; Y = y; Z = z; }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}]", X, Y, Z);
        }

        public void Normalize()
        {
            double len = Length;
            X /= len;
            Y /= len;
            Z /= len;
        }
        
        public static double DotProduct(Vector u, Vector v)
        {
            return u.X * v.X + u.Y * v.Y + u.Z * v.Z;
        }

        public static Vector CrossProduct(Vector u, Vector v)
        {
            return new Vector(
                u.Y * v.Z - u.Z * v.Y,
                u.Z * v.X - u.X * v.Z,
                u.X * v.Y - u.Y * v.X
                );
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector operator *(Vector v1, double k)
        {
            return new Vector(v1.X * k, v1.Y * k, v1.Z * k);
        }
    }
}
