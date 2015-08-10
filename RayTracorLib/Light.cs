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

        public virtual bool IsVisibleFrom(Vector position)
        {
            return true;
        }

        public virtual void Serialize(XmlDocument doc)
        {
            XmlNode lNode = doc.CreateElement("Light");

            XmlNode posNode = Position.Serialize(doc, "Position");
            lNode.AppendChild(posNode);

            XmlNode colNode = Color.Serialize(doc, "Color");
            lNode.AppendChild(colNode);

            XmlNode fovNode = doc.CreateElement("Strength");
            XmlAttribute fovAtr = doc.CreateAttribute("Value");
            fovAtr.Value = Strength.ToString();
            fovNode.Attributes.Append(fovAtr);
            lNode.AppendChild(fovNode);

            doc.SelectSingleNode("//Scene/Lights").AppendChild(lNode);
        }
    }
}