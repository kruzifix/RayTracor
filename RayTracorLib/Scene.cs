using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Xml;

namespace RayTracor.RayTracorLib
{
    public class Scene
    {
        // Camera
        // Lights
        // Objects

        public Camera camera;
        public List<Light> lights;
        public List<Object> objects;

        Color backgroundColor;

        public Scene()
        {
            //camera = Camera.CreateLookAt(new Vector(0, 1.8, 10), new Vector(0, 3, 0), new Vector(0,1,0), 45.0);
            //camera = Camera.CreateLookAt(new Vector(0, 3, 12), new Vector(0, 0, 0), new Vector(0, 1, 0), 45.0);
            lights = new List<Light>();
            objects = new List<Object>();

            //lights.Add(new Light(new Vector(-30, -10, 20), Color.White, 1));
            lights.Add(new Light(new Vector(7, 10, 7), Color.White, 1));
            //lights.Add(new SpotLight(new Vector(0, 10, 0), Color.White, 1, new Vector(0, -1, 0), 20.0));
            lights.Add(SpotLight.FromTo(new Vector(0, 10, 5), new Vector(0,0,0), Color.White, 1, 20.0));

            //objects.Add(new Sphere(new Vector(0, 3.5, -3), 3,
            //    new Material
            //    {
            //        Ambient = 0.1,
            //        Specular = 0.2,
            //        Color = Color.FromArgb(222, 194, 102),
            //        Lambert = 0.7
            //    }));

            //objects.Add(new Sphere(new Vector(0, 5, 1), 0.2,
            //    new Material
            //    {
            //        Ambient = 0.0,
            //        Specular = 0.1,
            //        Color = Color.FromArgb(88, 123, 237),
            //        Lambert = 0.9
            //    }));

            //objects.Add(new Sphere(new Vector(4, 3, -1), 0.1,
            //    new Material
            //    {
            //        Ambient = 0.1,
            //        Specular = 0.2,
            //        Color = Color.FromArgb(155, 155, 155),
            //        Lambert = 0.7
            //    }));

            //objects.Add(new Sphere(new Vector(0, 1, -4), 2,
            //    new Material
            //    {
            //        Ambient = 0.1,
            //        Specular = 0.0,
            //        Color = Color.GreenYellow,
            //        Lambert = 0.7, Textured = true
            //    }));

            //int numSpheres = 10;
            //Random rand = new Random();
            //for (int i = 0; i < numSpheres; i++)
            //{
            //    double x = rand.NextDouble(-3, 3);
            //    double y = rand.NextDouble(-1, 1);
            //    double z = rand.NextDouble(-5, 2);

            //    double radius = rand.NextDouble(0.3, 1.3);

            //    objects.Add(new Sphere(new Vector(x, y, z), radius,
            //        new Material
            //        {
            //            Ambient = 0.1,
            //            Specular = 0.0,
            //            Color = Extensions.ColorFromHSV(rand.NextDouble() * 360.0, 0.7, 0.8),
            //            Lambert = 0.7,
            //            Textured = true
            //        }));
            //}

            //Random rand = new Random();
            //for (int x = 0; x < 5; x++)
            //{
            //    for (int y = 0; y < 5; y++)
            //    {
            //        for (int z = 0; z < 5; z++)
            //        {
            //            objects.Add(new Sphere(
            //                new Vector(x - 2, y, z - 2), 0.25, new Material
            //                {
            //                    Ambient = 0.1, Specular = 0, Color = Extensions.ColorFromHSV(rand.NextDouble() * 360.0, 0.7, 0.8), Lambert = 0.7
            //                }));
            //        }
            //    }
            //}

            //objects.Add(new Plane(new Vector(0,0,0), new Vector(0,-1,0), new Material { Ambient = 0.1, Specular = 0.0, Color = Color.FromArgb(50, 155, 30), Lambert = 0.7 }));
            //objects.Add(new Plane(new Vector(0, 0, 0), new Vector(0, 1, 0), new Material { Ambient = 0.1, Specular = 0.4, Color = Color.White, Lambert = 0.7, Textured = false }));

            backgroundColor = Color.White;
        }

        public Bitmap Render(int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            byte[] pixelData = new byte[bmpData.Stride * bmpData.Height];

            camera.SetResolution(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Ray pixelRay = camera.CastRay(x, y);

                    Color? pixelColor = Trace(pixelRay, 0);
                    Color colr = Color.Magenta;
                    if (pixelColor.HasValue)
                        colr = pixelColor.Value;
                    pixelData[y * bmpData.Stride + x * 3 + 0] = colr.B;
                    pixelData[y * bmpData.Stride + x * 3 + 1] = colr.G;
                    pixelData[y * bmpData.Stride + x * 3 + 2] = colr.R;
                }
            }

            Marshal.Copy(pixelData, 0, bmpData.Scan0, pixelData.Length);

            bmp.UnlockBits(bmpData);
            return bmp;
        }

        public void SetResolution(int width, int height)
        {
            camera.SetResolution(width, height);
        }

        public byte[] Render(int x, int y, int width, int height)
        { 
            byte[] pixels = new byte[width * 3 * height];

            int stride = width * 3;
            
            for (int xx = 0; xx < width; xx++)
            {
                for (int yy = 0; yy < height; yy++)
                {
                    Ray pixelRay = camera.CastRay(xx + x, yy + y);
                    Color? pixelColor = Trace(pixelRay, 0);
                    Color colr = Color.Magenta;
                    if (pixelColor.HasValue)
                        colr = pixelColor.Value;
                    pixels[(yy * stride + xx * 3) + 0] = colr.B;
                    pixels[(yy * stride + xx * 3) + 1] = colr.G;
                    pixels[(yy * stride + xx * 3) + 2] = colr.R;
                }
            }

            return pixels;
        }

        private Color? Trace(Ray ray, int depth)
        {
            if (depth > 3)
                return null;

            SceneIntersectionResult sir = IntersectScene(ray);
            if (!sir.Result.Intersects)
                return backgroundColor;

            Vector point = ray.PointAt(sir.Result.Distance);
            Vector normal = sir.Object.Normal(point);

            // lights!
            double lambertAmount = 0;
            foreach (Light l in lights)
            {
                if (!IsLightVisible(point, l))
                    continue;

                double contribution = Vector.DotProduct((l.Position - point).Normalized, normal);
                if (contribution > 0)
                    lambertAmount += contribution;
            }

            lambertAmount = Math.Min(1, lambertAmount);

            double specular = sir.Object.Material.Specular;
            Vector result = sir.Object.EvalMaterial(point, normal, lambertAmount);
            
            if (specular != 0.0)
            {
                Ray reflected = ray.Reflect(point, normal);
                reflected.Start += reflected.Direction * 0.01;

                Color? reflectColor = Trace(reflected, depth + 1);
                if (reflectColor.HasValue)
                    result += new Vector(reflectColor.Value) * specular;
            }
            return result.ToColor();
        }

        private SceneIntersectionResult IntersectScene(Ray ray)
        {
            IntersectionResult closestDist = new IntersectionResult(false, double.MaxValue);
            Object closestObj = null;

            foreach (Object o in objects)
            {
                IntersectionResult res = o.Intersects(ray);

                if (res.Intersects && res.Distance < closestDist.Distance)
                {
                    closestDist = res;
                    closestObj = o;
                }
            }

            return new SceneIntersectionResult(closestObj, closestDist);
        }

        private bool IsLightVisible(Vector point, Light light)
        {
            if (!light.IsVisibleFrom(point))
                return false;

            Ray ray = Ray.FromTo(point, light.Position);

            foreach (Object o in objects)
            {
                IntersectionResult r = o.Intersects(ray);
                if (r.Intersects && r.Distance > -0.0005)
                    return false;
            }
            return true;
        }

        public XmlDocument Serialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateElement("Scene");
            doc.AppendChild(root);

            // camera
            camera.Serialize(doc);

            // lights
            XmlNode lightsNode = doc.CreateElement("Lights");
            root.AppendChild(lightsNode);
            foreach (Light l in lights)
            {
                l.Serialize(doc);
            }

            // objects

            return doc;
        }

        public static Scene Parse(XmlNode doc)
        {
            Scene s = new Scene();
            Camera cam = Camera.Parse(doc["Camera"]);
            s.camera = cam;
            
            // lights

            // objects

            return s;
        }
    }
}