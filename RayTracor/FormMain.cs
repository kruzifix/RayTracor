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
        enum SuperSampleMode { None, Deterministic, Stochastic }
        class RenderSettings { public int width; public int height; public int samples; public SuperSampleMode mode; }

        Scene scene;
        RenderSettings settings = new RenderSettings();

        int[] samples = new int[] { 2, 3, 4, 5 };
        SuperSampleMode[] modes = new SuperSampleMode[] { SuperSampleMode.None, SuperSampleMode.Deterministic, SuperSampleMode.Stochastic };

        public FormMain()
        {
            InitializeComponent();
            this.Load += FormMain_Load;
        }

        void FormMain_Load(object sender, EventArgs e)
        {
            scene = new Scene();
            scene.ProgressReport = new Progress<int>((i) => progBar.Increment(1));

            UpdateRenderControl();

            cBoxResolution.Items.AddRange(new object[] { "320x240", "640x360", "640x480", "960x540", "1024x768", "1280x720", "1920x1080", "2560x1440", "3840x2160" });
            cBoxResolution.Text = cBoxResolution.Items[0].ToString();

            cBoxSuSas.Items.AddRange(samples.Select<int, object>((i) => "x" + i).ToArray());
            cBoxSuSas.Text = cBoxSuSas.Items[0].ToString();
            settings.samples = samples[0];

            cBoxSuSaMode.Items.AddRange(modes.Select<SuperSampleMode, object>((s) => s.ToString()).ToArray());
            cBoxSuSaMode.Text = cBoxSuSaMode.Items[0].ToString();
            settings.mode = modes[0];

            ParseResolution();
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

        private int SaveBmp(Bitmap bmp, long ms, string type)
        {
            if (!Directory.Exists("renders"))
                Directory.CreateDirectory("renders");
            if (!File.Exists("renders/renders.txt"))
                File.WriteAllText("renders/renders.txt", "0");
            string number = File.ReadAllText("renders/renders.txt");
            int num = int.Parse(number);
            string filename = string.Format("renders/{0:000}_render_{1}ms_{2}.bmp", num, ms, type);
            bmp.Save(filename);
            File.WriteAllText("renders/renders.txt", (num + 1).ToString());
            return num;
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

        private void SetRenderButtons(bool b)
        {
            bRender.Enabled = b;
            bDepthMap.Enabled = b;
            bNormalMap.Enabled = b;
            bAO.Enabled = b;

            bSave.Enabled = b;
            bLoad.Enabled = b;
        }

        private async void Render(string type, Func<Bitmap> func)
        {
            SetRenderButtons(false);
            progBar.Value = 0;
            progBar.Maximum = settings.height;

            Stopwatch sw = null;
            Bitmap bmp = null;
            await Task.Factory.StartNew(() =>
            {
                sw = Stopwatch.StartNew();
                bmp = func();
                sw.Stop();
            }, TaskCreationOptions.LongRunning);
            int number = SaveBmp(bmp, sw.ElapsedMilliseconds, type);
            FormShowRender fsr = new FormShowRender(bmp);
            fsr.Text = string.Format("Render {0}; Time: {1}ms", number, sw.ElapsedMilliseconds);
            fsr.Show();
            SetRenderButtons(true);
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
            //scene.Serialize().Save("scene.xml");
        }

        private void bLoad_Click(object sender, EventArgs e)
        {
            //Stopwatch sw = Stopwatch.StartNew();
            XmlDocument doc = new XmlDocument();
            doc.Load("scenes/AO_test.xml");
            scene = Scene.Parse(doc);
            scene.ProgressReport = new Progress<int>((i) => progBar.Increment(1));

            //sw.Stop();
            //Console.WriteLine("scene loaded in {0}ms.", sw.ElapsedMilliseconds);

            UpdateRenderControl();
        }

        private void bRender_Click(object sender, EventArgs e)
        {
            switch(settings.mode)
            {
                case SuperSampleMode.None:
                    Render("NoSS", () => scene.Render(settings.width, settings.height));
                    break;
                case SuperSampleMode.Deterministic:
                    Render(string.Format("DetSSx{0}", settings.samples), () => scene.RenderDeterministicSuperSample(settings.width, settings.height, settings.samples));
                    break;
                case SuperSampleMode.Stochastic:
                    //Render(string.Format("StoSSx{0}", settings.samples), () => scene.Render(settings.width, settings.height));
                    break;
            }
        }

        private void bDepthMap_Click(object sender, EventArgs e)
        {
            Render("depthmap", () => scene.RenderGreyScaleDepthMap(settings.width, settings.height));
        }

        private void bNormalMap_Click(object sender, EventArgs e)
        {
            Render("normalmap", () => scene.RenderNormalMap(settings.width, settings.height));
        }

        private void bAO_Click(object sender, EventArgs e)
        {
            Render("AO", () => scene.RenderAmbientOcclusion(settings.width, settings.height, 512));
        }

        private void cBoxSuSas_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.samples = samples[cBoxSuSas.SelectedIndex];
        }

        private void cBoxSuSaMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.mode = modes[cBoxSuSaMode.SelectedIndex];
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