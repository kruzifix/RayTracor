//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;

//namespace RayTracor.RayTracorLib
//{
//    public class Quad : Object
//    {
//        Vector normal;
//        Vector quadDir;
        
//        public bool Centered { get; set; }
//        public Vector2 Size { get; set; }

//        public Quad(Vector position, Material material, Vector normal, Vector2 size)
//            :base(position, material)
//        {
//            this.normal = normal;
//            Size = size;
//        }

//        public Quad(Vector position, Material material, Vector normal, Vector2 size, bool centered)
//            : base(position, material)
//        {
//            this.normal = normal;
//            Size = size;
//            Centered = centered;
//        }

//        public override Vector EvalMaterial(Vector point, Vector normal, double lambertAmount)
//        {
//            return Material.AddAmbientLambert(Material.Color.ToVector(), lambertAmount);
//        }

//        public override IntersectionResult Intersects(Ray ray)
//        {
//            Vector T = ray.Start - Vertex0;
//            Vector P = Vector.CrossProduct(ray.Direction, E2);
//            Vector Q = Vector.CrossProduct(T, E1);

//            double denom = Vector.DotProduct(P, E1);
//            if (Math.Abs(denom) > 0.0001)
//            {
//                double u = Vector.DotProduct(P, T) / denom;
//                double v = Vector.DotProduct(Q, ray.Direction) / denom;
//                double t = Vector.DotProduct(Q, E2) / denom;
//                if ((u >= 0.0 && u <= 1.0) && (v >= 0.0 && v <= 1.0))
//                    return new IntersectionResultVector2(true, t, new Vector2(u, v));
//            }
//            return new IntersectionResult(false, 0.0);
//        }

//        public override Vector Normal(Vector point)
//        {
//            return normal;
//        }

//        public override void Serialize(XmlDocument doc, XmlNode parent)
//        {
            
//        }
//    }
//}
