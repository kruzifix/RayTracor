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
    public class PointLight : ILight
    {
        public Vector3 Position { get; set; }
        public Color Color { get; set; }
        public double Strength { get; set; }

        public PointLight() { Position = Vector3.Zero; Color = Color.White; Strength = 1; }

        public PointLight(Vector3 position, Color color, double strength)
        {
            Position = position;
            Color = color;
            Strength = strength;
        }
        
        public override void Serialize(XmlDocument doc, XmlNode parent)
        {
            XmlNode lNode = doc.CreateElement("pointlight");

            lNode.AppendChild(Position.Serialize(doc, "position"));
            lNode.AppendChild(Color.Serialize(doc, "color"));
            lNode.AppendChild(Strength.Serialize(doc, "strength"));

            parent.AppendChild(lNode);
        }

        public static PointLight Parse(XmlNode li)
        {
            Vector3 pos = Vector3.Parse(li["position"]);
            Color color = li["color"].ParseColor();
            double strength = li["strength"].ParseDouble();

            return new PointLight(pos, color, strength);
        }
    }
}