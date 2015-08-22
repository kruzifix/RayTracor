using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracor.RayTracorLib.Tracing
{
    public class Vertex
    {
        public Vector3 Position { get; set; }
        public Vector2 TexCoord { get; set; }

        public Vertex() { }

        public Vertex(double x, double y, double z, double u, double v)
        {
            Position = new Vector3(x, y, z);
            TexCoord = new Vector2(u, v);
        }
    }
}