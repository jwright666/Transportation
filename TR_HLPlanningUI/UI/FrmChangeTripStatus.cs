using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using TR_LanguageResource.Resources;
using TR_MessageDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_HLPlanDLL.BLL;
using TR_FormBaseLibrary;
using FM.TR_HLBookDLL.BLL;


namespace FM.TransportPlanning.UI
{
    public partial class FrmChangeTripStatus : frmEntryBase
    {
        public const string FORM_NAME = "FRM_CHANGE_TRIP_STATUS";
        SortableList<HaulierJobTrip> haulierJobTrips = new SortableList<HaulierJobTrip>();
        JobTripStatus jobTripStatus = new JobTripStatus();
        HaulierJobTrip haulierJobTrip = new HaulierJobTrip();

        string containerNo = "";
        bool ownTransport;
        List<HaulierJobTrip> selectedSubConJobTrips = new List<HaulierJobTrip>();

        public FrmChangeTripStatus()
        {            
            InitializeComponent();
            //dtpStartDate.CustomFormat = " ";
            //dtpStartDate.Format = DateTimePickerFormat.Custom;
            //dtpEndDate.CustomFormat = " ";
            //dtpEndDate.Format = DateTimePickerFormat.Custom;
            dtpStartDate.Value = DateTime.Today;
            dtpEndDate.Value = DateTime.Today;
            InitGrid();

           // HideAndDisplayControlsForSubConTripsMultiSelection(); // 2014-11-18 Zhou Kai
        }

        /*
        private void EnableButtonsAndPanels(FormMode formMode)
        {
            if (formMode == FormMode.Edit)
            {
                btnNewMaster.Enabled = false;
                btnDeleteMaster.Enabled = false;
                btnEditMaster.Enabled = false;
                pnlMaster.Enabled = true;
                pnlQuery.Enabled = true;
                btnSave.Enabled = true;
            }
            else
            {
                btnNewMaster.Enabled = false;
                btnEditMaster.Enabled = false;
                pnlMaster.Enabled = false;
                pnlQuery.Enabled = true;
                btnSave.Enabled = false;
            }
        }
        */

        //private void EnableButtonsAndPanels(FormMode formMode, bool set)
        //{
        //    if (formMode == FormMode.Edit)
        //    {
        //        btnNewMaster.Enabled = set;
        //        btnDeleteMaster.Enabled = set;
        //        btnEditMaster.Enabled = !set;
        //        pnlQuery.Enabled = set;
        //        pnlMaster.Enabled = set;
        //    }
        //    else
        //    {
        //        btnNewMaster.Enabled = !set;
        //        btnDeleteMaster.Enabled = !set;
        //        btnEditMaster.Enabled = set;
        //        pnlQuery.Enabled = set;
        //        pnlMaster.Enabled = !set;
        //    }
        //}

        private void btnQuery_Click(object sender, EventArgs e)
        {
            FilterDataForCriteria();  
            bdsMaster.DataSource = haulierJobTrips;

            if (bdsMaster.Count <= 0)
                MessageBox.Show(CommonResource.NoRecordFound, CommonResource.Alert);
            else
            {
                cboToStatus.Visible = rdbNo.Checked;
                lblToStatus.Visible = rdbNo.Checked;
            }

            //EnableButtonsAndPanels(FormMode.Delete, true);
            
            //20150829 - gerry removed
            // 2014-11-18 Zhou Kai adds
            //if (!(ownTransport || cboStatus.Text == String.Empty))
            //{
            //    btnSave.Enabled = (cboToStatus.Text != String.Empty);
            //}
            //20150829 - gerry end

            RefreshGridForSelectedTrips(this.dgvJobTrip);
            // 2014-11-18 Zhou Kai ends
            
        }      

        private void BindComponent()
        {
            //txtEditJobID.DataBindings.Add("Text", bdsMaster, "JobNo");
            //txtEditSequence.DataBindings.Add("Text", bdsMaster, "Sequence");
            //txtEditContainerNo.DataBindings.Add("Text", bdsMaster, "ContainerNo");
            //txtEditContainerCode.DataBindings.Add("Text", bdsMaster, "ContainerCode");
            //txtCurrentStatus.DataBindings.Add("Text", bdsMaster, "TripStatus");            
            //chkOwnTransport.DataBindings.Add("Checked", bdsMaster, "OwnTransport");
            if (haulierJobTrip != null)
            {
                txtEditJobID.Text = haulierJobTrip.JobNo;
                txtEditSequence.Text = haulierJobTrip.Sequence.ToString();
                txtEditContainerNo.Text = haulierJobTrip.ContainerNo;
                txtEditContainerCode.Text = haulierJobTrip.ContainerCode;
                txtCurrentStatus.Text = haulierJobTrip.TripStatus.ToString();
                chkOwnTransport.Checked = haulierJobTrip.OwnTransport;

                cboEditStatus.Items.Clear();
                if (!haulierJobTrip.OwnTransport)
                {
                    cboEditStatus.Items.Add(JobTripStatus.Booked);
                    cboEditStatus.Items.Add(JobTripStatus.Ready);
                    cboEditStatus.Items.Add(JobTripStatus.Assigned);
                    cboEditStatus.Items.Add(JobTripStatus.Completed);
                    cboEditStatus.Items.Remove(haulierJobTrip.TripStatus);
                }
                else
                {
                    if (haulierJobTrip.TripStatus == JobTripStatus.Booked)
                    {
                        cboEditStatus.Items.Add(JobTripStatus.Ready);
                    }
                    else if (haulierJobTrip.TripStatus == JobTripStatus.Ready)
                    {
                        cboEditStatus.Items.Add(JobTripStatus.Booked);
                    }
                }
                cboEditStatus.Enabled = true;
            }
        }

        private void UnbindComponent()
        {
            //txtEditJobID.DataBindings.Clear();
            //txtEditSequence.DataBindings.Clear();
            //txtEditContainerNo.DataBindings.Clear();
            //txtEditContainerCode.DataBindings.Clear();
            //txtCurrentStatus.DataBindings.Clear();
            //chkOwnTransport.DataBindings.Clear();
            txtEditJobID.Text = string.Empty;
            txtEditSequence.Text = string.Empty;
            txtEditContainerNo.Text = string.Empty;
            txtEditContainerCode.Text = string.Empty;
            txtCurrentStatus.Text = string.Empty;
        }

        private void FilterDataForCriteria()
        {
            haulierJobTrips = new SortableList<HaulierJobTrip>();
            JobTripStatus tempjtripstatus = new JobTripStatus();
            if (cboStatus.SelectedIndex == 0) tempjtripstatus = JobTripStatus.Booked;
            if (cboStatus.SelectedIndex == 1) tempjtripstatus = JobTripStatus.Ready;
            if (cboStatus.SelectedIndex == 2) tempjtripstatus = JobTripStatus.Assigned;
            if (cboStatus.SelectedIndex == 3) tempjtripstatus = JobTripStatus.Completed;
            if (cboStatus.SelectedIndex == 4) tempjtripstatus = JobTripStatus.Invoiced;
            if (rdbYes.Checked) ownTransport = true;
            if (rdbNo.Checked) ownTransport = false;
            if (rdbBoth.Checked)
            {
                if (cboStatus.Text != string.Empty)
                {
                    if (rdbDate.Checked)
                        haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByDate(tempjtripstatus, dtpStartDate.Value.Date, dtpEndDate.Value.Date);
                    else
                        haulierJobTrips = HaulierJobTrip.GetHaulierJobTrips(tempjtripstatus, cboDepartment.Text.ToString(), cboStartJobID.Text.ToString(), cboEndJobID.Text.ToString());
                }
                else
                {
                    if (rdbDate.Checked)
                        haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByDate(dtpStartDate.Value.Date, dtpEndDate.Value.Date);
                    else
                        haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByJobs(cboDepartment.Text.ToString(), cboStartJobID.Text.ToString(), cboEndJobID.Text.ToString());
                }
                #region 201908 - gerry replaced
                /*
                if ((cboStatus.Text == "") & (cboDepartment.Text == "") & (rdbJobNo.Checked) & (cboStartJobID.Text == "") & (cboEndJobID.Text == ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetAllHaulierJobTrips();
                }
                else if ((cboStatus.Text != "") & (cboDepartment.Text == "") & (rdbJobNo.Checked) & (cboStartJobID.Text == "") & (cboEndJobID.Text == ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTrips(tempjtripstatus);
                }
                else if ((cboStatus.Text == "") & (cboDepartment.Text != "") & (rdbJobNo.Checked) & (cboStartJobID.Text == "") & (cboEndJobID.Text == ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByDept(cboDepartment.Text.ToString());
                }
                else if ((cboStatus.Text == "") & (cboDepartment.Text != "") & (rdbJobNo.Checked) & (cboStartJobID.Text != "") & (cboEndJobID.Text != ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByJobs(cboDepartment.Text.ToString(), cboStartJobID.Text.ToString(), cboEndJobID.Text.ToString());
                }
                else if ((cboStatus.Text == "") & (cboDepartment.Text == "") & (rdbJobNo.Checked) & (cboStartJobID.Text != "") & (cboEndJobID.Text != ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByJobs(cboStartJobID.Text.ToString(), cboEndJobID.Text.ToString());
                }
                else if ((cboStatus.Text != "") & (cboDepartment.Text != "") & (rdbDate.Checked) & (dtpStartDate.Text != ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTrips(tempjtripstatus, cboDepartment.Text.ToString(), dtpStartDate.Value, dtpEndDate.Value);
                }
                else if ((cboStatus.Text == "") & (cboDepartment.Text != "") & (rdbDate.Checked) & (dtpStartDate.Text == "") & (dtpEndDate.Text == ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByDept(cboDepartment.Text);
                }
                else if ((cboStatus.Text != "") && (cboDepartment.Text != "") & (rdbJobNo.Checked) & (cboStartJobID.Text == "") & (cboEndJobID.Text == ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByDeptAndStatus(tempjtripstatus, cboDepartment.Text.ToString());
                }
                else if ((cboStatus.Text != "") & (cboDepartment.Text != "") & (rdbJobNo.Checked) & (cboStartJobID.Text != "") & (cboEndJobID.Text != ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTrips(tempjtripstatus, cboDepartment.Text.ToString(), cboStartJobID.Text.ToString(), cboEndJobID.Text.ToString());
                }
                else if ((cboStatus.Text != "") & (cboDepartment.Text == "") & (rdbDate.Checked) & (dtpStartDate.Text != "") & (dtpEndDate.Text != ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByDate(tempjtripstatus, dtpStartDate.Value, dtpEndDate.Value);
                }
                else if ((cboStatus.Text == "") & (cboDepartment.Text == "") & (rdbDate.Checked) & (dtpStartDate.Text != "") & (dtpEndDate.Text != ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByDate(dtpStartDate.Value, dtpEndDate.Value);
                }
                else if ((cboStatus.Text == "") & (cboDepartment.Text != "") & (rdbDate.Checked) & (dtpStartDate.Text != "") & (dtpEndDate.Text != ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByDate(cboDepartment.Text.ToString(), dtpStartDate.Value, dtpEndDate.Value);
                }
                 */
                #endregion
            }
            else
            {
                if (cboStatus.Text != string.Empty)
                {
                    if (rdbDate.Checked)
                        haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByDate(tempjtripstatus, ownTransport, dtpStartDate.Value.Date, dtpEndDate.Value.Date);
                    else
                        haulierJobTrips = HaulierJobTrip.GetHaulierJobTrips(tempjtripstatus, ownTransport, cboDepartment.Text.ToString(), cboStartJobID.Text.ToString(), cboEndJobID.Text.ToString());
                }
                else
                {
                    if (rdbDate.Checked)
                        haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByDate(ownTransport, dtpStartDate.Value.Date, dtpEndDate.Value.Date);
                    else
                    {
                        if (cboStartJobID.Text.ToString() != string.Empty)
                            haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByJobs(ownTransport, cboStartJobID.Text.ToString(), cboEndJobID.Text.ToString());
                        else
                            haulierJobTrips = HaulierJobTrip.GetHaulierJobTrips(ownTransport);
                    }
                }
                #region 20150908 - gerry replaced
                /*
                if ((cboStatus.Text == "") & (cboDepartment.Text == "") & (rdbJobNo.Checked) & (cboStartJobID.Text == "") & (cboEndJobID.Text == ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTrips(ownTransport);
                }
                else if ((cboStatus.Text == "") & (cboDepartment.Text != "") & (rdbJobNo.Checked) & (cboStartJobID.Text == "") & (cboEndJobID.Text == ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTrips(cboDepartment.Text.ToString(), ownTransport);
                }
                else if ((cboStatus.Text != "") & (cboDepartment.Text == "") & (!rdbDate.Checked) & (cboStartJobID.Text == "") & (cboEndJobID.Text == ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTrips(tempjtripstatus, ownTransport);
                }
                else if ((cboStatus.Text != "") & (cboDepartment.Text != "") & (!rdbDate.Checked) & (cboStartJobID.Text == "") & (cboEndJobID.Text == ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTrips(tempjtripstatus, cboDepartment.Text.ToString(), ownTransport);
                }
                else if ((cboStatus.Text != "") & (cboDepartment.Text != "") & (rdbJobNo.Checked) & (rdbJobNo.Checked) & (cboStartJobID.Text != "") & (cboEndJobID.Text != ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTrips(tempjtripstatus, ownTransport, cboDepartment.Text.ToString(), cboStartJobID.Text, cboEndJobID.Text);
                }
                else if ((cboStatus.Text == "") & (cboDepartment.Text == "") & (rdbJobNo.Checked) & (cboStartJobID.Text != "") & (cboEndJobID.Text != ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByJobs(ownTransport, cboStartJobID.Text.ToString(), cboEndJobID.Text.ToString());
                }
                else if ((cboStatus.Text == "") & (cboDepartment.Text != "") & (rdbJobNo.Checked) & (cboStartJobID.Text != "") & (cboEndJobID.Text != ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByJobs(ownTransport, cboDepartment.Text.ToString(), cboStartJobID.Text.ToString(), cboEndJobID.Text.ToString());
                }
                else if ((cboStatus.Text != "") & (cboDepartment.Text != "") & (rdbDate.Checked) & (dtpStartDate.Text != "") & (dtpEndDate.Text != ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTrips(tempjtripstatus, ownTransport, cboDepartment.Text.ToString(), dtpStartDate.Value, dtpEndDate.Value);
                }
                else if ((cboStatus.Text == "") & (cboDepartment.Text == "") & (rdbDate.Checked) & (dtpStartDate.Text != "") & (dtpEndDate.Text != ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByDate(ownTransport, dtpStartDate.Value, dtpEndDate.Value);
                }
                else if ((cboStatus.Text != "") & (cboDepartment.Text == "") & (rdbDate.Checked) & (dtpStartDate.Text != "") & (dtpEndDate.Text != ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByDate(tempjtripstatus, ownTransport, dtpStartDate.Value, dtpEndDate.Value);
                }
                else if ((cboStatus.Text == "") & (cboDepartment.Text != "") & (rdbDate.Checked) & (dtpStartDate.Text != "") & (dtpEndDate.Text != ""))
                {
                    haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByDate(ownTransport, cboDepartment.Text.ToString(), dtpStartDate.Value, dtpEndDate.Value);
                }
                 */
                #endregion
            }

        }

        protected override void SetSecurityControl()
        {
            base.ManageUserAccess(module, user);
        }
        private void InitGrid()
        {
            //BindComponent();
            dgvJobTrip.AutoGenerateColumns = false;
            //SortableList<HaulierJob> h1 = new SortableList<HaulierJob>();
            //h1 = HaulierJob.GetAllHaulierJobs();


            #region Removed
            //cboStartJobID.Items.Clear();
            //cboStartJobID.Items.Add("");
            //cboEndJobID.Items.Clear();
            //cboEndJobID.Items.Add("");
            //for (int i = 0; i < h1.Count; i++)
            //{
            //    cboStartJobID.Items.Add(h1[i].JobNo);
            //    cboEndJobID.Items.Add(h1[i].JobNo);
            //}
            #endregion

            updateVersionDataGridViewImageColumn.Visible = false;
            colSelected.DisplayIndex = 0;
            colSelected.Visible = false;
            jobNoDataGridViewTextBoxColumn.DisplayIndex = 1;
            legGroupDataGridViewTextBoxColumn.DisplayIndex = 2;
            legTypeCustomizedDataGridViewTextBoxColumn.DisplayIndex = 3;
            startStopDataGridViewTextBoxColumn.DisplayIndex = 4;
            endStopDataGridViewTextBoxColumn.DisplayIndex = 5;
            containerNoDataGridViewTextBoxColumn.DisplayIndex = 6;
            tripStatusDataGridViewTextBoxColumn.DisplayIndex = 7;
            subConDataGridViewTextBoxColumn.DisplayIndex = 8;
            vesselNameDataGridViewTextBoxColumn.DisplayIndex = 9;
            voyageNoDataGridViewTextBoxColumn.DisplayIndex = 10;
            startDateDataGridViewTextBoxColumn.DisplayIndex = 11;
            endDateDataGridViewTextBoxColumn.DisplayIndex = 12;
            bookRefNoDataGridViewTextBoxColumn.DisplayIndex = 13;
            sourceRefNoDataGridViewTextBoxColumn.DisplayIndex = 14;
            customerCodeDataGridViewTextBoxColumn.DisplayIndex = 15;
            customerNameDataGridViewTextBoxColumn.DisplayIndex = 15; 
            //ReArrangeColumns();
        }

        //currently not in use
        private void ReArrangeColumns()
        {
            if (dgvJobTrip != null)
            {
                string colName = string.Empty;
                dgvJobTrip.Columns["updateVersionDataGridViewImageColumn"].Visible = false;
                try
                {
                    dgvJobTrip.Columns["colSelected"].DisplayIndex = 0;
                    ArrayList changeJobTripStatusColumnsFromSettings = new ArrayList(); //FMGlobalSettings.TheInstance.GetChangeJobTripStatusColumns();
                    if (changeJobTripStatusColumnsFromSettings.Count > 0)
                    {
                        for (int i = 0; i < changeJobTripStatusColumnsFromSettings.Count; i++)
                        {
                            colName = changeJobTripStatusColumnsFromSettings[i].ToString();
                            dgvJobTrip.Columns[colName].DisplayIndex = i + 1;
                            dgvJobTrip.Columns[colName].SortMode = DataGridViewColumnSortMode.Programmatic;
                        }
                    }
                    colSelected.Visible = false;
                }
                catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Alert); }
                catch (Exception ex) { MessageBox.Show(colName + " - " + ex.Message.ToString(), CommonResource.Alert); }
            }
        }
        protected override bool AfterModifyClicked()
        {
            bool clicked = true;
            try
            {
                if (selectedSubConJobTrips.Count > 0)
                    throw new FMException("For multiple subcon job trips, please select subConTrip Status instead. ");
                //UnbindComponent();
                if (bdsMaster.Count != 0)
                {
                    haulierJobTrip = (HaulierJobTrip)bdsMaster.Current;

                    if (haulierJobTrip != null)
                    {
                        if (haulierJobTrip.TripStatus == JobTripStatus.Completed)
                        {
                            cboEditStatus.Enabled = false;
                            MessageBox.Show(TptResourceBLL.ErrCurrentStatusIsComplete, CommonResource.Alert);
                            clicked = false;
                        }
                        else if (haulierJobTrip.TripStatus == JobTripStatus.Invoiced)
                        {
                            cboEditStatus.Enabled = false;
                            MessageBox.Show(TptResourceBLL.ErrCurrentStatusIsInvoiced, CommonResource.Alert);
                            clicked = false;
                        }
                        else if (haulierJobTrip.TripStatus == JobTripStatus.Assigned && haulierJobTrip.OwnTransport)
                        {
                            cboEditStatus.Enabled = false;
                            MessageBox.Show("For not subcon job trip(s), Assigned status is not allowed to change from here because its already in planning schedule.", CommonResource.Alert);
                            clicked = false;
                        }
                        BindComponent();
                    }
                    ////20130416 - gerry change the condition
                    ////else if (jobTripStatus == JobTripStatus.Assigned)
                    //else if (jobTripStatus == JobTripStatus.Assigned && haulierJobTrip.OwnTransport)
                    //{
                    //    cboEditStatus.Enabled = false;
                    //    MessageBox.Show(TptResourceBLL.ErrCurrentStatusIsAssigned, CommonResource.Alert);
                    //    clicked = false;
                    //}
                    //else
                    //{
                    //    //FillComboBox();
                    //    cboEditStatus.Enabled = true;
                    //}
                    //cboEditStatus.Items.Remove(jobTripStatus);
                }
            }
            catch (FMException fme) { MessageBox.Show(fme.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
            //EnableButtonsAndPanels(mode, true);
            return clicked;
        }

        // 2014-11-18 Zhou Kai adds a branch into this function:
        // for multi-selecting of sub-con trips and change the status of the selected together
        protected override bool AfterSaveClicked()
        {
            bool clicked = true;
            try
            {
                selectedSubConJobTrips = GetSelectedTripsFromDGV(this.dgvJobTrip);
                if (selectedSubConJobTrips.Count > 0 && cboToStatus.SelectedItem != null)
                {
                    string msg = SaveSubConTripStatus(selectedSubConJobTrips, (JobTripStatus)cboToStatus.SelectedItem);
                    if (msg == String.Empty) // saved successfully
                    {
                        string msgSuccess = String.Format("Success, {0} subcon job trip(s) set to {1}.", selectedSubConJobTrips.Count.ToString(), cboToStatus.Text);
                        MessageBox.Show(msgSuccess, CommonResource.SaveSuccess);

                        FilterDataForCriteria();
                        bdsMaster.DataSource = haulierJobTrips;
                        formMode = FormMode.Delete;
                        clicked = true;
                    }
                    else
                        throw new FMException(msg);
                }
                else
                {
                    bool validationcheck = true;
                    string message = "";
                    if (cboEditStatus.SelectedItem != null)
                    {
                        JobTripStatus oldJobTripStatus = haulierJobTrip.TripStatus;
                        JobTripStatus newJobTripStatus = (JobTripStatus)cboEditStatus.SelectedItem;

                        validationcheck = haulierJobTrip.CanChangeStatus(oldJobTripStatus, newJobTripStatus,
                                                        ownTransport, out message);
                        if (validationcheck)
                        {
                            if (newJobTripStatus != oldJobTripStatus)
                            {
                                JobTripState jobTripState = new JobTripState(1, newJobTripStatus, DateTime.Today, txtRemarks.Text, true);
                                haulierJobTrip.AddJobTripState(jobTripState, "PL", user.UserID, FORM_NAME);

                                MessageBox.Show(CommonResource.SaveSuccess, CommonResource.Alert);
                                formMode = FormMode.Delete;
                                clicked = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show(message.ToString(), CommonResource.Alert);
                            clicked = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show(TptResourceBLL.WarnSetNewStatus, CommonResource.Warning);
                        cboEditStatus.Focus();
                        clicked = false;
                    }
                }
            }
            catch (FMException fme) { MessageBox.Show(fme.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
            return clicked;
            #region 20150917 - gerry removed
            //// 2014-11-18 Zhou Kai adds new logic
            //JobTripStatus newStatus = JobTripStatus.Ready;
            //if (cboToStatus.SelectedIndex == 0) { newStatus = JobTripStatus.Ready; }
            //if (cboToStatus.SelectedIndex == 1) { newStatus = JobTripStatus.Assigned; }
            //if (cboToStatus.SelectedIndex == 2) { newStatus = JobTripStatus.Completed; }
            //List<HaulierJobTrip> selectedTrips = GetSelectedTripsFromDGV(this.dgvJobTrip);
            //if (selectedTrips.Count > 0) // means the user has multi-selected sub-con trips
            //{
            //    string msg = SaveSubConTripStatus(selectedTrips, newStatus);
            //    if (msg == String.Empty) // saved successfully
            //    {
            //        string msgSuccess = String.Format(TptResourceUI.SuccessSavedStatus, 
            //            selectedTrips.Count, cboStatus.Text, cboToStatus.Text);
            //        MessageBox.Show(msgSuccess, CommonResource.SaveSuccess);

            //        FilterDataForCriteria();
            //        bdsMaster.DataSource = haulierJobTrips;
            //        return true;
            //    }
            //    else { MessageBox.Show(msg, CommonResource.Error); return false; }
            //} // else follow the old logic
            //// 2014-11-18 Zhou Kai ends
            //// the existing logic:
            //bool clicked = true;
            //UnbindComponent();
            //bool validationcheck = true;
            //string message = "";

            //haulierJobTrip = (HaulierJobTrip)bdsMaster.Current;  
            ////haulierJobTrip.UpdateVersion = haulierJobTrip.GetHaulierJobTripUpdateVersion();
            //if (chkOwnTransport.Checked)
            //{ ownTransport = true; }
            //else
            //{
            //    ownTransport = false;
            //    // 2014-11-19 Zhou Kai adds new logic for modifying a sub-con trip here:
            //    HaulierJob currentJob = HaulierJob.GetHaulierJob(haulierJobTrip.JobNo);
            //    KeyValuePair<HaulierJobTrip, bool> kvp = HaulierJobTrip.GetPreviousLeg(currentJob, haulierJobTrip);
            //    string msg = SaveSubConTripStatus(new List<HaulierJobTrip>() { haulierJobTrip}, newStatus);
            //    if (msg == String.Empty)
            //    {
            //        string msgSuccess = String.Format(TptResourceUI.SuccessSavedStatus, 1, cboStatus.Text, cboToStatus.Text);
            //        MessageBox.Show(msgSuccess, CommonResource.SaveSuccess);

            //        FilterDataForCriteria();
            //        bdsMaster.DataSource = haulierJobTrips;
            //        return true;
            //    }
            //    else { MessageBox.Show(msg, CommonResource.Error); return false; }

            //    // 2014-11-19
            //}            
            #endregion
        }

        private void rdbJobNo_CheckedChanged(object sender, EventArgs e)
        {
            radioBtnControl();
        }

        private void rdbDate_CheckedChanged(object sender, EventArgs e)
        {
            radioBtnControl();
        }

        private void radioBtnControl()
        {
            if (rdbJobNo.Checked)
            {
                lblJobID.Visible = true;
                cboStartJobID.Visible = true;
                cboEndJobID.Visible = true;

                lblDate.Visible = false;
                dtpStartDate.Visible = false;
                dtpEndDate.Visible = false;

                cboStartJobID.SelectedIndex = 0;
                cboEndJobID.SelectedIndex = 0;
            }
            else
            {
                lblJobID.Visible = false;
                cboStartJobID.Visible = false;
                cboEndJobID.Visible = false;

                lblDate.Visible = true;
                dtpStartDate.Visible = true;
                dtpEndDate.Visible = true;

                cboStartJobID.SelectedIndex = 0;
                cboEndJobID.SelectedIndex = 0;
            }
            dtpStartDate.Value = DateTime.Today;
            dtpEndDate.Value = DateTime.Today;
        }

        protected override bool AfterCancelClicked()
        {
            cboEditStatus.Items.Clear();
            return true;
        }

        private void tbcEntry_Click(object sender, EventArgs e)
        {
            SetTabControl(formMode);
        }

        private void SetTabControl(FormMode formMode)
        {
            if (formMode == FormMode.Edit)
            {
                if(tbcEntry.SelectedIndex == 0)
                    tbcEntry.SelectedTab = tabPageMaster;
            }
        }

        protected override void FillComboBox()
        {
            try
            {
                List<string> jobIDList = HaulierJob.GetAllHaulierJobNo();
                cboStartJobID.DataSource = jobIDList;
                cboEndJobID.DataSource = jobIDList;
                cboDepartment.DataSource = TptDept.GetAllTptDepts(DeptType.Haulage);
                cboDepartment.DisplayMember = "tptDeptCode";

                cboStatus.Items.Clear();
                cboStatus.Items.Add(JobTripStatus.Booked);
                cboStatus.Items.Add(JobTripStatus.Ready);
                cboStatus.Items.Add(JobTripStatus.Assigned);
                cboStatus.Items.Add(JobTripStatus.Completed);
                cboStatus.Items.Add(JobTripStatus.Invoiced);

                cboToStatus.Items.Clear();
                cboToStatus.Items.Add(JobTripStatus.Assigned);
                cboToStatus.Items.Add(JobTripStatus.Completed);

                #region REmoved 20150917
                ////cboStatus
                //if (haulierJobTrip.OwnTransport == true)
                //{
                //    cboEditStatus.Items.Add(JobTripStatus.Booked);
                //    cboEditStatus.Items.Add(JobTripStatus.Ready);
                //    cboEditStatus.Items.Remove(jobTripStatus);
                //}
                //else
                //{
                //    //20130416 - gerry removed and replaced
                //    #region removed 20130416
                //    //cboEditStatus.Items.Add(JobTripStatus.Booked);
                //    //cboEditStatus.Items.Add(JobTripStatus.Ready);
                //    //cboEditStatus.Items.Add(JobTripStatus.Assigned);
                //    //cboEditStatus.Items.Add(JobTripStatus.Completed);
                //    #endregion
                //    if (haulierJobTrip.TripStatus == JobTripStatus.Booked)
                //    {
                //        cboEditStatus.Items.Add(JobTripStatus.Ready);                    
                //    }
                //    else if (haulierJobTrip.TripStatus == JobTripStatus.Ready)
                //    {
                //        cboEditStatus.Items.Add(JobTripStatus.Assigned);
                //        cboEditStatus.Items.Add(JobTripStatus.Completed);                    
                //    }
                //    else if (haulierJobTrip.TripStatus == JobTripStatus.Assigned)
                //    {
                //        cboEditStatus.Items.Add(JobTripStatus.Completed);                    
                //    }
                //    //20130416 end
                //}
                #endregion
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), CommonResource.Error); }
        }

        private void cboStartJobID_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 2015-03-10 Zhou Kai comments out this block
            //if (init)
            //{
            if (string.Compare(cboStartJobID.Text.ToString().Trim(), cboEndJobID.Text.ToString().Trim(), StringComparison.Ordinal) > 0)
            {
                cboEndJobID.Text = cboStartJobID.Text;
            }
            //}
            // 2015-03-10 Zhou Kai ends

            // 2015-03-10 Zhou Kai adds this block
            // simply copy the text of the start job id to the end job id
            //cboEndJobID.Text = cboStartJobID.Text;
            // 2015-03-10 Zhou Kai ends
        }
        
        private void cboEndJobID_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 2015-03-10 Zhou Kai comments out this block
            //if (init)
            //{
            if (string.Compare(cboStartJobID.Text.ToString().Trim(), cboEndJobID.Text.ToString().Trim(), StringComparison.Ordinal) > 0)
            {
                cboStartJobID.Text = cboEndJobID.Text;
            }
            //}
            // 2015-03-10 Zhou Kai ends

            // 2015-03-10 Zhou Kai adds comments
            // do nothing when the end job id is changed
            // 2015-03-10 Zhou Kai ends

        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            dtpStartDate.Format = DateTimePickerFormat.Short;
            if (DateTime.Compare(dtpStartDate.Value.Date, dtpEndDate.Value.Date) > 0)
            {
                dtpEndDate.Value = dtpStartDate.Value;
            }
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            dtpEndDate.Format = DateTimePickerFormat.Short;
            if (DateTime.Compare(dtpStartDate.Value.Date, dtpEndDate.Value.Date) > 0)
            {
                dtpStartDate.Value = dtpEndDate.Value;
            }
        }

        #region "2014-11-17 Zhou Kai, Mulit-Selection / Setting of Sub-Con trips"
        // 2014-11-17 Zhou Kai adds this block
        private List<HaulierJobTrip> RefreshGridForSelectedTrips(DataGridView dgvTrips)
        {
            List<HaulierJobTrip> checkedTrips = new List<HaulierJobTrip>();
            foreach (DataGridViewRow dgvr in dgvTrips.Rows)
            {
                if (dgvr.DataBoundItem is HaulierJobTrip)
                {
                    HaulierJobTrip t = dgvr.DataBoundItem as HaulierJobTrip;
                    dgvr.Cells["subConDataGridViewTextBoxColumn"].Value = t.subCon;
                    //dgvr.Cells[colSubConCode.Name].Value = t.subCon.Code;
                    //dgvr.Cells[colSubConName.Name].Value = t.subCon.Name;
                    if (dgvr.Cells["colSelected"].Value is bool)
                    { if ((bool)dgvr.Cells["colSelected"].Value) { checkedTrips.Add(t); } }
                }
            }

            return checkedTrips;
        }

        // 2014-11-17 Zhou Kai adds
        private void dgvJobTrip_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int nRowIndex = e.RowIndex;
            int nColIndex = e.ColumnIndex;
            if (nColIndex == colSelected.Index)
            {
                if (dgvJobTrip.Rows[nRowIndex].Cells["colSelected"].Value == null ||
                    (bool)(dgvJobTrip.Rows[nRowIndex].Cells["colSelected"].Value) == false)
                {
                    dgvJobTrip.Rows[nRowIndex].Cells["colSelected"].Value = true;
                }
                else { dgvJobTrip.Rows[nRowIndex].Cells["colSelected"].Value = false; }

                selectedSubConJobTrips = GetSelectedTripsFromDGV(this.dgvJobTrip);
                btnSave.Enabled = selectedSubConJobTrips.Count > 0 ? (cboToStatus.Text != string.Empty) : false;
            }
        }

        private void rdbOwnTransportOption_CheckedChanged(object sender, EventArgs e)
        {
            HideAndDisplayControlsForSubConTripsMultiSelection();
        }

        private void HideAndDisplayControlsForSubConTripsMultiSelection()
        {

            try
            {
                if (rdbYes.Checked)
                {
                    lblToStatus.Visible = false;
                    cboToStatus.Visible = false;
                    ownTransport = true;
                    colSelected.Visible = false;
                    //btnUnSelectAll_Click(this, EventArgs.Empty); // unselect all trips(if any) 
                    btnSave.Enabled = false;
                    FilterDataForCriteria();
                }
                if (rdbNo.Checked)
                {
                    ownTransport = false;
                    colSelected.Visible = true;
                    FilterDataForCriteria();
                    lblToStatus.Visible = haulierJobTrips.Count > 0;
                    cboToStatus.Visible = haulierJobTrips.Count > 0;
                }
                if (rdbBoth.Checked)
                {
                    lblToStatus.Visible = false;
                    cboToStatus.Visible = false;
                    colSelected.Visible = false;
                    //btnUnSelectAll_Click(this, EventArgs.Empty); // unselect all trips(if any) 
                    btnSave.Enabled = false;
                    FilterDataForCriteria();
                }
                bdsMaster.DataSource = haulierJobTrips;
            }
            catch (FMException fme) { MessageBox.Show(fme.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in dgvJobTrip.Rows)
            {
                dgvr.Cells["colSelected"].Value = true;
            }
        }

        private void btnUnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in dgvJobTrip.Rows)
            {
                dgvr.Cells["colSelected"].Value = false;
            }
        }

        // this function is used only when ownTransport == false; it's a UI function
        private List<HaulierJobTrip> GetSelectedTripsFromDGV(DataGridView dgv)
        {
            List<HaulierJobTrip> selectedTrips = new List<HaulierJobTrip>();
            
            foreach(DataGridViewRow dgvr in dgv.Rows)
            {
                if (dgvr.DataBoundItem is HaulierJobTrip)
                {
                    HaulierJobTrip t = dgvr.DataBoundItem as HaulierJobTrip;
                    if (dgvr.Cells["colSelected"].Value != null)
                    { if ((bool)dgvr.Cells["colSelected"].Value) { selectedTrips.Add(t); } }
                }
            }

            return selectedTrips;
        }

        private string SaveSubConTripStatus(List<HaulierJobTrip> trips, JobTripStatus newStatus)
        {
            string msg = String.Empty;
            //Dictionary<HaulierJobTrip, HaulierJobTrip> currentAndPreviousLegPairs = HaulierJobTrip.GetPreviousTrips(true, trips);
            try
            {
                if(rdbNo.Checked)
                    return HaulierJobTrip.SaveMultipleSubConTripStatus(trips, newStatus,"PL", "FRM_CHANGE_TRIP_STATUS", user.UserID);

                //return HaulierJobTrip.SaveMultipleSubConTripStatus(currentAndPreviousLegPairs, newStatus);
            }
            catch (FMException fme) { return fme.Message; }
            return msg;
        }

        private void cboToStatus_TextChanged(object sender, EventArgs e)
        {
            if (rdbNo.Checked)
            {
                //btnSave.Enabled = (cboToStatus.Text != String.Empty);
                selectedSubConJobTrips = GetSelectedTripsFromDGV(this.dgvJobTrip);
                btnSave.Enabled = selectedSubConJobTrips.Count > 0;
            }
        }

        private void cboStatus_TextChanged(object sender, EventArgs e)
        {
            //if (!(ownTransport || cboStatus.Text == String.Empty))
            //{
            //    btnSave.Enabled = (cboToStatus.Text != String.Empty);
            //}
        }

        private void cboCBO_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.AutoCompleteComboBox(sender, e, true);
        }

        private void bdsMaster_CurrentChanged(object sender, EventArgs e)
        {
            if (bdsMaster.Count > 0)
            {
                haulierJobTrip = (HaulierJobTrip)bdsMaster.Current;
                BindComponent();
            }
        }


        // 2014-11-18 Zhou Kai ends
        
        #endregion
    }
        
}
