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
    public partial class FrmAutoCreateRoute : Form
    {
        private SortableList<TruckJobTrip> _dragJobTrips = new SortableList<TruckJobTrip>();
        public SortableList<DestinationRoute> _destinationList = new SortableList<DestinationRoute>();

        public FrmAutoCreateRoute(SortableList<TruckJobTrip> dragJobTrips)
        {
            InitializeComponent();
            this._dragJobTrips = dragJobTrips;
        }
        private SortableList<DestinationRoute> ReArrangedList()
        {
            _destinationList = new SortableList<DestinationRoute>();
            try
            {
                for (int i = 0; i < bdsDestinationRoutes.Count; i++)
                {
                    ((DestinationRoute)bdsDestinationRoutes[i]).priority = i + 1;
                    _destinationList.Add((DestinationRoute)bdsDestinationRoutes[i]);
                }
                bdsDestinationRoutes.DataSource = _destinationList;
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString()); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }
            return _destinationList;
        }
        private void btnUP_Click(object sender, EventArgs e)
        {
            try
            {
                int position = bdsDestinationRoutes.Position;
                if (position == 0) return;  // already at top

                bdsDestinationRoutes.RaiseListChangedEvents = false;

                DestinationRoute current = (DestinationRoute)bdsDestinationRoutes.Current;
                bdsDestinationRoutes.Remove(current);

                position--;

                bdsDestinationRoutes.Insert(position, current);
                bdsDestinationRoutes.Position = position;

                bdsDestinationRoutes.RaiseListChangedEvents = true;
                bdsDestinationRoutes.ResetBindings(false);

                ReArrangedList();
                bdsDestinationRoutes.ResetBindings(true);
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString()); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }

        }

        private void btnDOWN_Click(object sender, EventArgs e)
        {
            try
            {
                int position = bdsDestinationRoutes.Position;
                if (position == bdsDestinationRoutes.Count - 1) return;  // already at bottom
                bdsDestinationRoutes.RaiseListChangedEvents = false;
                DestinationRoute current = (DestinationRoute)bdsDestinationRoutes.Current;
                bdsDestinationRoutes.Remove(current);

                position++;

                bdsDestinationRoutes.Insert(position, current);
                bdsDestinationRoutes.Position = position;

                bdsDestinationRoutes.RaiseListChangedEvents = true;
                bdsDestinationRoutes.ResetBindings(false);

                ReArrangedList();
                bdsDestinationRoutes.ResetBindings(true);
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString()); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }

        }

        private void FrmAutoCreateRoute_Load(object sender, EventArgs e)
        {
            dgvAssignments.AutoGenerateColumns = false;
            int count = 1;
            foreach (TruckJobTrip trip in _dragJobTrips)
            {   //20170422 - gerry added for MTO logic
                if(trip.tripType == Trip_Type.MTO)
                    _destinationList.Add(new DestinationRoute { priority = count++, jobTrip = trip, destination = trip.StartStop });
                else
                    _destinationList.Add(new DestinationRoute { priority = count++, jobTrip = trip, destination = trip.EndStop });
            }
            bdsDestinationRoutes.DataSource = _destinationList;
            dgvAssignments.DataSource = bdsDestinationRoutes;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
