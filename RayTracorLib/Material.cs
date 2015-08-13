using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RayTracor.RayTracorLib
{
    public class Material
    {
        public double Specular { get; set; }
        public double Lambert { get; set; }
        public double Ambient { get; set; }
        public Color Color { get; set; }
        public bool Textured { get; set; }

        public Material() { }

        public Vector AddAmbientLambert(Vector col, double lambert)
        {
            return col * lambert * Lambert + col * Ambient;
        }

        public void Serialize(XmlDocument doc, XmlNode parent)
        {
            XmlNode node = doc.CreateElement("material");

            node.AppendChild(Specular.Serialize(doc, "specular"));
            node.AppendChild(Lambert.Serialize(doc, "lambert"));
            node.AppendChild(Ambient.Serialize(doc, "ambient"));
            node.AppendChild(Color.Serialize(doc, "color"));
            node.AppendChild(Textured.Serialize(doc, "textured"));

            parent.AppendChild(node);
        }

        public static Material Parse(XmlNode node)
        {
            Material mat = new Material();

            mat.Specular = node["specular"].ParseDouble();
            mat.Lambert = node["lambert"].ParseDouble();
            mat.Ambient = node["ambient"].ParseDouble();
            mat.Color = node["color"].ParseColor();
            mat.Textured = node["textured"].ParseBool();

            return mat;
        }
    }
}