using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using RayTracor.RayTracorLib.Tracing;
using RayTracor.RayTracorLib.Utilities;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            byte[] pixels = new byte[data.Stride * data.Height];
            Marshal.Copy(data.Scan0, pixels, 0, pixels.Length);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int i = x * 3 + y * data.Stride;
                    colors[x + y * width] = new Vector3(
                        pixels[i + 2],
                        pixels[i + 1],
                        pixels[i + 0]
                        );
                }
            }

            bmp.UnlockBits(data);
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