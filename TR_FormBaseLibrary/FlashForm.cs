using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TR_FormBaseLibrary
{
    public partial class FlashForm : Form
    {
        public int increment = 1; //default

        public FlashForm()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Increment(increment);
            if (progressBar1.Value == 100)
            {
                timerProgress.Stop();
                this.Close();
            }
        }
    }
}
