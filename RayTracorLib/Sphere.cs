using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracor.RayTracorLib
{
    public class Sphere : Object
    {
        public double Radius { get; set; }

        public Sphere(Vector position, double radius, Material material)
            : base(position, material)
        {
            Radius = radius;
        }

        public override IntersectionResult Intersects(Ray ray)
        {
            Vector rayToCenter = Position - ray.Start;
            double v = Vector.DotProduct(rayToCenter, ray.Direction);
            double eoDot = Vector.DotProduct(rayToCenter, rayToCenter);
            double discriminant = Radius * Radius - eoDot + v * v;

            if (discriminant < 0.0)
                return new IntersectionResult(false, 0);
            return new IntersectionResult(true, v - Math.Sqrt(discriminant));

            //double a = Vector.DotProduct(ray.Direction, ray.Direction);
            //double b = 2 * Vector.DotProduct(ray.Direction, ray.Start - Position);
            //Vector rayToCenter = ray.Start - Position;
            //double c = Vector.DotProduct(rayToCenter, rayToCenter) - Radius * Radius;

            //double dis = b * b - 4 * a * c;

            //if (dis >= 0.0)
            //    return new IntersectionResult(true, (-Math.Sqrt(dis) - b) / (2.0 * a));
            //return new IntersectionResult(false, 0.0);
        }

        public override Vector Normal(Vector point)
        {
            return (point - Position).Normalized;
        }

        public override Vector EvalMaterial(Vector point, double lambertAmount)
        {
            Vector col = Material.Color.ToVector();
            return col * lambertAmount * Material.Lambert + col * Material.Ambient;
        }
    }
}