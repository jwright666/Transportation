
using GMap.NET;
using GoogleMaps.LocationServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.TruckPlanDLL.BLL
{
    public class TaskObject
    {
        public int task_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public DateTime desired_date { get; set; }
        public TimeSpan desired_time { get; set; }
        public DateTime completion_time { get; set; }
        public string inquirer { get; set; }
        public string enterprise { get; set; }
        public string target { get; set; }
        public string performer { get; set; }
        public TaskUserProfile performer_details { get; set; }
        public string vehicle { get; set; }
        public string status { get; set; }
        public Constraints constraints { get; set; }
        public string rnd { get; set; }
    }
    public class Constraints
    {
        public bool task_only_in_geozone { get; set; }
    }
    public class TaskLocation
    {
        public int loc_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public double radius { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public MapPoint geometry { get; set; }
        public string external_id { get; set; }
        public string kind { get; set; }
        public string enterprise { get; set; }
        public string rnd { get; set; }
    }
    public class TaskMessage
    {
        public int id { get; set; }
        public string task { get; set; }
        public string message { get; set; }
        public TaskUserProfile author_info { get; set; }
        public DateTime creation_time { get; set; }
        [JsonProperty("photos")]
        public List<Photo> photos { get; set; }
        [JsonProperty("media")]
        public List<ProofDelivery> media { get; set; }
        [JsonProperty("objects")]
        public List<TaskMessage> messageList { get; set; }
    }
    public class TaskMessageTobeSend
    {
        public string task { get; set; }
        public string message { get; set; }
    }
    public class TaskUserProfile
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password1 { get; set; }
        public string password2 { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string enterprise { get; set; }
        public bool is_active { get; set; }
        public bool is_enterprise_admin { get; set; }
        public bool is_staff { get; set; }
        public string resource_uri { get; set; }
    }

    public class ProofDelivery
    {
        public string file_url { get; set; }
    }

    public class Photo
    {
        public string image_url { get; set; }
    }

    public enum PlanTruckSubTripJob_Status
    {
        Sent = 0,
        Accepted = 1,
        In_progress = 2,
        Held = 3,
        Rejected = 4,
        Completed = 5
    }
}
