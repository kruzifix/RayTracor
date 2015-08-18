using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml;
using RayTracor.RayTracorLib.Utility;
using RayTracor.RayTracorLib.Tracing;

namespace RayTracor.RayTracorLib.Lights
{
    public class SpotLight : PointLight
    {
        public Vector3 Direction { get; set; }
        // saved in radians; given in degrees
        public double Angle { get; set; }

        public SpotLight()
            : base()
        { Direction = Vector3.UnitX; Angle = (30.0).ToRadians(); }

        public SpotLight(Vector3 position, Color color, double strength, Vector3 direction, double angle)
            : base(position, color, strength)
        {
            Direction = direction;
            Angle = angle.ToRadians();
        }

        public SpotLight(PointLight lbase, Vector3 direction, double angle)
            : base(lbase.Position, lbase.Color, lbase.Strength)
        {
            Direction = direction;
            Angle = angle.ToRadians();
        }

        public bool PointLighted(Vector3 point)
        {
            double a = Vector3.DotProduct(Direction, (point - Position).Normalized);
            double b = Math.Acos(a);

            return b < Angle;
        }

        public override void Serialize(XmlDocument doc, XmlNode parent)
        {
            XmlNode lNode = doc.CreateElement("spotlight");

            lNode.AppendChild(Position.Serialize(doc, "position"));
            lNode.AppendChild(Direction.Serialize(doc, "direction"));
            lNode.AppendChild(Angle.ToDegrees().Serialize(doc, "angle"));
            lNode.AppendChild(Color.Serialize(doc, "color"));
            lNode.AppendChild(Strength.Serialize(doc, "strength"));

            parent.AppendChild(lNode);
        }

        public static new SpotLight Parse(XmlNode li)
        {
            Vector3 dir = Vector3.Parse(li["direction"]).Normalized;
            double angle = li["angle"].ParseDouble();

            PointLight lbase = PointLight.Parse(li);
            SpotLight spl = new SpotLight(lbase, dir, angle);

            return spl;
        }

        public static SpotLight FromTo(Vector3 position, Vector3 point, Color color, double strength, double angle)
        {
            return new SpotLight(position, color, strength, (point - position).Normalized, angle);
        }
    }
}