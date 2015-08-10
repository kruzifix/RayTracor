﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RayTracor
{
    public partial class FormShowRender : Form
    {
        // offset: x = 16; y = 39
        Bitmap bmp;

        public FormShowRender(Bitmap bmp)
        {
            InitializeComponent();

            this.bmp = bmp;
            this.Width = bmp.Width + 16;
            this.Height = bmp.Height + 39;
            this.CenterToScreen();
            pBox.Paint += pBox_Paint;
        }

        private void pBox_Paint(object sender, PaintEventArgs e)
        {
            double height = pBox.Width * bmp.Height * 1.0 / bmp.Width;
            double width = pBox.Height * bmp.Width * 1.0 / bmp.Height;
            if (pBox.Width * 1.0 / pBox.Height < bmp.Width * 1.0 / bmp.Height)
                e.Graphics.DrawImage(bmp, 0, 0, pBox.Width, (int)height);
            else
                e.Graphics.DrawImage(bmp, 0, 0, (int)width, pBox.Height);
        }

        private void pBox_DoubleClick(object sender, EventArgs e)
        {
            this.Width = pBox.Image.Width + 16;
            this.Height = pBox.Image.Height + 39;
            this.CenterToScreen();
        }
    }
}
