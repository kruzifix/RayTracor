using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RayTracor.RayTracorLib
{
    public class Texture
    {
        Bitmap bmp;

        int width, height;
        Vector[] colors;

        public Vector2 Scale { get; set; }

        public Texture(Bitmap bmp)
        {
            Scale = Vector2.One;
            this.bmp = bmp;
            this.width = bmp.Width;
            this.height = bmp.Height;

            colors = new Vector[width*height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    colors[x + y * width] = bmp.GetPixel(x, y).ToVector();
        }

        public Vector GetColor(double u, double v)
        {
            u = Math.Abs((u * Scale.X) % 1.0);
            v = Math.Abs((v * Scale.Y) % 1.0);

            int x = (int)(u * width);
            int y = (int)(v * height);

            return colors[x + y * width];
        }

        public Vector GetColor(Vector2 uv)
        {
            return GetColor(uv.X, uv.Y);
        }

        public static Texture FromPath(string p)
        {
            return new Texture((Bitmap)Bitmap.FromFile(p));
        }
    }
}