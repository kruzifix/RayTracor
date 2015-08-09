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

            this.Width = bmp.Width + 16;
            this.Height = bmp.Height + 39;
            this.CenterToScreen();
            pBox.Image = bmp;
        }

        private void pBox_DoubleClick(object sender, EventArgs e)
        {
            this.Width = pBox.Image.Width + 16;
            this.Height = pBox.Image.Height + 39;
            this.CenterToScreen();
        }
    }
}
