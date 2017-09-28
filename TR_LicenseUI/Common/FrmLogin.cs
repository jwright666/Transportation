using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FM.FMSystem.BLL;
using TR_LanguageResource.Resources;
using FM.FMSystem.DAL;
using System.Data.SqlClient;
using FMLicenseUI.Common;

namespace FMLicenseUI.Common
{
    public partial class FrmLogin : Form
    {
        FrmLicenseMenu parent;
        public FrmLogin(FrmLicenseMenu parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {       
            try
            {
                if (ValidateLogin())
                {           
                    parent.serverName = txtServerName.Text.ToString().Trim();
                    FMGlobalSettings.TheInstance.SetPubsConnectionString(parent.serverName);
                    //test pubs connection if success using the inputted server       
                    FMGlobalSettings.TheInstance.TestSQLConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
                    // end test pub connetion

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (FMException ex)
            {
                this.DialogResult = DialogResult.None;
                MessageBox.Show(ex.ToString(), CommonResource.Alert);
            }
            catch (InvalidOperationException ex)
            {
                this.DialogResult = DialogResult.None;
                MessageBox.Show(ex.ToString(), CommonResource.Alert);
            }
            catch (Exception ex)
            {
                this.DialogResult = DialogResult.None;
                MessageBox.Show(ex.ToString(), CommonResource.Alert);
            }

        }
        private bool ValidateLogin()
        {
            string errMsg = "";
            if (txtServerName.Text.Length == 0)
                 throw new FMException("Server name cannot be empty.");
            if (txtUserName.Text.Length == 0)
                throw new FMException("User name cannot be empty.");
                
            if (txtUserName.Text != "ipl")
            {
                errMsg += "\n Invalid userName";
            }
            if (txtPassword.Text != "support")
            {
                if (errMsg != "")
                {
                    errMsg += "\n";
                }
                errMsg += "Invalid Password";
            }
            if (errMsg != "")
            {
                throw new FMException(errMsg);
            }

            return true;
        }
    }
}
