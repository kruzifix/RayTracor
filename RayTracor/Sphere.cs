using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracor.RayTracor
{
    public class Sphere : Object
    {
        public double Radius { get; set; }

        public Sphere(Vector position, double radius)
            : base(position)
        {
            Radius = radius;
        }

        public override IntersectionResult Intersects(Ray ray)
        {
            Vector rayToCenter = Position - ray.Start;
            double v = Vector.DotProduct(rayToCenter, ray.Start);
            double eoDot = Vector.DotProduct(rayToCenter, rayToCenter);
            double discriminant = Radius * Radius - eoDot + v * v;

            if (discriminant < 0.0)
                return new IntersectionResult(false, 0);
            return new IntersectionResult(true, v - Math.Sqrt(discriminant));
        }

        public override Vector Normal(Vector point)
        {
            return (point - Position).Normalized();
        }
    }
}