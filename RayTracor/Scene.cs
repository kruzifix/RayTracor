using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RayTracor.RayTracor
{
    public class Scene
    {
        // Camera
        // Lights
        // Objects

        Camera camera;
        List<Light> lights;
        List<Object> objects;

        Color backgroundColor;

        public Scene()
        {
            camera = Camera.CreateLookAt(new Vector(0, 1.8, 10), new Vector(0, 3, 0), new Vector(0,1,0), 45.0 * Math.PI / 180.0);
            lights = new List<Light>();
            objects = new List<Object>();

            lights.Add(new Light(new Vector(-30, -10, 20), Color.White, 1));

            objects.Add(new Sphere(new Vector(0, 3.5, -3), 3, 
                new Material { Ambient = 0.1, 
                               Specular = 0.2, 
                               Color = Color.FromArgb(155, 200, 155), 
                               Lambert = 0.7 }));

            objects.Add(new Sphere(new Vector(-4, 2, -1), 0.2,
                new Material
                {
                    Ambient = 0.0,
                    Specular = 0.1,
                    Color = Color.FromArgb(200, 155, 155),
                    Lambert = 0.9
                }));

            objects.Add(new Sphere(new Vector(4, 3, -1), 0.1,
                new Material
                {
                    Ambient = 0.1,
                    Specular = 0.2,
                    Color = Color.FromArgb(255, 255, 255),
                    Lambert = 0.7
                }));

            backgroundColor = Color.White;
        }

        public Bitmap Render(int width, int height)
        { 
            Bitmap bmp = new Bitmap(width, height);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), 
                System.Drawing.Imaging.ImageLockMode.WriteOnly, 
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            byte[] pixelData = new byte[bmpData.Stride * bmpData.Height];
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                { 
                    // calc ray
                    Ray pixelRay = camera.CastRay(x, y, width, height);

                    if (x == 0 && y == 0)
                        Console.WriteLine(pixelRay);
                    if (x == width - 1 && y == height - 1)
                        Console.WriteLine(pixelRay);

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
            if (depth > 3) return null;

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

            Vector col = new Vector(sir.Object.Material.Color);
            double lambert = sir.Object.Material.Lambert;
            double ambient = sir.Object.Material.Ambient;
            double specular = sir.Object.Material.Specular;
            Vector result = col * lambertAmount * lambert + col * ambient;
            if (specular != 0.0)
            {
                Vector a = normal * Vector.DotProduct(point, normal);
                Ray reflected = new Ray(point, a * 0.5 - point);

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
                if (r.Intersects && r.Distance > -0.005)
                    return false;
            }
            return true;
        }
    }
}
