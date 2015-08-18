using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using RayTracor.RayTracorLib.Tracing;
using RayTracor.RayTracorLib.Utility;

namespace RayTracor.RayTracorLib.Materials
{
    public class Texture
    {
        Bitmap bmp;

        int width, height;
        Vector3[] colors;
        
        public Texture(Bitmap bmp)
        {
            this.bmp = bmp;
            width = bmp.Width;
            height = bmp.Height;

            colors = new Vector3[width*height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    colors[x + y * width] = bmp.GetPixel(x, y).ToVector();
        }

        public Vector3 GetColor(double u, double v)
        {
            u = Math.Abs(u % 1.0);
            v = Math.Abs(v % 1.0);

            int x = (int)(u * width);
            int y = (int)(v * height);

            return colors[x + y * width];
        }

        public Vector3 GetColor(Vector2 uv)
        {
            return GetColor(uv.X, uv.Y);
        }

        public static Texture FromPath(string p)
        {
            return new Texture((Bitmap)Bitmap.FromFile(p));
        }
    }
}