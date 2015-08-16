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
        public Vector Vertex0 { get; set; }
        public Vector Vertex1 { get; set; }
        public Vector Vertex2 { get; set; }
        public Vector Vertex3 { get; set; }

        public Triangle T1 { get; private set; }
        public Triangle T2 { get; private set; }

        public Quad(Vector v0, Vector v1, Vector v2, Vector v3, Material material)
            : base(Vector.Zero, material)
        {
            Vertex0 = v0;
            Vertex1 = v1;
            Vertex2 = v2;
            Vertex3 = v3;

            T1 = new Triangle(Vertex0, Vertex1, Vertex3, material);
            T2 = new Triangle(Vertex2, Vertex3, Vertex1, material);
        }

        public override Intersection Intersects(Ray ray)
        {
            Intersection interT1 = T1.Intersects(ray);
            Intersection interT2 = T2.Intersects(ray);
            Intersection res = Intersection.False;

            if (!interT1.Intersects && !interT2.Intersects)
                return res;
            if (interT1.Intersects && !interT2.Intersects)
                res = interT1;
            if (!interT1.Intersects && interT2.Intersects)
                res = interT2;
            res = interT1.Distance < interT2.Distance ? interT1 : interT2;
            
            if (res.Object == T2)
            {
                res.BaryCoords.X = 1.0 - res.BaryCoords.X;
                res.BaryCoords.Y = 1.0 - res.BaryCoords.Y;
            }

            res.Object = this;
            return res;
        }

        public override Vector EvalMaterial(Intersection intersec, double lambertAmount)
        {
            return new Vector(intersec.BaryCoords.X, intersec.BaryCoords.Y, 0) * 255.0;
        }

        public override void Serialize(XmlDocument doc, XmlNode parent)
        {
            XmlNode node = doc.CreateElement("quad");
            Material.Serialize(doc, node);
            node.AppendChild(Vertex0.Serialize(doc, "vertex0"));
            node.AppendChild(Vertex1.Serialize(doc, "vertex1"));
            node.AppendChild(Vertex2.Serialize(doc, "vertex2"));
            node.AppendChild(Vertex2.Serialize(doc, "vertex3"));
            parent.AppendChild(node);
        }

        public static Quad Parse(XmlNode node)
        {
            Vector v0 = Vector.Parse(node["vertex0"]);
            Vector v1 = Vector.Parse(node["vertex1"]);
            Vector v2 = Vector.Parse(node["vertex2"]);
            Vector v3 = Vector.Parse(node["vertex3"]);
            Material mat = Material.Parse(node["material"]);

            return new Quad(v0, v1, v2, v3, mat);
        }
    }
}
