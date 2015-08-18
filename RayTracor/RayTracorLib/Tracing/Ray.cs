using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracor.RayTracorLib.Tracing
{
    public class Ray
    {
        public Vector3 Start { get; set; }
        public Vector3 Direction { get { return dir; } set { dir = value; CalcDirFrac(); } }
        public Vector3 DirFrac { get { return dirFrac; } }

        Vector3 dir, dirFrac = Vector3.Zero;

        public Ray() { Start = Vector3.Zero; Direction = Vector3.UnitX; }

        public Ray(Vector3 start, Vector3 direction)
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

        public Vector3 PointAt(double k)
        {
            return Start + Direction * k;
        }

        public Ray Reflect(Vector3 point, Vector3 normal)
        {
            return new Ray(point, Direction - normal * 2.0 * Vector3.DotProduct(Direction, normal));
        }

        public override string ToString()
        {
            return string.Format("<{0}, {1}>", Start, Direction);
        }

        public static Ray FromTo(Vector3 from, Vector3 to)
        {
            return new Ray(from, to - from);
        }
    }
}