using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RayTracor.RayTracorLib.Materials;
using RayTracor.RayTracorLib.Tracing;
using RayTracor.RayTracorLib.Utilities;

namespace RayTracor.RayTracorLib.Objects
{
    public class Plane : IObject
    {
        Vector3 position;
        Vector3 normal;

        public Plane(Vector3 position, Vector3 normal, string material)
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

        public override Vector3 EvalMaterial(Intersection intersec, Material mat)
        {
            if (mat.Textured)
            {
                double x = Math.Floor(intersec.Point.X);
                double y = Math.Floor(intersec.Point.Z);

                if (x % 2 == 0 ^ y % 2 == 0)
                    return mat.Color.ToVector() * 0.5;
            }
            return mat.Color.ToVector();
        }

        public static IObject FromJToken(JToken tok)
        {
            SerializedPlane pl = JsonConvert.DeserializeObject<SerializedPlane>(tok.ToString());

            if (pl.position == null)
                throw new Exception("Plane: 'position' not defined.");
            if (pl.normal == null)
                throw new Exception("Plane: 'normal' not defined.");
            if (string.IsNullOrWhiteSpace(pl.material))
                throw new Exception("Plane: 'material' not defined.");

            return new Plane(pl.position, pl.normal, pl.material);
        }

        //public override void Serialize(XmlDocument doc, XmlNode parent)
        //{
        //    XmlNode node = doc.CreateElement("plane");

        //    node.AppendChild(normal.Serialize(doc, "normal"));
        //    node.AppendChild(position.Serialize(doc, "normal"));
        //    base.Serialize(doc, node);

        //    parent.AppendChild(node);
        //}

        //public static Plane Parse(XmlNode node)
        //{
        //    Vector3 pos = Vector3.Parse(node["position"]);
        //    Material mat = Material.Parse(node["material"]);
        //    Vector3 normal = Vector3.Parse(node["normal"]).Normalized;

        //    return new Plane(pos, normal, mat);
        //}
    }

    class SerializedPlane
    {
        public string material { get; set; }
        public Vector3 position { get; set; }
        public Vector3 normal { get; set; }
    }
}