using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RayTracor.RayTracorLib.Materials;
using RayTracor.RayTracorLib.Tracing;

namespace RayTracor.RayTracorLib.Objects
{
    public class Quad : IObject
    {
        public Vertex Vertex0 { get; set; }
        public Vertex Vertex1 { get; set; }
        public Vertex Vertex2 { get; set; }
        public Vertex Vertex3 { get; set; }

        public Triangle T1 { get; private set; }
        public Triangle T2 { get; private set; }

        public Quad(Vertex v0, Vertex v1, Vertex v2, Vertex v3, string material)
            : base(material)
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

        public override Vector3 EvalMaterial(Intersection intersec, Material mat)
        {
            Vector2 bary = intersec.BaryCoords;
            
            if (bary.X + bary.Y > 1.0)
            {
                intersec.BaryCoords.X = 1.0 - intersec.BaryCoords.X;
                intersec.BaryCoords.Y = 1.0 - intersec.BaryCoords.Y;
                return T2.EvalMaterial(intersec, mat);
            }
            return T1.EvalMaterial(intersec, mat);
        }
        
        //public override void Serialize(XmlDocument doc, XmlNode parent)
        //{
        //    XmlNode node = doc.CreateElement("quad");
        //    //node.AppendChild(Vertex0.Serialize(doc, "vertex0"));
        //    //node.AppendChild(Vertex1.Serialize(doc, "vertex1"));
        //    //node.AppendChild(Vertex2.Serialize(doc, "vertex2"));
        //    //node.AppendChild(Vertex2.Serialize(doc, "vertex3"));
        //    base.Serialize(doc, node);
        //    parent.AppendChild(node);
        //}

        //public static Quad Parse(XmlNode node)
        //{   
        //    Vector3 v0 = Vector3.Parse(node["vertex0"]);
        //    Vector3 v1 = Vector3.Parse(node["vertex1"]);
        //    Vector3 v2 = Vector3.Parse(node["vertex2"]);
        //    Vector3 v3 = Vector3.Parse(node["vertex3"]);
        //    Material mat = Material.Parse(node["material"]);
        //    Quad q = new Quad(new Vertex { Position = v0, TexCoord = Vector2.UnitX },
        //                      new Vertex { Position = v1, TexCoord = Vector2.Zero },
        //                      new Vertex { Position = v2, TexCoord = Vector2.UnitY },
        //                      new Vertex { Position = v3, TexCoord = Vector2.One }, mat);
            
        //    return q;
        //}
    }
}