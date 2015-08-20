using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RayTracor.RayTracorLib.Tracing;
using RayTracor.RayTracorLib.Utility;

namespace RayTracor.RayTracorLib.Materials
{
    public class Material
    {
        public double Specular { get; set; }
        public double Lambert { get; set; }
        public double Ambient { get; set; }
        public Color Color { get; set; }
        public bool Textured { get; set; }
        public Texture Texture { get; set; }

        public Material() { }

        //public Vector3 AddAmbientLambert(Vector3 col, double lambert)
        //{
        //    return col * lambert * Lambert + col * Ambient;
        //}

        public Vector3 GetTextureColor(Vector2 coords)
        {
            return Texture.GetColor(coords);
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

            if (mat.Textured)
                mat.Texture = Texture.FromPath(node["texture"].InnerText);

            return mat;
        }
    }
}
