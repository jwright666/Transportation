using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FMLicenseUI.Common;
using FMLicenseUI.Common.Transport;
using FMLicenseUI.Common.WmsMobile;

namespace FMLicenseUI.Common
{
    public partial class FrmLicenseMenu : Form
    {
        FrmLogin frmLogin;
        FrmTPTCompanyLicense frmTPTCompanyLicense;
        FrmWmsCompanyRegistration frmWmsCompanyRegistration;


        public string serverName;

        public FrmLicenseMenu()
        {
            InitializeComponent();
        }

        private void FrmLicenseMenu_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            frmLogin = new FrmLogin(this);
            frmLogin.StartPosition = FormStartPosition.CenterParent;
            if (frmLogin.ShowDialog() == DialogResult.OK)
            {
                this.mainMenu.Enabled = true;
            }
            else
                Application.Exit();
            
        }

        private void mmTransportRegistration_Click(object sender, EventArgs e)
        {
            frmTPTCompanyLicense = new FrmTPTCompanyLicense(this.serverName);
            frmTPTCompanyLicense.MdiParent = this;
            frmTPTCompanyLicense.Show();
            frmTPTCompanyLicense.BringToFront();  
        }

        private void registerMobileDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmWmsCompanyRegistration = new FrmWmsCompanyRegistration(this.serverName);
            frmWmsCompanyRegistration.MdiParent = this;
            frmWmsCompanyRegistration.Show();
            frmWmsCompanyRegistration.BringToFront();
        
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void miMobileDeviceRegistration_Click(object sender, EventArgs e)
        {

        }

        private void miWmsCompanyRegistration_Click(object sender, EventArgs e)
        {
            //frmCompanyRegistration = new FrmCompanyRegistration();
            //frmCompanyRegistration.MdiParent = this;
            //frmCompanyRegistration.Show();
            //frmCompanyRegistration.BringToFront();
        }    
    }
}
