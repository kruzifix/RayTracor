using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RayTracor.RayTracorLib.Materials;
using RayTracor.RayTracorLib.Tracing;
using RayTracor.RayTracorLib.Utilities;

namespace RayTracor.RayTracorLib.Objects
{
    public class Sphere : IObject
    {
        public Vector3 Position { get; set; }
        public double Radius { get; set; }
        double radius2;

        public Sphere(Vector3 position, double radius, string material)
            : base(material)
        {
            Position = position;
            Radius = radius;
            radius2 = radius * radius;
        }

        public override Intersection Intersects(Ray ray)
        {
            double a = Vector3.DotProduct(ray.Direction, ray.Direction);
            double b = 2 * Vector3.DotProduct(ray.Direction, ray.Start - Position);
            Vector3 rayToCenter = ray.Start - Position;
            double c = Vector3.DotProduct(rayToCenter, rayToCenter) - radius2;

            double dis = b * b - 4 * a * c;

            if (dis >= 0.0)
            {
                double t = (-Math.Sqrt(dis) - b) / (2.0 * a);
                Vector3 point = ray.PointAt(t);
                return new Intersection(t, point, this, (point - Position).Normalized, null);
            }
            return Intersection.False;
        }

        public override Vector3 EvalMaterial(Intersection intersec, Material mat)
        {
            if (mat.Textured)
            {
                double xtex = (1 + Math.Atan2(intersec.Normal.Z, intersec.Normal.X) / Math.PI) * 0.5;
                double ytex = Math.Acos(intersec.Normal.Y) / Math.PI;
                double scale = 4;

                bool pattern = ((xtex * scale) % 1.0 > 0.5) ^ ((ytex * scale) % 1.0 > 0.5);
                if (pattern)
                    return Vector3.Zero;

                //int x = (int)(Math.Abs(point.X) * 255.0);
                //int y = (int)(Math.Abs(point.Y) * 255.0);
                //int z = (int)(Math.Abs(point.Z) * 255.0);
                //int c = x ^ y ^ z;

                //col = Extensions.ColorFromHSV(c / 255.0 * 360.0, 0.8, 0.7).ToVector();
            }

            return mat.Color.ToVector();
        }

        public static IObject FromJToken(JToken tok)
        {
            SerializedSphere sp = JsonConvert.DeserializeObject<SerializedSphere>(tok.ToString());

            if (sp.position == null)
                throw new Exception("Sphere: 'position' not defined.");
            if (string.IsNullOrWhiteSpace(sp.material))
                throw new Exception("Sphere: 'material' not defined.");
            if (sp.radius < 0.0)
                throw new Exception("Sphere: 'radius' has to be greater than 0.");

            return new Sphere(sp.position, sp.radius, sp.material);
        }
    }

    class SerializedSphere
    {
        public string material { get; set; }
        public double radius { get; set; }
        public Vector3 position { get; set; }
    }
}