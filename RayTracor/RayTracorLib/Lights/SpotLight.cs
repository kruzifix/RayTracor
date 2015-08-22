using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml;
using RayTracor.RayTracorLib.Utilities;
using RayTracor.RayTracorLib.Tracing;

namespace RayTracor.RayTracorLib.Lights
{
    public class SpotLight : ILight
    {
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        // saved in radians; given in degrees
        public double Angle { get; set; }

        public SpotLight()
            : base()
        { Direction = Vector3.UnitX; Angle = (30.0).ToRadians(); }

        public SpotLight(Vector3 position, Color color, double strength, Vector3 direction, double angle)
            : base(color, strength)
        {
            Position = position;
            Direction = direction;
            Angle = angle.ToRadians();
        }
        
        public bool PointLighted(Vector3 point)
        {
            double a = Vector3.DotProduct(Direction, (point - Position).Normalized);
            double b = Math.Acos(a);

            return b < Angle;
        }
        
        public static SpotLight FromTo(Vector3 position, Vector3 point, Color color, double strength, double angle)
        {
            return new SpotLight(position, color, strength, (point - position).Normalized, angle);
        }
    }
}