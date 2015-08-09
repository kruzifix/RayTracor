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
        public Vector Direction { get; set; }

        public Ray() { Start = Vector.Zero; Direction = Vector.UnitX; }

        public Ray(Vector start, Vector direction)
        {
            Start = start;
            Direction = direction.Normalized;
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

    public class IntersectionResult
    {
        public bool Intersects { get; set; }
        public double Distance { get; set; }

        public IntersectionResult(bool intersects, double distance)
        {
            Intersects = intersects;
            Distance = distance;
        }
    }

    public class SceneIntersectionResult
    {
        public Object Object { get; set; }
        public IntersectionResult Result { get; set; }

        public SceneIntersectionResult(Object obj, IntersectionResult res)
        {
            Object = obj;
            Result = res;
        }
    }
}