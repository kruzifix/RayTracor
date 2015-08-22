using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        
        public static IObject FromJToken(JToken tok)
        {
            SerializedQuad sq = JsonConvert.DeserializeObject<SerializedQuad>(tok.ToString());

            if (string.IsNullOrWhiteSpace(sq.material))
                throw new Exception("Quad: 'material' not defined.");
            if (sq.vertex0 == null)
                throw new Exception("Quad: 'vertex0' not defined.");
            if (sq.vertex1 == null)
                throw new Exception("Quad: 'vertex1' not defined.");
            if (sq.vertex2 == null)
                throw new Exception("Quad: 'vertex2' not defined.");
            if (sq.vertex3 == null)
                throw new Exception("Quad: 'vertex3' not defined.");

            return new Quad(
                VertexFromSerialized(sq.vertex0),
                VertexFromSerialized(sq.vertex1),
                VertexFromSerialized(sq.vertex2),
                VertexFromSerialized(sq.vertex3),
                sq.material);
        }

        private static Vertex VertexFromSerialized(SerializedVertex v)
        {
            return new Vertex(v.x, v.y, v.z, v.u, v.v);
        }
    }

    class SerializedQuad
    {
        public string material { get; set; }
        public SerializedVertex vertex0 { get; set; }
        public SerializedVertex vertex1 { get; set; }
        public SerializedVertex vertex2 { get; set; }
        public SerializedVertex vertex3 { get; set; }
    }

    class SerializedVertex
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        public double u { get; set; }
        public double v { get; set; }
    }
}