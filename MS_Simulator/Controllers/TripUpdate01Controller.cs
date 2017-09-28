using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//
using FM.FMSystem.BLL;
using TR_MessageBLL.BLL;
using Newtonsoft.Json;

namespace MS_Simulator.Controllers
{
    public class TripUpdate01Controller : ApiController
    {
        /*
         * 2015-04-13 Zhou Kai creates this controller, to:
         * (1) listen to trip update messages sent from Growth Venture
         */

        #region "Actions"
        [HttpPost]
        [Route("USS_GV_WS/api/DriverUpdate")]
        public HttpResponseMessage OnReceiveTripUpdate01(TripUpdate02 tripUpdate)
        {
            HttpResponseMessage response;

            string runTimeError = String.Empty;
            #region Gerry removed
            /*if (tripUpdate.SelfCheck(tripUpdate, out runTimeError) &&
            runTimeError == String.Empty)
            {
                response =
                    new HttpResponseMessage(HttpStatusCode.Accepted);
                tripUpdate.InsertTripUpdate01IntoDB(tripUpdate, runTimeError, true);

                return response;
            }
            else
            {
                HttpError error = new HttpError("Invalid Trip Update Message.") 
                { Message = runTimeError };

                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
                tripUpdate.InsertTripUpdate01IntoDB(tripUpdate, runTimeError, false);
                return response;
            }
             * */
            #endregion
            //20150515 - gerry put inside try catch to handle exceptions
            try
            {
                #region REmoved 
                /*
                if (tripUpdate.SelfCheck(tripUpdate, out runTimeError))
                {
                    tripUpdate.SenderID = tripUpdate.PrimeMover;
                    TripUpdate01.InsertTripUpdate01IntoDB(tripUpdate, false, true);
                    response = Request.CreateResponse(HttpStatusCode.Accepted, tripUpdate);// "Message Received.");
                }
                else
                {
                    tripUpdate.PlannerRemark = runTimeError;
                    TripUpdate01.InsertTripUpdate01IntoDB(tripUpdate, false, false);
                    HttpError error = new HttpError("Invalid Trip Update Message.") { Message = runTimeError };
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
                }
                */
                #endregion
                if (tripUpdate.IsValidMessage(out runTimeError))
                {
                    tripUpdate.MsgTripStatus = MessageTripStatus.InProgress;
                    //tripUpdate.PlannerID = tripUpdate.PrimeMover;
                    TripUpdate02.InsertTripUpdate(tripUpdate);
                    response = Request.CreateResponse(HttpStatusCode.Accepted, tripUpdate);
                }
                else
                {
                    tripUpdate.IsProcessed = true;
                    tripUpdate.Remark = runTimeError;
                    TripUpdate02.InsertTripUpdate(tripUpdate);
                    HttpError error = new HttpError("Invalid Trip Update Message.") { Message = runTimeError };
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
                }
            }
            catch (FMException fmEx)
            {
                HttpError error = new HttpError("Invalid Trip Update Message.") { Message = fmEx.Message.ToString() };
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            }
            catch (Exception ex)
            {
                HttpError error = new HttpError("Invalid Trip Update Message.") { Message = ex.Message.ToString() };
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            }
            return response;
        }
        //Just example for growth venture webservice
        [HttpPost]
        [Route("USS_GV_WS/api/TripInstruction")]
        public HttpResponseMessage OnSendTripInstruction01(TripInstruction02 tripInstruction)
        {
            HttpResponseMessage response = null;
            string runTimeError = String.Empty;
            //20150515 - gerry put inside try catch to handle exceptions
            try
            {
                //GV has some validation logic
                //this is just an example of receiving an intruction from planner
                response = Request.CreateResponse(HttpStatusCode.Accepted, tripInstruction);
            }
            catch (FMException fmEx)
            {
                HttpError error = new HttpError("Invalid Trip Update Message.") { Message = fmEx.Message.ToString() };
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            }
            catch (Exception ex)
            {
                HttpError error = new HttpError("Invalid Trip Update Message.") { Message = ex.Message.ToString() };
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            }
            return response;
        }


        [HttpGet]
        [Route("USS_GV_WS/api/TripInstructions/{sender}/{format}")]
        public HttpResponseMessage OnSendTripInstruction01(string sender, string format)
        {
            HttpResponseMessage response = null;
            string runTimeError = String.Empty;
            //20150515 - gerry put inside try catch to handle exceptions
            try
            {
                List<TripInstruction02> instructions = TripInstruction02.GetAllTripInstructions(sender, DateUtility.GetSQLDateTimeMinimumValue(), DateUtility.GetSQLDateTimeMaximumValue());
                //GV has some validation logic
                //this is just an example of receiving an intruction from planner
                if (format != null)
                {
                    if (format.ToLower().Contains("json"))
                    {
                        var result = JsonConvert.SerializeObject(instructions, Formatting.None,
                               new JsonSerializerSettings
                               {
                                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                               });
                        //return Json(result, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                        //return new System.Web.Mvc.JsonResult { Data = result, ContentType = "application/json", JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet };
                        //JsonSerializer jsonSerializer = new JsonSerializer();
                        //jsonSerializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        response = Request.CreateResponse(HttpStatusCode.Accepted, result);
                        response.Content = new StringContent(result, System.Text.Encoding.UTF8, "application/json");
                    }
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.Accepted, instructions);
                }
            }
            catch (FMException fmEx)
            {
                HttpError error = new HttpError("Invalid Trip Update Message.") { Message = fmEx.Message.ToString() };
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            }
            catch (Exception ex)
            {
                HttpError error = new HttpError("Invalid Trip Update Message.") { Message = ex.Message.ToString() };
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            }
            return response;
        }

        #endregion
    }
}
