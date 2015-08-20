using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RayTracor.RayTracorLib.Tracing;
using RayTracor.RayTracorLib.Utility;

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
        
        public override void Serialize(XmlDocument doc, XmlNode parent)
        {
            XmlNode lNode = doc.CreateElement("arealight");

            lNode.AppendChild(Position.Serialize(doc, "position"));
            lNode.AppendChild(Color.Serialize(doc, "color"));
            lNode.AppendChild(Strength.Serialize(doc, "strength"));

            parent.AppendChild(lNode);
        }

        public static AreaLight Parse(XmlNode li)
        {
            Vector3 pos = Vector3.Parse(li["position"]);
            double size = li["size"].ParseDouble();
            Color color = li["color"].ParseColor();
            double strength = li["strength"].ParseDouble();
            
            return new AreaLight(pos, size, color, strength);
        }
    }
}