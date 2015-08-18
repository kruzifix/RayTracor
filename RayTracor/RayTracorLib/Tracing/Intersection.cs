using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracor.RayTracorLib.Objects;

namespace RayTracor.RayTracorLib.Tracing
{
    public class Intersection
    {
        public static readonly Intersection False = new Intersection();

        public bool Intersects { get; set; }
        public double Distance { get; set; }
        public Vector3 Point { get; set; }
        public IObject Object { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 BaryCoords { get; set; }
        
        private Intersection() { Intersects = false; }

        public Intersection(double distance, Vector3 point, IObject obj, Vector3 normal, Vector2 baryCoords)
        {
            Intersects = true;
            Distance = distance;
            Point = point;
            Object = obj;
            Normal = normal;
            BaryCoords = baryCoords;
        }
    }
}