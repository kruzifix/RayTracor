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
    public class AreaLight : ILight
    {
        public double Size { get; private set; }
        public Vector3 Position { get; private set; }
        
        public AreaLight() : base() { }

        public AreaLight(Vector3 position, double size, Color color, double strength)
            :base(color, strength)
        {
            Position = position;
            Size = size;
        }
                
        //public override void Serialize(XmlDocument doc, XmlNode parent)
        //{
        //    XmlNode lNode = doc.CreateElement("arealight");

        //    lNode.AppendChild(Position.Serialize(doc, "position"));
        //    lNode.AppendChild(Color.Serialize(doc, "color"));
        //    lNode.AppendChild(Strength.Serialize(doc, "strength"));

        //    parent.AppendChild(lNode);
        //}

        //public static AreaLight Parse(XmlNode li)
        //{
        //    Vector3 pos = Vector3.Parse(li["position"]);
        //    double size = li["size"].ParseDouble();
        //    Color color = li["color"].ParseColor();
        //    double strength = li["strength"].ParseDouble();
            
        //    return new AreaLight(pos, size, color, strength);
        //}

        public static AreaLight FromJToken(JToken tok)
        {
            SerializedAreaLight sal = JsonConvert.DeserializeObject<SerializedAreaLight>(tok.ToString());

            if (sal.position == null)
                throw new Exception("AreaLight: 'position' not defined.");
            if (sal.size <= 0.0)
                throw new Exception("AreaLight: 'size' has to be greater than 0.");
            if (sal.strength <= 0.0)
                throw new Exception("AreaLight: 'strength' has to be greater than 0.");
            if (string.IsNullOrWhiteSpace(sal.color))
                throw new Exception("AreaLight: 'color' not defined.");

            Color c = sal.color.ColorFromHexString("AreaLight: Unable to parse 'color'.");

            return new AreaLight(sal.position, sal.size, c, sal.strength);
        }
    }

    class SerializedAreaLight
    {
        public double size { get; set; }
        public Vector3 position { get; set; }
        public string color { get; set; }
        public double strength { get; set; }
    }
}