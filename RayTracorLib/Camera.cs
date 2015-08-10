using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracor.RayTracorLib
{
    public class Camera
    {
        Vector gup, up, right;
        Vector pos, dir;
        double fov, tan;

        int width, height;
        double ratio, tanratio;
        double xs, ys;

        public Vector Position { get { return pos; } set { pos = value; UpdateRightUp(); } }
        public Vector Direction { get { return dir; } set { dir = value; UpdateRightUp(); } }
        public Vector GlobalUp { get { return gup; } set { gup = value; UpdateRightUp(); } }
        public double FOV { get { return fov; } set { fov = value; tan = Math.Tan(FOV / 2.0); } }
        public double Tan { get { return tan; } }

        public Vector Right { get { return right; } }
        public Vector Up { get { return up; } }

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public Camera() 
        { 
            pos = Vector.Zero; 
            dir = Vector.UnitX;
            gup = Vector.UnitY;
            FOV = Math.PI / 2.0;

            UpdateRightUp();
        }

        public Camera(Vector position, Vector direction, Vector globalup, double fov)
        {
            pos = position;
            dir = direction;
            gup = globalup.Normalized;
            FOV = fov;

            UpdateRightUp();
        }

        private void UpdateRightUp()
        {
            right = Vector.CrossProduct(dir, gup);
            right.Normalize();
            up = Vector.CrossProduct(right, dir);
            up.Normalize();
        }

        public void SetResolution(int width, int height)
        {
            this.width = width;
            this.height = height;
            ratio = height / (double)width;
            tanratio = tan * ratio;
            xs = 2.0 / (width - 1);
            ys = 2.0 / (height - 1);
        }

        public Ray CastRay(int x, int y)
        {
            //double hwr = height / (double)width;
            //double hw = Math.Tan(FOV / 2.0);
            //double hh = hwr * hw;
            //double cw = hw * 2;
            //double ch = hh * 2;
            //double pw = cw / (width - 1);
            //double ph = ch / (height - 1);
            //return new Ray(pos, dir + right * (x * pw - hw) + up * (y * ph - hh));

            // y * ch / (height - 1) - hh
            // y * hh * 2 / (height - 1) - hwr * hw
            // y * hwr * hw * 2 / (height - 1) - height / width * tan
            // y * height / width * tan * 2 / (height  - 1) - height / width * tan
            
            // x * cw / (width-1) - hw
            // x * hw * 2 / (width - 1) - hw
            // x * tan * 2 / (width- 1) - tan

            double xx = x * xs - 1.0;
            double yy = y * ys - 1.0;

            return new Ray(pos, dir + right * xx * tan - up * yy * tanratio);
        }

        public static Camera CreateLookAt(Vector position, Vector target, Vector up, double fov)
        {
            return new Camera(position, (target - position).Normalized, up, fov);
        }
    }
}
