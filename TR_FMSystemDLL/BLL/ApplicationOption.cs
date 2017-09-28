using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.BLL
{
    public class ApplicationOption
    {
        //Special Settings ID for trucking module
        //20140522 - gerry changed from SETTINGS_ID
        public const string TRUCKING_SETTINGS_ID = "TRUCKING";
        //Special Settings ID for haulage module
        public const string HAULAGE_SETTINGS_ID = "HAULAGE";
        //Special Settings ID for both haulage and trucking module
        public const string BOTH_SETTINGS_ID = "TRANSPORT";

        /*
        * SETTINGS_RETRIEVE_JOB_TRIP_TO_INVOICE_NAME
        * if value is True  then,
         *         if job trip ismultipackage is FALSE then
         *              1 job trip is converted to 1 invoice detail(1 truckJobCharge) per trip
         *         if job trip ismultipackage is TRUE then
         *              1 job trip have multi jobTripdetails, each job trip details are converted to 1 invoice detail(1 truckJobCharge) base on UOM
         * 
         * If value is FALSE then,
         *         1 job trip is converted to 1 invoice detail(1 truckJobCharge) and qty are combined based on billing UOM
         *         
         * it is used under TruckJob.cs to change truck movement charges
         */
        public const string SETTINGS_RETRIEVE_JOB_TRIP_TO_INVOICE_NAME = "RETRIEVE_ONE_JOB_TRIP_TO_ONE_INVOICE_DETAIL";
              
        /*
         * SETTINGS_TRUKC_JOB_BY_SUBCON
         * (1) Meaning of this setting: 
         *       Indicate if this installation will use sub-con related functions
         * (2) Meaning of setting_value: 
         *       true: user can use FrmSubContractor to key in sub-con data, and auto-generate sub-con report
         *       false: FrmSubContractor is disabled, sub-con report option in main-menu is hide
         * (3) Dependency:
         *       Independent
         * (4) Default value:
         *       false
         * (5) Where used:
         *       a. When FrmTruckJob loads, it's used to decide if the 
         *          tab "TruckJobTripSubCon" is displayed
         *       b. When we check if a truck job / truck job trip
         *          is carried out by sub-con
         */
        public const string SETTINGS_TRUKC_JOB_BY_SUBCON = "TRUCKJOB_BY_SUBCON";
        /*
         * SETTINGS_ARE_SUBCON_JOB_PLAN
         * (1) Meanning of this setting: 
         *      Indicate if this installation will use FrmChangeJobTripStatusSubCon
         * (2) Meanning of setting_value: 
         *      true: user can use the form to set status for multi-jobtrips one time
         *      false: FrmChangeJobTripStatusSubCon is disabled
         * (3) Dpendency:
         *       a. Dependent on SETTINGS_TRUKC_JOB_BY_SUBCON
         *       b. If SETTINGS_TRUKC_JOB_BY_SUBCON == true, it'll be true
         *       c. If SETTINGS_TRUKC_JOB_BY_SUBCON == false, it can be true or false
         * (4) Default value:
         *       false
         * (5) Where used:
         *       Planning --> Settings --> Change Trip Status for subcontractor jobs...
         */
        public const string SETTINGS_ARE_SUBCON_JOB_PLAN = "RETRIEVE_ARE_SUBCON_JOB_PLAN";

        /*
         * 2014-05-21 Zhou Kai adds
         * SETTINGS_FLAG_SUBCON_BY_JOB_OR_JOBTRIP
         * (1) Meanning of this setting: 
         *      It's a flag of the haulier side, the sub-contractor is at job or jobtrip level
         * (2) Meanning of setting_value: 
         *      true: haulier sub-contract a whole job
         *      false: haulier sub-contract per job trip 
         * (3) Dpendency:
         *       a. Dependent on SETTINGS_TRUKC_JOB_BY_SUBCON
         *       b. If SETTINGS_TRUKC_JOB_BY_SUBCON == true, it'll be true
         *       c. If SETTINGS_TRUKC_JOB_BY_SUBCON == false, it can be true or false
         * (4) Default value:
         *       false
         * (5) Where used:
         *       a. WYN2K sub-contract at job level, but Jupiter does it at jobTrip level
         *       b. UI, header tab and job-trip tab
         *       c. ... other places to be confirmed
         */
        public const string SETTINGS_FLAG_HAULIER_SUBCONTRACT_BY_JOB = "SUB_CONTRACT_WHOLE_JOB";
        // 2014-05-21 Zhou Kai ends

        ///20140620 - GERRY ADDED to set status for next leg to ready
        /// If SETTINGS_UPDATE_STATUS_NEXT_LEG == true, it will automatically update the status of the next leg to Ready
        /// If SETTINGS_UPDATE_STATUS_NEXT_LEG == false, it will not update the status of the next leg
        public const string SETTINGS_AUTO_UPDATE_STATUS_NEXT_LEG = "UPDATE_STATUS_NEXT_LEG";
        /// <summary>
        /// 2014-07-24 Zhou Kai adds the setting below:
        ///          SETTINGS_HAULAGE_INVOCE_BY_INDIVIDUAL_ROUNDTRIP_OR_SINGLE_LEG
        /// (1) Meanning of this setting: 
        ///      It's a flag of the haulier side, for invoicing
        /// (2) Meanning of setting_value: 
        ///    true: Each round trip or single trip is invoiced separately
        ///    false: Group by charge code and UOM and NoOfLeg
        ///(3) Dpendency:
        ///     None
        ///(4) Default value:
        ///    False
        ///(5) Where used:
        /// when calculating the haulier container movement charges
        /// </summary>
        public const string SETTINGS_HAULAGE_INVOCE_BY_TPTRATE_OR_PERTRIP =  "INVOICE BY INDIVIDUAL ROUND TRIP OR SINGLE LEG";
        /// <summary>
        /// 2015-02-05 Zhou Kai adds this setting:
        /// (1) Meanning of this setting:
        ///   It's a switch, when it's on, when transferring a sea-freight job
        ///   to haulage, all the trips under that job are set to booked then ready
        ///   automatically, otherwise, they are set to booked
        /// (2) Dependency:
        ///   None
        /// (3) Default value:
        ///   False
        /// (4) Where used:
        ///   a) when transferring a sea-import / sea-export job into haulage
        /// </summary>
        public const string AUTO_SET_TRIPS_TO_READY_AFTER_TRANSFERRING =   "SetTripsReady_WhenImportingSeaJob";
        //20150703 - Gerry added to get the value of SeaJob transporter
        public const string SETTINGS_SEAJOB_TRANSPORTER = "FILTER_SF_JOB_BY_TRANSPORTER_OP_CODE";
        /// <summary>
        /// 20150826 - gerry added to set setting for Planning  header display
        /// A = Display vehicleNo only
        /// B = Display drivername only
        /// C = Display drivername(vehicleNo)
        /// </summary>
        public const string SETTINGS_PLANNING_HEADER_DISPLAY = "PLANNING_HEADER_DISPLAY";
        /// <summary>
        /// 20151021 - gerry added to set either we do checking container ISO standard or not
        /// T = to do checking container ISO standard
        /// F = no need to checking container ISO standard
        /// </summary>
        public const string SETTINGS_CHECK_CONTAINER_ISO = "SETTINGS_CHECK_CONTAINER_ISO";
        /// <summary>
        /// 20151120 - gerry added to set either when complete plan sub trip, trailer will be assigned to next plan sub trip or set to available which is trailer park
        /// T= set status assigned to next plan sub trip
        /// F= set to others status (trailerPark, customerStuffing, customerUnstuffing) 
        /// </summary>
        public const string SETTINGS_ASSIGN_TRAILER_TO_NEXT_PLANSUBTRIP = "ASSIGN_TRAILER_TO_NEXT_PLANSUBTRIP";
        /// <summary>
        /// 20151230 - gerry added 
        /// T= allow to send multiple message to a vehicle
        /// F= don't allow to send multiple message to a vehicle, message can only be send to vehicle after completion of the previous job 
        /// </summary>
        public const string SETTINGS_ALLOW_SEND_MULTIPLE_MSG = "ALLOW_SEND_MULTIPLE_MESSAGE";
        /// <summary>
        /// 20160215 - gerry added 
        /// To get the allowable time to overlap
        /// </summary>
        public const string SETTINGS_ALLOWABLE_TIME_OVERLAP = "ALLOWABLE_TIME_OVERLAP";
        /// <summary>
        /// 20160310 - gerry added
        /// T = default job trip status is Ready
        /// F = default job trip status is booked
        /// </summary>
        public const string SETTINGS_AUTO_SET_JOBTRIP_TO_READY = "AUTO_SET_JOBTRIP_TO_READY";
        /// <summary> 
        /// 20161020 - gerry added to have gap between items inside the truck
        /// value = in centimeter
        /// </summary>
        public const string SETTINGS_GAP_PER_ITEM_INSIDE_THE_TRUCK = "GAP_PER_ITEM_IN_THE_TRUCK";
        /// <summary> 
        /// 20161020 - gerry added to start working hour
        /// value must be from 0 - 23
        /// </summary>
        public const string SETTINGS_START_WORKING_HOUR = "START_WORKING_HOUR";
        /// <summary>
        /// 20161206 - gerry added to auto calculate the time to take the job done
        /// T = system will calculate
        /// F = default to 1 hr
        /// </summary>
        public const string SETTINGS_AUTO_CALCULATE_JOB_TIME = "AUTO_CALCULATE_JOB_TIME";
        /// <summary>
        /// 20161212 - gerry added to sort jobtrip by fixed time
        /// T = sorted by fixed time
        /// F = default sorting
        /// </summary>
        public const string SETTINGS_IMPLEMENT_JOB_TRIP_FIXEDTIME = "IMPLEMENT_JOB_TRIP_FIXEDTIME";
        /// <summary>
        /// for TRUCKING
        /// value = default time gap for each appointment when drop to plan chart
        /// default = 1 hr
        /// </summary>
        public const string SETTINGS_TRK_PLAN_CHART_APOINTMENT_TIME_INTERVAL = "TRK_PLAN_CHART_APOINTMENT_TIME_INTERVAL";
        /// <summary>
        /// for HAULAGE
        /// value = default time gap for each appointment when drop to plan chart
        /// default = 1 hr
        /// </summary>
        public const string SETTINGS_HL_PLAN_CHART_APOINTMENT_TIME_INTERVAL = "HL_PLAN_CHART_APOINTMENT_TIME_INTERVAL";
        /// <summary>
        /// for Trucking
        /// value = default cut off time before flight 
        /// default = 2 hr
        /// </summary>
        public const string SETTINGS_TRK_PLAN_CUTOFF_TIME_BEFORE_FLIGHT = "TRK_PLAN_CUTOFF_TIME_BEFORE_FLIGHT";
        /// <summary>
        /// for both haulage and trucking
        /// T = demo application no need web service for messaging
        /// F = customer UAT or live
        /// </summary>
        public const string SETTINGS_IS_DEMO = "IS_DEMO";
        /// <summary>
        /// for trucking planning
        /// T = show planning button to auto assign first job to driver
        /// F = hide button from planning
        /// </summary>
        public const string SETTINGS_AUTO_ASSIGN_FIRST_JOB_FOR_OTO = "AUTO_ASSIGN_FIRST_JOB_FOR_OTO";


        public string setting_ID { get; set; }
        public string setting_Name { get; set; }
        public string setting_Desc { get; set; }
        public string setting_Value { get; set; }

        public ApplicationOption()
        {
            setting_ID = string.Empty;
            setting_Name = string.Empty;
            setting_Desc = string.Empty;
            setting_Value = string.Empty;
        }

        public static ApplicationOption GetApplicationOption(string setting_ID, string setting_Name)
        {
            return ApplicationOptionDAL.GetApplicationOption(setting_ID, setting_Name);
        }

    }
}
