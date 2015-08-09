using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

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
            //camera = Camera.CreateLookAt(new Vector(0, 1.8, 10), new Vector(0, 3, 0), new Vector(0,1,0), 45.0 * Math.PI / 180.0);
            camera = Camera.CreateLookAt(new Vector(0, 3, 12), new Vector(0, 2, 0), new Vector(0, 1, 0), 45.0 * Math.PI / 180.0);
            lights = new List<Light>();
            objects = new List<Object>();

            //lights.Add(new Light(new Vector(-30, -10, 20), Color.White, 1));
            lights.Add(new Light(new Vector(7, 10, 7), Color.White, 1));

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

            //objects.Add(new Sphere(new Vector(0, 0, 0), 2,
            //    new Material
            //    {
            //        Ambient = 0.1,
            //        Specular = 0.0,
            //        Color = Color.FromArgb(222, 194, 102),
            //        Lambert = 0.7
            //    }));

            //int numSpheres = 10;
            //Random rand = new Random();
            //for (int i = 0; i < numSpheres; i++)
            //{
            //    double x = rand.NextDouble(-3, 3);
            //    double y = rand.NextDouble(-2, 2);
            //    double z = rand.NextDouble(-5, 2);

            //    double radius = rand.NextDouble(0.3, 1.5);

            //    objects.Add(new Sphere(new Vector(x, y, z), radius,
            //        new Material
            //        {
            //            Ambient = 0.1,
            //            Specular = 0.0,
            //            Color = Extensions.ColorFromHSV(rand.NextDouble() * 360.0, 0.7, 0.8),
            //            Lambert = 0.7
            //        }));
            //}

            Random rand = new Random();
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    for (int z = 0; z < 5; z++)
                    {
                        objects.Add(new Sphere(
                            new Vector(x - 2, y, z - 2), 0.25, new Material
                            {
                                Ambient = 0.1, Specular = 0, Color = Extensions.ColorFromHSV(rand.NextDouble() * 360.0, 0.7, 0.8), Lambert = 0.7
                            }));
                    }
                }
            }

            //objects.Add(new Plane(new Vector(0,0,0), new Vector(0,-1,0), new Material { Ambient = 0.1, Specular = 0.0, Color = Color.FromArgb(50, 155, 30), Lambert = 0.7 }));
            objects.Add(new Plane(new Vector(0, -2, 0), new Vector(0, 1, 0), new Material { Ambient = 0.1, Specular = 0.0, Color = Color.White, Lambert = 0.7 }));

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

            //Vector col = new Vector(sir.Object.Material.Color);
            //double lambert = sir.Object.Material.Lambert;
            //double ambient = sir.Object.Material.Ambient;
            double specular = sir.Object.Material.Specular;
            //Vector result = col * lambertAmount * lambert + col * ambient;
            Vector result = sir.Object.EvalMaterial(point, lambertAmount);
            if (specular != 0.0)
            {
                Ray reflected = ray.Reflect(point, normal);

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
            Ray ray = Ray.FromTo(point, light.Position);

            foreach (Object o in objects)
            {
                IntersectionResult r = o.Intersects(ray);
                if (r.Intersects && r.Distance > -0.0005)
                    return false;
            }
            return true;
        }
    }
}
