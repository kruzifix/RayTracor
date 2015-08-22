using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RayTracor.RayTracorLib.Tracing;
using RayTracor.RayTracorLib.Utilities;

namespace RayTracor.RayTracorLib.Lights
{
    public abstract class ILight
    {
        // types of light: 
        // point light, area light, spotlight, sun, hemisphere
        public Vector3 Color { get; set; }
        public double Strength { get; set; }

        protected ILight() { Color = new Vector3(255.0); Strength = 1.0; }

        protected ILight(Color color, double strength)
        {
            Color = color.ToVector();
            Strength = strength;
        }

        //public virtual void Serialize(XmlDocument doc, XmlNode parent)
        //{
        //    parent.AppendChild(Color.ToColor().Serialize(doc, "color"));
        //    parent.AppendChild(Strength.Serialize(doc, "strength"));
        //}

        public static ILight ParseJToken(JToken tok)
        {
            string type = null;
            try { type = tok["type"].ToString(); } catch { }
            ILight result = null;

            if (string.IsNullOrWhiteSpace(type))
                throw new Exception("Lights: 'type' not defined.");

            switch(type)
            {
                case "arealight":
                    result = AreaLight.FromJToken(tok);
                    break;
                case "pointlight":
                    result = PointLight.FromJToken(tok);
                    break;
                default:
                    throw new Exception("Lights: Unknown 'type'.");
            }
            return result;
        }
    }
}