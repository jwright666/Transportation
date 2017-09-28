using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TR_AirFreightDLL.BLL;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;

namespace TR_AirFreightDLL.DAL
{
    internal class AirJobDAL
    {
        #region Air Job Table Name constants
        const string AI_JOB_TABLE = "AI_JOB_TBL";
        const string AE_JOB_TABLE = "AE_JOB_TBL"; 

        const string AI_MAIN_INFO_TABLE = "AI_JOB_MAIN_INFO_TBL";
        const string AE_MAIN_INFO_TABLE = "AE_JOB_MAIN_INFO_TBL";
                                                                         
        const string AI_DETAIL_DELIVERY_TABLE = "AI_JOB_DTL_Delivery_Tbl";  
        const string AE_DETAIL_PICK_TABLE = "AE_JOB_DTL_PICKUP_TBL";

        const string AI_DETAIL_RATE_TABLE = "AI_JOB_DTL_RATE_TBL";
        const string AE_DETAIL_RATE_TABLE = "AE_JOB_DTL_RATE_TBL";

        const string AI_DETAIL_DIMENSION_TABLE = "AI_JOB_DTL_DIMENSION_TBL";
        const string AE_DETAIL_DIMENSION_TABLE = "AE_JOB_DTL_DIMENSION_TBL";

        #endregion

        private static string GetPickup_DeliveryFields(object objField)
        {
            string retValue = string.Empty;
            return retValue = objField == DBNull.Value ? string.Empty : (string)objField;    
        }

        private static AirJob FillAirJobProperties(IDataReader reader, AirJobType jobType)
        { 
            AirJob tempAirJob = new AirJob();
            tempAirJob.JobID = (int)reader["JOB_ID"];
            tempAirJob.JobNo = reader["JOB_NUMBER"] == DBNull.Value ? string.Empty : (string)reader["JOB_NUMBER"];
            tempAirJob.CustCode = reader["Customer_Code"] == DBNull.Value ?  string.Empty : (string)reader["Customer_Code"];
            
             tempAirJob.HAWB = reader["HAWB_NUMBER"] == DBNull.Value ? string.Empty : (string)reader["HAWB_NUMBER"];
            tempAirJob.MAWB = reader["MAWB_NUMBER"] == DBNull.Value ? string.Empty : (string)reader["MAWB_NUMBER"];
            tempAirJob.AWB = reader["AWB_NUMBER"] == DBNull.Value ? string.Empty :  (string)reader["AWB_NUMBER"];
            string tempOrigin = reader["ORIGIN_CITY_CODE"] == DBNull.Value ? string.Empty : (string)reader["ORIGIN_CITY_CODE"];
            string tempDestination = reader["DEST_CITY_CODE"] == DBNull.Value ? string.Empty : (string)reader["DEST_CITY_CODE"];

            if (jobType == AirJobType.AirExport)
            {
                //Only Export has a booking number; import doesn't have
                tempAirJob.BookingNo = reader["BOOKING_NUMBER"] == DBNull.Value ? string.Empty : (string)reader["BOOKING_NUMBER"];
            }
            tempAirJob.Origin = GetOrigin_DestinationName(tempOrigin);
            tempAirJob.Destination = GetOrigin_DestinationName(tempDestination);
            //20170426 - flightDateTime will based on depart time

            DateTime tempFlightDate = (DateTime)reader["FLT_DATE1"]; //DateTime.Today.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            DateTime departTime = reader["DEPART_TIME"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(reader["DEPART_TIME"]);
            tempFlightDate = tempFlightDate.AddHours(departTime.Hour).AddMinutes(departTime.Minute).AddSeconds(departTime.Second);
            string tempFlightNo = reader["FLT_NUMBER1"] == DBNull.Value ? string.Empty : (string)reader["FLT_NUMBER1"];
            #region Flight date pulling
            /*
            //case for only 1 flight date was filled in
            if (reader["FLT_DATE1"] != DBNull.Value && reader["FLT_DATE2"] == DBNull.Value && reader["FLT_DATE3"] == DBNull.Value)
            {
                //tempFlightDate = (DateTime)reader["FLT_DATE1"];
                tempFlightNo = reader["FLT_NUMBER1"] == DBNull.Value ? string.Empty : (string)reader["FLT_NUMBER1"];
                tempFlightDate = tempFlightDate.AddHours(departTime.Hour).AddMinutes(departTime.Minute).AddSeconds(departTime.Second);
            }
            //case for 2 flight dates are filled in take the 2nd flight date
            else if (reader["FLT_DATE1"] != DBNull.Value && reader["FLT_DATE2"] != DBNull.Value && reader["FLT_DATE3"] == DBNull.Value)
            {
                //tempFlightDate = (DateTime)reader["FLT_DATE2"];
                tempFlightNo = reader["FLT_NUMBER2"] == DBNull.Value ? string.Empty : (string)reader["FLT_NUMBER2"];
                tempFlightDate = tempFlightDate.AddHours(departTime.Hour).AddMinutes(departTime.Minute).AddSeconds(departTime.Second);
            }
            //case for 3 flight dates are filled in take the 3rd flight date
            else if (reader["FLT_DATE1"] != DBNull.Value && reader["FLT_DATE2"] != DBNull.Value && reader["FLT_DATE3"] != DBNull.Value)
            {
                //tempFlightDate = (DateTime)reader["FLT_DATE3"];
                tempFlightNo = reader["FLT_NUMBER3"] == DBNull.Value ? string.Empty : (string)reader["FLT_NUMBER3"];
                tempFlightDate = tempFlightDate.AddHours(departTime.Hour).AddMinutes(departTime.Minute).AddSeconds(departTime.Second);
            }
            */ 
            #endregion
            tempAirJob.FlightNo = tempFlightNo;
            tempAirJob.FlightDate = tempFlightDate;  //reader["FLT_DATE1"] == DBNull.Value ? (DateTime)reader["FLT_DATE1"] : DateTime.Now;
            tempAirJob.ShipperCode = reader["SHIPPER_CODE"] == DBNull.Value ? string.Empty : (string)reader["SHIPPER_CODE"];
            tempAirJob.ShipperName = reader["SHIPPER_NAME"] == DBNull.Value ? string.Empty : (string)reader["SHIPPER_NAME"];
            tempAirJob.ShipperAdd1 = reader["SHIPPER_ADD1"] == DBNull.Value ? string.Empty : (string)reader["SHIPPER_ADD1"];
            tempAirJob.ShipperAdd2 = reader["SHIPPER_ADD2"] == DBNull.Value ? string.Empty : (string)reader["SHIPPER_ADD2"];
            tempAirJob.ShipperAdd3 = reader["SHIPPER_ADD3"] == DBNull.Value ? string.Empty : (string)reader["SHIPPER_ADD3"];
            tempAirJob.ShipperAdd4 = reader["SHIPPER_ADD4"] == DBNull.Value ? string.Empty : (string)reader["SHIPPER_ADD4"];
            tempAirJob.ConsigneeCode = reader["CONSIGNEE_CODE"] == DBNull.Value ? string.Empty : (string)reader["CONSIGNEE_CODE"];
            tempAirJob.ConsigneeName = reader["CONSIGNEE_NAME"] == DBNull.Value ? string.Empty : (string)reader["CONSIGNEE_NAME"];
            tempAirJob.ConsigneeAdd1 = reader["CONSIGNEE_ADD1"] == DBNull.Value ? string.Empty : (string)reader["CONSIGNEE_ADD1"];
            tempAirJob.ConsigneeAdd2 = reader["CONSIGNEE_ADD2"] == DBNull.Value ? string.Empty : (string)reader["CONSIGNEE_ADD2"];
            tempAirJob.ConsigneeAdd3 = reader["CONSIGNEE_ADD3"] == DBNull.Value ? string.Empty : (string)reader["CONSIGNEE_ADD3"];
            tempAirJob.ConsigneeAdd4 = reader["CONSIGNEE_ADD4"] == DBNull.Value ? string.Empty : (string)reader["CONSIGNEE_ADD4"];
           
            return tempAirJob;                        
        }
        private static AirTruckInfo FillAirTruckInfoProperties(IDataReader reader, AirJobType jobType)
        {
            AirTruckInfo tempAirTruckInfo = new AirTruckInfo();
            try
            {
                string tempPickupCode = string.Empty;
                string tempPickupName = string.Empty;
                string tempPickupAdd1 = string.Empty;
                string tempPickupAdd2 = string.Empty;
                string tempPickupAdd3 = string.Empty;
                string tempPickupAdd4 = string.Empty;
                string tempDeliveryCode = string.Empty;
                string tempDeliveryName = string.Empty;
                string tempDeliveryAdd1 = string.Empty;
                string tempDeliveryAdd2 = string.Empty;
                string tempDeliveryAdd3 = string.Empty;
                string tempDeliveryAdd4 = string.Empty;

                if (jobType == AirJobType.AirExport)
                {
                    #region For Export job type pickup and delivery details
                    tempAirTruckInfo.JobID = (int)reader["JOB_ID"];
                    //pulling of pickup info, first from air job main info if null or empty pull from JOB_DTL_PICKUP_TBL 
                    #region Pickup details
                    tempPickupCode = GetPickup_DeliveryFields(reader["Pickup_Place_Code"]) == string.Empty ? GetPickup_DeliveryFields(reader["PICKUP_PARTY_CODE"]) : GetPickup_DeliveryFields(reader["Pickup_Place_Code"]);
                    if (tempPickupCode.Equals(string.Empty))
                    {
                        //if both tables has empty pickcode pull from transport settings, set default to airportOfficeCode field 
                        OperatorDTO tempOperator = new OperatorDTO();
                        tempPickupCode = GetPickup_DeliveryFields(reader["AirportOfficeCode"]);
                        tempOperator = OperatorDTO.GetOperatorDTO(tempPickupCode);
                        tempPickupName = tempOperator.Name;
                        tempPickupAdd1 = tempOperator.Add1;
                        tempPickupAdd2 = tempOperator.Add2;
                        tempPickupAdd3 = tempOperator.Add3;
                        tempPickupAdd4 = tempOperator.Add4;
                    }
                    else
                    {
                        tempPickupName = GetPickup_DeliveryFields(reader["Pickup_Place_Name"]) == string.Empty ? GetPickup_DeliveryFields(reader["PICKUP_PARTY_NAME"]) : GetPickup_DeliveryFields(reader["Pickup_Place_Name"]);
                        tempPickupAdd1 = GetPickup_DeliveryFields(reader["Pickup_Place_Add1"]) == string.Empty ? GetPickup_DeliveryFields(reader["ADDRESS1"]) : GetPickup_DeliveryFields(reader["Pickup_Place_Add1"]);
                        tempPickupAdd2 = GetPickup_DeliveryFields(reader["Pickup_Place_Add2"]) == string.Empty ? GetPickup_DeliveryFields(reader["ADDRESS2"]) : GetPickup_DeliveryFields(reader["Pickup_Place_Add2"]);
                        tempPickupAdd3 = GetPickup_DeliveryFields(reader["Pickup_Place_Add3"]) == string.Empty ? GetPickup_DeliveryFields(reader["ADDRESS3"]) : GetPickup_DeliveryFields(reader["Pickup_Place_Add3"]);
                        tempPickupAdd4 = GetPickup_DeliveryFields(reader["Pickup_Place_Add4"]) == string.Empty ? GetPickup_DeliveryFields(reader["ADDRESS4"]) : GetPickup_DeliveryFields(reader["Pickup_Place_Add4"]);
                    }
                    tempAirTruckInfo.PickupCode = tempPickupCode;
                    tempAirTruckInfo.PickupName = tempPickupName;
                    tempAirTruckInfo.PickupAdd1 = tempPickupAdd1;
                    tempAirTruckInfo.PickupAdd2 = tempPickupAdd2;
                    tempAirTruckInfo.PickupAdd3 = tempPickupAdd3;
                    tempAirTruckInfo.PickupAdd4 = tempPickupAdd4;
                    #endregion

                    #region Delivery details
                    tempDeliveryCode = reader["Delivery_To_Code"] == DBNull.Value ? string.Empty : (string)reader["Delivery_To_Code"];
                    if (tempDeliveryCode.Equals(string.Empty))
                    {
                        OperatorDTO tempOperator = new OperatorDTO();
                        tempDeliveryCode = GetPickup_DeliveryFields(reader["AirportOfficeCode"]);
                        tempOperator = OperatorDTO.GetOperatorDTO(tempDeliveryCode);
                        tempDeliveryName = tempOperator.Name;
                        tempDeliveryAdd1 = tempOperator.Add1;
                        tempDeliveryAdd2 = tempOperator.Add2;
                        tempDeliveryAdd3 = tempOperator.Add3;
                        tempDeliveryAdd4 = tempOperator.Add4;
                    }
                    else
                    {
                        tempDeliveryName = reader["Delivery_To_Name"] == DBNull.Value ? string.Empty : (string)reader["Delivery_To_Name"];
                        tempDeliveryAdd1 = reader["Delivery_To_Add1"] == DBNull.Value ? string.Empty : (string)reader["Delivery_To_Add1"];
                        tempDeliveryAdd2 = reader["Delivery_To_Add2"] == DBNull.Value ? string.Empty : (string)reader["Delivery_To_Add2"];
                        tempDeliveryAdd3 = reader["Delivery_To_Add3"] == DBNull.Value ? string.Empty : (string)reader["Delivery_To_Add3"];
                        tempDeliveryAdd4 = reader["Delivery_To_Add4"] == DBNull.Value ? string.Empty : (string)reader["Delivery_To_Add4"];
                    }
                    tempAirTruckInfo.DeliveryCode = tempDeliveryCode;
                    tempAirTruckInfo.DeliveryName = tempDeliveryName;
                    tempAirTruckInfo.DeliveryAdd1 = tempDeliveryAdd1;
                    tempAirTruckInfo.DeliveryAdd2 = tempDeliveryAdd2;
                    tempAirTruckInfo.DeliveryAdd3 = tempDeliveryAdd3;
                    tempAirTruckInfo.DeliveryAdd4 = tempDeliveryAdd4;
                    #endregion

                    DateTime tempDate = reader["PICKUP_DATE"] == DBNull.Value ? DateTime.Now : (DateTime)reader["PICKUP_DATE"];
                    DateTime tempTime = reader["PICKUP_TIME"] == DBNull.Value ? DateTime.Now : (DateTime)reader["PICKUP_TIME"];
                    DateTime pickUpDateTime = tempDate == DateTime.Now ? DateTime.Now : tempDate.Add(tempTime.TimeOfDay);

                    tempAirTruckInfo.PickupDate = pickUpDateTime;
                    tempAirTruckInfo.DeliveryDate = pickUpDateTime.AddHours(1);

                    tempAirTruckInfo.Remarks = reader["REMARKS"] == DBNull.Value ? string.Empty : (string)reader["REMARKS"];
                    #endregion
                }
                else if (jobType == AirJobType.AirImport)
                {
                    #region For Import job type pickup and delivery details
                    tempAirTruckInfo.JobID = (int)reader["JOB_ID"];
                    #region Pickup details
                    tempPickupCode = reader["Pickup_Place_Code"] == DBNull.Value ? string.Empty : GetPickup_DeliveryFields(reader["Pickup_Place_Code"]);
                    // tempPickupCode = GetPickup_DeliveryFields(reader["Pickup_Place_Code"]) == string.Empty ? GetPickup_DeliveryFields(reader["PICKUP_PARTY_CODE"]) : GetPickup_DeliveryFields(reader["Pickup_Place_Code"]);
                    if (tempPickupCode.Equals(string.Empty))
                    {
                        //if both tables has empty pickcode pull from transport settings, set default to airportOfficeCode field 
                        OperatorDTO tempOperator = new OperatorDTO();
                        tempPickupCode = GetPickup_DeliveryFields(reader["AirportOfficeCode"]);
                        tempOperator = OperatorDTO.GetOperatorDTO(tempPickupCode);
                        tempPickupName = tempOperator.Name;
                        tempPickupAdd1 = tempOperator.Add1;
                        tempPickupAdd2 = tempOperator.Add2;
                        tempPickupAdd3 = tempOperator.Add3;
                        tempPickupAdd4 = tempOperator.Add4;
                    }
                    else
                    {
                        tempPickupName = reader["Pickup_Place_Name"] == DBNull.Value ? string.Empty : (string)reader["Pickup_Place_Name"];
                        tempPickupAdd1 = reader["Pickup_Place_Add1"] == DBNull.Value ? string.Empty : (string)reader["Pickup_Place_Add1"];
                        tempPickupAdd2 = reader["Pickup_Place_Add2"] == DBNull.Value ? string.Empty : (string)reader["Pickup_Place_Add2"];
                        tempPickupAdd3 = reader["Pickup_Place_Add3"] == DBNull.Value ? string.Empty : (string)reader["Pickup_Place_Add3"];
                        tempPickupAdd4 = reader["Pickup_Place_Add4"] == DBNull.Value ? string.Empty : (string)reader["Pickup_Place_Add4"];
                    }
                    tempAirTruckInfo.PickupCode = tempPickupCode;
                    tempAirTruckInfo.PickupName = tempPickupName;
                    tempAirTruckInfo.PickupAdd1 = tempPickupAdd1;
                    tempAirTruckInfo.PickupAdd2 = tempPickupAdd2;
                    tempAirTruckInfo.PickupAdd3 = tempPickupAdd3;
                    tempAirTruckInfo.PickupAdd4 = tempPickupAdd4;
                    #endregion

                    #region Delivery details
                    tempDeliveryCode = reader["Delivery_To_Code"] == DBNull.Value ? string.Empty : (string)reader["Delivery_To_Code"];
                    if (tempDeliveryCode.Equals(string.Empty))
                    {
                        OperatorDTO tempOperator = new OperatorDTO();
                        tempDeliveryCode = GetPickup_DeliveryFields(reader["AirportOfficeCode"]);
                        tempOperator = OperatorDTO.GetOperatorDTO(tempDeliveryCode);
                        tempDeliveryName = tempOperator.Name;
                        tempDeliveryAdd1 = tempOperator.Add1;
                        tempDeliveryAdd2 = tempOperator.Add2;
                        tempDeliveryAdd3 = tempOperator.Add3;
                        tempDeliveryAdd4 = tempOperator.Add4;
                    }
                    else
                    {
                        tempDeliveryName = reader["Delivery_To_Name"] == DBNull.Value ? string.Empty : (string)reader["Delivery_To_Name"];
                        tempDeliveryAdd1 = reader["Delivery_To_Add1"] == DBNull.Value ? string.Empty : (string)reader["Delivery_To_Add1"];
                        tempDeliveryAdd2 = reader["Delivery_To_Add2"] == DBNull.Value ? string.Empty : (string)reader["Delivery_To_Add2"];
                        tempDeliveryAdd3 = reader["Delivery_To_Add3"] == DBNull.Value ? string.Empty : (string)reader["Delivery_To_Add3"];
                        tempDeliveryAdd4 = reader["Delivery_To_Add4"] == DBNull.Value ? string.Empty : (string)reader["Delivery_To_Add4"];
                    }

                    tempAirTruckInfo.DeliveryCode = tempDeliveryCode;
                    tempAirTruckInfo.DeliveryName = tempDeliveryName;
                    tempAirTruckInfo.DeliveryAdd1 = tempDeliveryAdd1;
                    tempAirTruckInfo.DeliveryAdd2 = tempDeliveryAdd2;
                    tempAirTruckInfo.DeliveryAdd3 = tempDeliveryAdd3;
                    tempAirTruckInfo.DeliveryAdd4 = tempDeliveryAdd4;
                    #endregion
                    DateTime tempDate = reader["Delivery_Date"] == DBNull.Value ? DateTime.Now : (DateTime)reader["Delivery_Date"];
                    DateTime tempTime = reader["Delivery_Time"] == DBNull.Value ? DateTime.Now : (DateTime)reader["Delivery_Time"];
                    DateTime deliveryDateTime = tempDate == DateTime.Now ? DateTime.Now : tempDate.Add(tempTime.TimeOfDay);

                    tempAirTruckInfo.DeliveryDate = deliveryDateTime;
                    tempAirTruckInfo.PickupDate = deliveryDateTime.AddHours(-1);
                    //Delivery table doesn't have remarks field, I'm not sure which field to pull out
                    tempAirTruckInfo.Remarks = reader["PKG_Desc"] == DBNull.Value ? string.Empty : (string)reader["PKG_Desc"];
                    #endregion
                }
                //get dimension
                //dimension can be take from AE_JOB_DTL_PICKUP_TBL/AI_JOB_DTL_DELIVERY_TBL to cater multiple pickup/delivery
                AirJobDtlDimension tempAirJobDtlDimension = new AirJobDtlDimension();
                tempAirJobDtlDimension.JobID = (int)reader["JOB_ID"];
                tempAirJobDtlDimension.RateSeqNo = (int)reader["SEQUENCE_NO"];
                tempAirJobDtlDimension.DimenSeqNo = 1;//only 1 dimension detail
                tempAirJobDtlDimension.UOM = reader["UOM"] == DBNull.Value ? "PCS" : (string)reader["UOM"];
                if (jobType == AirJobType.AirExport)
                {
                    tempAirJobDtlDimension.GrossWeight = reader["GROSS_WEIGHT"] == DBNull.Value ? 0 : (decimal)reader["GROSS_WEIGHT"];
                    tempAirJobDtlDimension.Qty = reader["QUANTITY"] == DBNull.Value ? 0 : Convert.ToInt32(reader["QUANTITY"]);
                }
                else if (jobType == AirJobType.AirImport)
                {
                    tempAirJobDtlDimension.GrossWeight = reader["GROSS_WEIGHT"] == DBNull.Value ? 0 : (decimal)reader["GROSS_WEIGHT"];
                    tempAirJobDtlDimension.Qty = reader["Qty_Delivered"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Qty_Delivered"]);
                }
                //20170422 - wait for the fields names from soon
                tempAirJobDtlDimension.Length = reader["LENGTH"] == DBNull.Value ? 0 : (decimal)reader["LENGTH"];
                tempAirJobDtlDimension.Width = reader["WIDTH"] == DBNull.Value ? 0 : (decimal)reader["WIDTH"];
                tempAirJobDtlDimension.Heigth = reader["HEIGHT"] == DBNull.Value ? 0 : (decimal)reader["HEIGHT"];
                tempAirTruckInfo.airJobDtlDimensions.Add(tempAirJobDtlDimension);
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return tempAirTruckInfo;
        }
        private static AirJobRate FillAirJobRateProperties(IDataReader reader, AirJobType jobType)
        {
            AirJobRate tempAirJobRate = new AirJobRate();
            try
            {
                tempAirJobRate.JobID = (int)reader["JOB_ID"];
                tempAirJobRate.RateSeqNo = (int)reader["SEQUENCE_NO"];
                tempAirJobRate.GrossWeight = reader["GROSS_WEIGHT"] == DBNull.Value ? 0 : (decimal)reader["GROSS_WEIGHT"];
                if (jobType == AirJobType.AirExport)
                    tempAirJobRate.GoodsDescription = reader["GOODS_DESC"] == DBNull.Value ? string.Empty : (string)reader["GOODS_DESC"];
                else if (jobType == AirJobType.AirImport)
                    tempAirJobRate.GoodsDescription = reader["GOOD_DESC"] == DBNull.Value ? string.Empty : (string)reader["GOOD_DESC"];

                //dimension can be take from AE_JOB_DTL_PICKUP_TBL/AI_JOB_DTL_DELIVERY_TBL to cater multiple pickup/delivery
                tempAirJobRate.airJobDtlDimensions = GetAirJobDtlDimension(tempAirJobRate.JobID, tempAirJobRate.RateSeqNo, jobType);
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return tempAirJobRate;
        }
        //20170422 - gerry added for multiple pickup/delivery place
        private static AirJobRate FillAirJobRatePropertiesForMultiplePickupOrDelivery(IDataReader reader, AirJobType jobType)
        {
            AirJobRate tempAirJobRate = new AirJobRate();
            try
            {
                tempAirJobRate.JobID = (int)reader["JOB_ID"];
                tempAirJobRate.RateSeqNo = (int)reader["SEQUENCE_NO"];
                tempAirJobRate.GrossWeight = reader["GROSS_WEIGHT"] == DBNull.Value ? 0 : (decimal)reader["GROSS_WEIGHT"];
                if (jobType == AirJobType.AirExport)
                {
                    tempAirJobRate.GrossWeight = reader["GROSS_WEIGHT"] == DBNull.Value ? 0 : (decimal)reader["GROSS_WEIGHT"];
                    tempAirJobRate.GoodsDescription = reader["GOODS_DESC"] == DBNull.Value ? string.Empty : (string)reader["GOODS_DESC"];
                }
                else if (jobType == AirJobType.AirImport)
                {
                    tempAirJobRate.GrossWeight = reader["Weight_Delivered"] == DBNull.Value ? 0 : (decimal)reader["Weight_Delivered"];
                    tempAirJobRate.GoodsDescription = reader["GOOD_DESC"] == DBNull.Value ? string.Empty : (string)reader["GOOD_DESC"];
                }
                tempAirJobRate.airJobDtlDimensions = GetAirJobDtlDimension(tempAirJobRate.JobID, tempAirJobRate.RateSeqNo, jobType);
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return tempAirJobRate;
        }
        private static AirJobDtlDimension FillAirJobDtlDimensionProperties(IDataReader reader)
        {
            AirJobDtlDimension tempAirJobDtlDimension = new AirJobDtlDimension();
            try
            {
                tempAirJobDtlDimension.JobID = (int)reader["JOB_ID"];
                tempAirJobDtlDimension.RateSeqNo = (int)reader["SEQUENCE_NO"];
                tempAirJobDtlDimension.DimenSeqNo = (int)reader["SUB_SEQUENCE_NO"];
                tempAirJobDtlDimension.UOM = "PCS";
                tempAirJobDtlDimension.Qty = (int)reader["PCS"];
                tempAirJobDtlDimension.Length = reader["CARGO_LENGTH"] == DBNull.Value ? 0 : (decimal)reader["CARGO_LENGTH"];
                tempAirJobDtlDimension.Width = reader["CARGO_WIDTH"] == DBNull.Value ? 0 : (decimal)reader["CARGO_WIDTH"];
                tempAirJobDtlDimension.Heigth = reader["CARGO_HEIGHT"] == DBNull.Value ? 0 : (decimal)reader["CARGO_HEIGHT"];
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return tempAirJobDtlDimension;
        }
        internal static AirJob GetAirJob(string jobNo, AirJobType jobType, string hawb, string mawb)
        {   
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            AirJob tempAirJob = null;
            try
            {
                string SQLString = @"SELECT * FROM {0} job 
                                            INNER JOIN {1} mainInfo
                                            ON job.JOB_ID = mainInfo.JOB_ID 
                                            WHERE job.JOB_NUMBER = '{2}' ";

                #region Filter for Optional parameters
                //case where both hawb and mawb were supplied
                if (!hawb.Equals(string.Empty) && !mawb.Equals(string.Empty))
                {
                    SQLString += " AND job.HAWB_NUMBER = '" + hawb + "' AND job.MAWB_NUMBER = '" + mawb + "'";
                }
                //case where only hawb was supplied
                else if (!hawb.Equals(string.Empty) && mawb.Equals(string.Empty))
                {
                    SQLString += " AND job.HAWB_NUMBER = '" + hawb + "'";
                }
                //case where only mawb was supplied
                else if (hawb.Equals(string.Empty) && !mawb.Equals(string.Empty))
                {
                    SQLString += " AND job.MAWB_NUMBER = '" + mawb + "'";
                }
                #endregion

                if (jobType == AirJobType.AirExport)
                {
                    SQLString = string.Format(SQLString, AE_JOB_TABLE, AE_MAIN_INFO_TABLE, jobNo);
                }
                else if (jobType == AirJobType.AirImport)
                {
                    SQLString = string.Format(SQLString, AI_JOB_TABLE, AI_MAIN_INFO_TABLE, jobNo);
                }

                SqlCommand cmd = new SqlCommand(SQLString, cn); 
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tempAirJob = FillAirJobProperties(reader, jobType);
                    if (jobType == AirJobType.AirExport)
                        tempAirJob.CustCode = OperationDetail.GetOperationDetail(tempAirJob.ShipperCode).customerCode.ToString();//tempAirJob.ShipperCode; //since cust code not compulsory in air freight set default to shipper if Export job
                    else
                        tempAirJob.CustCode = OperationDetail.GetOperationDetail(tempAirJob.ConsigneeCode).customerCode.ToString();//tempAirJob.ConsigneeCode; //since cust code not compulsory in air freight set default to Consignee if Import job
                        
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { cn.Close(); }

            return tempAirJob;
        }
        //20130502 - Gerry modify to get all airtruckInfos not the first 1
        internal static SortableList<AirTruckInfo> GetAirTruckInfos(string jobNo, AirJobType jobType)
        {
            //AirTruckInfo tempAirTruckInfo = new AirTruckInfo();
            SortableList<AirTruckInfo> tempAirTruckInfos = new SortableList<AirTruckInfo>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string SQLString = @"SELECT *,(select distinct airportOfficeCode from TPT_SPECIAL_DATA_Tbl) airportOfficeCode FROM {0} job 
                                            INNER JOIN {1} mainInfo
                                            ON job.JOB_ID = mainInfo.JOB_ID 
                                            INNER JOIN {2} detail
                                            ON mainInfo.JOB_ID = detail.JOB_ID 
                                            WHERE job.JOB_NUMBER = '{3}' 
                                            ORDER BY detail.SEQUENCE_NO ";

                if (jobType == AirJobType.AirExport)
                {
                    SQLString = string.Format(SQLString,AE_JOB_TABLE, AE_MAIN_INFO_TABLE, AE_DETAIL_PICK_TABLE, jobNo);
                }
                else if (jobType == AirJobType.AirImport)
                {
                    SQLString = string.Format(SQLString, AI_JOB_TABLE, AI_MAIN_INFO_TABLE, AI_DETAIL_DELIVERY_TABLE, jobNo);
                }

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //tempAirTruckInfos = FillAirTruckInfoProperties(reader, jobType); 
                    //20170424 - gerry replace tot cater multiple pickup/delivery
                    tempAirTruckInfos.Add(FillAirTruckInfoProperties(reader, jobType));
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { cn.Close(); }
            return tempAirTruckInfos;
        }
        //not yet in used - wait the update from soon
        internal static SortableList<AirJobRate> GetAirJobMultiplePickupOrDelivery(int jobID, AirJobType jobType)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SortableList<AirJobRate> tempList = new SortableList<AirJobRate>();
            try
            {
                string SQLString = @"SELECT * FROM {0} 
                                     WHERE JOB_ID = '{1}'";

                if (jobType == AirJobType.AirExport)
                    SQLString = string.Format(SQLString, AE_DETAIL_PICK_TABLE, jobID);
                else if (jobType == AirJobType.AirImport)
                    SQLString = string.Format(SQLString, AI_DETAIL_DELIVERY_TABLE, jobID);

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tempList.Add(FillAirJobRatePropertiesForMultiplePickupOrDelivery(reader, jobType));
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { cn.Close(); }

            return tempList;
        }  
        internal static SortableList<AirJobRate> GetAirJobRates(int jobID, AirJobType jobType)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SortableList<AirJobRate> tempList = new SortableList<AirJobRate>();
            try
            {
                string SQLString = @"SELECT * FROM {0} rate
                                        inner join {1} pickup
                                    on pickup.job_id = rate.JOB_ID

                                     WHERE rate.JOB_ID = '{2}'";

                if (jobType == AirJobType.AirExport)
                    SQLString = string.Format(SQLString, AE_DETAIL_RATE_TABLE, AE_DETAIL_PICK_TABLE, jobID);
                else if (jobType == AirJobType.AirImport)
                    SQLString = string.Format(SQLString, AI_DETAIL_RATE_TABLE, AI_DETAIL_DELIVERY_TABLE, jobID);

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //20170424 - gerry replace tot cater multiple pickup/delivery
                    //tempList.Add(FillAirJobRateProperties(reader, jobType));
                    tempList.Add(FillAirJobRatePropertiesForMultiplePickupOrDelivery(reader, jobType));
                    
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { cn.Close(); }

            return tempList;
        }
        internal static SortableList<AirJobDtlDimension> GetAirJobDtlDimension(int jobID, int rateSeqNo, AirJobType jobType)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SortableList<AirJobDtlDimension> tempList = new SortableList<AirJobDtlDimension>();
            try
            {
                string SQLString = @"SELECT * FROM {0} 
                                     WHERE JOB_ID = {1} AND SEQUENCE_NO = {2}";

                if (jobType == AirJobType.AirExport)
                {
                    SQLString = string.Format(SQLString, AE_DETAIL_DIMENSION_TABLE, jobID, rateSeqNo);
                }
                else if (jobType == AirJobType.AirImport)
                {
                    SQLString = string.Format(SQLString, AI_DETAIL_DIMENSION_TABLE, jobID, rateSeqNo);
                }

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tempList.Add(FillAirJobDtlDimensionProperties(reader));
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { cn.Close(); }

            return tempList;
        }
        
        internal static ArrayList GetAllJobNos(AirJobType jobType, string awbNo)
        {
            ArrayList list = new ArrayList();
            list.Add(string.Empty);
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT DISTINCT job.JOB_NUMBER FROM {0} job 
                                    INNER JOIN {1} mainInfo
                                    ON job.JOB_ID = mainInfo.JOB_ID 
                                    WHERE job.JOB_NUMBER not in (Select SOURCE_REF_NUMBER from TRK_JOB_MAIN_Tbl)
                                        ";            
            try
            {
                if (awbNo.Equals(string.Empty))
                {
                    if (jobType == AirJobType.AirExport)
                    {
                        SQLString = string.Format(SQLString, AE_JOB_TABLE, AE_MAIN_INFO_TABLE);
                    }
                    else if (jobType == AirJobType.AirImport)
                    {
                        SQLString = string.Format(SQLString, AI_JOB_TABLE, AI_MAIN_INFO_TABLE);
                    }
                }
                else
                {
                    SQLString += " AND job.AWB_NUMBER ='{2}' ";
                    if (jobType == AirJobType.AirExport)
                    {
                        SQLString = string.Format(SQLString, AE_JOB_TABLE, AE_MAIN_INFO_TABLE, awbNo);
                    }
                    else if (jobType == AirJobType.AirImport)
                    {
                        SQLString = string.Format(SQLString, AI_JOB_TABLE, AI_MAIN_INFO_TABLE, awbNo);
                    }                      
                }

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader.GetString(0).ToString().Trim());
                }
                cn.Close();
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return list;
        }
        internal static ArrayList GetAllAWBNos(AirJobType jobType)
        {
            ArrayList list = new ArrayList();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT DISTINCT job.AWB_NUMBER FROM {0} job 
                                    INNER JOIN {1} mainInfo
                                    ON job.JOB_ID = mainInfo.JOB_ID ";
            try
            {
                if (jobType == AirJobType.AirExport)
                {
                    SQLString = string.Format(SQLString, AE_JOB_TABLE, AE_MAIN_INFO_TABLE);
                }
                else if (jobType == AirJobType.AirImport)
                {
                    SQLString = string.Format(SQLString, AI_JOB_TABLE, AI_MAIN_INFO_TABLE);
                }

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader.GetString(0).ToString().Trim());
                }
                cn.Close();
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return list;
        }
        internal static ArrayList GetAllCustomerCodes(AirJobType jobType)
        {
            ArrayList list = new ArrayList();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT DISTINCT mainInfo.{0} FROM FROM {1} job 
                                            INNER JOIN {2} mainInfo
                                            ON job.JOB_ID = mainInfo.JOB_ID";
            try
            {
                if (jobType == AirJobType.AirExport)
                {
                    SQLString = string.Format(SQLString, "SHIPPER_CODE", AE_JOB_TABLE, AE_MAIN_INFO_TABLE); 
                }
                else if (jobType == AirJobType.AirImport)
                {
                    SQLString = string.Format(SQLString, "CONSIGNEE_CODE", AI_JOB_TABLE, AI_MAIN_INFO_TABLE);
                }

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader.GetString(0).ToString().Trim());
                }
                cn.Close();
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return list;
        }

        internal static string GetOrigin_DestinationName(string code)
        {
            string retValue = string.Empty;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT  Airport_Name FROM CRT_Airport_Tbl
                                    Where  Airport_Code = '"+ code +"'";
            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue = reader.GetString(0);
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally
            {
                cn.Close();
            }
            return retValue;
        }
    }
}
