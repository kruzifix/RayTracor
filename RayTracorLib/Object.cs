using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RayTracor.RayTracorLib
{
    public abstract class Object
    {
        public Vector Position { get; set; }
        public Material Material { get; set; }
        
        protected Object(Vector position, Material material) { Position = position; Material = material; }

        public abstract Intersection Intersects(Ray ray);
        public abstract Vector EvalMaterial(Intersection intersec, double lambertAmount);

        public abstract void Serialize(XmlDocument doc, XmlNode parent);

        protected void SerializeBase(XmlDocument doc, XmlNode parent)
        {
            parent.AppendChild(Position.Serialize(doc, "position"));
            Material.Serialize(doc, parent);
        }
    }
}