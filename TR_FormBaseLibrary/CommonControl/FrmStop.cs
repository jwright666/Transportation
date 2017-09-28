using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FM.TR_FMSystemDLL.BLL;

namespace FormBaseLibrary
{
    internal partial class FrmStop : Form
    {
        private Stop stop;
        public Stop GetSelectedStop() { return stop; }

        public FrmStop(List<Stop> lstAllStops)
        {
            InitializeComponent();

            myStop.LstAllStops = lstAllStops;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            stop = myStop.Current;

            //20141127 - gerry replaced hide to close
            //this.Hide();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            stop = null;

            this.Close();
            this.Dispose();
        }

    }
}
