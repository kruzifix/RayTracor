using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracor.RayTracor
{
    public static class Extensions
    {
        public static int Clamp(this int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        public static int ClampToInt(this double value, int min, int max)
        {
            int x = (int)value;
            return x.Clamp(min, max);
        }
    }
}