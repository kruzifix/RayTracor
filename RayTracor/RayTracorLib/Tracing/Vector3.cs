using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RayTracor.RayTracorLib.Utility;

namespace RayTracor.RayTracorLib.Tracing
{
    public class Vector3
    {
        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 One = new Vector3(1, 1, 1);
        public static readonly Vector3 UnitX = new Vector3(1, 0, 0);
        public static readonly Vector3 UnitY = new Vector3(0, 1, 0);
        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);
        
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double Length { get { return Math.Sqrt(X * X + Y * Y + Z * Z); } }
        public Vector3 Normalized { get { double len = Length; return new Vector3(X / len, Y / len, Z / len); } }
        public Vector3 Negated { get { return new Vector3(-X, -Y, -Z); } }

        public Vector3() { X = 0; Y = 0; Z = 0; }

        public Vector3(double v) { X = Y = Z = v; }

        public Vector3(double x, double y, double z) { X = x; Y = y; Z = z; }

        public Vector3(Color color) { X = color.R; Y = color.G; Z = color.B; }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}]", X, Y, Z);
        }

        public void Normalize()
        {
            double len = Length;
            X /= len;
            Y /= len;
            Z /= len;
        }

        public Color ToColor()
        {
            return Color.FromArgb(X.ClampToInt(0, 255), Y.ClampToInt(0, 255), Z.ClampToInt(0, 255));
        }

        public XmlNode Serialize(XmlDocument doc, string name)
        {
            XmlNode node = doc.CreateElement(name);
            XmlAttribute atrX = doc.CreateAttribute("x");
            atrX.Value = X.ToString();
            XmlAttribute atrY = doc.CreateAttribute("y");
            atrY.Value = Y.ToString();
            XmlAttribute atrZ = doc.CreateAttribute("z");
            atrZ.Value = Z.ToString();

            node.Attributes.Append(atrX);
            node.Attributes.Append(atrY);
            node.Attributes.Append(atrZ);

            return node;
        }
        
        public static double DotProduct(Vector3 u, Vector3 v)
        {
            return u.X * v.X + u.Y * v.Y + u.Z * v.Z;
        }

        public static Vector3 CrossProduct(Vector3 u, Vector3 v)
        {
            return new Vector3(
                u.Y * v.Z - u.Z * v.Y,
                u.Z * v.X - u.X * v.Z,
                u.X * v.Y - u.Y * v.X
                );
        }

        public static Vector3 Parse(XmlElement node)
        {
            double x = double.Parse(node.Attributes["x"].Value);
            double y = double.Parse(node.Attributes["y"].Value);
            double z = double.Parse(node.Attributes["z"].Value);

            return new Vector3(x, y, z);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3 operator *(Vector3 v1, double k)
        {
            return new Vector3(v1.X * k, v1.Y * k, v1.Z * k);
        }
        
        public static Vector3 operator /(Vector3 v1, double k)
        {
            return new Vector3(v1.X / k, v1.Y / k, v1.Z / k);
        }
    }
}
