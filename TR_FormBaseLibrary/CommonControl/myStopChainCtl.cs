using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FM.TR_FMSystemDLL.BLL;

namespace FormBaseLibrary.CommonControl
{
    public partial class myStopChainCtrl : UserControl
    {
        // store a stop temporarily
        private Stop tmpStop;
        private List<Stop> selectedStops;
        public List<Stop> SelectedStops
        {
            get { return selectedStops; }
            set
            {
                selectedStops = value;
                BindingSource bds = new BindingSource { DataSource = selectedStops };
                dgvStops.DataSource = bds;
            }
        }
        // LstAllStops will be initialized only when the
        //  user clicks Add, and will be initialized only 
        // once
        public List<Stop> LstAllStops;
        public myStopChainCtrl()
        {
            InitializeComponent();

            // 2014-11-20 Zhou Kai, hides the addresses
            //Description.Visible = false;
            //Address1.Visible = false;
            //Address2.Visible = false;
            //Address3.Visible = false;
            //Address4.Visible = false;
            // 2014-11-20 Zhou Kai ends
        }

        public EventHandler On_OK_Clicked;
        public EventHandler On_Cancel_Clicked;

        private bool MoveUp(int index)
        {
            if (index > 0 && index < selectedStops.Count)
            {
                var tmp = selectedStops[index];
                selectedStops[index] = selectedStops[index - 1];
                selectedStops[index - 1] = tmp;
                SelectedStops = selectedStops;

                return true;
            }

            return false;
        }

        private bool MoveDown(int index)
        {
            if (index >= 0 && index < selectedStops.Count - 1)
            {
                var tmp = selectedStops[index];
                selectedStops[index] = selectedStops[index + 1];
                selectedStops[index + 1] = tmp;
                SelectedStops = selectedStops;

                return true;
            }

            return false;
        }

        private bool Delete(int index)
        {
            if (index >= 0 && index < selectedStops.Count)
            {
                selectedStops.RemoveAt(index);
                SelectedStops = selectedStops;

                return true;
            }

            return false;
        }

        private bool Add(/*CustomerDTO stop*/)
        {
            // initialize only once
            if (LstAllStops == null)
            {
                throw new Exception("Please provide all possible stops for the user to choose from.");
            }
            FrmStop frmStop = new FrmStop(LstAllStops);
            frmStop.ShowDialog();
            tmpStop = frmStop.GetSelectedStop();

            // only add when the user selected a valid stop
            if (tmpStop != null) { selectedStops.Add(tmpStop); SelectedStops = selectedStops; }
            frmStop.Close();
            frmStop.Dispose();

            return false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // this.Hide();
            if (On_OK_Clicked != null)
            {
                // the user plug in logic
                On_OK_Clicked(this, EventArgs.Empty);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Add(/*tmpStop*/);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvStops.SelectedRows.Count == 1)
            {
                int index = dgvStops.SelectedRows[0].Index;
                Delete(index);
                SelectedStops = selectedStops;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (dgvStops.SelectedRows.Count == 1)
            {
                int index = dgvStops.SelectedRows[0].Index;
                if (MoveUp(index))
                {
                    dgvStops.ClearSelection(); // 2014-10-20 Zhou Kai adds
                    dgvStops.Rows[index - 1].Selected = true;
                }
            }

        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (dgvStops.SelectedRows.Count == 1)
            {
                int index = dgvStops.SelectedRows[0].Index;
                if (MoveDown(index))
                {
                    dgvStops.ClearSelection(); // 2014-10-20 Zhou Kai adds
                    dgvStops.Rows[index + 1].Selected = true;
                }

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (On_Cancel_Clicked == null)
            {
                // the default logic goes here
                throw new NotImplementedException();
            }
            else // the user plug in logic
            { On_Cancel_Clicked(this, EventArgs.Empty); }
        }

        private void dgvStops_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvStops.SelectedCells[0].RowIndex;
            dgvStops.Rows[index].Selected = true;
        }

        private void dgvStops_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = dgvStops.SelectedRows[0].Index;
            dgvStops.Rows[index].Selected = true;
        }

    }
}
