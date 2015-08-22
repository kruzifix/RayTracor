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
using RayTracor.RayTracorLib.Utilities;
using RayTracor.RayTracorLib.Materials;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

namespace RayTracor.RayTracorLib
{
    public class Scene
    {
        #region 16 poisson samples
        static Vector2[] poissonSamples = new Vector2[] {
                new Vector2(-0.94201624, -0.39906216),
                new Vector2(0.94558609, -0.76890725),
                new Vector2(-0.094184101, -0.92938870),
                new Vector2(0.34495938, 0.29387760),
                new Vector2(-0.91588581, 0.45771432),
                new Vector2(-0.81544232, -0.87912464),
                new Vector2(-0.38277543, 0.27676845),
                new Vector2(0.97484398, 0.75648379),
                new Vector2(0.44323325, -0.97511554),
                new Vector2(0.53742981, -0.47373420),
                new Vector2(-0.26496911, -0.41893023),
                new Vector2(0.79197514, 0.19090188),
                new Vector2(-0.24188840, 0.99706507),
                new Vector2(-0.81409955, 0.91437590),
                new Vector2(0.19984126, 0.78641367),
                new Vector2(0.14383161, -0.14100790)
            };
        #endregion

        public Camera camera;
        public List<ILight> lights;
        public List<IObject> objects;
        public Dictionary<string, Material> materials;
        PoissonDisk2 pdisk_32, pdisk_64;
        
        public IProgress<int> ProgressReport;

        public Vector3 backgroundColor;

        public Scene()
        {
            camera = new Camera();
            lights = new List<ILight>();
            objects = new List<IObject>();
            materials = new Dictionary<string, Material>();

            pdisk_32 = new PoissonDisk2(0.15);
            pdisk_32.Generate();
            pdisk_32.Save("pdisk_32.bmp");
            pdisk_64 = new PoissonDisk2(0.103);
            pdisk_64.Generate();
            pdisk_64.Save("pdisk_64.bmp");
            //Console.WriteLine("Radius: {0}, Samples: {1}", pdisk_64.Radius, pdisk_64.Samples.Count);
            
            backgroundColor = new Vector3(255.0);
        }
        
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
        
        private Bitmap RenderFunc(int width, int height, Func<int, int, Vector3> func)
        {
            Bitmap bmp;
            BitmapData data;
            byte[] pixels = CreateAndLockBitmap(width, height, PixelFormat.Format24bppRgb, out bmp, out data);

            camera.SetResolution(width, height);

            Parallel.For(0, height, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, (y) =>
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
            double maxDist = 30.0;

            return RenderFunc(width, height, (x, y) =>
            {
                Intersection res = IntersectScene(camera.CastRay(x, y));
                if (!res.Intersects || res.Distance > maxDist)
                    return new Vector3(0);
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

                int col = (int)(255.0 * GetAmbientOcclusion(res, rays));
                return new Vector3(col);
            });
        }

        public Bitmap RenderWithAO(int width, int height)
        {
            return RenderFunc(width, height, (x, y) => {
                Ray ray = camera.CastRay(x, y);
                Intersection res = IntersectScene(ray);
                if (!res.Intersects)
                    return backgroundColor;
                Vector3 col = Trace(ray, 0);
                double ao = GetAmbientOcclusion(res, 128);

                double lambertAmount = EvalLights(res);
                
                IObject obj = res.Object;
                Material mat = materials[obj.Material];
                Vector3 objColor = obj.EvalMaterial(res, mat);
                
                return objColor * (mat.Ambient * ao + mat.Lambert * lambertAmount);
            });
        }

        private double GetAmbientOcclusion(Intersection res, int rays)
        {
            int notblocked = 0;
            for (int i = 0; i < rays; i++)
            {
                //Vector2 sample = pdisk_64[i];
                double theta = 2 * Math.PI * MwcRng.GetUniform();
                double phi = Math.Acos(MwcRng.GetUniform() * 2 - 1);

                Vector3 rand = new Vector3(
                    Math.Cos(theta) * Math.Sin(phi),
                    Math.Sin(theta) * Math.Sin(phi),
                    Math.Cos(phi)
                    );

                if (Vector3.DotProduct(rand, res.Normal) < 0.0)
                    rand.Negate();

                Ray newray = new Ray(res.Point, rand);
                Intersection res2 = IntersectSceneExcept(newray, res.Object);
                if (!res2.Intersects)
                    notblocked++;
            }

            return notblocked * 1.0 / rays;
        }

        public Bitmap RenderSSAO(int width, int height)
        {
            Intersection[] intersecmap = new Intersection[width * height];
            camera.SetResolution(width, height);

            Parallel.For(0, width, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, (x) =>
            {
                for (int y = 0; y < height; y++)
                {
                    intersecmap[x + y * width] = IntersectScene(camera.CastRay(x, y));
                }
            });

            Bitmap bmp;
            BitmapData data;
            byte[] pixels = CreateAndLockBitmap(width, height, PixelFormat.Format24bppRgb, out bmp, out data);
            
            Vector2 filterRadius = new Vector2(10.0 / width, 10.0 / height);
            double distanceThreshold = 1.0;

            Parallel.For(0, width, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, (x) =>
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 pos = intersecmap[x + y * width].Point;
                    Vector3 normal = intersecmap[x + y * width].Normal;

                    double ambientOcclusion = 0;

                    for (int i = 0; i < poissonSamples.Length; i++)
                    {
                        Vector2 coord = new Vector2(x, y) + poissonSamples[i] * filterRadius;

                        int coordX = Math.Floor(coord.X).ClampToInt(0, width);
                        int coordY = Math.Floor(coord.Y).ClampToInt(0, height);
                        
                        double depth = intersecmap[(coordX + 0) + (coordY + 0) * width].Distance;
                        int depthSamples = 1;
                        if (coordX < width - 1)
                        {
                            depth += intersecmap[(coordX + 1) + (coordY + 0) * width].Distance;
                            depthSamples++;
                        }
                        if (coordY < height - 1)
                        {
                            depth += intersecmap[(coordX + 0) + (coordY + 1) * width].Distance;
                            depthSamples++;
                        }
                        if (coordX < width - 1 && coordY < height - 1)
                        {
                            depth += intersecmap[(coordX + 1) + (coordY + 1) * width].Distance;
                            depthSamples++;
                        }
                        depth /= depthSamples;

                        Vector3 samplePos = camera.CalcPos(coord.X, coord.Y, depth);
                        Vector3 sampleDir = (samplePos - pos).Normalized;

                        double dot = Math.Max(Vector3.DotProduct(sampleDir, normal), 0);
                        double dist = (samplePos - pos).Length;

                        double a = 1.0 - Utility.SmoothStep(distanceThreshold, distanceThreshold * 2.0, dist);

                        ambientOcclusion += a * dot;
                    }

                    double ao = 255 * (1.0 - ambientOcclusion / poissonSamples.Length);
                    byte col = (byte)ao.ClampToInt(0, 255);
                    pixels[y * data.Stride + x * 3 + 0] = col;
                    pixels[y * data.Stride + x * 3 + 1] = col;
                    pixels[y * data.Stride + x * 3 + 2] = col;
                }
                ProgressReport.Report(1);
            });

            CopyAndUnLock(bmp, data, pixels);
            
            return bmp;
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
            double lambertAmount = EvalLights(res);

            IObject obj = res.Object;
            Material mat = materials[obj.Material];
            Vector3 objColor = obj.EvalMaterial(res, mat);

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

        private double EvalLights(Intersection res)
        {
            double lambertAmount = 0;
            foreach (var light in lights)
            {
                double contribution = 0;
                if (light is PointLight)
                {
                    if (!IsLightVisible(res.Point, light))
                        continue;
                    contribution = Vector3.DotProduct(((light as PointLight).Position - res.Point).Normalized, res.Normal);
                }
                if (light is AreaLight)
                {
                    AreaLight alight = light as AreaLight;
                    double lambert = Vector3.DotProduct((alight.Position - res.Point).Normalized, res.Normal).Clamp(0, 1);

                    int tests = pdisk_32.Samples.Count;
                    int obstructed = 0;
                    for (int i = 0; i < tests; i++)
                    {
                        Vector2 uv = pdisk_32[i] * 2.0 - Vector2.One;

                        Vector3 diff = res.Point - alight.Position;
                        Vector3 normDiff = diff.Normalized;

                        Vector3 right = Vector3.CrossProduct(normDiff, camera.GlobalUp).Normalized;
                        Vector3 up = Vector3.CrossProduct(right, normDiff).Normalized;
                        right *= uv.X * alight.Size;
                        up *= uv.Y * alight.Size;

                        Vector3 apoint = alight.Position + right + up;

                        if (PointObstructed(res.Point, apoint))
                            obstructed++;
                    }

                    double visibility = (tests - obstructed) / (double)tests;

                    contribution = visibility * lambert;
                }

                lambertAmount += contribution * light.Strength;
            }
            
            return Math.Min(1, lambertAmount);
        }

        private Intersection IntersectScene(Ray ray)
        {
            Intersection closestIntersec = Intersection.False;
            closestIntersec.Distance = double.MaxValue;

            foreach (var o in objects)
            {
                Intersection res = o.Intersects(ray);

                if (res.Intersects && res.Distance > 0.0005 && res.Distance < closestIntersec.Distance)
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

                if (res.Intersects && res.Distance > 0.0005 && res.Distance < closestIntersec.Distance)
                    closestIntersec = res;
            }

            return closestIntersec;
        }

        private bool IsLightVisible(Vector3 point, ILight light)
        {
            if (light is PointLight)
            {
                PointLight plight = light as PointLight;
                return !PointObstructed(point, plight.Position);
            }

            if (light is SpotLight)
            {
                SpotLight slight = light as SpotLight;
                if (!slight.PointLighted(point))
                    return false;

                return !PointObstructed(point, slight.Position);
            }

            return false;
        }

        private bool PointObstructed(Vector3 from, Vector3 to)
        {
            Ray ray = Ray.FromTo(from, to);
            double dist = (to - from).Length;

            foreach (var o in objects)
            {
                Intersection res = o.Intersects(ray);
                if (res.Intersects && res.Distance > 0.0005 && res.Distance < dist)
                    return true;
            }
            return false;
        }
        
        public static Scene ParseJson(string path)
        {
            JObject root = JObject.Parse(File.ReadAllText(path));

            Scene scene = new Scene();

            scene.camera = Camera.FromJToken(root["camera"]);
            if (root["bgcolor"] == null)
                throw new Exception("Scene: 'bgcolor' not defined.");
            scene.backgroundColor = Utility.ColorFromHexString(root["bgcolor"].ToString()).ToVector();

            foreach(JToken tok in root["lights"].Children())
                scene.lights.Add(ILight.ParseJToken(tok));
            foreach(JToken tok in root["materials"].Children())
            {
                Material m = Material.FromJToken(tok);
                if (scene.materials.ContainsKey(m.Name))
                    throw new Exception(string.Format("Materials: 'name' has to be unique. Duplicate: '{0}'", m.Name));
                scene.materials.Add(m.Name, m);
            }

            foreach(JToken tok in root["objects"].Children())
            {
                IObject obj = IObject.ParseJToken(tok);
                if (!scene.materials.ContainsKey(obj.Material))
                    throw new Exception(string.Format("Objects: Unknown Material '{0}'", obj.Material));
                scene.objects.Add(obj);
            }
            
            return scene;
        }
    }
}