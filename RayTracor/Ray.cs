using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracor.RayTracor
{
    public class Ray
    {
        public Vector Start { get; set; }
        public Vector Direction { get; set; }

        public Ray() { Start = Vector.Zero; Direction = Vector.UnitX; }

        public Ray(Vector start, Vector direction)
        {
            Start = start;
            Direction = direction;
        }

        public Vector PointAt(double k)
        {
            return Start + Direction * k;
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
        public IntersectionResult Distance { get; set; }

        public SceneIntersectionResult(Object obj, IntersectionResult dist)
        {
            Object = interObj;
            Distance = dist;
        }
    }
}