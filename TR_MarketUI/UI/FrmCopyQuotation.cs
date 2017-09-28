using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FM.FMSystem.BLL;
using TR_LanguageResource.Resources;
using FormBaseLibrary;
using FM.TransportMarket.BLL;

namespace FM.TransportMarket.UI
{
    public partial class FrmCopyQuotation : AbstractForm
    {
        public const string frmCopyQuotation = "TPT_HAU_E_COPY_QUOTATION";
        frmQuotationEntry parent;
        QuotationType quotationType;
        CustomerDTO cust = new CustomerDTO();
        SortableList<TransportRate> transportRates = new SortableList<TransportRate>();
        Quotation quotation = new Quotation();

        public FrmCopyQuotation()
        {
            InitializeComponent();
            Localize();
        }
        public FrmCopyQuotation(frmQuotationEntry parent, QuotationType quotationType, CustomerDTO cust, Quotation quotation)
        {
            InitializeComponent();
            Localize();
            // 1 is Customergroup , 2 is customer
            this.quotationType = quotationType;
            this.cust = cust;
            this.quotation = quotation;
            this.parent = parent;
        }

        public void Localize()
        {
            this.Text = TptResourceUI.ChooseQuotationNo;
            this.btnCancel.Text = CommonResource.Cancel;
            this.btnOk.Text = CommonResource.Save;
            this.lblQuotationNo.Text = CommonResource.QuotationNo;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Quotation tempQuotation = Quotation.GetAllQuotationHeader(cboQuotationNo.Text.Trim());
                //quotation.TransferTransportRates(parent.fmModule, (Quotation)cboQuotationNo.SelectedItem, frmCopyQuotation, "ipl");
                quotation.TransferTransportRates(parent.fmModule, tempQuotation, frmCopyQuotation, "ipl");
                Close();
            }
            catch (FMException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void FrmCopyQuotation_Load(object sender, EventArgs e)
        {
            //cboQuotationNo.DataSource = Quotation.GetAllQuotationHeaderExcept(quotation.QuotationID);
            //cboQuotationNo.DataSource = Quotation.GetLatestQuotationHeader("", "", true);
            //cboQuotationNo.DisplayMember = "QuotationNo";
            //cboQuotationNo.DataSource = Quotation.GetLatestQuotationNo(true);
            cboQuotationNo.DataSource = Quotation.GetQuotationNoToCopy(parent.quotation.QuotationNo);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cboQuotationNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtCustomerName.Text = (CustomerDTO.GetCustomerByCustCode(((Quotation)cboQuotationNo.SelectedItem).CustNo.ToString())).Name;
            txtCustomerName.Text = (CustomerDTO.GetCustomerByCustCode(Quotation.GetAllQuotationHeader(cboQuotationNo.Text.Trim()).CustNo.ToString())).Name;
            txtQuotationPeriod.Text = Quotation.GetAllQuotationHeader(cboQuotationNo.Text.Trim()).ValidFrom.ToShortDateString() + " - " + Quotation.GetAllQuotationHeader(cboQuotationNo.Text.Trim()).ValidTo.ToShortDateString();
        }

        private void cboQuotationNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cboQuotationNo.Text.Length > 0)
                base.AutoCompleteComboBox(sender, e, true);
        }
    }
}
