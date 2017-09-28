using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using TR_FormBaseLibrary;
using TR_LanguageResource.Resources;

namespace FM.TR_MaintenanceUI.UI
{
    public partial class FrmDepartment : frmEntryBase
    {
        TptDept newTptDept = new TptDept();
        BindingSource bdsDeptType = new BindingSource();

        public FrmDepartment(string moduleName, User userID)
        {
            InitializeComponent();            
            base.module = moduleName;
            base.user = userID;
            FillComboBox();
            btnPrint.Visible = false;
        }

        public FrmDepartment(DeptType deptType)
        {
            newTptDept.TptDeptType = deptType;
            InitializeComponent();
            FillComboBox();
            btnPrint.Visible = false;
        }

        protected override void SetSecurityControl()
        {
            base.ManageUserAccess(module, user);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            bdsMaster.DataSource = TptDept.GetAllTptDeptsByCode(cboDepartmentCode.Text.ToString());
            if (bdsMaster.Count > 0)
            {
                btnDeleteMaster.Enabled = true;
                btnPrint.Visible = false;
            }
            else
            {
                btnDeleteMaster.Enabled = false;
                btnPrint.Visible = false;
            }
            SetSecurityControl();
        }

        protected override bool AfterNewClicked()
        {
            txtCode.Enabled = true;

            txtCode.Text="";
            txtName.Text = "";

            return true;
        }

        protected override bool AfterModifyClicked()
        {
            if (bdsMaster.Count != 0)
            {
                txtCode.Enabled = false;
                newTptDept = (TptDept)bdsMaster.Current;
                txtCode.Text = newTptDept.TptDeptCode;
                txtName.Text = newTptDept.TptDeptName;
                cboType.Text = newTptDept.TptDeptType.ToString();
                if (newTptDept.IsTptDeptUsed())
                {
                    cboType.Enabled = false;
                    lblErrorEdit.Text = TptResourceBLL.ErrDeptTypeorEdit;
                }
                else
                {
                    cboType.Enabled = true;
                    lblErrorEdit.Text = "";
                }
            }
            return true;
        }

        protected override bool AfterSaveClicked()
        {
            string message = "";
            bool flag = false;
            if (formMode == FormMode.Add)
            {
                newTptDept = new TptDept();
                try
                {                    
                    newTptDept.TptDeptCode = txtCode.Text;
                    newTptDept.TptDeptName = txtName.Text;
                    newTptDept.TptDeptType = (DeptType)cboType.SelectedIndex +1;

                    flag = newTptDept.AddTptDept(out message);
                    if (flag)
                    {
                        pnlMaster.Enabled = false;
                        formMode = FormMode.Delete;
                    }
                    else
                        txtCode.Focus();
                }
                catch (FMException ex)
                {
                    message = TptResourceDAL.ErrInsertFailed + "\n" + ex.ToString();
                    formMode = FormMode.Add;
                }
            }
            else if (formMode == FormMode.Edit)
            {
                newTptDept = (TptDept)bdsMaster.Current;
                try
                {
                    newTptDept.TptDeptCode = txtCode.Text;
                    newTptDept.TptDeptName = txtName.Text;
                    newTptDept.TptDeptType = (DeptType)bdsDeptType.Current;
                    flag = newTptDept.EditTptDept(out message);
                    if (flag)
                    {
                        pnlMaster.Enabled = false;
                        formMode = FormMode.Delete;
                    }
                }
                catch (FMException ex)
                {
                    message = TptResourceDAL.ErrEditFailed + "\n" + ex.ToString();
                    formMode = FormMode.Edit;
                }

            }

            MessageBox.Show(message, CommonResource.Alert);
            btnQuery.PerformClick();
            //ChangeButtonsControls(formMode);     
            return true;
        }        
        
        protected override bool AfterDeleteClicked()
        {
            string message ="";
            bool flag;
            if (bdsMaster.Count > 0)
            {  
                formMode = FormMode.Delete;
                newTptDept = (TptDept)bdsMaster.Current;
                if (MessageBox.Show(CommonResource.DeleteConfirmation + TptResourceUI.Department + "?", CommonResource.Confirmation,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        flag = newTptDept.DeleteTptDept(out message);
                    }
                    catch (FMException ex)
                    {
                        message = TptResourceDAL.ErrDeleteFailed +"\n" + ex.ToString();
                    }
                    MessageBox.Show(message, CommonResource.Alert);
                }

                btnQuery.PerformClick();
                FillComboBox();
                //ChangeButtonsControls(formMode);                
            }
            return true;
        }

        private void bdsMaster_CurrentChanged(object sender, EventArgs e)
        {
            if (bdsMaster.Count != 0)
            {
                newTptDept = (TptDept)bdsMaster.Current;
                txtCode.Text = newTptDept.TptDeptCode;
                txtName.Text = newTptDept.TptDeptName;
                cboType.Text = Convert.ToString(newTptDept.TptDeptType);
            }
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            cboDepartmentCode.SelectedIndex = -1;
        }       

        private void FillComboBox()
        {
            bdsDeptType.Add(DeptType.Haulage);
            bdsDeptType.Add(DeptType.Trucking);
            cboType.DataSource = bdsDeptType;
            cboType.DropDownStyle = ComboBoxStyle.DropDownList;

            cboDepartmentCode.DataSource = TptDept.GetAllTptDepts();
            cboDepartmentCode.DisplayMember = "TptDeptCode";
            cboDepartmentCode.SelectedIndex = -1;
        }

        protected override void LocalizeOtherControls()
        {
            this.Text = TptResourceUI.DepartmentEntry;
            tabPageMaster.Text = CommonResource.Master;
            tabPageSearch.Text = CommonResource.Search;
            #region Buttons
            btnNewMaster.Text = CommonResource.New;
            btnEditMaster.Text = CommonResource.Edit;
            btnDeleteMaster.Text = CommonResource.Delete;
            btnSave.Text = CommonResource.Save;
            btnCancel.Text = CommonResource.Cancel;
            btnClose.Text = CommonResource.Close;
            btnPrint.Text = CommonResource.Print;
            btnQuery.Text = CommonResource.Query;
            btnClear.Text = CommonResource.Clear;
            #endregion

            #region Labels
            lblDeptCodeCriteria.Text = TptResourceUI.DeptCode + ":";
            lblDeptCode.Text = TptResourceUI.DeptCode + ":";
            lblDeptName.Text = TptResourceUI.DeptName + ":";
            lblDeptType.Text = TptResourceUI.DeptType + ":";
            #endregion

            #region Data Grid
            dgvSearch.Columns["tptDeptCodeDataGridViewTextBoxColumn"].HeaderText = TptResourceUI.DeptCode;
            dgvSearch.Columns["tptDeptNameDataGridViewTextBoxColumn"].HeaderText = TptResourceUI.DeptName;
            dgvSearch.Columns["tptDeptTypeDataGridViewTextBoxColumn"].HeaderText = TptResourceUI.DeptType;
            #endregion


        }

        private void SetTabControl(FormMode formMode)
        {
            switch (formMode)
            {
                case FormMode.Delete:
                    if (tbcEntry.SelectedIndex == 0)
                        btnQuery.PerformClick();
                    break;

                default:
                    if (tbcEntry.SelectedIndex == 0)
                        tbcEntry.SelectedTab = tabPageMaster;
                    break;
            }
        }

        private void tbcEntry_Click(object sender, EventArgs e)
        {
            SetTabControl(formMode);
        } 
    }
}
