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

namespace RayTracor
{
    public partial class FormMain : Form
    {
        Scene scene;
        
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

            renderControl1.camera = scene.camera;
            renderControl1.objects.AddRange(scene.objects);
            renderControl1.lights.AddRange(scene.lights);
        }

        private void bRender_Click(object sender, EventArgs e)
        {
            bRender.Enabled = false;
            Stopwatch sw = Stopwatch.StartNew();
            Bitmap bmp = scene.Render(320, 240);
            sw.Stop();
            Console.WriteLine("Render time: {0} ms", sw.ElapsedMilliseconds);
            
            if (!File.Exists("renders.txt"))
                File.WriteAllText("renders.txt", "0");
            string number = File.ReadAllText("renders.txt");
            int num = int.Parse(number);
            string filename = string.Format("{0:000}_render_{1}.bmp", num, sw.ElapsedMilliseconds);
            bmp.Save(filename);
            File.WriteAllText("renders.txt", (num + 1).ToString());

            FormShowRender fsr = new FormShowRender(bmp);
            fsr.Text = string.Format("Render {0}; Time: {1}ms", number, sw.ElapsedMilliseconds);
            fsr.Show();

            bRender.Enabled = true;
        }
    }
}
