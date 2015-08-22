using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RayTracor.RayTracorLib.Tracing;
using RayTracor.RayTracorLib.Utilities;

namespace RayTracor.RayTracorLib.Materials
{
    public class Material
    {
        public string Name { get; set; }
        public double Specular { get; set; }
        public double Lambert { get; set; }
        public double Ambient { get; set; }
        public Color Color { get; set; }
        public bool Textured { get; set; }
        public Texture Texture { get; set; }

        public Material() { }
        
        public Vector3 GetTextureColor(Vector2 coords)
        {
            return Texture.GetColor(coords);
        }
        
        public static Material FromJToken(JToken tok)
        {
            SerializedMaterial mat = JsonConvert.DeserializeObject<SerializedMaterial>(tok.ToString());

            if (string.IsNullOrWhiteSpace(mat.name))
                throw new Exception("Material: 'name' not defined.");
            if (string.IsNullOrWhiteSpace(mat.color))
                throw new Exception("Material: 'color' not defined.");
            Color c = mat.color.ColorFromHexString("Material: Unable to parse 'color'.");

            if (mat.texture == null)
                return new Material { Name = mat.name, Color = c,
                                      Ambient = mat.ambient, Lambert = mat.lambert,
                                      Specular = mat.specular, Textured = false };
            Texture t = null;
            try
            {
                t = Texture.FromPath(mat.texture);
            }
            catch { throw new Exception(string.Format("Material: Unable to load 'texture' at path '{0}'", mat.texture)); }

            return new Material
            {
                Name = mat.name,
                Color = c,
                Ambient = mat.ambient,
                Lambert = mat.lambert,
                Specular = mat.specular,
                Textured = true,
                Texture = t
            };
        }
    }

    class SerializedMaterial
    {
        public string name { get; set; }
        public string color { get; set; }
        public double ambient { get; set; }
        public double lambert { get; set; }
        public double specular { get; set; }

        public string texture { get; set; }
    }
}