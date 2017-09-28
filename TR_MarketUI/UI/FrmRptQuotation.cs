using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FM.FMSystem.BLL;
using FM.TransportMarket.BLL;
using Microsoft.Reporting.WinForms;
using TR_LanguageResource.Resources;
using FM.FMSystem.DAL;

namespace FM.TransportMarket.UI
{
    public partial class FrmRptQuotation : Form
    {
        private string quotationNo = "";
        string start_cust_code = "";
        string end_cust_code = "";
        DateTime startQuotDate = DateTime.Today;
        DateTime endQuotDate = DateTime.Today;

        public User user;
        public string module;
        public ModuleItemAccess moduleAccess;
        FormMode formMode;


        protected void ManageUserAccess(string module, User user)
        {
            this.user = user;
            this.module = module;
            moduleAccess = UserAccess.GetUserAccessRightsForModuleItem(user, module);
            if (moduleAccess != null)
            {
                if (moduleAccess.Print == false)
                {
                    btnPreview.Visible = false;
                }
            }
        }

        public FrmRptQuotation()
        {
            InitializeComponent();
        }

        public FrmRptQuotation(Quotation quotation)
        {
            InitializeComponent();
            this.pnlHeader.Dock = DockStyle.None;
            try
            {
                this.quotationNo = quotation.QuotationNo.ToString();
            }
            catch
            {
                throw new FMException(TptResourceReport.RptNoRecordFoundtoPrint);
            }    
        }

        private void FrmRptQuotation_Load(object sender, EventArgs e)
        {
            Localize();
            this.Text = TptResourceUI.QuotationReport;
            cboStartCustNo.DataSource = CustomerDTO.GetAllCustomerCodes();
            cboEndCustNo.DataSource = CustomerDTO.GetAllCustomerCodes();
            cboStartCustNo.SelectedIndex =-1;
            cboEndCustNo.SelectedIndex = -1;

            lblStartCustCode.Text = TptResourceUI.StartCustCode;
            lblEndCustCode.Text = TptResourceUI.EndCustCode;
            btnPreview.Text = CommonResource.Preview;

            this.dataTable1TableAdapter.Connection.ConnectionString = FMGlobalSettings.TheInstance.getConnectionString();
            this.dtbSpecialDataTableAdapter.Connection.ConnectionString = FMGlobalSettings.TheInstance.getConnectionString();
            this.dtbSpecialDataTableAdapter.Fill(this.dstSpecialData.DtbSpecialData);
            //Feb. 25, 2011 - Gerry Added this for print button
            if (!quotationNo.Equals(string.Empty))
            {
                this.dataTable1TableAdapter.FillBy(this.dstQuotation.DataTable1, quotationNo);
                //this.reportViewer1.RefreshReport();
                this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                if (dstQuotation.Tables[0].Rows.Count <= 0)
                {
                    MessageBox.Show(TptResourceReport.ErrCriteriaNoRecordFound, CommonResource.Alert);
                }
            }
            ManageUserAccess(module, user);
        }

        private void Localize()
        {
            this.reportViewer1.LocalReport.EnableExternalImages = true;
            
            ReportParameterInfoCollection parameterInfos = reportViewer1.LocalReport.GetParameters();
            List<ReportParameter> parameters = new List<ReportParameter>();
            string reportPath = Application.StartupPath + "\\logo2.jpeg";
            if (parameterInfos.Count > 0)
            {                
                    parameters.Add(new ReportParameter("CustomerCode", CommonResource.CustomerCode + ":", true));
                    parameters.Add(new ReportParameter("CustomerName", CommonResource.CustomerName + ":", true));
                    parameters.Add(new ReportParameter("Address", CommonResource.Address + ":", true));
                    parameters.Add(new ReportParameter("Salesman", CommonResource.Salesman + ":", true));
                    parameters.Add(new ReportParameter("QuotationNo", CommonResource.QuotationNo + ":", true));
                    parameters.Add(new ReportParameter("QuotationDate", CommonResource.QuotationDate + ":", true));
                    parameters.Add(new ReportParameter("ValidFrom", CommonResource.ValidFrom + ":", true));
                    parameters.Add(new ReportParameter("ValidTo", CommonResource.ValidTo + ":", true));
                    parameters.Add(new ReportParameter("PaymentTerms", CommonResource.PaymentTerms + ":", true));
                    parameters.Add(new ReportParameter("Conditions", TptResourceUI.Conditions + ":", true));
                    parameters.Add(new ReportParameter("Number", CommonResource.No + ".", true));
                    parameters.Add(new ReportParameter("ChargeCode", CommonResource.ChargeCode, true));
                    parameters.Add(new ReportParameter("ChargeDescription", CommonResource.ChargeDescription, true));
                    parameters.Add(new ReportParameter("UOM", CommonResource.UOM, true));
                    parameters.Add(new ReportParameter("Currency", CommonResource.Currency, true));
                    parameters.Add(new ReportParameter("Charge", TptResourceUI.Charge, true));
                    parameters.Add(new ReportParameter("AmountH", CommonResource.Amount, true));
                    parameters.Add(new ReportParameter("Upper", CommonResource.Upper, true));
                    parameters.Add(new ReportParameter("AmountL", CommonResource.Amount, true));
                    parameters.Add(new ReportParameter("ReportDate", TptResourceReport.RptDate + ":", true));
                    parameters.Add(new ReportParameter("Page", TptResourceReport.RptPage, true));
                    parameters.Add(new ReportParameter("logo", reportPath));

                // Add report parameters
                reportViewer1.LocalReport.SetParameters(parameters);
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            Localize();
            start_cust_code = cboStartCustNo.Text;
            end_cust_code = cboEndCustNo.Text;
            startQuotDate = dtpStartQuotDate.Value.Date;
            endQuotDate = dtpEndQuotDate.Value.Date;

            switch (rdoAllQuotations.Checked)
            {
                case true:
                    if (!start_cust_code.Equals(string.Empty) && !end_cust_code.Equals(string.Empty))
                        this.dataTable1TableAdapter.Fill(this.dstQuotation.DataTable1, start_cust_code, end_cust_code, startQuotDate, endQuotDate);
                    else if (!start_cust_code.Equals(string.Empty) && end_cust_code.Equals(string.Empty))
                        this.dataTable1TableAdapter.Fill(this.dstQuotation.DataTable1, start_cust_code, start_cust_code, startQuotDate, endQuotDate);
                    else if (start_cust_code.Equals(string.Empty) && !end_cust_code.Equals(string.Empty))
                        this.dataTable1TableAdapter.Fill(this.dstQuotation.DataTable1, end_cust_code, end_cust_code, startQuotDate, endQuotDate);
                    else if (start_cust_code.Equals(string.Empty) && end_cust_code.Equals(string.Empty))
                        this.dataTable1TableAdapter.FillByQuotationDate(this.dstQuotation.DataTable1, startQuotDate, endQuotDate);
                    break;

                case false:
                    if (!start_cust_code.Equals(string.Empty) && !end_cust_code.Equals(string.Empty))
                        this.dataTable1TableAdapter.FillLatestQuotation(this.dstQuotation.DataTable1, start_cust_code, end_cust_code, startQuotDate, endQuotDate);
                    else if (!start_cust_code.Equals(string.Empty) && end_cust_code.Equals(string.Empty))
                        this.dataTable1TableAdapter.FillLatestQuotation(this.dstQuotation.DataTable1, start_cust_code, start_cust_code, startQuotDate, endQuotDate);
                    else if (start_cust_code.Equals(string.Empty) && !end_cust_code.Equals(string.Empty))
                        this.dataTable1TableAdapter.FillLatestQuotation(this.dstQuotation.DataTable1, end_cust_code, end_cust_code, startQuotDate, endQuotDate);
                    else if (start_cust_code.Equals(string.Empty) && end_cust_code.Equals(string.Empty))
                        this.dataTable1TableAdapter.FillByLatestAndQuotDate(this.dstQuotation.DataTable1, startQuotDate, endQuotDate);
                    break;

                default:
                    break;
            }
            //if (rdoAllQuotations.Checked)
            //    this.dataTable1TableAdapter.Fill(this.dstQuotation.DataTable1, start_cust_code, end_cust_code, startQuotDate, endQuotDate);
            //else
            //    this.dataTable1TableAdapter.FillLatestQuotation(this.dstQuotation.DataTable1, start_cust_code, end_cust_code, startQuotDate, endQuotDate);
           
            
            if (dstQuotation.Tables[0].Rows.Count <= 0)
            {
                MessageBox.Show(TptResourceReport.ErrCriteriaNoRecordFound, CommonResource.Alert);
            }
            this.reportViewer1.RefreshReport();
        }

        private void FrmRptQuotation_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void cboStartCustNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.Compare(cboStartCustNo.Text, cboEndCustNo.Text, StringComparison.Ordinal) > 0)
                cboStartCustNo.Text = cboEndCustNo.Text;
        }

        private void cboEndCustNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.Compare(cboStartCustNo.Text, cboEndCustNo.Text, StringComparison.Ordinal) > 0)
                cboEndCustNo.Text = cboStartCustNo.Text;
        }

        private void dtpStartQuotDate_CloseUp(object sender, EventArgs e)
        {
            if (DateTime.Compare(dtpStartQuotDate.Value, dtpEndQuotDate.Value) > 0)
                dtpStartQuotDate.Value = dtpEndQuotDate.Value;
        }

        private void dtpEndQuotDate_CloseUp(object sender, EventArgs e)
        {
            if (DateTime.Compare(dtpStartQuotDate.Value, dtpEndQuotDate.Value) > 0)
                dtpEndQuotDate.Value = dtpStartQuotDate.Value;
        }

    }
}
