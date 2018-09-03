using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary1;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClassLibrary1.Form1 form1a = new ClassLibrary1.Form1();
            form1a.PathVideo = @"C:\Users\Carlos\Videos\VID_20180811_144136.mp4";
            form1a.ShowDialog();
        }
    }
}
