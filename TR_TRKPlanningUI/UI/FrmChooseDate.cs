using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FM.TR_TRKBookDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_TRKPlanDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using TR_LanguageResource.Resources;
using TR_FormBaseLibrary;

namespace FM.TR_TRKPlanningUI.UI
{
    public partial class FrmChooseDate : Form
    {
        public FrmTruckPlanningEntry parent;
        public FrmChooseDate()
        {
            InitializeComponent();
        }

        public FrmChooseDate(FrmTruckPlanningEntry parent)
        {
            InitializeComponent();
            this.parent = parent;
        }
        private void Localize()
        {
            this.Text = TptResourceUI.ChoseDate;
            lblScheduleDate.Text = TptResourceUI.ScheduleDate + ":";
            lblDepartment.Text = TptResourceUI.Department + ":";
            btnCancel.Text = CommonResource.Cancel;
            btnOk.Text = CommonResource.OK;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (PlanTruckTrip.IsPlanDateDuplicate(dtpScheduleDate.Value, cboDept.Text) == true)
                {
                    if (PlanDateDept.GetDriversForPlanDateDeptForTrucking(dtpScheduleDate.Value, cboDept.Text).Count == 0)
                    {
                        PlanDateDept.CreatePlanDateForTrucking(dtpScheduleDate.Value, cboDept.Text.ToString(), parent.user.UserID.ToString(), FrmTruckPlanningEntry.formName);
                        parent.success = true;
                        parent.chosenDate = dtpScheduleDate.Value;
                        parent.deptcode = cboDept.Text;
                    }
                    Close();
                }
                else
                {
                    throw new FMException(TptResourceBLL.ErrDateDuplicate);
                }
            }
            catch (FMException fmEx)
            {
                parent.success = false;
                MessageBox.Show(fmEx.ToString(), CommonResource.Alert);
            }
            catch (Exception ex)
            {
                parent.success = false;
                MessageBox.Show(ex.ToString(), CommonResource.Alert);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            parent.success = false;
            Close();
        }

        private void FrmChooseDate_Load(object sender, EventArgs e)
        {
            Localize();
            cboDept.DataSource = TptDept.GetAllTptDepts(parent.deptType); 
            cboDept.DisplayMember = "tptDeptCode";
        }
    }
}
