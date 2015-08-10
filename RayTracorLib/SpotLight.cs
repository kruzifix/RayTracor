using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml;

namespace RayTracor.RayTracorLib
{
    public class SpotLight : Light
    {
        public Vector Direction { get; set; }
        public double Angle { get; set; }

        public SpotLight()
            : base()
        { Direction = Vector.UnitX; Angle = (30.0).ToRadians(); }

        public SpotLight(Vector position, Color color, double strength, Vector direction, double angle)
            :base(position, color, strength)
        {
            Direction = direction;
            Angle = angle;
        }

        public override bool IsVisibleFrom(Vector position)
        {
            double a = Vector.DotProduct(Direction, (position - Position).Normalized);
            double b = Math.Acos(a);

            if (b > Angle.ToRadians())
                return false;
            return true;
        }

        public override void Serialize(System.Xml.XmlDocument doc)
        {
            XmlNode lNode = doc.CreateElement("SpotLight");

            XmlNode posNode = Position.Serialize(doc, "Position");
            lNode.AppendChild(posNode);

            XmlNode dirNode = Direction.Serialize(doc, "Direction");
            lNode.AppendChild(dirNode);
            
            XmlNode anglNode = doc.CreateElement("Angle");
            XmlAttribute anglAtr = doc.CreateAttribute("Value");
            anglAtr.Value = Angle.ToString();
            anglNode.Attributes.Append(anglAtr);
            lNode.AppendChild(anglNode);

            XmlNode colNode = Color.Serialize(doc, "Color");
            lNode.AppendChild(colNode);

            XmlNode fovNode = doc.CreateElement("Strength");
            XmlAttribute fovAtr = doc.CreateAttribute("Value");
            fovAtr.Value = Strength.ToString();
            fovNode.Attributes.Append(fovAtr);
            lNode.AppendChild(fovNode);

            doc.SelectSingleNode("//Scene/Lights").AppendChild(lNode);
        }

        public static SpotLight FromTo(Vector position, Vector point, Color color, double strength, double angle)
        {
            return new SpotLight(position, color, strength, (point - position).Normalized, angle);
        }
    }
}