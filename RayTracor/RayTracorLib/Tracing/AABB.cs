using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracor.RayTracorLib.Tracing
{
    public class AABB
    {
        public Vector3 Min { get; private set; }
        public Vector3 Max { get; private set; }

        public AABB(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public bool Contains(Vector3 v)
        {
            return  v.X >= Min.X && v.X <= Max.X &&
                    v.Y >= Min.Y && v.Y <= Max.Y &&
                    v.Z >= Min.Z && v.Z <= Max.Z;
        }

        public bool Intersects(Ray ray, out double distance)
        {
            distance = 0.0;
            if (Contains(ray.Start))
                return true;

            double t1 = (Min.X - ray.Start.X) * ray.DirFrac.X;
            double t2 = (Max.X - ray.Start.X) * ray.DirFrac.X;
            double t3 = (Min.Y - ray.Start.Y) * ray.DirFrac.Y;
            double t4 = (Max.Y - ray.Start.Y) * ray.DirFrac.Y;
            double t5 = (Min.Z - ray.Start.Z) * ray.DirFrac.Z;
            double t6 = (Max.Z - ray.Start.Z) * ray.DirFrac.Z;

            double tmin = Math.Max(Math.Max(Math.Min(t1, t2), Math.Min(t3, t4)), Math.Min(t5, t6));
            double tmax = Math.Min(Math.Min(Math.Max(t1, t2), Math.Max(t3, t4)), Math.Max(t5, t6));

            // if tmax < 0, ray (line) is intersecting AABB, but whole AABB is behind us
            if (tmax < 0.0)
                return false;

            // if tmin > tmax, ray doesn't intersect AABB
            if (tmin > tmax)
                return false;

            distance = tmin;
            return true;
        }
    }
}