
using FM.TR_FMSystemDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using TR_FormBaseLibrary;
using TR_LanguageResource.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FM.TR_MaintenanceUI.UI
{
    public partial class FrmDriverJobAssignment : AbstractForm
    {
        private DriverPriority currDrvPriority;
        public Driver driver;
        JobAssignmentPriority prior;


        public FrmDriverJobAssignment(DriverPriority currDrvPriority)
        {
            InitializeComponent();
            this.currDrvPriority = currDrvPriority;
            this.dgvAssignments.AutoGenerateColumns = false;
        }
        
        private void FrmDriverJobAssignment_Load(object sender, EventArgs e)
        {
            try
            {
                bdsAssignments.DataSource = JobAssignmentPriority.GetDriverJobAssignments().OrderBy(djp =>djp.Description);
                dgvAssignments.DataSource = bdsAssignments; 
            }
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString(), CommonResource.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (currDrvPriority != null)
                { 
                    currDrvPriority.EditDriverPriority(prior.Code);
                }
                this.Close();
            }
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString(), CommonResource.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
            }
        }

        private void bdsAssignments_CurrentChanged(object sender, EventArgs e)
        {
            prior = (JobAssignmentPriority)bdsAssignments.Current;
        }
    }
}
