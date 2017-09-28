using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_TRKBookDLL.BLL;
using FM.TR_TRKBookUI.UI;
using FM.TR_MaintenanceDLL.BLL;
using TR_FormBaseLibrary;
using TR_LanguageResource.Resources;
using TR_AirFreightDLL.BLL;
using FM.TR_TRKBookDLL.Facade;
using FM.TR_MarketDLL.BLL;

namespace FM.TR_TRKBookUI.UI
{
    public partial class FrmAirJob : AbstractForm
    {
        FrmTruckJob parent;
        private AirJob airJob;
        private AirJobType airJobType;
        private string status = string.Empty;
        public string quotationNo;

        public FrmAirJob(FrmTruckJob parent)
        {
            InitializeComponent();
            this.parent = parent;
            this.quotationNo = string.Empty;
        }

        private void FrmAirJob_Load(object sender, EventArgs e)
        {
            rdbAirExport.Checked = true;
            rdbBillByAirFreight.Checked = true;
            status = CommonResource.Status + "\n--------------------";
        }  
        private void btnRetreiveAirJob_Click(object sender, EventArgs e)
        {
            btnRetrieveAirJob.Enabled = false;
            try
            {
                if (cboJobNo.Text.ToString().Length > 0)
                {
                    airJob = TruckingFacadeIn.GetAirJob(cboJobNo.Text.ToString(), airJobType, txtHAWB.Text.ToString().Trim(), txtMAWB.Text.ToString().Trim());
                    //string quotationNo = string.Empty;
                    if (CheckDuplicate(airJob.JobNo))
                    {
                        string rateMsg = string.Empty;
                        //if all checking above were pass now create the truck job from air job 
                        //all air job rates has no dimension detail and gross weight equal 0 will be not converted into truck job trip
                        parent.truckJob = TruckingFacadeIn.ConvertAirFreightJobToTruckJob(airJob, airJobType, quotationNo, out rateMsg);
                        parent.truckJob.BillByTransport = rdbBillByTrucking.Checked ? true : false;
                        if (!rateMsg.Equals(string.Empty))
                        {
                            if (ShowWarning(rateMsg) == DialogResult.Yes) { this.Close(); }
                            else
                            {
                                status += "\n\n" + rateMsg;
                                txtStatus.Text = status;
                            }
                        }
                        else { this.Close(); }
                    }
                }
                else
                {
                    throw new FMException(TptResourceBLL.ErrAirJobNoEmpty);
                }
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            parent.truckJob = new TruckJob();
            this.Close();
        }
      
        private bool CheckDuplicate(string jobNo)
        {
            bool retValue = true;
            string outTruckJobNo = string.Empty;
            if (TruckJob.DoesAirJobNumberExist(jobNo, out outTruckJobNo))
            {
                retValue = false;
                string msg = TptResourceBLL.ErrAirJobAlreadyConverted;
                throw new FMException(string.Format(msg, jobNo, outTruckJobNo));
            }
            return retValue;
        }
        private void CheckBillingOption(string custCode)
        {
            //bool retValue = true;
            string quotationMsg = string.Empty;
            if (rdbBillByTrucking.Checked)
            {
                quotationNo = Quotation.GetValidQuotationNo(custCode, DateTime.Today.Date, out quotationMsg);
                if (quotationNo.Length > 0)
                {
                    bool haveTransportRatesForCBM_MT = Quotation.HaveTruckMovementTransportRatesForMT_CBM(quotationNo, out quotationMsg);
                    if (!haveTransportRatesForCBM_MT)
                    {
                        status = CommonResource.Status + "\n--------------------";
                        if (ShowWarning(quotationMsg) == DialogResult.No)
                            rdbBillByAirFreight.Checked = true;

                        quotationNo = string.Empty;
                        status += "\n\n" + quotationMsg;
                        txtStatus.Text = status;
                    }
                }
                else
                {
                    MessageBox.Show(TptResourceBLL.WarnAirJobNoQuotation, CommonResource.Warning);
                    status += "\n\n" + TptResourceBLL.WarnAirJobNoQuotation;
                    txtStatus.Text = status;
                }  
            }
            //return retValue;
        }
        #region 20130502 Gerry Removed
        /*
        private bool CheckPickupAndDelivery(AirJob airJob)
        {
            bool retValue = true;
            try
            {
                string pickupDeliveryMsg = string.Empty;
                //TruckingFacadeIn.CheckPickUpAndDeliveryDetails(airJob, airJobType, out pickupDeliveryMsg);
                if (!pickupDeliveryMsg.Equals(string.Empty))
                {
                    if (ShowWarning(pickupDeliveryMsg) == DialogResult.No)
                    {
                        //ClearFields();
                        retValue = false;
                        status += "\n\n" + pickupDeliveryMsg; 
                        txtStatus.Text = status;
                    }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return retValue;
        }
         */
        #endregion

        private void ClearFields()
        {
            //cboJobNo.Text = string.Empty;
            txtHAWB.Text = string.Empty;
            txtMAWB.Text = string.Empty;
            txtCustName.Text = string.Empty;
            txtStatus.Text = string.Empty;
            cboJobNo.Focus();
        }   
        private DialogResult ShowWarning(string msg)
        {
            DialogResult dr = MessageBox.Show(msg + CommonResource.ConfirmToProceed, CommonResource.Warning, MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                return DialogResult.Yes;
            }
            else
            {                                       
                return DialogResult.No; 
            }   
        }

        private void rdbAirJobType_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdbAirExport.Checked)
                {
                    airJobType = AirJobType.AirExport;
                }
                else if (rdbAirImport.Checked)
                {
                    airJobType = AirJobType.AirImport;
                }
                ClearFields();
                cboAWBNo.DataSource = TruckingFacadeIn.GetAllAWBNos(airJobType);
                cboJobNo.DataSource = TruckingFacadeIn.GetAllJobNos(airJobType, cboAWBNo.Text.ToString().Trim());
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }

        private void cboJobNo_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                ClearFields();
                btnRetrieveAirJob.Enabled = true;
                txtStatus.Text = string.Empty;
                if (!cboJobNo.Text.ToString().Trim().Equals(string.Empty))
                {
                    status = CommonResource.Status + "\n--------------------";
                    airJob = TruckingFacadeIn.GetAirJob(cboJobNo.Text.ToString().Trim(), airJobType, txtHAWB.Text.ToString().Trim(), txtMAWB.Text.ToString().Trim());
                    if (airJob != null)
                    {
                        CheckBillingOption(airJob.CustCode);
                        txtHAWB.Text = airJob.HAWB;
                        txtMAWB.Text = airJob.MAWB;
                        cboAWBNo.Text = airJob.AWB;
                        cboJobNo.Text = airJob.JobNo;
                        if (airJobType == AirJobType.AirExport)
                        {
                            txtCustName.Text = airJob.ShipperName;
                        }
                        else
                            txtCustName.Text = airJob.ConsigneeName;

                    }
                }
            }
            catch { }

        }

        private void ChkBillingOption_CheckChanged(object sender, EventArgs e)
        {
            try
            {
                if (airJob != null)
                {
                    if (rdbBillByTrucking.Checked && !airJob.CustCode.Trim().Equals(string.Empty))
                    {
                        CheckBillingOption(airJob.CustCode.Trim());
                    }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

        }  

        private void CBO_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {                                 
                base.AutoCompleteComboBox(sender, e, true);
            }
            catch { }
        }

        private void cboAWBNo_Leave(object sender, EventArgs e)
        {
            try
            {
                cboJobNo.DataSource = TruckingFacadeIn.GetAllJobNos(airJobType, cboAWBNo.Text.ToString().Trim());
            }
            catch { }

        }       
    }
}
