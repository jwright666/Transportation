using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TR_LanguageResource.Resources;
using TR_MessageDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_HLPlanDLL.BLL;
using TR_FormBaseLibrary;

namespace FM.TransportPlanning.UI
{
    public partial class FrmChooseDate : Form
    {
        public FrmHaulierPlanningEntry parent;
        public FrmChooseDate()
        {
            InitializeComponent();
        }

        public FrmChooseDate(FrmHaulierPlanningEntry parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (PlanHaulierTrip.IsPlanDateDuplicate(dtpScheduleDate.Value, cboDept.Text) == true)
                {
                    if (PlanDateDept.GetDriversForPlanDateDept(dtpScheduleDate.Value, cboDept.Text).Count == 0)
                    {
                        //20140619 - gerry added to auto select all available drivers and trailers                           
                        if (rdbNo.Checked)
                        {
                            parent.success = false;
                            throw new FMException(TptResourceBLL.ErrNoDriver);
                        }
                        else if (rdbYes.Checked)
                        {
                            PlanDateDept.CreatePlanDate(dtpScheduleDate.Value, cboDept.Text.ToString(), parent.user.UserID.ToString(), FrmHaulierPlanningEntry.FORM_NAME);
                        }
                        //20140619 - gerry end
                    }                    
                    parent.chosenDate = dtpScheduleDate.Value;
                    parent.deptcode = cboDept.Text;
                    parent.InitPlanningScreen();

                    Close();
                }
                else
                    throw new FMException(TptResourceBLL.ErrDateDuplicate);
                
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), CommonResource.Error); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            parent.success = false;
            Close();
        }

        private void FrmChooseDate_Load(object sender, EventArgs e)
        {
            cboDept.DataSource = TptDept.GetAllTptDepts(parent.deptType);
            cboDept.DisplayMember = "tptDeptCode";
        }
    }
}
