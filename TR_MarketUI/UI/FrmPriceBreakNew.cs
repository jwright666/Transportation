using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FM.TransportMarket.BLL;
using FM.FMSystem.BLL;
using TR_LanguageResource.Resources;
using FormBaseLibrary;


namespace FM.TransportMarket.UI
{
    public partial class FrmPriceBreakNew : AbstractForm
    {
        private FrmTransportRateNew parent;
        private PriceBreaks priceBreak = new PriceBreaks();

        public FrmPriceBreakNew()
        {
            InitializeComponent();
            Localize();
        }

        public FrmPriceBreakNew(FrmTransportRateNew parent)
        {
            InitializeComponent();
            this.parent = parent;
            Localize();
        }

        public void Localize()
        {
            lblEnd.Text = CommonResource.End;
            lblRemarks.Text = CommonResource.Remarks;
            rdbLumpSum.Text = TptResourceUI.LumpSum;
            rdbRate.Text = CommonResource.Rate;
            btnSave.Text = CommonResource.Save;
            btnCancel.Text = CommonResource.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                priceBreak.Seq_No = this.parent.transportrate.PriceBreaks.Count + 1;
                priceBreak.End = Convert.ToDecimal(txtEnd.Text);
                if (rdbLumpSum.Checked == true)
                {
                    priceBreak.IsLumpSum = true;
                    priceBreak.LumpSumValue = Convert.ToDecimal(txtLumpSumValue.Text);
                    priceBreak.Rate = 0;
                }
                else
                {
                    priceBreak.IsLumpSum = false;
                    priceBreak.LumpSumValue = 0;
                    priceBreak.Rate = Convert.ToDecimal(txtRate.Text);
                }
                priceBreak.Remarks = txtRemarks.Text;

                //TODO
                //Add Validation for pricebreak range...                      
                if (this.parent.transportrate.ValidateAddPricebreak(priceBreak))
                {
                    this.parent.transportrate.PriceBreaks.Add(priceBreak);
                    Close();
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show(TptResourceBLL.ErrNotNumeric, CommonResource.Error);
            }
            catch (FMException ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void rdbLumpSum_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbLumpSum.Checked == true)
            {
                txtLumpSumValue.Visible = true;
                txtRate.Visible = false;
            }
            else
            {
                txtLumpSumValue.Visible = false;
                txtRate.Visible = true;
            }

        }

        private void txtEnd_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                base.NumbersOnlyTextBox(sender, e);
            }
            catch { }
        }
    }
}
