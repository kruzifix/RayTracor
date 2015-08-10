using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RayTracor.RayTracorLib
{
    public class Vector
    {
        public static readonly Vector Zero = new Vector(0, 0, 0);
        public static readonly Vector UnitX = new Vector(1, 0, 0);
        public static readonly Vector UnitY = new Vector(0, 1, 0);
        public static readonly Vector UnitZ = new Vector(0, 0, 1);
        
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double Length { get { return Math.Sqrt(X * X + Y * Y + Z * Z); } }
        public Vector Normalized { get { double len = Length; return new Vector(X / len, Y / len, Z / len); } }

        public Vector() { X = 0; Y = 0; Z = 0; }

        public Vector(double x, double y, double z) { X = x; Y = y; Z = z; }

        public Vector(Color color) { X = color.R; Y = color.G; Z = color.B; }

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
            XmlAttribute atrX = doc.CreateAttribute("X");
            atrX.Value = X.ToString();
            XmlAttribute atrY = doc.CreateAttribute("Y");
            atrY.Value = Y.ToString();
            XmlAttribute atrZ = doc.CreateAttribute("Z");
            atrZ.Value = Z.ToString();

            node.Attributes.Append(atrX);
            node.Attributes.Append(atrY);
            node.Attributes.Append(atrZ);

            return node;
        }
        
        public static double DotProduct(Vector u, Vector v)
        {
            return u.X * v.X + u.Y * v.Y + u.Z * v.Z;
        }

        public static Vector CrossProduct(Vector u, Vector v)
        {
            return new Vector(
                u.Y * v.Z - u.Z * v.Y,
                u.Z * v.X - u.X * v.Z,
                u.X * v.Y - u.Y * v.X
                );
        }

        public static Vector Parse(XmlElement node)
        {
            double x = double.Parse(node.Attributes["X"].Value);
            double y = double.Parse(node.Attributes["Y"].Value);
            double z = double.Parse(node.Attributes["Z"].Value);

            return new Vector(x, y, z);
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector operator *(Vector v1, double k)
        {
            return new Vector(v1.X * k, v1.Y * k, v1.Z * k);
        }
        
        public static Vector operator /(Vector v1, double k)
        {
            return new Vector(v1.X / k, v1.Y / k, v1.Z / k);
        }
    }
}
