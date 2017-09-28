using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FM.TransportPlanning.UI
{
    public partial class FrmChangeEndTime : Form
    {
        private FrmTrailer parent;

        public FrmChangeEndTime()
        {
            InitializeComponent();
        }

        public FrmChangeEndTime(FrmTrailer parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parent.t1.EndTime = parent.t1.EndTime.Date + dtpEndTime.Value.TimeOfDay;
            parent.refresh();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
