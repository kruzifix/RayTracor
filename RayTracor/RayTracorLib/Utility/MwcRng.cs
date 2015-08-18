using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracor.RayTracorLib.Utility
{
    /// <summary>
    /// MwcRng is a simple random number generator based on 
    /// George Marsaglia's MWC (multiply with carry) generator.
    /// </summary>
    public static class MwcRng
    {
        private static uint w;
        private static uint z;

        static MwcRng()
        {
            // These values are not magical, just the default values Marsaglia used.
            // Any pair of unsigned integers should be fine.
            w = 521288629;
            z = 362436069;
        }
        
        public static void SetSeed(uint u, uint v)
        {
            if (u != 0) w = u;
            if (v != 0) z = v;
        }

        public static void SetSeed(uint u)
        {
            w = u;
        }

        public static void SeedFromTime()
        {
            long x = DateTime.Now.ToFileTime();
            SetSeed((uint)(x >> 16), (uint)(x % 4294967296));
        }

        public static uint GetUint()
        {
            z = 36969 * (z & 65535) + (z >> 16);
            w = 18000 * (w & 65535) + (w >> 16);
            return (z << 16) + w;
        }

        public static int GetInt(int exclusiveMax)
        {
            if (exclusiveMax < 1)
                throw new ArgumentException("exclusiveMax has to be atleast 1.");
            return (int)(GetUint() % exclusiveMax);
        }

        /// <summary>
        /// Generates a uniform distributed double floating point number from the open interval (0,1).
        /// Does not contain 0 or 1.
        /// </summary>
        /// <returns></returns>
        public static double GetUniform()
        {
            // 0 <= u < 2^32
            uint u = GetUint();
            // The magic number below is 1/(2^32 + 2).
            // The result is strictly between 0 and 1.
            return (u + 1.0) * 2.328306435454494e-10;
        }

        public static double GetUniformRange(double lower, double upper)
        {
            if (lower >= upper)
                throw new ArgumentException("lower has to be less than upper.");
            
            return GetUniform() * (upper - lower) + lower;
        }
    }
}