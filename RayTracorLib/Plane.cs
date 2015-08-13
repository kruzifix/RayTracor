using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RayTracor.RayTracorLib
{
    public class Plane : Object
    {
        Vector normal;

        public Plane(Vector position, Material material, Vector normal)
            :base(position, material)
        {
            this.normal = normal.Normalized;
        }

        public override IntersectionResult Intersects(Ray ray)
        {
            double denom = Vector.DotProduct(normal, ray.Direction);
            if (Math.Abs(denom) > 0.0001)
            {
                double t = Vector.DotProduct(Position - ray.Start, normal) / denom;
                if (t > 0.0005)
                    return new IntersectionResult(true, t);
            }
            return new IntersectionResult(false, 0.0);
        }

        public override Vector Normal(Vector point)
        {
            return normal;
        }

        public override Vector EvalMaterial(Vector point, Vector normal, double lambertAmount)
        {
            Vector col = Material.Color.ToVector();
            if (Material.Textured)
            {
                double x = Math.Floor(point.X);
                double y = Math.Floor(point.Z);

                if (x % 2 == 0 ^ y % 2 == 0)
                    col *= 0.5;
            }
            return Material.AddAmbientLambert(col, lambertAmount);
        }

        public override void Serialize(XmlDocument doc, XmlNode parent)
        {
            XmlNode node = doc.CreateElement("plane");

            SerializeBase(doc, node);
            node.AppendChild(normal.Serialize(doc, "normal"));

            parent.AppendChild(node);
        }

        public static Plane Parse(XmlNode node)
        {
            Vector pos = Vector.Parse(node["position"]);
            Material mat = Material.Parse(node["material"]);
            Vector normal = Vector.Parse(node["normal"]).Normalized;

            return new Plane(pos, mat, normal);
        }
    }
}