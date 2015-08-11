using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RayTracor.RayTracorLib
{
    public class Light
    {
        public Vector Position { get; set; }
        public Color Color { get; set; }
        public double Strength { get; set; }

        public Light() { Position = Vector.Zero; Color = Color.White; Strength = 1; }

        public Light(Vector position, Color color, double strength)
        {
            Position = position;
            Color = color;
            Strength = strength;
        }

        public virtual double LightVisibility(Vector position)
        {
            return 1.0;
        }

        public virtual void Serialize(XmlDocument doc, XmlNode parent)
        {
            XmlNode lNode = doc.CreateElement("light");

            lNode.AppendChild(Position.Serialize(doc, "position"));
            lNode.AppendChild(Color.Serialize(doc, "color"));
            lNode.AppendChild(Strength.Serialize(doc, "strength"));

            parent.AppendChild(lNode);
        }

        public static Light Parse(XmlNode li)
        {
            Vector pos = Vector.Parse(li["position"]);
            Color color = li["color"].ParseColor();
            double strength = li["strength"].ParseDouble();

            return new Light(pos, color, strength);
        }
    }
}