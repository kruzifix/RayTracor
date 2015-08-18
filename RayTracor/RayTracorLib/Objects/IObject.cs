using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RayTracor.RayTracorLib.Materials;
using RayTracor.RayTracorLib.Tracing;

namespace RayTracor.RayTracorLib.Objects
{
    public abstract class IObject
    {
        public Material Material { get; set; }
        
        protected IObject(Material material) { Material = material; }

        public abstract Intersection Intersects(Ray ray);
        public abstract Vector3 EvalMaterial(Intersection intersec);
        public virtual void Serialize(XmlDocument doc, XmlNode parent)
        {
            Material.Serialize(doc, parent);
        }
    }
}