﻿
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
    public partial class FrmAddDriverAssignment : AbstractForm
    {
        public List<Driver> drivers;
        public DateTime dateFrom = DateTime.Today;
        public DateTime dateTo = DateTime.Today;

        public FrmAddDriverAssignment()
        {
            InitializeComponent();
        }

        private void FrmAddDriverAssignment_Load(object sender, EventArgs e)
        {
            try
            {
                lblAssignTo.Visible = drivers.Count == 1 ? true : false;
                lblDriver.Visible = drivers.Count == 1 ? true : false;
                lblDriver.Text = drivers == null ? "No Driver" : drivers[0].defaultVehicleNumber;

                #region removed
                /*
                int count = 0;
                string selectedDrivers = string.Empty;
                foreach (Driver drv in drivers)
                {
                    count++;
                    selectedDrivers += count > 1 ? ", " : string.Empty;
                    selectedDrivers += drv.defaultVehicleNumber;
                }
                lblDriver.Text = selectedDrivers;
                 * */
                #endregion
                
                cboAssignment.DataSource = JobAssignmentPriority.GetDriverJobAssignments();
                cboAssignment.DisplayMember = "Code";
                //20160810 - gerry added reusable from planning
                dtpAssignmentDateFrom.Value = dateFrom;
                dtpAssignmentDateTo.Value = dateTo;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (drivers != null)
                {
                    DriverPriority driverPriority = new DriverPriority();
                    foreach (Driver dr in drivers)
                    {
                        driverPriority.DriverCode = dr.Code;
                        driverPriority.DriverName = dr.Name;
                        driverPriority.DefaultVehicle = dr.defaultVehicleNumber;
                        driverPriority.Prior_Code = cboAssignment.Text.ToString();

                        if (dtpAssignmentDateFrom.Value.Date == dtpAssignmentDateTo.Value.Date)
                        {
                            driverPriority.ScheduleDate = dtpAssignmentDateFrom.Value;
                            driverPriority.AddDriverPriority();
                        }
                        else
                        {
                            driverPriority.AddDriverPriority(dtpAssignmentDateFrom.Value.Date, dtpAssignmentDateTo.Value.Date);
                        }
                    }
                }
                MessageBox.Show(CommonResource.SaveSuccess, CommonResource.Alert);
                this.DialogResult = DialogResult.OK;
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



    }
}
