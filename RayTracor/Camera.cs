using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracor.RayTracor
{
    public class Camera
    {
        Vector gup, up, right;
        Vector pos, dir;

        public Vector Position { get { return pos; } set { pos = value; UpdateRightUp(); } }
        public Vector Direction { get { return dir; } set { dir = value; UpdateRightUp(); } }
        public Vector GlobalUp { get { return gup; } set { gup = value; UpdateRightUp(); } }
        public double FOV { get; set; }

        public Vector Right { get { return right; } }
        public Vector Up { get { return up; } }

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

        public Ray CastRay(int x, int y, int width, int height)
        {
            //double hwr = height / (double)width;
            //double hw = Math.Tan(FOV);
            //double hh = hwr * hw;
            //double cw = hw * 2;
            //double ch = hh * 2;
            //double pw = cw / (width - 1);
            //double ph = ch / (height - 1);

            //return new Ray(pos, dir + right * (x * pw - hw) + up * (y*ph-hh));

            double aspect = width / (double)height;
            double tan = Math.Tan(FOV);
            double xs = (x / (double)width) * 2.0 - 1.0;
            double ys = (y / (double)height) * 2.0 - 1.0;

            return new Ray(pos, dir + right * xs * aspect / tan + up * ys / tan);
        }

        public static Camera CreateLookAt(Vector position, Vector target, Vector up, double fov)
        {
            return new Camera(position, (target - position).Normalized, up, fov);
        }
    }
}
