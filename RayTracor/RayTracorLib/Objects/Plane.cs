using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RayTracor.RayTracorLib.Materials;
using RayTracor.RayTracorLib.Tracing;
using RayTracor.RayTracorLib.Utility;

namespace RayTracor.RayTracorLib.Objects
{
    public class Plane : IObject
    {
        Vector3 position;
        Vector3 normal;

        public Plane(Vector3 position, Vector3 normal, Material material)
            :base(material)
        {
            this.position = position;
            this.normal = normal.Normalized;
        }

        public override Intersection Intersects(Ray ray)
        {
            double denom = Vector3.DotProduct(normal, ray.Direction);
            if (Math.Abs(denom) > 0.0001)
            {
                double t = Vector3.DotProduct(position - ray.Start, normal) / denom;
                if (t > 0.0005)
                    return new Intersection(t, ray.PointAt(t), this, normal, null);
            }
            return Intersection.False;
        }

        public override Vector3 EvalMaterial(Intersection intersec)
        {
            if (Material.Textured)
            {
                double x = Math.Floor(intersec.Point.X);
                double y = Math.Floor(intersec.Point.Z);

                if (x % 2 == 0 ^ y % 2 == 0)
                    return Material.Color.ToVector() * 0.5;
            }
            return Material.Color.ToVector();
        }

        public override void Serialize(XmlDocument doc, XmlNode parent)
        {
            XmlNode node = doc.CreateElement("plane");
            
            node.AppendChild(normal.Serialize(doc, "normal"));
            node.AppendChild(position.Serialize(doc, "normal"));
            base.Serialize(doc, node);

            parent.AppendChild(node);
        }

        public static Plane Parse(XmlNode node)
        {
            Vector3 pos = Vector3.Parse(node["position"]);
            Material mat = Material.Parse(node["material"]);
            Vector3 normal = Vector3.Parse(node["normal"]).Normalized;

            return new Plane(pos, normal, mat);
        }
    }
}