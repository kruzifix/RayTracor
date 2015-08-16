using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RayTracor.RayTracorLib
{
    public class Triangle : Object
    {
        public Vector Vertex0 { get; set; }
        public Vector Vertex1 { get; set; }
        public Vector Vertex2 { get; set; }

        public Vector E1 { get; private set; }
        public Vector E2 { get; private set; }
        public Vector Normal { get; private set; }

        public Triangle(Vector v0, Vector v1, Vector v2, Material material)
            :base(Vector.Zero, material)
        {
            Vertex0 = v0;
            Vertex1 = v1;
            Vertex2 = v2;

            E1 = Vertex1 - Vertex0;
            E2 = Vertex2 - Vertex0;
            Normal = Vector.CrossProduct(E2, E1).Normalized;
        }
        
        public override Intersection Intersects(Ray ray)
        {
            Vector T = ray.Start - Vertex0;
            Vector P = Vector.CrossProduct(ray.Direction, E2);
            Vector Q = Vector.CrossProduct(T, E1);

            double denom = Vector.DotProduct(P, E1);
            if (Math.Abs(denom) > 0.0001)
            {
                double u = Vector.DotProduct(P, T) / denom;
                double v = Vector.DotProduct(Q, ray.Direction) / denom;
                double t = Vector.DotProduct(Q, E2) / denom;
                if ((u >= 0.0 && u <= 1.0) && (v >= 0.0 && u + v <= 1.0))
                    return new Intersection(true, t, ray.PointAt(t), this, Normal, new Vector2(u, v));
            }
            return Intersection.False;
        }

        public override Vector EvalMaterial(Intersection intersec, double lambertAmount)
        {
            Vector col = Material.Color.ToVector();
            //if (bary.X > 0.25 && bary.Y < 0.75 && bary.Y > 0.25 && bary.Y < 0.75)
            //    col *= 0.5;
            if (Material.Textured)
            {
                double scale = 255.0;
                int u = (int)Math.Floor(intersec.BaryCoords.X * scale);
                int v = (int)Math.Floor(intersec.BaryCoords.Y * scale);

                double fac = (u ^ v) * 0.75 / scale + 0.25;
                col *= fac;
            }
            return Material.AddAmbientLambert(col, lambertAmount);
        }
        
        public override void Serialize(XmlDocument doc, XmlNode parent)
        {
            XmlNode node = doc.CreateElement("triangle");
            Material.Serialize(doc, node);
            node.AppendChild(Vertex0.Serialize(doc, "vertex0"));
            node.AppendChild(Vertex1.Serialize(doc, "vertex1"));
            node.AppendChild(Vertex2.Serialize(doc, "vertex2"));
            parent.AppendChild(node);
        }

        public static Triangle Parse(XmlNode node)
        {
            Vector v0 = Vector.Parse(node["vertex0"]);
            Vector v1 = Vector.Parse(node["vertex1"]);
            Vector v2 = Vector.Parse(node["vertex2"]);
            Material mat = Material.Parse(node["material"]);

            return new Triangle(v0, v1, v2, mat);
        }
    }
}
