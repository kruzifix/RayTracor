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

            Vector u = Vector.UnitX * 2.0;
            Vector v = Vector.UnitY * 5.0;
            Vector w = Vector.CrossProduct(u, v);
            Vector l = u - v;
            
        }
    }
}
