using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using FM.FMSystem.BLL;
using TR_LanguageResource.Resources;
using System.Globalization;

namespace FM.TransportMarket.UI
{
    public partial class frmMarketMain : Form
    {
        public const string micQuotationEntry = "TPT_HAU_E_QUOTATION";
        public const string micQuotationReport = "TPT_HAU_R_QUOTATION";
        public const string micSector = "TPT_HAU_E_SECTOR";
        public const string micCharge = "TPT_HAU_E_CHARGE";
        private frmQuotationEntry FrmQuotation;
        private FrmRptQuotation frmRptQuotation;
        private FrmPackages frmPackage;
        private int childFormNumber = 0;
        private UserAccess userAccess;
        private User user;
        private FrmSector frmSector;
        private FrmCharge frmCharge;
        private Mutex mutex;
        public FMModule fmModule = FMModule.Trucking;


        private void frmMarketMain_Load(object sender, EventArgs e)
        {
            // correct syntax will be to pass the variable, userId
            // user = User.GetUser(userId);
            // the following for testing

            //user = User.GetUser("ipl");

            // This will be used to set the rights for reports, if any
            
            userAccess = UserAccess.GetUserAccess(user);
            Localize();

            quotationEntryToolStripMenuItem.Enabled = UserAccess.IsModuleItemAllowed(user, micQuotationEntry);
            tlsbQuotation.Enabled = quotationEntryToolStripMenuItem.Enabled;
            quotationReportToolStripMenuItem.Enabled = UserAccess.IsModuleItemAllowed(user, micQuotationReport);
            sectorToolStripMenuItem.Enabled = UserAccess.IsModuleItemAllowed(user, micSector);
            chargeToolStripMenuItem.Enabled = UserAccess.IsModuleItemAllowed(user, micCharge);

        }

        public void Localize()
        {
            tlsbQuotation.Text = CommonResource.Quotation;
            quotationToolStripMenuItem.Text = CommonResource.Quotation;
            quotationEntryToolStripMenuItem.Text = CommonResource.QuotationEntry;
            reportToolStripMenuItem.Text = TptResourceUI.Report;
            quotationReportToolStripMenuItem.Text = TptResourceUI.QuotationReport;
            sectorToolStripMenuItem.Text = TptResourceUI.Sector;
            chargeToolStripMenuItem.Text = TptResourceUI.Charge;
            statusStrip.Text = CommonResource.Status;
        }

        public frmMarketMain(string userid, Mutex mutex, string module)
        {
            InitializeComponent();
            this.user = User.GetUser(userid);
            this.mutex = mutex;
            if (user.UserID == "")
            {
                MessageBox.Show(CommonResource.InvalidLogin);
            }
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
 
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void quotationEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FrmQuotation == null || FrmQuotation.IsDisposed)
            {

                FrmQuotation = new frmQuotationEntry(user, micQuotationEntry, fmModule);
                FrmQuotation.WindowState = FormWindowState.Maximized;
                FrmQuotation.MdiParent = this;
                FrmQuotation.Show();
            }
            else
            {
                FrmQuotation.BringToFront();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            quotationEntryToolStripMenuItem.PerformClick();
/*
            if (FrmQuotation == null || FrmQuotation.IsDisposed)
            {
                FrmQuotation = new frmQuotationEntry();
                FrmQuotation.WindowState = FormWindowState.Maximized;
                FrmQuotation.MdiParent = this;
                FrmQuotation.user = "ipl";
                FrmQuotation.Show();
            }
            else
            {
                FrmQuotation.BringToFront();
            }
*/
        }



        private void quotationReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmRptQuotation == null || frmRptQuotation.IsDisposed)
            {
                frmRptQuotation = new FrmRptQuotation();
                frmRptQuotation.WindowState = FormWindowState.Maximized;
                frmRptQuotation.MdiParent = this;
                frmRptQuotation.user = user;
                frmRptQuotation.module = micQuotationReport;
                frmRptQuotation.Show();
            }
            else
            {
                frmRptQuotation.BringToFront();
            }
        }

        private void sectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmSector == null || frmSector.IsDisposed)
            {
                frmSector = new FrmSector();
                frmSector.WindowState = FormWindowState.Maximized;
                frmSector.MdiParent = this;
                frmSector.user = user;
                frmSector.module = micSector; 
                frmSector.Show();
            }
            else
            {
                frmSector.BringToFront();
            }
        }

        private void chargeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmCharge == null || frmCharge.IsDisposed)
            {
                frmCharge = new FrmCharge(user);
                frmCharge.WindowState = FormWindowState.Maximized;
                frmCharge.MdiParent = this;
                frmCharge.user = user;
                frmCharge.module = micCharge; 
                frmCharge.Show();
            }
            else
            {
                frmCharge.BringToFront();
            }
        }

        private void frmMarketMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            mutex.ReleaseMutex();
        }

        private void packageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmPackage == null || frmPackage.IsDisposed)
            {
                frmPackage = new FrmPackages(user);
                frmPackage.WindowState = FormWindowState.Maximized;
                frmPackage.MdiParent = this;
                frmPackage.Show();
            }
            else
            {
                frmPackage.BringToFront();
            }

        }
    }
}
