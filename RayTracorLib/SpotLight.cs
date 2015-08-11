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

        public SpotLight(Light lbase, Vector direction, double angle)
            : base(lbase.Position, lbase.Color, lbase.Strength)
        {
            Direction = direction;
            Angle = angle;
        }

        public override double LightVisibility(Vector position)
        {
            double a = Vector.DotProduct(Direction, (position - Position).Normalized);
            double b = Math.Acos(a);

            double margin = Angle * 0.05;

            if (b < (Angle - margin).ToRadians())
                return 1.0;
            if (b > (Angle + margin).ToRadians())
                return 0.0;
            return Math.Tanh((Angle - b.ToDegrees()) / margin * 3.0) * 0.5 + 0.5;
        }

        public override void Serialize(XmlDocument doc, XmlNode parent)
        {
            XmlNode lNode = doc.CreateElement("spotlight");

            lNode.AppendChild(Position.Serialize(doc, "position"));
            lNode.AppendChild(Direction.Serialize(doc, "direction"));
            lNode.AppendChild(Angle.Serialize(doc, "angle"));
            lNode.AppendChild(Color.Serialize(doc, "color"));
            lNode.AppendChild(Strength.Serialize(doc, "strength"));

            parent.AppendChild(lNode);
        }

        public static new SpotLight Parse(XmlNode li)
        {
            Vector dir = Vector.Parse(li["direction"]);
            double angle = li["angle"].ParseDouble();

            Light lbase = Light.Parse(li);

            return new SpotLight(lbase, dir, angle);
        }

        public static SpotLight FromTo(Vector position, Vector point, Color color, double strength, double angle)
        {
            return new SpotLight(position, color, strength, (point - position).Normalized, angle);
        }
    }
}