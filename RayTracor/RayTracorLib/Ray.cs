using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracor.RayTracorLib
{
    public class Ray
    {
        public Vector Start { get; set; }
        public Vector Direction { get { return dir; } set { dir = value; CalcDirFrac(); } }
        public Vector DirFrac { get { return dirFrac; } }

        Vector dir, dirFrac = Vector.Zero;

        public Ray() { Start = Vector.Zero; Direction = Vector.UnitX; }

        public Ray(Vector start, Vector direction)
        {
            Start = start;
            Direction = direction.Normalized;
        }

        private void CalcDirFrac()
        {
            dirFrac.X = dir.X == 0 ? double.MaxValue : 1.0 / dir.X;
            dirFrac.Y = dir.Y == 0 ? double.MaxValue : 1.0 / dir.Y;
            dirFrac.Z = dir.Z == 0 ? double.MaxValue : 1.0 / dir.Z;
        }

        public Vector PointAt(double k)
        {
            return Start + Direction * k;
        }

        public Ray Reflect(Vector point, Vector normal)
        {
            return new Ray(point, Direction - normal * 2.0 * Vector.DotProduct(Direction, normal));
        }

        public override string ToString()
        {
            return string.Format("<{0}, {1}>", Start, Direction);
        }

        public static Ray FromTo(Vector from, Vector to)
        {
            return new Ray(from, to - from);
        }
    }
}