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
            scene.camera = Camera.CreateLookAt(new Vector(0, 3, 12), new Vector(0, 0, 0), new Vector(0, 1, 0), 60);
            scene.Serialize().Save("scene.xml");

            XmlDocument doc = new XmlDocument();
            doc.Load("scene.xml");
            scene = Scene.Parse(doc["Scene"]);
            
            renderControl.camera = scene.camera;
            renderControl.objects.AddRange(scene.objects);
            renderControl.lights.AddRange(scene.lights);

            for (int i = 0; i < 6; i++)
                cBoxTasks.Items.Add((int)Math.Pow(2, i));
            cBoxTasks.Text = cBoxTasks.Items[2].ToString();

            cBoxResolution.Items.AddRange(new object[] { "320x240", "640x360", "640x480", "960x540", "1024x768", "1280x720", "1920x1080", "2560x1440", "3840x2160" });
            cBoxResolution.Text = cBoxResolution.Items[0].ToString();
        }

        private void bRender_Click(object sender, EventArgs e)
        {
            bRender.Enabled = false;
            Stopwatch sw = Stopwatch.StartNew();
            Bitmap bmp = scene.Render(resolution.width, resolution.height);
            sw.Stop();
            Console.WriteLine("Render time: {0} ms", sw.ElapsedMilliseconds);

            int number = SaveBmp(bmp, sw.ElapsedMilliseconds, 1);

            FormShowRender fsr = new FormShowRender(bmp);
            fsr.Text = string.Format("Render {0}; Time: {1}ms", number, sw.ElapsedMilliseconds);
            fsr.Show();

            bRender.Enabled = true;
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
                        byte[] b = scene.Render(0, i * h, width, h);
                        Array.Copy(b, 0, pixels, 0 + i * h * data.Stride, b.Length);   
                    }, y);
            }
            Task.WaitAll(taskArray);
            
            Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);

            bmp.UnlockBits(data);
            sw.Stop();
            Console.WriteLine("Finished parallel render: {0}ms", sw.ElapsedMilliseconds);

            int number = SaveBmp(bmp, sw.ElapsedMilliseconds, tasks);

            this.UI(() => bRenderParallel.Enabled = true);

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
            bRenderParallel.Enabled = false;
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