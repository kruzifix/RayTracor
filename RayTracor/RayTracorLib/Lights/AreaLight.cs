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
        public Vector3 Direction { get; private set; }

        Vector3 right, up;

        public AreaLight() : base() { }

        public AreaLight(Vector3 position, Vector3 direction, double size, Color color, double strength)
            :base(color, strength)
        {
            Position = position;
            Direction = direction.Normalized;
            Size = size;

            // TODO: use generic globalup not constant!!
            right = Vector3.CrossProduct(direction, Vector3.UnitY).Normalized;
            up = Vector3.CrossProduct(right, direction).Normalized;
        }

        public Vector3 GetPoint(double u, double v)
        {
            return Position + right * u * Size + up * v * Size;
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
            Vector3 dir = Vector3.UnitX;
            try
            {
                dir = Vector3.Parse(li["direction"]);
            }
            catch { }
            double size = li["size"].ParseDouble();
            Color color = li["color"].ParseColor();
            double strength = li["strength"].ParseDouble();

            if (li["looksat"] != null)
                dir = (Vector3.Parse(li["looksat"]) - pos).Normalized;

            return new AreaLight(pos, dir, size, color, strength);
        }
    }
}