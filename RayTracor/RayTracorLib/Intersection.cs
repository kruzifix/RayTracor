using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracor.RayTracorLib
{
    public class Intersection
    {
        public static readonly Intersection False = new Intersection();

        public bool Intersects { get; set; }
        public double Distance { get; set; }
        public Vector Point { get; set; }
        public Object Object { get; set; }
        public Vector Normal { get; set; }
        public Vector2 BaryCoords { get; set; }
        
        private Intersection() { Intersects = false; }

        public Intersection(double distance, Vector point, Object obj, Vector normal, Vector2 baryCoords)
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