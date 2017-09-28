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

namespace FM.TR_TRKBookUI.UI
{
    public partial class FrmPrintOption : AbstractForm
    {
        private FrmRptBookingConfirmation frmBookingConfirmation;
        private FrmRptDeliveryNote frmRptDeliveryNote;
        private FrmTruckJob parent;
        private TruckJob truckJob;
        private SortableList<TruckJobTrip> truckJobTrip;
        string jobNo;
        int jobTripSeqNo;
        int jobID;
        bool printAll = true;
        private ModuleItemAccess moduleAccess;

        #region Constructors
        public FrmPrintOption(FrmTruckJob parent, TruckJob truckJob, SortableList<TruckJobTrip> truckJobTrip)
        {
            this.parent = parent;
            this.truckJob = truckJob;
            this.truckJobTrip = truckJobTrip;
            this.jobNo = truckJob.JobNo;
            this.jobID = truckJob.JobID;
            InitializeComponent();
        }

        #endregion

        #region Button Events
        private void btnBookingConfirmation_Click(object sender, EventArgs e)
        {
            if (frmBookingConfirmation == null || frmBookingConfirmation.IsDisposed)
            {
                frmBookingConfirmation = new FrmRptBookingConfirmation(jobNo, jobID, parent.user);
                frmBookingConfirmation.WindowState = FormWindowState.Maximized;
                frmBookingConfirmation.Show();
            }
            else
            {
                frmBookingConfirmation.BringToFront();
            }
            gboOption.Enabled = false;
            rdbAll.Checked = true;
        }

        private void btnDeliveryNote_Click(object sender, EventArgs e)
        {
            gboOption.Enabled = true;
            btnPrint.Visible = true;
            btnBookingConfirmation.Enabled = false;
            FillComboBox();
        }       

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (frmRptDeliveryNote == null || frmRptDeliveryNote.IsDisposed)
                {
                    if (printAll)
                    {
                        frmRptDeliveryNote = new FrmRptDeliveryNote(jobNo, jobTripSeqNo, true, parent.user);
                        frmRptDeliveryNote.WindowState = FormWindowState.Maximized;
                        frmRptDeliveryNote.Show();
                    }
                    else
                    {
                        frmRptDeliveryNote = new FrmRptDeliveryNote(jobNo, jobTripSeqNo, false, parent.user);
                        frmRptDeliveryNote.WindowState = FormWindowState.Maximized;
                        frmRptDeliveryNote.Show();
                    }

                }
                else
                {
                    frmRptDeliveryNote.BringToFront();
                }
                btnBookingConfirmation.Enabled = true;
                pnlJobTripSeqNo.Visible = false;
                rdbAll.Checked = true;
                gboOption.Enabled = false;
            }
            catch { }
        }
        #endregion

        public void FillComboBox()
        {
            //cboJobTripSeqNo.DataSource = null;
            cboJobTripSeqNo.DataSource = truckJobTrip;
            cboJobTripSeqNo.DisplayMember = "Sequence";
        }

        private void cboJobTripSeqNo_SelectedIndexChanged(object sender, EventArgs e)
        {            
            jobTripSeqNo = cboJobTripSeqNo.SelectedIndex + 1;
            txtStartStop.Text = truckJob.truckJobTrips[jobTripSeqNo - 1].StartStop.ToString().Replace(",","");
            txtEndStop.Text = truckJob.truckJobTrips[jobTripSeqNo - 1].EndStop.ToString().Replace(",","");
        }

        private void rdbSingle_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbSingle.Checked)
            {
                printAll = false;
                pnlJobTripSeqNo.Visible = true;
            }
            if (rdbAll.Checked)
            {
                printAll = true;
                pnlJobTripSeqNo.Visible = false;
            }
        }

        private void FrmPrintOption_Load(object sender, EventArgs e)
        {
            Localize();
            btnPrint.Visible = false;
            gboOption.Enabled = false;

            cboJobTripSeqNo.DropDownStyle = ComboBoxStyle.DropDownList;
            
            moduleAccess = UserAccess.GetUserAccessRightsForModuleItem(parent.user, "TPT_TRK_E_PRINT_OPTION");
            if (moduleAccess != null)
            {
                btnPrint.Visible = UserAccess.IsModuleItemAllowed(parent.user, "TPT_TRK_R_DELIVERY_NOTE");
                btnDeliveryNote.Enabled = UserAccess.IsModuleItemAllowed(parent.user, "TPT_TRK_R_DELIVERY_NOTE");
                btnBookingConfirmation.Enabled = UserAccess.IsModuleItemAllowed(parent.user, "TPT_TRK_R_BOOK_CONFIRM");
            }
        }

        public void Localize()
        {
            this.Text = CommonResource.PrintOption;
            btnBookingConfirmation.Text = CommonResource.BookingConfirmation;
            btnDeliveryNote.Text = CommonResource.DeliveryNote;
            btnCancel.Text = CommonResource.Cancel;
            btnPrint.Text = CommonResource.Print;
            gboOption.Text = CommonResource.PrintOption;
            rdbAll.Text = CommonResource.PrintAll;
            rdbSingle.Text = TptResourceUI.Single;
            lblSeqNo.Text = CommonResource.SeqNo;
            lblStartStop.Text = TptResourceUI.StartStop;
            lblEndStop.Text = TptResourceUI.EndStop;
        }
        
    }
}
