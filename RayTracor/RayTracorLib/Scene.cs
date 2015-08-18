using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Xml;
using RayTracor.RayTracorLib.Lights;
using RayTracor.RayTracorLib.Objects;
using RayTracor.RayTracorLib.Tracing;
using RayTracor.RayTracorLib.Utility;
using RayTracor.RayTracorLib.Materials;

namespace RayTracor.RayTracorLib
{
    public class Scene
    {
        public Camera camera;
        public List<ILight> lights;
        public List<IObject> objects;

        //public event EventHandler<int> ProgressChanged;
        public IProgress<int> ProgressReport;

        Vector3 backgroundColor;

        public Scene()
        {
            //camera = Camera.CreateLookAt(new Vector(0, 1.8, 10), new Vector(0, 3, 0), new Vector(0,1,0), 45.0);
            //camera = Camera.CreateLookAt(new Vector(0, 3, 12), new Vector(0, 0, 0), new Vector(0, 1, 0), 45.0);
            camera = new Camera();
            lights = new List<ILight>();
            objects = new List<IObject>();

            //lights.Add(new Light(new Vector(-30, -10, 20), Color.White, 1));
            //lights.Add(new Light(new Vector(7, 10, 7), Color.White, 1));
            //lights.Add(new SpotLight(new Vector(0, 10, 0), Color.White, 1, new Vector(0, -1, 0), 20.0));
            //lights.Add(SpotLight.FromTo(new Vector(0, 10, 5), new Vector(0,0,0), Color.White, 1, 20.0));

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

            backgroundColor = new Vector3(255.0);
        }

        //private void OnProgressChanged(int p)
        //{
        //    if (ProgressChanged != null)
        //        ProgressChanged(this, p);
        //}

        public Bitmap Render(int width, int height)
        {
            return RenderFunc(width, height, (x, y) => { return Trace(camera.CastRay(x, y), 0); });
        }

        public Bitmap RenderDeterministicSuperSample(int width, int height, int samples)
        {
            return RenderFunc(width, height, (x, y) =>
            {
                Vector3 col = Vector3.Zero;
                double delta = 1.0 / samples;
                double offset = samples * 0.5 * delta;
                for (int i = 0; i < samples; i++)
                    for (int j = 0; j < samples; j++)
                        col += Trace(camera.CastRay(x + i * delta - offset, y + j * delta - offset), 0);
                col /= samples * samples;
                return col;
            });
        }

        //public Bitmap RenderStochasticSuperSample(int width, int height)
        //{
        //    int samples = 64;
        //    uint hash = (uint)this.GetHashCode();
        //    MwcRng.SetSeed(hash);
        //    return RenderFunc(width, height, (x, y) =>
        //    {
        //        Vector col = Vector.Zero;
        //        for (int i = 0; i < samples; i++)
        //                col += Trace(camera.CastRay(x + MwcRng.GetUniform() - 0.5, y + MwcRng.GetUniform() - 0.5), 0).ToVector();
        //        col /= samples;
        //        return col.ToColor();
        //    });
        //}

        private Bitmap RenderFunc(int width, int height, Func<int, int, Vector3> func)
        {
            Bitmap bmp;
            BitmapData data;
            byte[] pixels = CreateAndLockBitmap(width, height, PixelFormat.Format24bppRgb, out bmp, out data);

            camera.SetResolution(width, height);
            
            Parallel.For(0, height, (y) => 
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixelColor = func(x, y).ToColor();
                    pixels[y * data.Stride + x * 3 + 0] = pixelColor.B;
                    pixels[y * data.Stride + x * 3 + 1] = pixelColor.G;
                    pixels[y * data.Stride + x * 3 + 2] = pixelColor.R;
                }
                ProgressReport.Report(1);
            });

            CopyAndUnLock(bmp, data, pixels);

            return bmp;
        }

        public static byte[] CreateAndLockBitmap(int width, int height, PixelFormat pFormat, out Bitmap bmp, out BitmapData data)
        {
            bmp = new Bitmap(width, height);
            data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, pFormat);
            return new byte[data.Stride * data.Height];
        }

        public static void CopyAndUnLock(Bitmap bmp, BitmapData data, byte[] pixels)
        {
            Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);
            bmp.UnlockBits(data);
        }

        public Bitmap RenderDepthMap(int width, int height)
        {
            Bitmap bmp;
            BitmapData data;
            byte[] pixels = CreateAndLockBitmap(width, height, PixelFormat.Format32bppRgb, out bmp, out data);

            camera.SetResolution(width, height);

            Parallel.For(0, height, (y) =>
            {
                for (int x = 0; x < width; x++)
                {
                    Intersection res = IntersectScene(camera.CastRay(x, y));
                    float fdepth = -1;
                    if (res.Intersects)
                        fdepth = (float)res.Distance;

                    byte[] bdepth = BitConverter.GetBytes(fdepth);
                    int i = y * data.Stride + x * 4;
                    Array.Copy(bdepth, 0, pixels, i, 4);
                }
                ProgressReport.Report(1);
            });

            CopyAndUnLock(bmp, data, pixels);
            return bmp;
        }

        public Bitmap RenderGreyScaleDepthMap(int width, int height)
        {
            double maxDist = 20.0;

            return RenderFunc(width, height,  (x,y)=> {
                Intersection res = IntersectScene(camera.CastRay(x, y));
                if (!res.Intersects || res.Distance > maxDist)
                    return Vector3.Zero;
                int col = 255 - (int)(res.Distance / maxDist * 255.0);
                return new Vector3(col);
            });
        }

        public Bitmap RenderNormalMap(int width, int height)
        {
            return RenderFunc(width, height, (x, y) =>
            {
                Intersection res = IntersectScene(camera.CastRay(x, y));
                if (res.Intersects)
                    return (res.Normal * 127.0 + new Vector3(128.0));
                return Vector3.Zero;
            });
        }

        public Bitmap RenderAmbientOcclusion(int width, int height, int rays)
        {
            return RenderFunc(width, height, (x, y) =>
            {
                Ray ray = camera.CastRay(x, y);
                Intersection res = IntersectScene(ray);
                if (!res.Intersects)
                    return new Vector3(255.0);

                //MwcRng.SetSeed((uint)GetHashCode());
                
                //double theta = Math.Atan2(res.Normal.Y, res.Normal.X);
                //double phi = Math.Acos(res.Normal.Z);
                
                int notblocked = 0;
                int castedRays = 0;
                int tries = 0;
                while(castedRays < rays)
                {
                    Vector3 rand = new Vector3(MwcRng.GetUniformRange(-1, 1), MwcRng.GetUniformRange(-1, 1), MwcRng.GetUniformRange(-1, 1));

                    if (Vector3.DotProduct(rand, res.Normal) < 0.0)
                    {
                        if (tries++ > rays * 2)
                            break;
                        continue;
                    }

                    Ray newray = new Ray(res.Point, rand);
                    Intersection res2 = IntersectSceneExcept(newray, res.Object);
                    if (!res2.Intersects)
                        notblocked++;
                    castedRays++;
                    
                    //    double newtheta = theta + MwcRng.GetUniformRange(-Math.PI / 2.0, Math.PI / 2.0);
                    //    double newphi = phi + MwcRng.GetUniformRange(0, Math.PI * 2.0);

                    //    Vector newnormal = new Vector(Math.Cos(newtheta), Math.Sin(newtheta), Math.Cos(newphi));
                    //    Ray newray = new Ray(res.Point, newnormal);
                    //    if (!IntersectSceneExcept(newray, res.Object).Intersects)
                    //        notblocked++;
                }
                
                //rays = 1;
                //Ray newRay = new Ray(res.Point, res.Normal);
                //if (!IntersectSceneExcept(newRay, res.Object).Intersects)
                //if (!IntersectScene(newRay).Intersects)
                //        notblocked++;

                int col = (int)(notblocked * 255.0 / castedRays);
                return new Vector3(col);
            });
        }

        public void SetResolution(int width, int height)
        {
            camera.SetResolution(width, height);
        }
        
        private Vector3 Trace(Ray ray, int depth)
        {
            if (depth > 3)
                return Vector3.Zero;
            
            Intersection res = IntersectScene(ray);
            if (!res.Intersects)
                return backgroundColor;
            
            // lights!
            double lambertAmount = 0;
            foreach (ILight light in lights)
            {
                if (!IsLightVisible(res.Point, light))
                    continue;

                double contribution = 0;
                if (light is PointLight)
                    contribution = Vector3.DotProduct(((light as PointLight).Position - res.Point).Normalized, res.Normal);
                
                lambertAmount += contribution;
            }

            lambertAmount = Math.Min(1, lambertAmount);

            IObject obj = res.Object;
            Material mat = obj.Material;
            Vector3 objColor = obj.EvalMaterial(res);

            Vector3 resultColor = objColor * mat.Ambient + objColor * lambertAmount * mat.Lambert;
            
            //if (specular != 0.0)
            //{
            //    Ray reflected = ray.Reflect(res.Point, res.Normal);
            //    reflected.Start += reflected.Direction * 0.001;

            //    Color? reflectColor = Trace(reflected, depth + 1);
            //    if (reflectColor.HasValue)
            //        resultColor += new Vector(reflectColor.Value) * specular;
            //}
            return resultColor;
        }
        
        private Intersection IntersectScene(Ray ray)
        {
            Intersection closestIntersec = Intersection.False;
            closestIntersec.Distance = double.MaxValue;
            
            foreach (var o in objects)
            {
                Intersection res = o.Intersects(ray);

                if (res.Intersects && res.Distance < closestIntersec.Distance)
                    closestIntersec = res;
            }

            return closestIntersec;
        }

        private Intersection IntersectSceneExcept(Ray ray, params IObject[] objs)
        {
            Intersection closestIntersec = Intersection.False;
            closestIntersec.Distance = double.MaxValue;
            
            foreach (var o in objects.Except(objs))
            {
                Intersection res = o.Intersects(ray);

                if (res.Intersects && res.Distance < closestIntersec.Distance)
                    closestIntersec = res;
            }

            return closestIntersec;
        }

        private bool IsLightVisible(Vector3 point, ILight light)
        {
            if (light is PointLight)
            {
                PointLight plight = light as PointLight;
                return PointObstructed(point, plight.Position);
            }

            if (light is SpotLight)
            {
                SpotLight slight = light as SpotLight;
                if (!slight.PointLighted(point))
                    return false;

                return PointObstructed(point, slight.Position);
            }

            return false;
        }

        private bool PointObstructed(Vector3 from, Vector3 to)
        {
            Ray ray = Ray.FromTo(from, to);
            ray.Start += ray.Direction * 0.001;
            double dist = (to - from).Length;

            foreach (var o in objects)
            {
                Intersection res = o.Intersects(ray);
                if (res.Intersects && res.Distance > -0.0005 && res.Distance < dist)
                    return false;
            }
            return true;
        }

        public XmlDocument Serialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateElement("scene");
            doc.AppendChild(root);

            // background color
            root.AppendChild(backgroundColor.Serialize(doc, "backgroundcolor"));

            // camera
            camera.Serialize(doc);

            // lights
            XmlNode lightsNode = doc.CreateElement("lights");
            root.AppendChild(lightsNode);
            foreach (PointLight l in lights)
                l.Serialize(doc, lightsNode);

            // objects
            XmlNode objectsNode = doc.CreateElement("objects");
            root.AppendChild(objectsNode);
            foreach (IObject o in objects)
                o.Serialize(doc, objectsNode);

            return doc;
        }

        public static Scene Parse(XmlDocument doc)
        {
            XmlNode root = doc["scene"];
            Scene s = new Scene();
            Camera cam = Camera.Parse(root["camera"]);
            s.camera = cam;
            s.backgroundColor = root["backgroundcolor"].ParseColor().ToVector();

            // lights
            XmlNode lights = root.SelectSingleNode("//scene/lights");
            foreach (XmlNode li in lights.SelectNodes("pointlight"))
                s.lights.Add(PointLight.Parse(li));
            foreach (XmlNode sli in lights.SelectNodes("spotlight"))
                s.lights.Add(SpotLight.Parse(sli));

            // objects
            XmlNode objects = root.SelectSingleNode("//scene/objects");
            foreach (XmlNode n in objects.SelectNodes("sphere"))
                s.objects.Add(Sphere.Parse(n));
            foreach (XmlNode n in objects.SelectNodes("plane"))
                s.objects.Add(Plane.Parse(n));
            foreach (XmlNode n in objects.SelectNodes("triangle"))
                s.objects.Add(Triangle.Parse(n));
            foreach (XmlNode n in objects.SelectNodes("quad"))
                s.objects.Add(Quad.Parse(n));

            return s;
        }
    }
}