using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracor.RayTracor
{
    public class Camera
    {
        public Vector Position { get; set; }
        public Vector Direction { get; set; }
        public double FOV { get; set; }

        public Camera() { Position = Vector.Zero; Direction = Vector.UnitX; FOV = Math.PI/2.0; }

        public Camera(Vector position, Vector direction, double fov)
        {
            Position = position;
            Direction = direction;
            FOV = fov;
        }

        public static Camera CreateLookAt(Vector position, Vector target, double fov)
        {
            return new Camera(position, (target - position).Normalized(), fov);
        }
    }
}