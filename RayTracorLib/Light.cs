using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracor.RayTracorLib
{
    public class Light
    {
        public Vector Position { get; set; }
        public Color Color { get; set; }
        public double Strength { get; set; }

        public Light() { Position = Vector.Zero; Color = Color.White; Strength = 1; }

        public Light(Vector position, Color color, double strength)
        {
            Position = position;
            Color = color;
            Strength = strength;
        }
    }
}