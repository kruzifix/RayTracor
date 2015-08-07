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
            camera = Camera.CreateLookAt(new Vector(0, 1.8, 10), new Vector(0, 3, 0), Math.PI / 2.0);
            lights = new List<Light>();
            lights.Add(new Light(new Vector(-30, -10, 20), Color.White, 1));
            objects = new List<Object>();
            objects.Add(new Sphere(new Vector(0, 3.5, -3), 3));
            backgroundColor = Color.White;
        }

        public Bitmap Render(int width, int height)
        { 
            // ray casting calculation

            Bitmap bmp = new Bitmap(width, height);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), 
                System.Drawing.Imaging.ImageLockMode.WriteOnly, 
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            byte[] pixelData = new byte[bmpData.Stride * bmpData.Height];

            // calc camera vectors

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                { 
                    // calc ray
                    Ray pixelRay = new Ray();

                    Color? pixelColor = Trace(pixelRay, 0);

                    pixelData[y * bmpData.Stride + x * 3 + 0] = pixelColor.Value.R;
                    pixelData[y * bmpData.Stride + x * 3 + 1] = pixelColor.Value.G;
                    pixelData[y * bmpData.Stride + x * 3 + 2] = pixelColor.Value.B;
                }
            }

            Marshal.Copy(pixelData, 0, bmpData.Scan0, pixelData.Length);

            bmp.UnlockBits(bmpData);
            return bmp;
        }

        private Color? Trace(Ray ray, int depth)
        {
            if (depth > 3) return null;

            SceneIntersectionResult res = IntersectScene(ray);
            if (!res.Distance.Intersects)
                return Color.White;

            // lights!
            
            // calculate surface material 
            return Color.Black;
        }

        private SceneIntersectionResult IntersectScene(Ray ray)
        {
            IntersectionResult closestDist = new IntersectionResult(false, 0.0);
            Object closestObj = null;

            foreach (Object o in objects)
            {
                IntersectionResult res = o.Intersects(ray);

                if (res.Intersects)
                {
                    if ((res.Distance < closestDist.Distance) || !closestDist.Intersects)
                    {
                        closestDist.Intersects = true;
                        closestDist.Distance = res.Distance;
                        closestObj = o;
                    }
                }
            }

            return new SceneIntersectionResult(closestObj, closestDist);
        }
    }
}
