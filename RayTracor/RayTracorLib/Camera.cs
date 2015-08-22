using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RayTracor.RayTracorLib.Tracing;
using RayTracor.RayTracorLib.Utilities;

namespace RayTracor.RayTracorLib
{
    public class Camera
    {
        Vector3 gup, up, right;
        Vector3 pos, dir;
        double fov, tan;

        int width, height;
        double ratio, tanratio;
        double xs, ys;

        public Vector3 Position { get { return pos; } set { pos = value; UpdateRightUp(); } }
        public Vector3 Direction { get { return dir; } set { dir = value; UpdateRightUp(); } }
        public Vector3 GlobalUp { get { return gup; } set { gup = value; UpdateRightUp(); } }
        public double FOV { get { return fov; } set { fov = value; tan = Math.Tan(FOV.ToRadians() / 2.0); } }
        public double Tan { get { return tan; } }

        public Vector3 Right { get { return right; } }
        public Vector3 Up { get { return up; } }

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public Camera() 
        { 
            pos = Vector3.Zero; 
            dir = Vector3.UnitX;
            gup = Vector3.UnitY;
            FOV = 45;

            UpdateRightUp();
        }

        public Camera(Vector3 position, Vector3 direction, Vector3 globalup, double fov)
        {
            pos = position;
            dir = direction;
            gup = globalup.Normalized;
            FOV = fov;

            UpdateRightUp();
        }

        private void UpdateRightUp()
        {
            right = Vector3.CrossProduct(dir, gup);
            right.Normalize();
            up = Vector3.CrossProduct(right, dir);
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

        public Ray CastRay(double x, double y)
        {
            double xx = x * xs - 1.0;
            double yy = y * ys - 1.0;

            return new Ray(pos, dir + right * xx * tan - up * yy * tanratio);
        }

        public Vector3 CalcPos(double x, double y, double depth)
        {
            return CastRay(x, y).PointAt(depth);
        }
        
        public static Camera FromJToken(JToken token)
        {
            var cam = JsonConvert.DeserializeObject<SerializedCamera>(token.ToString());

            if (cam.position == null)
                throw new Exception("Camera: Position not defined.");
            if (cam.globalup == null)
                throw new Exception("Camera: GlobalUp not defined.");
            if (cam.fov < 1.0 || cam.fov > 180.0)
                throw new Exception("Camera: FOV has to be between 1 and 180 degrees.");
            if (cam.target == null)
                throw new Exception("Camera: No Target defined.");

            return Camera.CreateLookAt(cam.position, cam.target, cam.globalup, cam.fov);
        }

        public static Camera CreateLookAt(Vector3 position, Vector3 target, Vector3 up, double fov)
        {
            return new Camera(position, (target - position).Normalized, up, fov);
        }
    }

    public class SerializedCamera
    {
        public Vector3 position { get; set; }
        public Vector3 target { get; set; }
        public Vector3 globalup { get; set; }
        public double fov { get; set; }
    }
}