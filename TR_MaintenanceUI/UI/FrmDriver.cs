using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using TR_FormBaseLibrary;
using TR_LanguageResource.Resources;
using System.Globalization;
using System.Threading;


namespace FM.TR_MaintenanceUI.UI
{
    public partial class FrmDriver : frmEntryBase
    {
        public const string FORM_NAME = "DRIVER_SETUP";
        public Driver driver =new Driver();
        //private BindingSource bdsUnavailableDates = new BindingSource();
        public DriverUnavailable selectedUnavailableDate;
        private FrmDriverUnavailable frmdriverUnavailable;

        public FrmDriver(string moduleName, User userID)
        {
            InitializeComponent();
            base.module = moduleName;
            base.user = userID;
            dtgrdSeach.AutoGenerateColumns = false;
        }
      
        private void btnQuery_Click(object sender, EventArgs e)
        {
            bdsMaster.DataSource =Driver.GetDrivers(cboDriverCode.Text.Trim());
            if (bdsMaster.Count > 0)
            {
                btnDeleteMaster.Enabled = true;
            }
            else
            {
                btnDeleteMaster.Enabled = false;
            }
            SetSecurityControl();
        }

        protected override bool AfterNewClicked()
        {
            UnBindComponent();
            ClearField();    
            txtDriverCode.Enabled = true;
            pnlMaster.Enabled = true;
            //20140707 - gerry added
            bdsUnavailableDates.Clear(); 
            FillcboDefaultVehicle();
            gbxUnavailableDates.Enabled = false;
            //20140707 end
            return true;
        }
        protected override bool AfterModifyClicked()
        {
            try
            {
                UnBindComponent();
                if(driver==null)
                    driver = (Driver)bdsMaster.Current;
                FillcboDefaultVehicle();

                if (driver != null)
                {
                    PopulateData(driver);
                    txtDriverCode.Enabled = false;
                    pnlMaster.Enabled = true;
                }
                gbxUnavailableDates.Enabled = false;
                #region Removed
                /*
            if (driver != null)
            {
                
                dtpLicenceExpiryDate.Value =driver.licenceExpiryDate;
                cboDefaultVehicle.SelectedIndex = cboDefaultVehicle.FindString(driver.defaultVehicleNumber);
                txtDriverCode.Text = driver.Code.Trim();
                txtDriverName.Text = driver.Name.Trim();
                txtDrivingClass.Text = driver.DrivingClass.Trim();
                txtDrivingLicence.Text = driver.DrivingLicence.Trim();
                txtNationality.Text = driver.nationality.Trim();
                txtNIRC.Text = driver.nirc.Trim();
                //txtRatePerLadenTrip.Text = driver.ratePerLadenTrip.ToString().Trim();
                //txtRatePerUnladenTrip.Text = driver.ratePerUnladenTrip.ToString().Trim();
                chkAvailablity.Checked = driver.isAvailable;
                txtDriverCode.Enabled = false;
                pnlMaster.Enabled = true;
                
            }
            else
            {
                return;
            }
                 */
                #endregion
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return true;
        }
        protected override bool AfterDeleteClicked()
        {
            driver = (Driver)bdsMaster.Current;
            if (driver != null)
            {
                if (MessageBox.Show(CommonResource.DeleteConfirmation + " Driver?", CommonResource.Confirmation,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        driver.DeleteDriver(driver.Code.Trim(), FORM_NAME, user.UserID.ToString());
                        MessageBox.Show(CommonResource.DeleteSuccess, CommonResource.Alert);
                    }
                    catch (FMException ex)
                    {
                        MessageBox.Show(ex.ToString(), CommonResource.Error);
                    }
                }
            }
            #region Removed
                /*
                cboDefaultVehicle.SelectedIndex = cboDefaultVehicle.FindString(driver.defaultVehicleNumber);
                txtDriverCode.Text = driver.Code.Trim();
                txtDriverName.Text = driver.Name.Trim();
                txtDrivingClass.Text = driver.DrivingClass.Trim();
                txtDrivingLicence.Text = driver.DrivingLicence.Trim();
                txtNationality.Text = driver.nationality.Trim();
                txtNIRC.Text = driver.nirc.Trim();
                //txtRatePerLadenTrip.Text = driver.ratePerLadenTrip.ToString().Trim();
                //txtRatePerUnladenTrip.Text = driver.ratePerUnladenTrip.ToString().Trim();
                chkAvailablity.Checked = driver.isAvailable;
                 
                if (txtDriverCode.Text != "")
                {
                    if (MessageBox.Show("Are you sure want to delete it?", "Confirmation",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            driver.DeleteDriver(driver.Code.Trim());
                            //Driver.DeleteDriver(driver.Code.Trim());
                        }
                        catch (FMException ex)
                        {
                            MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                        }
                        
                        
                    }
                }
            }
            else
            {
                return;

            }
                 */
                #endregion
        

            return true;
        }
        protected override bool AfterSaveClicked()
        {
            bool clicked = true;
            try
            {
                switch (formMode)
                {
                    case FormMode.Add:
                        driver = new Driver();
                        driver = CollectDriverDetail(driver);
                        if (driver.AddDriver(driver, FORM_NAME, user.UserID))
                        {
                            MessageBox.Show(CommonResource.SaveSuccess, CommonResource.Alert);
                            PopulateData(driver);
                            formMode = FormMode.Delete;
                            FillcboDriverCode();
                        }
                        else
                        {
                            formMode = FormMode.Add;
                            clicked = false;
                        }
                        break;

                    case FormMode.Edit:
                        driver = CollectDriverDetail(driver);
                        if (Driver.EditDriver(driver, FORM_NAME, user.UserID))
                        {
                            MessageBox.Show(CommonResource.EditSuccess, CommonResource.Alert);
                            PopulateData(driver);
                            formMode = FormMode.Delete;   
                        }
                        else
                        {
                            formMode = FormMode.Edit;
                            clicked = false;
                        }
                        break;                 
                }
                FillcboDefaultVehicle();
            }
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString(), CommonResource.Error);
                clicked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
                clicked = false;
            }

            return clicked;
        }
        //20140707 - gerry modified based method
        protected override void EnableAllButtonsForFormMode()
        {
            base.EnableAllButtonsForFormMode();
            gbxUnavailableDates.Enabled = base.formMode == FormMode.Delete ? true : false;                 
        }
        protected override bool AfterCancelClicked()
        {
            if (driver != null)
            {
                PopulateData(driver);
                RefreshUnAvailableDatesGrid();
            }
            return base.AfterCancelClicked();
        }
        private void ClearField()
        {
            cboDefaultVehicle.SelectedIndex = -1;
            txtDriverCode.Text = "";
            txtDriverName.Text = "";
            txtDrivingClass.Text = "";
            txtDrivingLicence.Text = "";
            txtNationality.Text = "";
            txtNIRC.Text = "";
            //chkAvailablity.Checked = false;
        } 

        #region Remove
        /*
        private void EnableAllButtonsForFormMode()
        {
            btnDeleteMaster.Enabled = true;
            btnNewMaster.Enabled = true;
            btnEditMaster.Enabled = true;
            pnlQuery.Enabled = true;
            btnSave.Enabled = false;
        }
        
        private void EnableAllButtonsForFormMode(FormMode formMode)
        {

            if (formMode == FormMode.Add)
            {
                btnSave.Enabled = true;
                pnlQuery.Enabled = false;
                btnEditMaster.Enabled = false;
                btnDeleteMaster.Enabled = false;
            }
            else if (formMode == FormMode.Delete)
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
        }
        */
        #endregion
        protected override void FillComboBox()
        {
            try
            {
                FillcboDriverCode();
                FillcboDefaultVehicle();

                cboEmployeeStatus.Items.Clear();
                cboEmployeeStatus.Items.Add("Available");
                cboEmployeeStatus.Items.Add("Resigned");

                //20140627 - gerry added
                dtpUnavailableStartDate.Value = DateTime.Now.AddDays(-7);
                dtpUnavailableEndDate.Value = DateTime.Now.AddDays(7);

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
        private void FillcboDriverCode()
        {
            try
            {
                cboDriverCode.DataSource = Driver.GetAllDrivers();
                cboDriverCode.DisplayMember = "code";
                cboDriverCode.SelectedIndex = -1;
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
        private void FillcboDefaultVehicle()
        {
            try
            {
                SortableList<Vehicle> vehicleList = Vehicle.GetAllAvailableVehiclesForDefault();
                vehicleList.Insert(0, new Vehicle());
                cboDefaultVehicle.DataSource = vehicleList;
                cboDefaultVehicle.DisplayMember = "number";
                cboDefaultVehicle.SelectedIndex = -1;
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


        private Driver CollectDriverDetail(Driver driver)
        { 
            driver.Code = txtDriverCode.Text.Trim();
            driver.defaultVehicleNumber = cboDefaultVehicle.Text.Trim();
            driver.DrivingClass = txtDrivingClass.Text.Trim();
            driver.DrivingLicence = txtDrivingLicence.Text.Trim();
            //driver.isAvailable = chkAvailablity.Checked;
            driver.licenceExpiryDate = dtpLicenceExpiryDate.Value.Date;
            driver.Name = txtDriverName.Text.Trim();
            driver.nationality = txtNationality.Text.Trim();
            driver.nirc = txtNIRC.Text.Trim();
            if (cboEmployeeStatus.Text == "Available")
            {
                driver.EmployeeStatus = EmployeeStatus.Available;
            }
            else
            {
                driver.EmployeeStatus = EmployeeStatus.Resigned; 
            }

            return driver;
        }

        //Feb 23, 2011 - Gerry Added Methods for control
        #region  New Method and events

        private void UnBindComponent()
        {
            txtDriverCode.DataBindings.Clear();
            txtDriverName.DataBindings.Clear();
            txtDrivingClass.DataBindings.Clear();
            txtDrivingLicence.DataBindings.Clear();
            txtNationality.DataBindings.Clear();
            txtNIRC.DataBindings.Clear();
            cboEmployeeStatus.DataBindings.Clear();
            cboDefaultVehicle.DataBindings.Clear();
            dtpLicenceExpiryDate.DataBindings.Clear();
            //chkAvailablity.DataBindings.Clear();
        }

        private void PopulateData(Driver driver)
        {
            if (driver != null)
            {
                dtpLicenceExpiryDate.Value = driver.licenceExpiryDate;
                cboDefaultVehicle.Text = driver.defaultVehicleNumber.ToString();
                txtDriverCode.Text = driver.Code.Trim();
                txtDriverName.Text = driver.Name.Trim();
                txtDrivingClass.Text = driver.DrivingClass.Trim();
                txtDrivingLicence.Text = driver.DrivingLicence.Trim();
                txtNationality.Text = driver.nationality.Trim();
                txtNIRC.Text = driver.nirc.Trim();
                cboEmployeeStatus.Text = driver.EmployeeStatus.ToString();
                //20140626 - gerry removed
                //chkAvailablity.Checked = driver.isAvailable; 
            }         
        }

        private void tbcEntry_Click(object sender, EventArgs e)
        {
            SetTabControl(formMode);
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            cboDriverCode.SelectedIndex = -1;
        }

        protected override void LocalizeOtherControls()
        {
            lblWarnMsg.Text = TptResourceBLL.WarnHowtoAddEditUnavailableDates;
            this.Text = TptResourceUI.DriverEntry;
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
            lblDriverCode.Text = TptResourceUI.DriverCode + ":";
            lblCode.Text = TptResourceUI.DriverCode + ":";
            lblName.Text = TptResourceUI.DriverName + ":";
            lblNIRC.Text = TptResourceUI.NRIC + ":";
            lblNationality.Text = CommonResource.Nationality + ":";
            lblDrivingLicence.Text = TptResourceUI.DrivingLicenceNo + ":";
            lblDrivingClass.Text = TptResourceUI.DrivingClass + ":";
            lblLicenceExpiryDate.Text = TptResourceUI.DrivingExpiryDate + ":";
            lblDefaultVehicle.Text = TptResourceUI.DefaultVehicle + ":";
            lblEmployStatus.Text = TptResourceUI.EmployeeStatus + ":";
            //chkAvailablity.Text = TptResourceUI.IsAvailable;
            #endregion

            #region Data Grid 
            dtgrdSeach.Columns["priorityDataGridViewTextBoxColumn"].Visible = false;
            dtgrdSeach.Columns["codeDataGridViewTextBoxColumn"].DisplayIndex = 0;
            dtgrdSeach.Columns["nameDataGridViewTextBoxColumn"].DisplayIndex = 1;
            dtgrdSeach.Columns["drivingLicenceDataGridViewTextBoxColumn"].DisplayIndex = 2;
            dtgrdSeach.Columns["nationalityDataGridViewTextBoxColumn"].DisplayIndex = 3;
            dtgrdSeach.Columns["nircDataGridViewTextBoxColumn"].DisplayIndex =4;
            dtgrdSeach.Columns["drivingClassDataGridViewTextBoxColumn"].DisplayIndex = 5;
            dtgrdSeach.Columns["licenceExpiryDateDataGridViewTextBoxColumn"].DisplayIndex = 6;
            dtgrdSeach.Columns["defaultVehicleNumberDataGridViewTextBoxColumn"].DisplayIndex = 7;
            dtgrdSeach.Columns["employeeStatusDataGridViewTextBoxColumn"].DisplayIndex = 8;
            //dtgrdSeach.Columns["isAvailableDataGridViewCheckBoxColumn"].DisplayIndex = 9;
            dtgrdSeach.Columns["descriptionForPlanningPurposeDataGridViewTextBoxColumn"].DisplayIndex = 9;            


            dtgrdSeach.Columns["nameDataGridViewTextBoxColumn"].HeaderText = TptResourceUI.DriverName;
            dtgrdSeach.Columns["codeDataGridViewTextBoxColumn"].HeaderText = TptResourceUI.DriverCode;
            dtgrdSeach.Columns["nircDataGridViewTextBoxColumn"].HeaderText = TptResourceUI.NRIC;
            dtgrdSeach.Columns["nationalityDataGridViewTextBoxColumn"].HeaderText = CommonResource.Nationality;
            dtgrdSeach.Columns["drivingLicenceDataGridViewTextBoxColumn"].HeaderText = TptResourceUI.DrivingLicenceNo;
            dtgrdSeach.Columns["drivingClassDataGridViewTextBoxColumn"].HeaderText = TptResourceUI.DrivingClass;
            dtgrdSeach.Columns["licenceExpiryDateDataGridViewTextBoxColumn"].HeaderText = TptResourceUI.DrivingExpiryDate;
            dtgrdSeach.Columns["defaultVehicleNumberDataGridViewTextBoxColumn"].HeaderText = TptResourceUI.DefaultVehicle;
            dtgrdSeach.Columns["employeeStatusDataGridViewTextBoxColumn"].HeaderText = TptResourceUI.EmployeeStatus;
            //dtgrdSeach.Columns["isAvailableDataGridViewCheckBoxColumn"].HeaderText = TptResourceUI.IsAvailable;
            dtgrdSeach.Columns["descriptionForPlanningPurposeDataGridViewTextBoxColumn"].HeaderText = CommonResource.Description;
            #endregion

        }

        protected override void SetSecurityControl()
        {
            base.ManageUserAccess(module, user);
        }
        #endregion

        private void cboDriverCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.AutoCompleteComboBox((ComboBox)sender, e, true);
        }

        private void bdsMaster_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                driver = (Driver)bdsMaster.Current;
                PopulateData(driver);
                //20150618 - gerry added
                RefereshAssignmentGrid();
                //20150618 ended
                //20140627 - gerry added
                if (base.formMode == FormMode.Delete)
                {
                    if (driver != null)
                    {
                        if (driver.EmployeeStatus == EmployeeStatus.Available)
                            gbxUnavailableDates.Enabled = true;
                        else
                            gbxUnavailableDates.Enabled = false; // dont allow add/edit/delete for resigned employee
                    }
                }
                else
                    gbxUnavailableDates.Enabled = false;
                RefreshUnAvailableDatesGrid();
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
      
        private void btnUnavailableDateQuery_Click(object sender, EventArgs e)
        {
            try
            {
                bdsUnavailableDates.DataSource = driver.unAvailableDates.Where(ud => ud.startDateTime >= dtpUnavailableStartDate.Value.AddHours(dtpUnavailableStartTime.Value.Hour).AddMinutes(dtpUnavailableStartTime.Value.Minute)
                                                           && ud.startDateTime <= dtpUnavailableEndDate.Value.AddHours(dtpUnavailableEndTime.Value.Hour).AddMinutes(dtpUnavailableEndTime.Value.Minute)).ToList();
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
    
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                frmdriverUnavailable = new FrmDriverUnavailable(this, FormMode.Add);
                frmdriverUnavailable.ShowDialog();

                RefreshUnAvailableDatesGrid();
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

        private void btnEditUnavailableDate_Click(object sender, EventArgs e)
        {
            try
            {
                selectedUnavailableDate = (DriverUnavailable)bdsUnavailableDates.Current;
                frmdriverUnavailable = new FrmDriverUnavailable(this, FormMode.Edit);
                frmdriverUnavailable.ShowDialog();

                RefreshUnAvailableDatesGrid();
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

        private void btnDeleteUnavailableDate_Click(object sender, EventArgs e)
        {
            try
            {
                selectedUnavailableDate = (DriverUnavailable)bdsUnavailableDates.Current;
                driver.DeleteDriverUnavailable(selectedUnavailableDate);

                RefreshUnAvailableDatesGrid();
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
        private void RefreshUnAvailableDatesGrid()
        {
            try
            {
                if (driver != null)
                {
                    //driver.unAvailableDates = driver.GetDriverUnavailableDates(dtpUnavailableStartDate.Value.AddHours(dtpUnavailableStartTime.Value.Hour).AddMinutes(dtpUnavailableStartTime.Value.Minute)
                    //                                                        , dtpUnavailableEndDate.Value.AddHours(dtpUnavailableEndTime.Value.Hour).AddMinutes(dtpUnavailableEndTime.Value.Minute)); 
                    driver.unAvailableDates = driver.GetDriverUnavailableDates(DateUtility.GetSQLDateTimeMinimumValue(), DateUtility.GetSQLDateTimeMaximumValue());
                    bdsUnavailableDates.DataSource = driver.unAvailableDates;
                    dgvUnavailableDates.DataSource = bdsUnavailableDates;

                    btnEditUnvailableDate.Enabled = bdsUnavailableDates.Count > 0;
                    btnDeleteUnavailableDate.Enabled = bdsUnavailableDates.Count > 0; 
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

        }

        private void dtpStartTimeTo_FormatChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.CustomFormat = "HH:mm";
        }

        private void dtpUnavailableStartDate_ValueChanged(object sender, EventArgs e)
        {
            dtpUnavailableStartTime.Value = dtpUnavailableStartDate.Value.Date.AddHours(0);
            if (dtpUnavailableStartDate.Value > dtpUnavailableEndDate.Value)
                dtpUnavailableEndDate.Value = dtpUnavailableStartDate.Value;
        }

        private void dtpUnavailableEndDate_ValueChanged(object sender, EventArgs e)
        {
            dtpUnavailableEndTime.Value = dtpUnavailableEndDate.Value.Date.AddHours(23).AddMinutes(59);
            if (dtpUnavailableStartDate.Value > dtpUnavailableEndDate.Value)
                dtpUnavailableEndDate.Value = dtpUnavailableStartDate.Value;
        }
        private void RefereshAssignmentGrid()
        {
            try
            {
                if (driver != null)
                {
                    SortableList<DriverPriority> list = DriverPriority.GetDriverPriority(driver.Code, dtpAssignmentFrom.Value, dtpAssignmentTo.Value);
                    if (list.Count > 0)
                        bdsAssignmentByDriver.DataSource = list.OrderBy(drp => drp.ScheduleDate).ToList();
                    else
                        bdsAssignmentByDriver.DataSource = list;

                    dgvAssignmentsByDriver.DataSource = bdsAssignmentByDriver;
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        private void btnGetJobAssignments_Click(object sender, EventArgs e)
        {
            try
            {
                RefereshAssignmentGrid();
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

        private void btnAddAssignments_Click(object sender, EventArgs e)
        {
            try
            {
                if (driver != null)
                {
                    List<Driver> selectedDrivers = new List<Driver>();
                    selectedDrivers.Add(driver);
                    FrmAddDriverAssignment addDriverAssignment = new FrmAddDriverAssignment();
                    addDriverAssignment.drivers = selectedDrivers;
                    addDriverAssignment.ShowDialog();

                    RefereshAssignmentGrid();
                    RefereshAssignmentSummaryGrid();
                }
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

        private void dgvAssignments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DriverPriority currDrvPriority = (DriverPriority)bdsAssignmentByDriver.Current;
                if (currDrvPriority != null)
                {
                    FrmDriverJobAssignment driverJobAssignment = new FrmDriverJobAssignment(currDrvPriority);
                    driverJobAssignment.driver = driver;
                    driverJobAssignment.ShowDialog();

                    RefereshAssignmentGrid();
                    RefereshAssignmentSummaryGrid();
                }
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

        private void dtpAssignmentFrom_ValueChanged(object sender, EventArgs e)
        {
            if (dtpAssignmentFrom.Value > dtpAssignmentTo.Value)
                dtpAssignmentTo.Value = dtpAssignmentFrom.Value;
        }

        private void dtpAssignmentTo_ValueChanged(object sender, EventArgs e)
        {
            if (dtpAssignmentFrom.Value > dtpAssignmentTo.Value)
                dtpAssignmentFrom.Value = dtpAssignmentTo.Value;
        }

        private void dtpJobSummaryDateFrom_ValueChanged(object sender, EventArgs e)
        {
            if (dtpJobSummaryDateFrom.Value > dtpJobSummaryDateTo.Value)
                dtpJobSummaryDateTo.Value = dtpJobSummaryDateFrom.Value;
        }

        private void dtpJobSummaryDateTo_ValueChanged(object sender, EventArgs e)
        {
            if (dtpJobSummaryDateFrom.Value > dtpJobSummaryDateTo.Value)
                dtpJobSummaryDateFrom.Value = dtpJobSummaryDateTo.Value;
        }

        private void btnAssignmentSummary_Click(object sender, EventArgs e)
        {
            try
            {
                RefereshAssignmentSummaryGrid();
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

        private void RefereshAssignmentSummaryGrid()
        {
            try
            {
                SortableList<DriverPriority> list = DriverPriority.GetJobAssignmentPriorityByDate(dtpJobSummaryDateFrom.Value, dtpJobSummaryDateTo.Value);
                if (list.Count > 0)
                    bdsAssignmentSummary.DataSource = list.OrderBy(drp => drp.ScheduleDate);
                else
                    bdsAssignmentSummary.DataSource = list;

                dgvAssignmentSummary.DataSource = bdsAssignmentSummary;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        private void dgvAssignmentSummary_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DriverPriority currDrvPriority = (DriverPriority)bdsAssignmentSummary.Current;
                if (currDrvPriority != null)
                {
                    FrmDriverJobAssignment driverJobAssignment = new FrmDriverJobAssignment(currDrvPriority);
                    driverJobAssignment.driver = driver;
                    driverJobAssignment.ShowDialog();

                    RefereshAssignmentGrid();
                    RefereshAssignmentSummaryGrid();
                }
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

        private void dtgrdSeach_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
           
            if (e.Button == MouseButtons.Right)
            { 
                
            }
        }

        private void FrmDriver_Load(object sender, EventArgs e)
        {
            try
            {
                FillComboBox();
                btnPrint.Enabled = false;
                btnClear.PerformClick();
                dtpLicenceExpiryDate.Value = DateTime.Today.AddYears(5);
                dgvUnavailableDates.AutoGenerateColumns = false;
                dtgrdSeach.ContextMenuStrip = dgvContextMenu;
                dtpUnavailableStartDate.Value = DateTime.Today;
                dtpUnavailableStartTime.Value = DateTime.Today;
                dtpUnavailableEndDate.Value = DateTime.Today;
                dtpUnavailableEndTime.Value = DateTime.Today;
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

        private void addPriorityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //TODO multiple driver assign priority 
                List<Driver> selectedDrivers = new List<Driver>();
                string noPMDrivers = string.Empty;
                int count = 0;
                foreach (DataGridViewRow row in this.dtgrdSeach.SelectedRows)
                {
                    Driver drv = this.bdsMaster[row.Index] as Driver;
                    if (drv.defaultVehicleNumber != string.Empty)
                        selectedDrivers.Add(drv);
                    else
                    {
                        count++;
                        noPMDrivers += count > 1 ? ", ":string.Empty; 
                        noPMDrivers += drv.Name;
                    }
                }
                if (noPMDrivers != string.Empty)
                    MessageBox.Show("Drivers with no default prime mover cannot be assign a priority. (" + noPMDrivers + ")", CommonResource.Warning);

                if (selectedDrivers.Count > 0)
                {
                    FrmAddDriverAssignment addDriverAssignment = new FrmAddDriverAssignment();
                    addDriverAssignment.drivers = selectedDrivers;
                    addDriverAssignment.ShowDialog();
                }
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
    }
}
