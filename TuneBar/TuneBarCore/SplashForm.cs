using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TuneBar
{
    public partial class SplashForm : Form
    {
        private int show_count = 0; 

        public SplashForm()
        {
            InitializeComponent();
        }

        private void SplashForm_Load(object sender, EventArgs e)
        {
            show_count = 0;
            timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            show_count++;
            if(show_count > 1)
            {
                Close();
            }
        }
    }
}
