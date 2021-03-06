﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RayTracor.RayTracorLib;
using RayTracor.RayTracorLib.Tracing;
using RayTracor.RayTracorLib.Lights;
using RayTracor.RayTracorLib.Objects;
using RayTracor.RayTracorLib.Utilities;
using RayTracor.RayTracorLib.Materials;

namespace RayTracor
{
    public partial class RenderControl : UserControl
    {
        public Camera camera;
        public List<Ray> rays = new List<Ray>();
        public List<IObject> objects = new List<IObject>();
        public List<ILight> lights = new List<ILight>();
        public Dictionary<string, Material> materials = new Dictionary<string, Material>();

        double areaSize = 15.0;
        Vector2 offset = new Vector2();
        Vector2 scale = new Vector2();

        Graphics g;

        bool mousedown = false;
        Vector2 lastMouse = null;

        public RenderControl()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            offset = PixelToVector(Width / 2, Height / 2);

            RecalcScale();
        }

        protected override void OnResize(EventArgs e)
        {
            RecalcScale();
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;
            g.Clear(Color.White);
            DrawGrid();
            foreach (Ray r in rays)
                DrawRay(Color.Red, r);
            foreach (var o in objects)
                DrawObject(o);
            foreach (ILight l in lights)
                DrawLight(l);
            if (camera != null)
                DrawCamera();
            DrawLine(Pens.Black, -0.5, 0, 0.5, 0);
            DrawLine(Pens.Black, 0, -0.5, 0, 0.5);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mousedown = true;
                lastMouse = new Vector2(e.X, e.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            mousedown = false;
            lastMouse = null;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (mousedown && lastMouse != null)
            {
                Vector2 mouse = new Vector2(e.X, e.Y);
                Vector2 diff = mouse - lastMouse;
                offset += diff / scale;
                lastMouse = mouse;

                Refresh();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            areaSize -= e.Delta * 0.0075;
            areaSize = areaSize.Clamp(5.0, 50.0);
            RecalcScale();
            Refresh();
        }

        private void RecalcScale()
        {
            double aspect = this.Width / (double)this.Height;
            if (Width > Height)
            {
                scale.X = Width / areaSize / aspect;
                scale.Y = Height / areaSize;
            }
            else
            {
                scale.X = Width / areaSize;
                scale.Y = Height / areaSize * aspect;
            }
        }

        private void DrawCamera()
        {
            Vector2 start = new Vector2(camera.Position.X, camera.Position.Z);
            Vector2 dir = new Vector2(camera.Direction.X, camera.Direction.Z).Normalized;
            Vector2 orth = new Vector2(-dir.Y, dir.X);

            Vector2 left = dir - orth * camera.Tan;
            Vector2 right = dir + orth * camera.Tan;

            DrawLine(Pens.Red, start, start + left * 100.0);
            DrawLine(Pens.Red, start, start + right * 100.0);

            Pen cpen = new Pen(Color.DarkRed, 2f);
            DrawLine(cpen, start, start + left * 2.0);
            DrawLine(cpen, start, start + right * 2.0);
            DrawLine(cpen, start + left * 2.0, start + right * 2.0);
        }

        private void DrawLight(ILight l)
        {
            if (l is PointLight)
            {
                PointLight pl = l as PointLight;
                Vector2 center = new Vector2(pl.Position.X, pl.Position.Z);
                double crossSize = 500.0 / areaSize;
                Vector2 pc = VectorToPixel(center);

                g.DrawLine(Pens.Black, pc.X - crossSize, pc.Y - crossSize, pc.X + crossSize, pc.Y + crossSize);
                g.DrawLine(Pens.Black, pc.X - crossSize, pc.Y + crossSize, pc.X + crossSize, pc.Y - crossSize);
                Rectangle rect = new Rectangle((int)(pc.X - crossSize * 0.6), (int)(pc.Y - crossSize * 0.6), (int)(crossSize * 1.2), (int)(crossSize * 1.2));
                g.FillEllipse(new SolidBrush(pl.Color.ToColor()), rect);
                g.DrawEllipse(Pens.Black, rect);
            }

            if (l is AreaLight)
            {
                AreaLight al = l as AreaLight;
                Vector2 center = new Vector2(al.Position.X, al.Position.Z);
                double crossSize = 500.0 / areaSize;
                Vector2 pc = VectorToPixel(center);

                g.DrawLine(Pens.Black, pc.X, pc.Y - crossSize, pc.X, pc.Y + crossSize);
                g.DrawLine(Pens.Black, pc.X - crossSize, pc.Y, pc.X + crossSize, pc.Y);
                Rectangle rect = new Rectangle((int)(pc.X - crossSize * 0.6), (int)(pc.Y - crossSize * 0.6), (int)(crossSize * 1.2), (int)(crossSize * 1.2));
                g.FillEllipse(new SolidBrush(al.Color.ToColor()), rect);
                g.DrawEllipse(Pens.Black, rect);
            }

            if (l is SpotLight)
            { 
                SpotLight sl = l as SpotLight;
                Vector2 center = new Vector2(sl.Position.X, sl.Position.Z);
                double crossSize = 500.0 / areaSize;
                Vector2 pc = VectorToPixel(center);

                g.DrawLine(Pens.Black, pc.X - crossSize, pc.Y - crossSize, pc.X + crossSize, pc.Y + crossSize);
                g.DrawLine(Pens.Black, pc.X - crossSize, pc.Y + crossSize, pc.X + crossSize, pc.Y - crossSize);
                Rectangle rect = new Rectangle((int)(pc.X - crossSize * 0.6), (int)(pc.Y - crossSize * 0.6), (int)(crossSize * 1.2), (int)(crossSize * 1.2));
                g.FillEllipse(new SolidBrush(sl.Color.ToColor()), rect);
                g.DrawEllipse(Pens.Black, rect);
                Vector2 dir = new Vector2(sl.Direction.X, sl.Direction.Z);
                if (dir.Length == 0.0)
                    return;
                dir.Normalize();
                Vector2 orth = new Vector2(-dir.Y, dir.X) * Math.Tan(sl.Angle.ToRadians());

                Vector2 left = dir - orth;
                Vector2 right = dir + orth;

                DrawLine(Pens.Blue, center, center + left * 50.0);
                DrawLine(Pens.Blue, center, center + right * 50.0);
            }
        }

        private void DrawObject(IObject obj)
        {
            if (obj is Sphere)
            {
                Sphere s = obj as Sphere;
                Vector2 center = new Vector2(s.Position.X, s.Position.Z);
                Vector2 pc = VectorToPixel(center);

                double radius = s.Radius * scale.X;
                Rectangle rect = new Rectangle((int)(pc.X - radius), (int)(pc.Y - radius), (int)(radius * 2.0), (int)(radius * 2.0));
                g.FillEllipse(new SolidBrush(Color.FromArgb(64, materials[s.Material].Color)), rect);
                g.DrawEllipse(new Pen(materials[s.Material].Color, 1.5f), rect);
            }
            if (obj is Triangle)
                DrawTriangle(obj as Triangle);
            if (obj is Quad)
            {
                Quad q = obj as Quad;
                DrawTriangle(q.T1);
                DrawTriangle(q.T2);
            }
        }

        private void DrawTriangle(Triangle tri)
        {
            Vector2 v0 = new Vector2(tri.Vertex0.Position.X, tri.Vertex0.Position.Z);
            Vector2 v1 = new Vector2(tri.Vertex1.Position.X, tri.Vertex1.Position.Z);
            Vector2 v2 = new Vector2(tri.Vertex2.Position.X, tri.Vertex2.Position.Z);

            DrawLine(Pens.Black, v0, v1);
            DrawLine(Pens.Black, v1, v2);
            DrawLine(Pens.Black, v2, v0);
        }

        private void DrawRay(Color color, Ray ray)
        {
            Vector2 start = new Vector2(ray.Start.X, ray.Start.Z);
            Vector2 center = VectorToPixel(start);

            double ellipseRadius = 50.0 / areaSize;
            g.FillEllipse(new SolidBrush(color), (int)(center.X - ellipseRadius), (int)(center.Y - ellipseRadius), (int)(ellipseRadius * 2), (int)(ellipseRadius * 2));

            Pen p = new Pen(color, 2f);
            Vector2 dir = new Vector2(ray.Direction.X, ray.Direction.Z).Normalized;
            DrawLine(p, start, start + dir * 2);
        }

        private void DrawGrid()
        {
            Vector2 topleft = PixelToVector(0, 0);
            Vector2 botRight = PixelToVector(Width - 1, Height - 1);

            double dx = 1.0, dy = 1.0;

            for (double x = Math.Floor(topleft.X); x < Math.Ceiling(botRight.X); x += dx)
            {
                int px = (int)((x + offset.X) * scale.X);
                g.DrawLine(Pens.LightGray, px, 0, px, Height - 1);
            }

            for (double y = Math.Floor(topleft.Y); y < Math.Ceiling(botRight.Y); y += dy)
            {
                int py = (int)((y + offset.Y) * scale.Y);
                g.DrawLine(Pens.LightGray, 0, py, Width - 1, py);
            }
        }

        private void DrawLine(Pen p, Vector2 v1, Vector2 v2)
        {
            Vector2 start = VectorToPixel(v1);
            Vector2 end = VectorToPixel(v2);
            g.DrawLine(p, start.X, start.Y, end.X, end.Y);
        }

        private void DrawLine(Pen p, double x1, double y1, double x2, double y2)
        {
            DrawLine(p, new Vector2(x1, y1), new Vector2(x2, y2));
        }

        private Vector2 VectorToPixel(Vector2 v)
        {
            return (v + offset) * scale;
        }

        private Vector2 PixelToVector(int x, int y)
        {
            return new Vector2(x, y) / scale - offset;
        }
    }
}