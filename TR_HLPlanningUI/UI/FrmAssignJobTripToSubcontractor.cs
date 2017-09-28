using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
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
    public partial class FrmAssignJobTripToSubcontractor : frmEntryBase
    {
        SortableList<HaulierJobTrip> haulierJobTrips = new SortableList<HaulierJobTrip>();
        JobTripStatus jobTripStatus = new JobTripStatus();
        HaulierJobTrip haulierJobTrip = new HaulierJobTrip();
        bool init = false;

        public FrmAssignJobTripToSubcontractor()
        {
            InitializeComponent();
            dtpStartDate.CustomFormat = " ";
            dtpStartDate.Format = DateTimePickerFormat.Custom;
            dtpEndDate.CustomFormat = " ";
            dtpEndDate.Format = DateTimePickerFormat.Custom;
        }

        private void EnableButtonsForAllModes()
        {
            btnNewMaster.Enabled = true;
            btnEditMaster.Enabled = true;
            btnDeleteMaster.Enabled = true;
            btnSave.Enabled = false;
            pnlQuery.Enabled = true;

        }

        private void EnableButtonsAndPanels(FormMode formMode)
        {
            if (formMode == FormMode.Edit)
            {
                btnNewMaster.Enabled = false;
                btnDeleteMaster.Enabled = false;
                pnlQuery.Enabled = true;
                btnSave.Enabled = true;
            }
            else if (formMode == FormMode.Delete)
            {
                btnNewMaster.Enabled = false;
                btnEditMaster.Enabled = false;
                pnlQuery.Enabled = true;
                btnSave.Enabled = false;
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            haulierJobTrips = new SortableList<HaulierJobTrip>();
            if ((cboStatus.Text == "") & (cboStartJobID.Text == "") & (dtpStartDate.Text == " ") & (dtpEndDate.Text == " "))
            {
                haulierJobTrips = HaulierJobTrip.GetAllHaulierJobTrips();
            }
            if ((cboStatus.Text != "") & (cboStartJobID.Text == "") & (dtpStartDate.Text == " ") & (dtpEndDate.Text == " "))
            {
                haulierJobTrips = HaulierJobTrip.GetJobTrips((JobTripStatus)cboStatus.SelectedItem);
            }
            if ((cboStatus.Text == "") & (cboStartJobID.Text != "") & (dtpStartDate.Text == " ") & (dtpEndDate.Text == " "))
            {
                haulierJobTrips = HaulierJobTrip.GetJobTrips(Convert.ToInt32(cboStartJobID.Text), Convert.ToInt32(cboEndJobID.Text));
            }
            if ((cboStatus.Text == "") & (cboStartJobID.Text == "") & (dtpStartDate.Text != " "))
            {
                haulierJobTrips = HaulierJobTrip.GetJobTrips(dtpStartDate.Value, dtpEndDate.Value);
            }

            if (rdbYes.Checked==true)
              haulierJobTrips = HaulierJobTrip.GetSubconJobTrips(haulierJobTrips,true);
            if (rdbNo.Checked == true)
              haulierJobTrips = HaulierJobTrip.GetSubconJobTrips(haulierJobTrips, false);


            bdsMaster.DataSource = haulierJobTrips;
        }

        private void BindComponent()
        {
            txtEditJobID.DataBindings.Add("Text", bdsMaster, "JobID");
            txtEditSequence.DataBindings.Add("Text", bdsMaster, "Sequence");
            txtEditContainerNo.DataBindings.Add("Text", bdsMaster, "ContainerNo");
            txtEditContainerCode.DataBindings.Add("Text", bdsMaster, "ContainerCode");
            txtEditStatus.DataBindings.Add("Text", bdsMaster, "TripStatus");
            
            chkOwnTransport.DataBindings.Add("Checked", bdsMaster, "OwnTransport");
        }

        private void FrmChangeTripStatus_Load(object sender, EventArgs e)
        {
            ManageUserAccess(module, user);


            dgvJobTrip.AutoGenerateColumns = false;
            dgvJobTrip.Columns["updateVersionDataGridViewImageColumn"].Visible = false;
            dgvJobTrip.Columns["jobIDDataGridViewTextBoxColumn"].DisplayIndex = 0;
            dgvJobTrip.Columns["sequenceDataGridViewTextBoxColumn"].DisplayIndex = 1;
            dgvJobTrip.Columns["customerCodeDataGridViewTextBoxColumn"].DisplayIndex = 2;
            dgvJobTrip.Columns["customerNameDataGridViewTextBoxColumn"].DisplayIndex = 3;
            dgvJobTrip.Columns["ContainerNoGrid"].DisplayIndex = 4;
            dgvJobTrip.Columns["containerCodeDataGridViewTextBoxColumn"].DisplayIndex = 5;
            dgvJobTrip.Columns["ownTransportDataGridViewCheckBoxColumn"].DisplayIndex = 6;
            dgvJobTrip.Columns["tripStatusDataGridViewTextBoxColumn"].DisplayIndex = 7;
            dgvJobTrip.Columns["subContractorCodeDataGridViewTextBoxColumn"].DisplayIndex = 8;

            cboEditSubContractor.DataSource = Stop.GetAllStops();//OperatorDTO.GetAllOperators();
            cboEditSubContractor.DisplayMember = "Code";
            BindComponent();
            #region REmoved
            /*
            List<string> startJobIDList = new List<string>();
            List<string> endJobIDList = new List<string>();
            startJobIDList = HaulierJob.GetAllHaulierJobNo();//.GetAllHaulierJobs();
            //startJobIDList.Insert(0, "");
            startJobIDList.Sort();
            endJobIDList = HaulierJob.GetAllHaulierJobNo();
            //endJobIDList.Insert(0, "");
            endJobIDList.Sort();

            cboStartJobID.DataSource = startJobIDList;
            cboEndJobID.DataSource = endJobIDList;    
             */ 
            #endregion        
            SortableList<HaulierJob> h1 = new SortableList<HaulierJob>();
            cboStartJobID.Items.Clear();
            cboStartJobID.Items.Add("");
            cboEndJobID.Items.Clear();
            cboEndJobID.Items.Add("");
            for (int i = 0; i < h1.Count; i++)
            {
                cboStartJobID.Items.Add(h1[i].JobID.ToString());
                cboEndJobID.Items.Add(h1[i].JobID.ToString());
            }

            init = true;
            EnableButtonsForAllModes();

        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            dtpStartDate.Format = DateTimePickerFormat.Short;
            if (DateTime.Compare(dtpStartDate.Value.Date, dtpEndDate.Value.Date) > 0)
            {
                dtpStartDate.Value = dtpEndDate.Value;
            }
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            dtpEndDate.Format = DateTimePickerFormat.Short;
            if (DateTime.Compare(dtpStartDate.Value.Date, dtpEndDate.Value.Date) > 0)
            {
                dtpEndDate.Value = dtpStartDate.Value;
            }
        }

        private void cboStartJobID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (init)
            {
                if (string.Compare(cboStartJobID.Text.ToString().Trim(), cboEndJobID.Text.ToString().Trim(), StringComparison.Ordinal) > 0)
                {
                    cboStartJobID.Text = cboEndJobID.Text;
                }
            }
        }

        private void cboEndJobID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (init)
            {
                if (string.Compare(cboStartJobID.Text.ToString().Trim(), cboEndJobID.Text.ToString().Trim(), StringComparison.Ordinal) > 0)
                {
                    cboEndJobID.Text = cboStartJobID.Text;
                }
            }
        }    

        private void btnEditMaster_Click(object sender, EventArgs e)
        {
            if (bdsMaster.Count != 0)
            {
                haulierJobTrip = (HaulierJobTrip)bdsMaster.Current;
                if ((haulierJobTrip.TripStatus == JobTripStatus.Completed) || (haulierJobTrip.TripStatus == JobTripStatus.Invoiced) || (haulierJobTrip.TripStatus == JobTripStatus.Assigned))
                {
                    MessageBox.Show("Cant assign if trip status is assigned or completed or invoiced already");
                    btnCancel.PerformClick();

                }
                else
                {

                    txtEditSubContractorRef.Text = haulierJobTrip.SubContractorReference;
                    cboEditSubContractor.SelectedIndex = cboEditSubContractor.FindString(haulierJobTrip.subCon.Code); //20140312 - gerry modified

                    EnableButtonsAndPanels(FormMode.Edit);
                }
            }
            else
            {
                EnableButtonsForAllModes();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (jobTripStatus == JobTripStatus.Completed)
            {
                MessageBox.Show(TptResourceBLL.ErrCurrentStatusIsComplete, CommonResource.Alert);
            }
            else
            {
                if (jobTripStatus == JobTripStatus.Invoiced)
                {
                    MessageBox.Show(TptResourceBLL.ErrCurrentStatusIsInvoiced, CommonResource.Alert);
                }
                else
                {

                        SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                        con.Open();
                        SqlTransaction tran = con.BeginTransaction();
                        try
                        {
                            haulierJobTrip = new HaulierJobTrip();
                            haulierJobTrip = (HaulierJobTrip)bdsMaster.Current;

                            //haulierJobTrip.SubContractorCode = cboEditSubContractor.Text;
                            //201040502 - gerry change subCon from CustomerDTO to operatorDTO
                            //haulierJobTrip.subCon = CustomerDTO.GetSubContractorByCode(haulierJobTrip.subCon.Code.ToString().Trim());
                            haulierJobTrip.subCon = OperatorDTO.GetOperatorDTO(haulierJobTrip.subCon.Code.ToString().Trim());
                            haulierJobTrip.SubContractorReference = txtEditSubContractorRef.Text;
                            haulierJobTrip.OwnTransport = false;

                            haulierJobTrip.UpdateSubContractor(con, tran);

                            tran.Commit();
                            MessageBox.Show(CommonResource.SaveSuccess, CommonResource.Alert);

                        }
                        catch (FMException ex)
                        {
                            tran.Rollback();
                            MessageBox.Show(ex.ToString());
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            MessageBox.Show(ex.ToString());
                        }
                        finally
                        {
                            con.Close();
                        }

                }

            }
        }

       
    }
}
