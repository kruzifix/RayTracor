using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RayTracor.RayTracorLib
{
    public class Quad : Object
    {
        Vector normal;
        Vector quadDir;
        
        public bool Centered { get; set; }
        public Vector2 Size { get; set; }

        public Quad(Vector position, Material material, Vector normal, Vector2 size)
            :base(position, material)
        {
            this.normal = normal;
            Size = size;
        }

        public Quad(Vector position, Material material, Vector normal, Vector2 size, bool centered)
            : base(position, material)
        {
            this.normal = normal;
            Size = size;
            Centered = centered;
        }

        public override Vector EvalMaterial(Vector point, Vector normal, double lambertAmount)
        {
            return Material.AddAmbientLambert(Material.Color.ToVector(), lambertAmount);
        }

        public override IntersectionResult Intersects(Ray ray)
        {
            double denom = Vector.DotProduct(normal, ray.Direction);
            if (Math.Abs(denom) > 0.0001)
            {
                double t = Vector.DotProduct(Position - ray.Start, normal) / denom;
                if (t > 0.0005)
                {
                    Vector p = ray.PointAt(t);


                }
            }
            return new IntersectionResult(false, 0.0);
        }

        public override Vector Normal(Vector point)
        {
            return normal;
        }

        public override void Serialize(XmlDocument doc, XmlNode parent)
        {
            
        }
    }
}
