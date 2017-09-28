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
using FM.TR_MaintenanceDLL.BLL;
using TR_FormBaseLibrary;

namespace FM.TR_MaintenanceUI.UI
{
    public partial class FrmDriverUnavailable : AbstractForm
    {
        private FrmDriver parent;
        private DriverUnavailable driverUnavailable= new DriverUnavailable();
        private FormMode frmMode;
        public FrmDriverUnavailable()
        {
            InitializeComponent();
        }   
        public FrmDriverUnavailable(FrmDriver parent, FormMode frmMode)
        {
            InitializeComponent();
            this.parent = parent;
            this.frmMode = frmMode; 
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DisplayData()
        {
            dtpStartDate.Value = DateTime.Today;
            dtpStartTime.Value = DateTime.Today.Date;
            dtpEndDate.Value = DateTime.Today;
            dtpEndTime.Value = DateTime.Today.Date.AddHours(23).AddMinutes(59);
            txtRemarks.Text = string.Empty;
            if (parent.driver != null)
            {
                txtDriverCode.Text = parent.driver.Code;
                txtDriverName.Text = parent.driver.Name;
            }
            if (parent.selectedUnavailableDate != null)
            {
                if (frmMode == FormMode.Edit)
                {
                    dtpStartDate.Value = parent.selectedUnavailableDate.startDateTime.Date;
                    dtpStartTime.Value = parent.selectedUnavailableDate.startDateTime;
                    dtpEndDate.Value = parent.selectedUnavailableDate.endDateTime.Date;
                    dtpEndTime.Value = parent.selectedUnavailableDate.endDateTime;
                    txtRemarks.Text = parent.selectedUnavailableDate.remarks;
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                FillData();
                if (driverUnavailable.startDateTime >= driverUnavailable.endDateTime)
                    throw new FMException("End date and time must be later than start date and time. ");
                //20160531 Jana - Not allow set unavailable is job assigned.
                if (Driver.IsDriverAssignJob(driverUnavailable.driverCode, driverUnavailable.startDateTime.Date, driverUnavailable.startDateTime, driverUnavailable.endDateTime))
                    throw new FMException("Job assigned during this period, please define another date/time ");

                string warnMsg = driverUnavailable.startDateTime.Date < DateTime.Today? driverUnavailable.driverCode +TptResourceBLL.WarnUnavailableDateLessThanToday : string.Empty;
                switch (frmMode)
                { 
                    case FormMode.Add:
                        if (!warnMsg.Equals(string.Empty))
                        {
                            DialogResult dr = MessageBox.Show(warnMsg+CommonResource.ConfirmToProceed, CommonResource.Warning, MessageBoxButtons.YesNo);
                            if (dr == DialogResult.Yes)
                                parent.driver.AddDriverUnavailable(driverUnavailable);
                        }
                        else
                            parent.driver.AddDriverUnavailable(driverUnavailable);

                        MessageBox.Show(CommonResource.SaveSuccess, CommonResource.Alert);
                        this.Close();
                        break;

                    case FormMode.Edit:           
                        driverUnavailable.seqNo = parent.selectedUnavailableDate.seqNo;
                        parent.driver.EditDriverUnavailable(driverUnavailable);                          
                        MessageBox.Show(CommonResource.EditSuccess, CommonResource.Alert);   
                        this.Close();
                        break;
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
        private void FillData()
        {
            try
            {
                driverUnavailable = new DriverUnavailable();
                driverUnavailable.driverCode = txtDriverCode.Text.ToString();
                driverUnavailable.startDateTime = dtpStartDate.Value.Date.AddHours(dtpStartTime.Value.Hour).AddMinutes(dtpStartTime.Value.Minute);
                driverUnavailable.endDateTime = dtpEndDate.Value.Date.AddHours(dtpEndTime.Value.Hour).AddMinutes(dtpEndTime.Value.Minute);
                driverUnavailable.remarks = txtRemarks.Text.ToString();
            }
            catch (Exception ex) { throw ex; }
        }

        private void FrmDriverUnavailable_Load(object sender, EventArgs e)
        {
            DisplayData();
            //dtpStartDate.Enabled = frmMode == FormMode.Add ? true : false;
        }

        private void dtpStartTime_FormatChanged(object sender, EventArgs e)
        {                      
            DateTimePicker dtp = (DateTimePicker)sender;
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.CustomFormat = "HH:mm";
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                dtpStartTime.Value = dtpStartDate.Value.Date;  //set default to 0000
                if (dtpStartDate.Value > dtpEndDate.Value)
                    dtpEndDate.Value = dtpStartDate.Value;
            }
            catch (Exception ex) { throw ex; }
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                dtpEndTime.Value = dtpEndDate.Value;
                if (dtpStartDate.Value > dtpEndDate.Value)
                    dtpEndDate.Value = dtpStartDate.Value;

                dtpEndTime.Value = dtpEndDate.Value.Date.AddHours(23).AddMinutes(59);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
