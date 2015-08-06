using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracor.RayTracor
{
    public abstract class Object
    {
        public Vector Position { get; set; }

        protected Object(Vector position) { Position = position; }

        public abstract IntersectionResult Intersects(Ray ray);
        public abstract Vector Normal(Vector point);
    }
}