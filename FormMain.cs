using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RayTracor.RayTracor;

namespace RayTracor
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            this.Load += FormMain_Load;
        }

        void FormMain_Load(object sender, EventArgs e)
        {
            Scene s = new Scene();
            pictureBox1.Image = s.Render(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
