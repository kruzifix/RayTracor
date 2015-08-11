﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
            //Vector rayToCenter = Position - ray.Start;
            //double v = Vector.DotProduct(rayToCenter, ray.Direction);
            //double eoDot = Vector.DotProduct(rayToCenter, rayToCenter);
            //double discriminant = Radius * Radius - eoDot + v * v;

            //if (discriminant < 0.0)
            //    return new IntersectionResult(false, 0);
            //return new IntersectionResult(true, v - Math.Sqrt(discriminant));

            //double a = Vector.DotProduct(ray.Direction, ray.Direction);
            //double b = 2 * Vector.DotProduct(ray.Direction, ray.Start - Position);
            //Vector rayToCenter = ray.Start - Position;
            //double c = Vector.DotProduct(rayToCenter, rayToCenter) - Radius * Radius;

            //double dis = b * b - 4 * a * c;

            //if (dis >= 0.0)
            //    return new IntersectionResult(true, (-Math.Sqrt(dis) - b) / (2.0 * a));
            //return new IntersectionResult(false, 0.0);

            Vector L = ray.Start - Position;
            double a = Vector.DotProduct(ray.Direction, ray.Direction);
            double b = 2 * Vector.DotProduct(ray.Direction, L);
            double c = Vector.DotProduct(L, L) - Radius * Radius;

            double t0, t1;

            double discr = b * b - 4 * a * c;
            if (discr < 0) 
                return new IntersectionResult(false, 0.0);
            else if (discr == 0)
                t0 = t1 = -0.5 * b / a;
            else
            {
                double q = (b > 0) ?
                    -0.5 * (b + Math.Sqrt(discr)) :
                    -0.5 * (b - Math.Sqrt(discr));
                t0 = q / a;
                t1 = c / q;
            }
            if (t0 > t1)
            {
                double t = t0;
                t0 = t1;
                t1 = t;
            }
            if (t0 < 0)
            {
                t0 = t1;
                if (t0 < 0.0001)
                    return new IntersectionResult(false, 0.0);
            }

            return new IntersectionResult(true, t0);
        }

        public override Vector Normal(Vector point)
        {
            return (point - Position).Normalized;
        }

        public override Vector EvalMaterial(Vector point, Vector normal, double lambertAmount)
        {
            Vector col = Material.Color.ToVector();
            if (Material.Textured)
            {
                double xtex = (1 + Math.Atan2(normal.Z, normal.X) / Math.PI) * 0.5;
                double ytex = Math.Acos(normal.Y) / Math.PI;
                double scale = 4;

                bool pattern = ((xtex * scale) % 1.0 > 0.5) ^ ((ytex * scale) % 1.0 > 0.5);
                col *= pattern ? 0.5 : 1;
            }
            return col * lambertAmount * Material.Lambert + col * Material.Ambient;
        }

        public override void Serialize(XmlDocument doc, XmlNode parent)
        {
            XmlNode node = doc.CreateElement("sphere");

            SerializeBase(doc, node);
            node.AppendChild(Radius.Serialize(doc, "radius"));
            
            parent.AppendChild(node);
        }

        public static Sphere Parse(XmlNode node)
        {
            Vector pos = Vector.Parse(node["position"]);
            Material mat = Material.Parse(node["material"]);
            double radius = node["radius"].ParseDouble();

            return new Sphere(pos, radius, mat);
        }
    }
}