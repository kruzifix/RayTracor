using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RayTracor.RayTracorLib.Tracing;
using RayTracor.RayTracorLib.Utilities;

namespace RayTracor.RayTracorLib.Lights
{
    public class PointLight : ILight
    {
        public Vector3 Position { get; set; }

        public PointLight() : base() { Position = Vector3.Zero; }

        public PointLight(Vector3 position, Color color, double strength)
            :base(color, strength)
        {
            Position = position;
        }
        
        public static PointLight FromJToken(JToken tok)
        {
            SerializedPointLight spl = JsonConvert.DeserializeObject<SerializedPointLight>(tok.ToString());

            if (spl.position == null)
                throw new Exception("PointLight: 'position' not defined.");
            if (spl.strength <= 0.0)
                throw new Exception("PointLight: 'strength' has to be greater than 0.");
            if (string.IsNullOrWhiteSpace(spl.color))
                throw new Exception("PointLight: 'color' not defined.");

            Color c = spl.color.ColorFromHexString("Point: Unable to parse 'color'.");

            return new PointLight(spl.position, c, spl.strength);
        }
    }

    class SerializedPointLight
    {
        public Vector3 position { get; set; }
        public double strength { get; set; }
        public string color { get; set; }
    }
}