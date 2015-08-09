using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracor.RayTracorLib
{
    public abstract class Object
    {
        public Vector Position { get; set; }
        public Material Material { get; set; }

        protected Object(Vector position, Material material) { Position = position; Material = material; }

        public abstract IntersectionResult Intersects(Ray ray);
        public abstract Vector Normal(Vector point);
        public abstract Vector EvalMaterial(Vector point, double lambertAmount);
    }

    public class Material
    {
        public double Specular { get; set; }
        public double Lambert { get; set; }
        public double Ambient { get; set; }
        public Color Color { get; set; }

        public Material() { }
    }
}