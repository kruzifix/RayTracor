using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RayTracor.RayTracorLib;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml;

namespace RayTracor
{
    public partial class FormMain : Form
    {
        class Resolution { public int width; public int height; }

        Scene scene;
        Resolution resolution;
        
        public FormMain()
        {
            InitializeComponent();
            this.Load += FormMain_Load;
        }

        void FormMain_Load(object sender, EventArgs e)
        {
            //Sphere sphere = new Sphere(new Vector(0, 0, 0), 1, null);
            //Ray ray = Ray.FromTo(new Vector(3, 0, 0.5), new Vector(0, 0, 0.25));
            //Console.WriteLine("Ray: {0}", ray);

            //IntersectionResult res = sphere.Intersects(ray);
            //if (res.Intersects)
            //{
            //    Vector point = ray.PointAt(res.Distance);
            //    Vector normal = sphere.Normal(point);
            //    Vector a = normal * Vector.DotProduct(point, normal);
            //    Ray reflected = ray.Reflect(point, normal);
            //    Vector p = reflected.PointAt(res.Distance);

            //    Console.WriteLine("Intersection!\r\nDistance: {0}\r\nPoint: {1}\r\nNormal: {2}\r\nReflected Ray: {3}\r\nReflected Point: {4}", res.Distance, point, normal, reflected, p);
            //}

            scene = new Scene();
            //scene.camera = Camera.CreateLookAt(new Vector(0, 3, 12), new Vector(0, 0, 0), new Vector(0, 1, 0), 60);
            //scene.lights.Add(new Light(new Vector(7, 10, 7), Color.White, 1));
            //scene.lights.Add(SpotLight.FromTo(new Vector(0, 10, 5), new Vector(0,0,0), Color.White, 1, 20.0));

            //scene.objects.Add(new Sphere(new Vector(0, 3.5, -3), 3,
            //    new Material
            //    {
            //        Ambient = 0.1,
            //        Specular = 0.2,
            //        Color = Color.FromArgb(222, 194, 102),
            //        Lambert = 0.7
            //    }));

            //scene.objects.Add(new Sphere(new Vector(0, 5, 1), 0.2,
            //    new Material
            //    {
            //        Ambient = 0.0,
            //        Specular = 0.1,
            //        Color = Color.FromArgb(88, 123, 237),
            //        Lambert = 0.9
            //    }));

            //scene.objects.Add(new Sphere(new Vector(4, 3, -1), 0.1,
            //    new Material
            //    {
            //        Ambient = 0.1,
            //        Specular = 0.2,
            //        Color = Color.FromArgb(155, 155, 155),
            //        Lambert = 0.7
            //    }));

            //scene.objects.Add(new Sphere(new Vector(0, 1, -4), 2,
            //    new Material
            //    {
            //        Ambient = 0.1,
            //        Specular = 0.0,
            //        Color = Color.GreenYellow,
            //        Lambert = 0.7,
            //        Textured = true
            //    }));
            
            UpdateRenderControl();

            for (int i = 0; i < 6; i++)
                cBoxTasks.Items.Add((int)Math.Pow(2, i));
            cBoxTasks.Text = cBoxTasks.Items[2].ToString();

            cBoxResolution.Items.AddRange(new object[] { "320x240", "640x360", "640x480", "960x540", "1024x768", "1280x720", "1920x1080", "2560x1440", "3840x2160" });
            cBoxResolution.Text = cBoxResolution.Items[0].ToString();
        }

        private void UpdateRenderControl()
        {
            renderControl.camera = scene.camera;
            renderControl.objects.Clear();
            renderControl.objects.AddRange(scene.objects);
            renderControl.lights.Clear();
            renderControl.lights.AddRange(scene.lights);
            renderControl.Invalidate();
        }

        private void bRender_Click(object sender, EventArgs e)
        {
            SetRenderButtons(false);
            Stopwatch sw = Stopwatch.StartNew();
            Bitmap bmp = scene.Render(resolution.width, resolution.height);
            sw.Stop();
            Console.WriteLine("Render time: {0} ms", sw.ElapsedMilliseconds);

            int number = SaveBmp(bmp, sw.ElapsedMilliseconds, 1);

            FormShowRender fsr = new FormShowRender(bmp);
            fsr.Text = string.Format("Render {0}; Time: {1}ms", number, sw.ElapsedMilliseconds);
            fsr.Show();
            SetRenderButtons(true);
        }

        private void RenderParallel(int width, int height, int tasks)
        {
            Stopwatch sw = Stopwatch.StartNew();

            int h = height / tasks;
            
            scene.SetResolution(width, height);

            Bitmap bmp = new Bitmap(width, height);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            byte[] pixels = new byte[data.Stride * height];

            Task[] taskArray = new Task[tasks];
            for(int y = 0; y < tasks; y++)
            {
                taskArray[y] = Task.Factory.StartNew((x) =>
                    {
                        int i = (int)x;
                        byte[] b = scene.RenderSuperSample(0, i * h, width, h);
                        Array.Copy(b, 0, pixels, 0 + i * h * data.Stride, b.Length);   
                    }, y);
            }
            Task.WaitAll(taskArray);
            
            Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);

            bmp.UnlockBits(data);
            sw.Stop();
            Console.WriteLine("Finished parallel render: {0}ms", sw.ElapsedMilliseconds);

            int number = SaveBmp(bmp, sw.ElapsedMilliseconds, tasks);

            this.UI(() => SetRenderButtons(true));

            this.UI(() =>
            {
                FormShowRender fsr = new FormShowRender(bmp);
                fsr.Text = string.Format("Render {0}; Time: {1}ms; Tasks: {2}", number, sw.ElapsedMilliseconds, tasks);
                fsr.Show();
            });
        }

        private int SaveBmp(Bitmap bmp, long ms, int tasks)
        {
            if (!File.Exists("renders.txt"))
                File.WriteAllText("renders.txt", "0");
            string number = File.ReadAllText("renders.txt");
            int num = int.Parse(number);
            string filename = string.Format("{0:000}_render_{1}ms_{2}tasks.bmp", num, ms, tasks);
            bmp.Save(filename);
            File.WriteAllText("renders.txt", (num + 1).ToString());
            return num;
        }

        private void bRenderParallel_Click(object sender, EventArgs e)
        {
            int tasks = (int)cBoxTasks.SelectedItem;
            SetRenderButtons(false);
            Thread t = new Thread(new ThreadStart(() => RenderParallel(resolution.width, resolution.height, tasks)));
            t.Start();
        }

        private bool ParseResolution(out Resolution res)
        {
            res = null;
            if (string.IsNullOrWhiteSpace(cBoxResolution.Text))
                return false;
            string[] tokens = cBoxResolution.Text.Split('x');
            if (tokens.Length != 2)
                return false;

            int w;
            if (!int.TryParse(tokens[0], out w))
                return false;
            int h;
            if (!int.TryParse(tokens[1], out h))
                return false;
            res = new Resolution { width = w, height = h };
            return true;
        }

        private void cBoxResolution_TextUpdate(object sender, EventArgs e)
        {
            bool b = ParseResolution(out resolution);
            bRender.Enabled = b;
            bRenderParallel.Enabled = b;
            if (b)
                cBoxResolution.BackColor = SystemColors.Window;
            else
                cBoxResolution.BackColor = Color.FromArgb(0xFF, 0xCC, 0xCC);
        }

        private void renderControl_Click(object sender, EventArgs e)
        {
            renderControl.Focus();
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            scene.Serialize().Save("scene.xml");
        }

        private void bLoad_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("scene.xml");
            scene = Scene.Parse(doc["scene"]);

            UpdateRenderControl();
        }

        private void bDepthMap_Click(object sender, EventArgs e)
        {
            SetRenderButtons(false);

            Stopwatch sw = Stopwatch.StartNew();
            Bitmap bmp = scene.RenderDepthMap(resolution.width, resolution.height);
            sw.Stop();
            Console.WriteLine("Render time: {0} ms", sw.ElapsedMilliseconds);

            int number = SaveBmp(bmp, sw.ElapsedMilliseconds, 1);

            FormShowRender fsr = new FormShowRender(bmp);
            fsr.Text = string.Format("Render {0}; Time: {1}ms", number, sw.ElapsedMilliseconds);
            fsr.Show();

            SetRenderButtons(true);
        }

        private void SetRenderButtons(bool b)
        {
            bRender.Enabled = b;
            bDepthMap.Enabled = b;
            bRenderParallel.Enabled = b;
        }

        private void bNormalMap_Click(object sender, EventArgs e)
        {
            SetRenderButtons(false);

            Stopwatch sw = Stopwatch.StartNew();
            Bitmap bmp = scene.RenderNormalMap(resolution.width, resolution.height);
            sw.Stop();
            Console.WriteLine("Render time: {0} ms", sw.ElapsedMilliseconds);

            int number = SaveBmp(bmp, sw.ElapsedMilliseconds, 1);

            FormShowRender fsr = new FormShowRender(bmp);
            fsr.Text = string.Format("Render {0}; Time: {1}ms", number, sw.ElapsedMilliseconds);
            fsr.Show();

            SetRenderButtons(true);
        }
    }

    public static class FormExtensions
    {
        public static void UI(this Form f, Action a)
        { 
            if (f.IsHandleCreated)
                f.Invoke(a);
        }
    }
}