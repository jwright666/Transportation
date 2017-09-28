
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FM.TR_TRKBookDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_TRKPlanDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using TR_LanguageResource.Resources;
using TR_FormBaseLibrary;

namespace FM.TR_TRKPlanningUI.UI
{
    public partial class FrmActualQuantity : AbstractForm
    {
        FrmAddTripPlan parent;
        int actQty = 0;

        public FrmActualQuantity()
        {
            InitializeComponent();
        }
        public FrmActualQuantity(FrmAddTripPlan parent)
        {
            InitializeComponent();
            this.parent = parent;
            txtQty.Text = parent.truckJobTripDetail.balQty.ToString();
            //List<PlanTruckSubTripJobDetail> tempPlanTruckSubTripJobDetails = GetPlanTruckSubTripJobDetails();
            if (parent.planTruckSubTripJob.planTruckSubTripJobDetails.Count == 0)
                lblLoadedQty.Text += "0";
            else
            {
                var tempPlanTruckSubTripJobDetails = parent.planTruckSubTripJob.planTruckSubTripJobDetails.Where(ptstjd => ptstjd.jobID == parent.truckJobTripDetail.jobID &&
                                                                                                                   ptstjd.jobSeq == parent.truckJobTripDetail.jobTripSequence &&
                                                                                                                   ptstjd.planSubTripJobDetailSeqNo == parent.truckJobTripDetail.detailSequence).ToList();
                if (tempPlanTruckSubTripJobDetails.Count > 0)
                    lblLoadedQty.Text += tempPlanTruckSubTripJobDetails[0].qty.ToString();
                else
                    lblLoadedQty.Text += "0";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                AddPlanTruckSubTripJobDetail();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), CommonResource.Error); }
        }
        private void AddPlanTruckSubTripJobDetail()
        {
            try
            {
                actQty = Convert.ToInt32(txtQty.Text.ToString());
                ////Create planTruckSubTripJobDetail
                PlanTruckSubTripJobDetail planTruckSubTripJobDetail = PlanTruckSubTripJobDetail.CreatePlanTruckSubTripJobDetail(actQty, parent.planTruckSubTripJob, parent.truckJobTripDetail);
                var tempPlanTruckSubTripJobDetails = parent.planTruckSubTripJob.planTruckSubTripJobDetails.Where(ptstjd => ptstjd.jobID == planTruckSubTripJobDetail.jobID &&
                                                                                                                    ptstjd.jobSeq == planTruckSubTripJobDetail.jobSeq &&
                                                                                                                    ptstjd.planSubTripJobDetailSeqNo == planTruckSubTripJobDetail.planSubTripJobDetailSeqNo).ToList();
                ////remove PlanTruckSubTripJobDetail if exist in the list
                ////parent.planTruckSubTripJob.planTruckSubTripJobDetails.Remove(tempPlanTruckSubTripJobDetails[0]);

                if (tempPlanTruckSubTripJobDetails.Count > 0)
                {
                    if (actQty < 0)
                    {
                        if (tempPlanTruckSubTripJobDetails[0].qty < (actQty * (-1)))
                            throw new FMException("Cannot unload more than the loaded quantity. Loaded quantity is only " + tempPlanTruckSubTripJobDetails[0].qty.ToString());
                    }
                    tempPlanTruckSubTripJobDetails[0].qty += actQty;
                }
                else
                {
                    if (actQty < 0)
                    {
                        if (parent.planTruckSubTripJob.planTruckSubTripJobDetails[0].qty < (actQty * (-1)))
                            throw new FMException("Cannot unload because you haven't loaded any quantity yet. ");
                    }
                    parent.planTruckSubTripJob.planTruckSubTripJobDetails.Add(planTruckSubTripJobDetail);
                }
                parent.truckJobTripDetail.balQty -= actQty;
            }
            catch (FMException ex) { throw; }
            catch (Exception ex) { throw; }   
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.NumbersOnlyTextBox(sender, e);
        }
    }
}
