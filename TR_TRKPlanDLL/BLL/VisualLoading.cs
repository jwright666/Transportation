using FM.TR_FMSystemDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_TRKBookDLL.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.TR_TRKPlanDLL.BLL
{
    public class Authentication
    {
        public Uri apiBaseUri { get; set; }
        public string username { get; set; }
        public string api_key { get; set; }
        public string authentication_token { get; set; }
        public string appLogin_key { get; set; }
    }
    public class Shipment
    {
        public Guid id { get; set; } //Shipment ID
        public string name { get; set; } //Name of the shipment
        public Guid cargospace_id { get; set; } //ID of user CargoSpace
        public int measure_system { get; set; } //Measurement system used for all dimensions and weights in this shipment; 
        public Array items { get; set; } //Shipment cargo items
        public Guid user_id { get; set; } //ID of author User
        public string open_shipment_url { get; set; } //url to open for this specific shipment
        public int report_version { get; set; }
        public string report_url { get; set; }//url to open report for this specific shipment
    }
    public class CargoItem
    {
        public string group_name { get; set; }
        public string description { get; set; }
        public int pieces { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public double length { get; set; }
        public double total_weight { get; set; }
        public bool is_stackable { get; set; }
        public bool is_tiltable { get; set; }
        public bool is_rotable { get; set; }
    }
    public class CargoSpace
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public double length { get; set; }
        public double max_load { get; set; }
        public int measure_system { get; set; }
        public int type { get; set; }
        public Guid user_id { get; set; }
    }
    //for routing
    public class DestinationRoute
    {
        public int priority { get; set; }
        public Stop destination { get; set; }
        public TruckJobTrip jobTrip { get; set; }
        public string routeDescription { 
            get { return destination.Code + (destination.Description != string.Empty ? ("(" + destination.Description + "), ") : string.Empty) + destination.Address1;
            }
        }

        public override string ToString()
        {
            return priority.ToString() + " - " + jobTrip.EndStop.ToString();
        }
    }
}
