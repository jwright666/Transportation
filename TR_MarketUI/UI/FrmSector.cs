using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FM.TransportMaintenanceDLL.BLL;
using FM.FMSystem.BLL;
using TR_LanguageResource.Resources;

namespace FM.TransportMarket.UI
{
    public partial class FrmSector : FormBaseLibrary.frmEntryBase
    {
        FormMode formmode;
        public FrmSector()
        {
            InitializeComponent();
            Localize();
        }

        private void btnNewMaster_Click(object sender, EventArgs e)
        {
            formmode = FormMode.Add;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            txtSectorCode.Enabled = true;
            txtSectorCode.Text = "";
            txtDesc.Text = "";

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            bdsMaster.DataSource = Sector.GetAllSectors();

        }

        public void Localize()
        {
            //TabPage
            tabPageSearch.Text = CommonResource.Search;
            tabPageMaster.Text = CommonResource.Master;
            //Buttons
            btnNewMaster.Text = CommonResource.New;
            btnEditMaster.Text = CommonResource.Edit;
            btnDeleteMaster.Text = CommonResource.Delete;
            btnQuery.Text = CommonResource.Query;
            btnSave.Text = CommonResource.Save;
            btnCancel.Text = CommonResource.Cancel;
            btnClose.Text = CommonResource.Close;
            btnPrint.Text = CommonResource.Print;
            //Labels
            lblSectorCode.Text = CommonResource.SectorCode + ":";
            lblDescription.Text = CommonResource.Description + ":";            
            //DataGrid
            dgvMaster.Columns["sectorCodeDataGridViewTextBoxColumn"].HeaderText = CommonResource.SectorCode;
            dgvMaster.Columns["sectorDescriptionDataGridViewTextBoxColumn"].HeaderText = CommonResource.Description;         

        }

        private void btnEditMaster_Click(object sender, EventArgs e)
        {
            if (bdsMaster.Count > 0)
            {
                formmode = FormMode.Edit;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;

                txtSectorCode.Enabled = false;
            }
        }

        private void btnDeleteMaster_Click(object sender, EventArgs e)
        {
            if (bdsMaster.Count > 0)
            {
                Sector tempsector = new Sector();
                try
                {
                    tempsector = (Sector)bdsMaster.Current;
                    tempsector.DeleteSector();
                    MessageBox.Show(CommonResource.DeleteSuccess, CommonResource.Alert);

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
        }

        private void FrmSector_Load(object sender, EventArgs e)
        {
            Localize();
            btnSave.Enabled = false;
            btnCancel.Enabled = false;

            ManageUserAccess(module, user);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (formmode == FormMode.Add)
            {
                Sector tempsector = new Sector();
                try
                {
                    tempsector.SectorCode = txtSectorCode.Text;
                    tempsector.SectorDescription = txtDesc.Text;
                    tempsector.AddSector();

                    MessageBox.Show(CommonResource.SaveSuccess, CommonResource.Alert);
                    pnlMaster.Enabled = false;
                    pnlQuery.Enabled = true;
                    btnNewMaster.Enabled = true;
                    btnEditMaster.Enabled = true;
                    btnDeleteMaster.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
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
            if (formmode == FormMode.Edit)
            {
                Sector tempsector = new Sector();
                try
                {
                    tempsector.SectorCode = txtSectorCode.Text;
                    tempsector.SectorDescription = txtDesc.Text;
                    tempsector.EditSector();

                    MessageBox.Show(CommonResource.EditSuccess, CommonResource.Alert);
                    pnlMaster.Enabled = false;
                    pnlQuery.Enabled = true;
                    btnNewMaster.Enabled = true;
                    btnEditMaster.Enabled = true;
                    btnDeleteMaster.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
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

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnNewMaster.Enabled = true;
            btnEditMaster.Enabled = true;
            btnDeleteMaster.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void bdsMaster_CurrentChanged(object sender, EventArgs e)
        {
            Sector tempsector = new Sector();
            tempsector = (Sector)bdsMaster.Current;
            txtSectorCode.Text = tempsector.SectorCode;
            txtDesc.Text = tempsector.SectorDescription;

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }
    }
}
