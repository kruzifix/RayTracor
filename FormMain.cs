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
        class RenderSettings { public int width; public int height; public int tasks; }

        Scene scene;
        RenderSettings settings = new RenderSettings();
        
        public FormMain()
        {
            InitializeComponent();
            this.Load += FormMain_Load;
        }

        void FormMain_Load(object sender, EventArgs e)
        {
            scene = new Scene();
            
            UpdateRenderControl();

            for (int i = 0; i < 6; i++)
                cBoxTasks.Items.Add((int)Math.Pow(2, i));
            cBoxTasks.Text = cBoxTasks.Items[2].ToString();

            cBoxResolution.Items.AddRange(new object[] { "320x240", "640x360", "640x480", "960x540", "1024x768", "1280x720", "1920x1080", "2560x1440", "3840x2160" });
            cBoxResolution.Text = cBoxResolution.Items[0].ToString();

            ParseResolution();
            settings.tasks = (int)cBoxTasks.SelectedItem;
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
            Render(() => scene.RenderAmbientOcclusion(settings.width, settings.height, 16), 1);
        }

        private void RenderParallel()
        {
            int tasks = settings.tasks;
            int width = settings.width;
            int height = settings.height;

            Render(() => {
                int h = height / tasks;

                scene.SetResolution(width, height);

                Bitmap bmp = new Bitmap(width, height);
                BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                byte[] pixels = new byte[data.Stride * height];

                Task[] taskArray = new Task[tasks];
                for (int y = 0; y < tasks; y++)
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
                return bmp;
            }, tasks);
        }

        private int SaveBmp(Bitmap bmp, long ms, int tasks)
        {
            if (!File.Exists("renders.txt"))
                File.WriteAllText("renders.txt", "0");
            string number = File.ReadAllText("renders.txt");
            int num = int.Parse(number);
            string filename = string.Format("{0:000}_render_{1}ms_{2}task{3}.bmp", num, ms, tasks, tasks > 1 ? "s" : "");
            bmp.Save(filename);
            File.WriteAllText("renders.txt", (num + 1).ToString());
            return num;
        }

        private void bRenderParallel_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(RenderParallel);
            t.Start();
        }

        private bool ParseResolution()
        {
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
            settings.width = w;
            settings.height = h;
            return true;
        }

        private void cBoxResolution_TextUpdate(object sender, EventArgs e)
        {
            bool b = ParseResolution();
            SetRenderButtons(b);
            cBoxResolution.BackColor = b ? SystemColors.Window : Color.FromArgb(0xFF, 0xCC, 0xCC);
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
            doc.Load("AO_test.xml");
            scene = Scene.Parse(doc["scene"]);

            UpdateRenderControl();
        }

        private void bDepthMap_Click(object sender, EventArgs e)
        {
            Render(() => scene.RenderDepthMap(settings.width, settings.height), 1);
        }

        private void SetRenderButtons(bool b)
        {
            this.UI(() =>
            {
                bRender.Enabled = b;
                bRenderParallel.Enabled = b;
                bDepthMap.Enabled = b;
                bNormalMap.Enabled = b;
            });
        }

        private void bNormalMap_Click(object sender, EventArgs e)
        {
            Render(() => scene.RenderNormalMap(settings.width, settings.height), 1);
        }

        private void Render(Func<Bitmap> func, int tasks)
        {
            SetRenderButtons(false);

            Stopwatch sw = Stopwatch.StartNew();
            Bitmap bmp = func();
            sw.Stop();

            int number = SaveBmp(bmp, sw.ElapsedMilliseconds, tasks);
            this.UI(() =>
            {
                FormShowRender fsr = new FormShowRender(bmp);
                fsr.Text = string.Format("Render {0}; Time: {1}ms", number, sw.ElapsedMilliseconds);
                fsr.Show();
            });
            SetRenderButtons(true);
        }

        private void cBoxTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.tasks = (int)cBoxTasks.SelectedItem;
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