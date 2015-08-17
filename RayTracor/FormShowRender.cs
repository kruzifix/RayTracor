using System;
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
        public FormShowRender(Bitmap bmp)
        {
            InitializeComponent();
            MinimumSize = new Size(bmp.Width+16, bmp.Height+39);

            InterpolPictureBox pBox = new InterpolPictureBox();
            pBox.Image = bmp;
            pBox.Dock = DockStyle.Fill;
            pBox.SizeMode = PictureBoxSizeMode.Zoom;

            Controls.Add(pBox);
        }
    }

    public class InterpolPictureBox : PictureBox
    {
        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            base.OnPaint(pe);
        }
    }
}
