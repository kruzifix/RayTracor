using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RayTracor.RayTracorLib.Tracing;

namespace RayTracor.RayTracorLib.Utilities
{
    public static class Utility
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

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

        public static double Clamp(this double value, double min, double max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        public static double SmoothStep(double edge0, double edge1, double x)
        {
            // Scale, bias and saturate x to 0..1 range
            x = Clamp((x - edge0) / (edge1 - edge0), 0.0, 1.0);
            // Evaluate polynomial
            return x * x * (3 - 2 * x);
        }

        public static double ToRadians(this double value)
        {
            return value * Math.PI / 180.0;
        }

        public static double ToDegrees(this double value)
        {
            return value * 180.0 / Math.PI;
        }

        public static void DrawLine(this Graphics g, Pen pen, double x1, double y1, double x2, double y2)
        {
            g.DrawLine(pen, (int)x1, (int)y1, (int)x2, (int)y2);
        }

        public static Vector3 ToVector(this Color color)
        {
            return new Vector3(color);
        }

        public static double NextDouble(this Random random, double min, double max)
        {
            return min + random.NextDouble() * (max - min);
        }

        public static Color NextColor(this Random random)
        {
            return Color.FromArgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            return Color.FromArgb(255, v, p, q);
        }

        public static string ToHexString(this Color color)
        {
            return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", color.A, color.R, color.G, color.B);
        }

        public static Color ColorFromHexString(this string s, string excep)
        {
            Color c = System.Drawing.Color.Empty;
            try
            {
                c = Utility.ColorFromHexString(s);
            }
            catch
            {
                throw new Exception(excep);
            }
            return c;
        }

        public static Color ColorFromHexString(this string s)
        {
            int a = int.Parse(s.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int r = int.Parse(s.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(s.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(s.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }

        public static XmlNode Serialize(this Color color, XmlDocument doc, string name)
        {
            XmlNode node = doc.CreateElement(name);
            node.InnerText = color.ToHexString();
            return node;
        }

        public static XmlNode Serialize(this double val, XmlDocument doc, string name)
        {
            XmlNode node = doc.CreateElement(name);
            node.InnerText = val.ToString();
            return node;
        }

        public static XmlNode Serialize(this bool b, XmlDocument doc, string name)
        {
            XmlNode node = doc.CreateElement(name);
            node.InnerText = b.ToString().ToLower();
            return node;
        }

        public static bool ParseBool(this XmlNode node)
        {
            return bool.Parse(node.InnerText);
        }

        public static double ParseDouble(this XmlNode node)
        {
            return double.Parse(node.InnerText);
        }
        
        public static Color ParseColor(this XmlNode node)
        {
            return node.InnerText.ColorFromHexString();
        }
    }
}