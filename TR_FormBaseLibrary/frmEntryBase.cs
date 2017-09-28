using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TR_LanguageResource.Resources;
using FM.TR_FMSystemDLL.BLL;


namespace TR_FormBaseLibrary
{
    public partial class frmEntryBase : AbstractForm
    {
        public User user;
        public string module;
        public ModuleItemAccess moduleAccess;
        public FormMode formMode = FormMode.Delete;

        public frmEntryBase()
        {
            InitializeComponent();
        }
        /*
                public frmEntryBase(User user, string module)
                {
                    InitializeComponent();
                }
        */
        protected virtual void CloseForm()
        {
            //do closing form.
        }

        private void btnEditMaster_Click(object sender, EventArgs e)
        {
            try
            {
                //if (bdsMaster.Count > 0)
                //{
                if (AfterModifyClicked())
                {
                    formMode = FormMode.Edit;
                    tbcEntry.SelectTab(1);
                    EnableAllButtonsForFormMode();
                    SetSecurityControl();
                }
                //}
                //else
                //{
                //    throw new FMException(CommonResource.NoRecordforEdit);
                //}
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.ToString(), CommonResource.Alert); }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), CommonResource.Alert); }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (AfterSaveClicked())
            {
                //formMode = FormMode.Delete;
                EnableAllButtonsForFormMode();
                //btnQuery.PerformClick();
                //AfterQueryClicked();
                SetSecurityControl();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (AfterCancelClicked())
            {
                formMode = FormMode.Delete;
                EnableAllButtonsForFormMode();
                SetSecurityControl();
            }
        }

        private void btnDeleteMaster_Click(object sender, EventArgs e)
        {
            try
            {
                //if (bdsMaster.Count > 0)
                //{
                if (AfterDeleteClicked())
                {
                    formMode = FormMode.Delete;
                    tbcEntry.SelectTab(0);
                    EnableAllButtonsForFormMode();
                    btnQuery.PerformClick();
                    SetSecurityControl();
                }
                //}
                //else
                //{
                //    throw new FMException(CommonResource.NoRecordforEdit);
                //}
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.ToString(), CommonResource.Alert); }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), CommonResource.Alert); }
        }

        private void btnNewMaster_Click(object sender, EventArgs e)
        {
            if (AfterNewClicked())
            {
                formMode = FormMode.Add;
                tbcEntry.SelectTab(1);
                EnableAllButtonsForFormMode();
                SetSecurityControl();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("Are you sure want to exit the form?", CommonResource.Confirmation,
            //    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            //{
            this.Close();
            //}
        }

        private void frmEntryBase_Load(object sender, EventArgs e)
        {
            LocalizeMasterButtons();
            formMode = FormMode.Delete;
            LocalizeOtherControls();
            FillComboBox();
            EnableAllButtonsForFormMode();
            SetSecurityControl();
            setAllTextBoxToUppercase(this);
            btnEditMaster.Enabled = false;
            btnDeleteMaster.Enabled = false;
        }

        //23 June 2011 - Gerry Modify and this can be used to all derived forms
        protected virtual void EnableAllButtonsForFormMode()
        {
            #region Removed
            /*
            if (frmMode == FormMode.Add)
            {
                btnSave.Enabled = true;
                pnlQuery.Enabled = false;
                btnEditMaster.Enabled = false;
                btnDeleteMaster.Enabled = false;
            }
            else if (frmMode == FormMode.Delete)
            {
                btnNewMaster.Enabled = false;
                btnEditMaster.Enabled = false;
                btnSave.Enabled = false;
                pnlQuery.Enabled = true;

            }
            else
            {
                btnNewMaster.Enabled = false;
                btnDeleteMaster.Enabled = false;
                btnSave.Enabled = true;
                pnlQuery.Enabled = true;
            }
             */
            #endregion
            switch (formMode)
            {
                case FormMode.Delete:
                    {
                        btnNewMaster.Enabled = true;
                        btnEditMaster.Enabled = true;
                        btnCancel.Enabled = false;
                        btnSave.Enabled = false;
                        btnDeleteMaster.Enabled = true;
                        btnPrint.Enabled = true;
                        pnlQuery.Enabled = true;
                        pnlMaster.Enabled = false;
                        break;
                    }
                case FormMode.Add:
                    {
                        btnNewMaster.Enabled = false;
                        btnEditMaster.Enabled = false;
                        btnCancel.Enabled = true;
                        btnSave.Enabled = true;
                        btnDeleteMaster.Enabled = false;
                        btnPrint.Enabled = false;
                        pnlQuery.Enabled = false;
                        pnlMaster.Enabled = true;
                        break;
                    }
                case FormMode.Edit:
                    {
                        btnNewMaster.Enabled = false;
                        btnEditMaster.Enabled = false;
                        btnCancel.Enabled = true;
                        btnSave.Enabled = true;
                        btnDeleteMaster.Enabled = false;
                        btnPrint.Enabled = false;
                        pnlQuery.Enabled = false;
                        pnlMaster.Enabled = true;
                        break;
                    }
            }
        }

        private void LocalizeMasterButtons()
        {
            tabPageMaster.Text = CommonResource.Master;
            tabPageSearch.Text = CommonResource.Search;
            #region Buttons
            btnNewMaster.Text = CommonResource.New;
            btnEditMaster.Text = CommonResource.Edit;
            btnDeleteMaster.Text = CommonResource.Delete;
            btnQuery.Text = CommonResource.Query;
            btnSave.Text = CommonResource.Save;
            btnCancel.Text = CommonResource.Cancel;
            btnClose.Text = CommonResource.Close;
            btnPrint.Text = CommonResource.Print;
            #endregion
        }

        // These virtual functions are called after the ToolBarButton has been clicked
        protected virtual bool AfterNewClicked() { return true; }
        protected virtual bool AfterModifyClicked() { return true; }
        protected virtual bool AfterCancelClicked() { return true; }
        protected virtual bool AfterSaveClicked() { return true; }
        protected virtual bool AfterDeleteClicked() { return true; }
        protected virtual bool AfterPrintClicked() { return true; }

        protected virtual bool AfterQueryClicked() { return true; }

        protected void ManageUserAccess(string module, User user)
        {
            this.user = user;
            this.module = module;
            moduleAccess = UserAccess.GetUserAccessRightsForModuleItem(user, module);
            if (moduleAccess != null)
            {
                if (moduleAccess.Add == false)
                {
                    btnNewMaster.Enabled = false;
                }
                if (moduleAccess.Edit == false)
                {
                    btnEditMaster.Enabled = false;
                }
                if (moduleAccess.Print == false)
                {
                    btnPrint.Enabled = false;
                }
                if (moduleAccess.Delete == false)
                {
                    btnDeleteMaster.Enabled = false;
                }
                if (moduleAccess.Browse == false)
                {
                    btnQuery.Enabled = false;
                }
            }
        }

        protected virtual void SetSecurityControl()
        {
            //throw new FMException("Must be override");
        }

        protected virtual void LocalizeOtherControls() { }

        protected virtual void FillComboBox() { }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (AfterQueryClicked())
            {
                EnableAllButtonsForFormMode();
                SetSecurityControl();     
            }
        }

        protected virtual void frmEntryBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (formMode == FormMode.Delete)
            //{
            if (MessageBox.Show(CommonResource.ConfirmToCloseForm, CommonResource.Confirmation,
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CloseForm();
                }
                else
                    e.Cancel = true;
            //}
            //else
            //{
            //    MessageBox.Show("Some pending transaction(s), please save or cancel first before closing. ", CommonResource.Alert, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    e.Cancel = true;
            //}
        }

        protected object GetCurrentObjectFrombdsMaster()
        {
            return bdsMaster.Current;
        }
    }
}
