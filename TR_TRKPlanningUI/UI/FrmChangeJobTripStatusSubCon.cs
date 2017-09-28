using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using FM.TR_TRKBookDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_TRKPlanDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using TR_LanguageResource.Resources;
using TR_FormBaseLibrary;

namespace FM.TR_TRKPlanningUI.UI
{
    public partial class FrmChangeJobTripStatusSubCon : AbstractForm
    {
        private SortableList<TruckJobTrip> jobtrips = new SortableList<TruckJobTrip>();
        private CheckBox ckBox = new CheckBox();

        public FrmChangeJobTripStatusSubCon()
        {
            InitializeComponent();
        }

        private void cboCurrentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ArrayList newStatus = new ArrayList();
            btnQuery.PerformClick();
            switch ((JobTripStatus)cboCurrentStatus.SelectedItem)
            { 
                case JobTripStatus.Booked:
                    newStatus.Add(JobTripStatus.Ready);
                    newStatus.Add(JobTripStatus.Assigned);
                    break;
                case JobTripStatus.Ready:
                    newStatus.Add(JobTripStatus.Assigned); 
                    newStatus.Add(JobTripStatus.Completed);
                    break;
                case JobTripStatus.Assigned:
                    newStatus.Add(JobTripStatus.Completed);
                    break;
            } 
            cboNewStatus.DataSource = newStatus;
            cboNewStatus.Text = string.Empty;
            btnSaveStatus.Enabled = cboNewStatus.Text == string.Empty ? false : true;
        }

        private void ckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvJobTrips.RowCount > 0)
            {
                for (int i = 0; i < dgvJobTrips.RowCount; i++)
                {
                    dgvJobTrips.Rows[i].Cells[0].Value = ckBox.Checked ? "T" : "F";
                }
                btnSaveStatus.Enabled = ckBox.Checked && !cboNewStatus.Text.ToString().Equals(string.Empty) ? true : false;
                dgvJobTrips.Columns["Select"].ReadOnly = false;
            }
        }

        private void InitializeDisplay()
        {
            cboCurrentStatus.SelectedIndex = 0;
            cboNewStatus.Text = string.Empty;
            rdbByJobNo.Checked = true;
            
        }
        private void FrmChangeJobTripStatusSubCon_Load(object sender, EventArgs e)
        {
            try
            {
                BindDataSource();
                InitializeDisplay();
                dgvJobTrips.AutoGenerateColumns = false;

                ckBox.Name = "chkCheckAll";
                ckBox.Text = "ALL";
                ckBox.Appearance = Appearance.Button;
                ckBox.TextAlign = ContentAlignment.MiddleCenter;
                //ckBox.CheckAlign = ContentAlignment.BottomCenter;
                Rectangle rect = this.dgvJobTrips.GetCellDisplayRectangle(0, -1, true);  
                ckBox.Size = new Size(rect.Width, rect.Height);

                ckBox.Location = rect.Location;
                ckBox.CheckedChanged += new EventHandler(ckBox_CheckedChanged);
                this.dgvJobTrips.Controls.Add(ckBox);
                for (int i = 0; i < dgvJobTrips.Columns.Count; i++)
                {
                    dgvJobTrips.Columns[i].ReadOnly = true;
                }                
            }
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString(), CommonResource.Alert);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Alert);
            }

        }
        private void BindDataSource()
        {
            try
            {
                ArrayList currentstatus = new ArrayList(); 
                currentstatus.Add(JobTripStatus.Booked);
                currentstatus.Add(JobTripStatus.Ready);
                currentstatus.Add(JobTripStatus.Assigned);

                cboDept.DataSource = TptDept.GetAllTptDepts(DeptType.Trucking);
                cboDept.DisplayMember = "tptDeptCode";

                cboFromJobNo.DataSource = TruckJob.GetAllTruckJobNo();
                cboToJobNo.DataSource = TruckJob.GetAllTruckJobNo();

                cboCurrentStatus.DataSource = currentstatus;
            }

            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        private void rdb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdbByJobNo.Checked)
                {
                    pnlByJobTripDate.Visible = false;
                    pnlByJobNo.Visible = true;
                    dtpDateFrom.Value = DateUtility.GetSQLDateTimeMinimumValue();
                    dtpDateTo.Value = DateUtility.GetSQLDateTimeMaximumValue();
                }
                else if (rdbByJobTripDate.Checked)
                {
                    pnlByJobNo.Visible = false;
                    pnlByJobTripDate.Visible = true;
                    cboFromJobNo.Text = string.Empty;
                    cboToJobNo.Text = string.Empty;
                    dtpDateFrom.Value = DateTime.Today.AddDays(-10);
                    dtpDateTo.Value = DateTime.Today;                 
                }
            }
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString(), CommonResource.Alert);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Alert);
            }
        }                             
        private void RefreshGid()
        {
            try
            {  
                dgvJobTrips.Columns["Select"].ReadOnly = false;
                int status =  cboCurrentStatus.Text == string.Empty ? 0 : (int)((JobTripStatus)cboCurrentStatus.SelectedItem);  
                jobtrips = TruckJobTrip.GetTruckJobTripsToChangeStatus(status, false, (DateTime)dtpDateFrom.Value, (DateTime)dtpDateTo.Value, cboFromJobNo.Text, cboToJobNo.Text, cboDept.Text);
                bdsJobTrips.DataSource = jobtrips;
                dgvJobTrips.DataSource = bdsJobTrips;

                ckBox.Checked = false;
                btnSaveStatus.Enabled = bdsJobTrips.Count <= 0 || cboNewStatus.Text == "" ? false : true;
            } 
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString(), CommonResource.Alert);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Alert);
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            RefreshGid();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            InitializeDisplay();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SortableList<TruckJobTrip> truckJobTripTobeUpdate = new SortableList<TruckJobTrip>();
                for (int i = 0; i < dgvJobTrips.RowCount; i++)
                {
                    if (dgvJobTrips.Rows[i].Cells[0].Value != null)
                    {
                        if ((string)dgvJobTrips.Rows[i].Cells[0].Value == "T")
                        {
                            TruckJobTrip tempJobTrip = (TruckJobTrip)bdsJobTrips[i];
                            tempJobTrip.TripStatus = (JobTripStatus)cboNewStatus.SelectedItem;
                            truckJobTripTobeUpdate.Add(tempJobTrip);
                        }
                    }
                }
                if (truckJobTripTobeUpdate.Count > 0)
                {
                    DialogResult dr = MessageBox.Show(TptResourceUI.ConfirmUpdateJobTripStatus, CommonResource.Confirmation, MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        TruckJobTrip.UpdateJobTripStatus(truckJobTripTobeUpdate);
                        MessageBox.Show(CommonResource.EditSuccess, CommonResource.Alert);
                        cboNewStatus.Text = string.Empty;
                    }
                }
                else
                    MessageBox.Show(TptResourceUI.NoJobSelectedTobeSet, CommonResource.Alert);
            }
            catch (FMException ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
            }
            RefreshGid();
        }

        private void cboFromJobNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                base.AutoCompleteComboBox(sender, e, true);
            }
            catch { }
        }

   
        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            if (dgvJobTrips.RowCount > 0)
            {
                for (int i = 0; i < dgvJobTrips.RowCount; i++)
                {
                    dgvJobTrips.Rows[i].Cells[0].Value = "T";
                }
                btnSaveStatus.Enabled = true;  
                dgvJobTrips.Columns["Select"].ReadOnly = false;
            }
        }

        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvJobTrips.RowCount; i++)
            {
                dgvJobTrips.Rows[i].Cells[0].Value = "F";
            }
            btnSaveStatus.Enabled = false;
        }

        private void cboNewStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvJobTrips.RowCount; i++)
            {
                if (dgvJobTrips.Rows[i].Cells[0].Value != null)
                {
                    if ((string)dgvJobTrips.Rows[i].Cells[0].Value == "T")
                    {
                        btnSaveStatus.Enabled = true;
                        break;
                    }
                }
            }
        }

        private void cboCurrentStatus_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                base.AutoCompleteComboBox(sender, e, true);
            }
            catch { }
        }    
    }
}
