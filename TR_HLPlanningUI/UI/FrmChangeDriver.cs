using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FM.TR_MaintenanceDLL.BLL;

namespace FM.TransportPlanning.UI
{
    public partial class FrmChangeDriver : Form
    {
        Driver oldDriver;
        FrmHaulierPlanningEntry parent;
        public Driver newDriver = new Driver();

        public FrmChangeDriver()
        {
            InitializeComponent();
        }

        public FrmChangeDriver(FrmHaulierPlanningEntry parent,Driver oldDriver)
        {
            InitializeComponent();
            this.oldDriver = oldDriver;
            this.parent = parent;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void FrmChooseDriver_Load(object sender, EventArgs e)
        {
            txtOldDriverCode.Text = oldDriver.Name;
            txtOldDriverName.Text = oldDriver.Code;

            cboNewDriverCode.DataSource = Driver.GetAllDriverThatNotInThePlan(parent.chosenDate);
            cboNewDriverCode.DisplayMember = "Name";
        }

        private void cboNewDriverCode_TextChanged(object sender, EventArgs e)
        {
            Driver a = (Driver)cboNewDriverCode.SelectedItem;
            txtNewDriverName.Text = a.Code;
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            parent.choosedriver = true;
            parent.newdriver = (Driver)cboNewDriverCode.SelectedItem;
            this.newDriver = (Driver)cboNewDriverCode.SelectedItem;
            Close();
        }
    }
}
