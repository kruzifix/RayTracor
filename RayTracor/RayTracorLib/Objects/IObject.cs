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

namespace RayTracor.RayTracorLib.Objects
{
    public abstract class IObject
    {   
        public string Material { get; set; }

        protected IObject(string material) { Material = material; }

        public abstract Intersection Intersects(Ray ray);
        public abstract Vector3 EvalMaterial(Intersection intersec, Material mat);

        public static IObject ParseJToken(JToken tok)
        {
            string type = null;
            try { type = tok["type"].ToString(); } catch { }
            IObject result = null;

            if (string.IsNullOrWhiteSpace(type))
                throw new Exception("Objects: 'type' not defined.");

            switch (type)
            {
                case "sphere":
                    result = Sphere.FromJToken(tok);
                    break;
                case "plane":
                    result = Plane.FromJToken(tok);
                    break;
                default:
                    throw new Exception("Objects: Unknown 'type'.");
            }
            return result;
        }
    }
}