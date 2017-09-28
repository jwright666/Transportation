using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MarketDLL.BLL;
using TR_LanguageResource.Resources;
using System.Collections;

namespace FM.TR_MarketDLL.DAL
{
    class TransportMarketDAL
    {
        const string QUOTATION_TYPE_CUSTOMER = "C";
        const string QUOTATION_TYPE_CUSTOMER_GROUP = "G";

        internal static SortableList<TransportRate> GetTransportRatesBasedOnQuotationNo(string QuotationNo)
        {
            Quotation tempquotation = GetAllQuotation(QuotationNo);
            SortableList<TransportRate> templist = new SortableList<TransportRate>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL ";
            SQLString += "WHERE QUOTATION_ID = " + tempquotation.QuotationID;
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                templist.Add(GetTransportRate(reader));
            }
            cn.Close();
            return templist;
        }
        internal static SortableList<TransportRate> GetContainerMovementRatesBasedOnQuotationQuotationIDStopendent(string quotationNo, bool stopDependent)
        {
            Quotation tempquotation = GetAllQuotation(quotationNo);
            SortableList<TransportRate> templist = new SortableList<TransportRate>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL ";
            SQLString += "WHERE QUOTATION_ID = " + tempquotation.QuotationID;
            SQLString += " AND IS_CONTAINER_MOVEMENT = 'T'";
            if (stopDependent == true)
                SQLString += " AND IS_STOP_DEPENDENT = 'T'";
            else
                SQLString += " AND IS_STOP_DEPENDENT = 'F'";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                templist.Add(GetTransportRate(reader));
            }
            cn.Close();
            return templist;

        }

        internal static SortableList<TransportRate> GetTruckMovementTransportRatesBasedOnQuotationNoAndStopDependent(string QuotationNo, bool stopDependent)
        {
            Quotation tempquotation = GetAllQuotation(QuotationNo);
            SortableList<TransportRate> templist = new SortableList<TransportRate>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL ";
            SQLString += "WHERE QUOTATION_ID = " + tempquotation.QuotationID;
            SQLString += " AND IS_TRUCK_MOVEMENT = 'T'";
            if (stopDependent == true)
                SQLString += " AND IS_STOP_DEPENDENT = 'T'";
            else
                SQLString += " AND IS_STOP_DEPENDENT = 'F'";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                templist.Add(GetTransportRate(reader));
            }
            cn.Close();
            return templist;
        }

        internal static SortableList<TransportRate> GetNonTruckMovementTransportRatesBasedOnQuotationNoAndStopDependent(string QuotationNo, bool stopDependent)
        {
            Quotation tempquotation = GetAllQuotation(QuotationNo);
            SortableList<TransportRate> templist = new SortableList<TransportRate>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL ";
            SQLString += "WHERE QUOTATION_ID = " + tempquotation.QuotationID;
            SQLString += " AND IS_CONTAINER_MOVEMENT = 'F'";
            SQLString += " AND IS_TRUCK_MOVEMENT = 'F'";
            if (stopDependent == true)
                SQLString += " AND IS_STOP_DEPENDENT = 'T'";
            else
                SQLString += " AND IS_STOP_DEPENDENT = 'F'";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                templist.Add(GetTransportRate(reader));
            }
            cn.Close();
            return templist;
        }


        internal static SortableList<TransportRate> GetNotContainerMovementTransportRatesBasedOnQuotationNo(string QuotationNo)
        {
            Quotation tempquotation = GetAllQuotation(QuotationNo);
            SortableList<TransportRate> templist = new SortableList<TransportRate>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL ";
            SQLString += "WHERE QUOTATION_ID = " + tempquotation.QuotationID;
            SQLString += " AND IS_CONTAINER_MOVEMENT = 'F'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                templist.Add(GetTransportRate(reader));
            }
            cn.Close();
            return templist;
        }

        internal static SortableList<TransportRate> GetTransportRatesAndPriceBreaksBasedOnQuotationID(Quotation quotation)
        {
            SortableList<TransportRate> templist = new SortableList<TransportRate>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL ";
            SQLString += "WHERE QUOTATION_ID = " + quotation.QuotationID;
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                templist.Add(GetTransportRate(reader));
            }
            reader.Close();
            cn.Close();
            return templist;
        }

        internal static TransportRate GetTransportRate(string QuotationNo, string ChargeCode, int SeqNo)
        {
            Quotation tempquotation = GetAllQuotation(QuotationNo);
            TransportRate temp = new TransportRate();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL ";
            SQLString += "Where QUOTATION_ID=" + tempquotation.QuotationID;
            SQLString += " AND CHARGE_CODE='" + ChargeCode + "'";
            SQLString += " AND SEQUENCE_NO=" + SeqNo;
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                temp = GetTransportRate(reader);
            }
            cn.Close();
            return temp;
        }

        internal static TransportRate GetTransportRate(int QuotationID, string ChargeCode)
        {
            TransportRate temp = new TransportRate();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL ";
            SQLString += "Where QUOTATION_ID=" + QuotationID;
            SQLString += "AND CHARGE_CODE='" + ChargeCode + "'";
            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    temp = GetTransportRate(reader);
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return temp;
        }

        internal static SortableList<TransportRate> GetAllTransportRates(string custID)
        {
            SortableList<TransportRate> templist = new SortableList<TransportRate>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT TPT_MKT_QUOTATION_DETAIL_RATE_TBL.* FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL,TPT_MKT_QUOTATION_HEADER_TBL ";
            SQLString += "WHERE TPT_MKT_QUOTATION_DETAIL_RATE_TBL.QUOTATION_ID = TPT_MKT_QUOTATION_HEADER_TBL.QUOTATION_ID";
            SQLString += " AND TPT_MKT_QUOTATION_HEADER_TBL.CUST_CODE = '" + custID + "'";


            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                templist.Add(GetTransportRate(reader));
            }
            reader.Close();
            cn.Close();
            return templist;
        }

        internal static TransportRate GetTransportRate(IDataReader reader)
        {
            string temp = (string)reader["IS_SECTOR_DEPENDENT"];
            bool is_sector_dependent = false;
            if (temp == "T")
            {
                is_sector_dependent = true;
            }

            temp = (string)reader["IS_STOP_DEPENDENT"];
            bool is_stop_dependent = false;
            if (temp == "T")
            {
                is_stop_dependent = true;
            }

            temp = (string)reader["IS_CONTAINER_MOVEMENT"];
            bool is_container_movement = false;
            if (temp == "T")
            {
                is_container_movement = true;
            }

            temp = (string)reader["IS_OVERWEIGHT"];
            bool is_overweight = false;
            if (temp == "T")
            {
                is_overweight = true;
            }

            temp = (string)reader["IS_TRUCK_MOVEMENT"];
            bool is_truck_movement = false;
            if (temp == "T")
            {
                is_truck_movement = true;
            }

            SortableList<PriceBreaks> priceBreaks = new SortableList<PriceBreaks>();
            TransportRate transportrate = new TransportRate(
               (int)reader["QUOTATION_ID"],
               (int)reader["SEQUENCE_NO"],
                //20140506 - gerry added to assign blank if dbvalue is null
               reader["CHARGE_CODE"] == DBNull.Value ? string.Empty : (string)reader["CHARGE_CODE"],
               reader["DESCRIPTION"] == DBNull.Value ? string.Empty : (string)reader["DESCRIPTION"],
               reader["RATE_TYPE"] == DBNull.Value ? string.Empty : (string)reader["RATE_TYPE"],
               reader["UOM"] == DBNull.Value ? string.Empty : (string)reader["UOM"],
               reader["MINIMUM_VALUE"] == DBNull.Value ? 0 : (decimal)reader["MINIMUM_VALUE"],
               reader["CURRENCY_CODE"] == DBNull.Value ? string.Empty : (string)reader["CURRENCY_CODE"],
                priceBreaks,
               (byte[])reader["UPDATE_VERSION"],
               reader["REMARKS"] == DBNull.Value ? string.Empty : (string)reader["REMARKS"],
               is_sector_dependent,
               reader["START_SECTOR_CODE"] == DBNull.Value ? string.Empty : (string)reader["START_SECTOR_CODE"],
               reader["END_SECTOR_CODE"] == DBNull.Value ? string.Empty : (string)reader["END_SECTOR_CODE"],
               is_stop_dependent,
               reader["START_STOP_CODE"] == DBNull.Value ? string.Empty : (string)reader["START_STOP_CODE"],
               reader["END_STOP_CODE"] == DBNull.Value ? string.Empty : (string)reader["END_STOP_CODE"],
               is_container_movement,
               reader["CONTAINER_CODE"] == DBNull.Value ? string.Empty : (string)reader["CONTAINER_CODE"],
               is_overweight,
               is_truck_movement,
               true,
               reader["NoOfLeg"] == DBNull.Value ? 0 : Convert.ToInt32(reader["NoOfLeg"]));


            transportrate.PriceBreaks = GetAllPriceBreakRange(transportrate);

            return transportrate;
        }

        internal static TransportRate GetTransportRateWithPriceBreaks(IDataReader reader)
        {
            SortableList<PriceBreaks> priceBreaks = new SortableList<PriceBreaks>();

            string temp = (string)reader["IS_SECTOR_DEPENDENT"];
            bool is_sector_dependent = false;
            if (temp == "T")
            {
                is_sector_dependent = true;
            }

            temp = (string)reader["IS_STOP_DEPENDENT"];
            bool is_stop_dependent = false;
            if (temp == "T")
            {
                is_stop_dependent = true;
            }

            temp = (string)reader["IS_CONTAINER_MOVEMENT"];
            bool is_container_movement = false;
            if (temp == "T")
            {
                is_container_movement = true;
            }

            temp = (string)reader["IS_OVERWEIGHT"];
            bool is_overweight = false;
            if (temp == "T")
            {
                is_overweight = true;
            }

            temp = (string)reader["IS_TRUCK_MOVEMENT"];
            bool is_truck_movement = false;
            if (temp == "T")
            {
                is_truck_movement = true;
            }

            return new TransportRate(
               (int)reader["QUOTATION_ID"],
               (int)reader["SEQUENCE_NO"],
                //20140506 - gerry added to assign blank if dbvalue is null
               reader["CHARGE_CODE"] == DBNull.Value ? string.Empty : (string)reader["CHARGE_CODE"],
               reader["DESCRIPTION"] == DBNull.Value ? string.Empty : (string)reader["DESCRIPTION"],
               reader["RATE_TYPE"] == DBNull.Value ? string.Empty : (string)reader["RATE_TYPE"],
               reader["UOM"] == DBNull.Value ? string.Empty : (string)reader["UOM"],
               reader["MINIMUM_VALUE"] == DBNull.Value ? 0 : (decimal)reader["MINIMUM_VALUE"],
               reader["CURRENCY_CODE"] == DBNull.Value ? string.Empty : (string)reader["CURRENCY_CODE"],
                priceBreaks,
               (byte[])reader["UPDATE_VERSION"],
               reader["REMARKS"] == DBNull.Value ? string.Empty : (string)reader["REMARKS"],
               is_sector_dependent,
               reader["START_SECTOR_CODE"] == DBNull.Value ? string.Empty : (string)reader["START_SECTOR_CODE"],
               reader["END_SECTOR_CODE"] == DBNull.Value ? string.Empty : (string)reader["END_SECTOR_CODE"],
               is_stop_dependent,
               reader["START_STOP_CODE"] == DBNull.Value ? string.Empty : (string)reader["START_STOP_CODE"],
               reader["END_STOP_CODE"] == DBNull.Value ? string.Empty : (string)reader["END_STOP_CODE"],
               is_container_movement,
               reader["CONTAINER_CODE"] == DBNull.Value ? string.Empty : (string)reader["CONTAINER_CODE"],
               is_overweight,
               is_truck_movement,
               true,
               reader["NoOfLeg"] == DBNull.Value ? 0 : Convert.ToInt32(reader["NoOfLeg"]));

        }

        internal static bool AddTransportRate(Quotation quotation, TransportRate transportRate, SqlConnection con, SqlTransaction tran, out TransportRate transportRateOut)
        {
            bool temp = false;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_INSERT_TPT_MKT_QUOTATION_DETAIL_RATE_TBL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@QUOTATION_ID", quotation.QuotationID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", 0);
                cmd.Parameters.AddWithValue("@CHARGE_CODE", transportRate.ChargeID);
                cmd.Parameters.AddWithValue("@DESCRIPTION", transportRate.Description);
                cmd.Parameters.AddWithValue("@RATE_TYPE", transportRate.ChargeType);
                cmd.Parameters.AddWithValue("@UOM", transportRate.UOM);
                cmd.Parameters.AddWithValue("@MINIMUM_VALUE", transportRate.Minimum);
                cmd.Parameters.AddWithValue("@CURRENCY_CODE", transportRate.Currency);
                cmd.Parameters.AddWithValue("@REMARKS", transportRate.Remarks);
                if (transportRate.IsSectorDependent == true)
                    cmd.Parameters.AddWithValue("@IS_SECTOR_DEPENDENT", "T");
                else
                    cmd.Parameters.AddWithValue("@IS_SECTOR_DEPENDENT", "F");
                if (transportRate.IsStopDependent == true)
                    cmd.Parameters.AddWithValue("@IS_STOP_DEPENDENT", "T");
                else
                    cmd.Parameters.AddWithValue("@IS_STOP_DEPENDENT", "F");
                cmd.Parameters.AddWithValue("@START_SECTOR_CODE", transportRate.StartSectorCode);
                cmd.Parameters.AddWithValue("@END_SECTOR_CODE", transportRate.EndSectorCode);
                cmd.Parameters.AddWithValue("@START_STOP_CODE", transportRate.StartStop);
                cmd.Parameters.AddWithValue("@END_STOP_CODE", transportRate.EndStop);
                if (transportRate.IsContainerMovement == true)
                    cmd.Parameters.AddWithValue("@IS_CONTAINER_MOVEMENT", "T");
                else
                    cmd.Parameters.AddWithValue("@IS_CONTAINER_MOVEMENT", "F");
                cmd.Parameters.AddWithValue("@CONTAINER_CODE", transportRate.ContainerCode);
                if (transportRate.IsOverweight == true)
                    cmd.Parameters.AddWithValue("@IS_OVERWEIGHT", "T");
                else
                    cmd.Parameters.AddWithValue("@IS_OVERWEIGHT", "F");
                if (transportRate.IsTruckMovement == true)
                    cmd.Parameters.AddWithValue("@IS_TRUCK_MOVEMENT", "T");
                else
                    cmd.Parameters.AddWithValue("@IS_TRUCK_MOVEMENT", "F");

                cmd.Parameters.AddWithValue("@NoOfLeg", transportRate.NoOfLeg);

                SqlParameter newReqNumber = new SqlParameter("@Value", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber);
                newReqNumber.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                transportRate.SequenceNo = Convert.ToInt32(newReqNumber.Value);

                transportRateOut = GetTransportRate(quotation, transportRate.ChargeID, transportRate.SequenceNo, con, tran);

                temp = true;
            }
            catch (Exception ex)
            {
                temp = false;
                throw ex;
            }
            return temp;
        }

        internal static bool EditTranportRate(Quotation quotation, TransportRate transportRate, SqlConnection con, SqlTransaction tran, out TransportRate transportRateOut)
        {
            bool temp = false;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_EDIT_TPT_MKT_QUOTATION_DETAIL_RATE_TBL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@QUOTATION_ID", transportRate.QuotationID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", transportRate.SequenceNo);
                cmd.Parameters.AddWithValue("@CHARGE_CODE", transportRate.ChargeID);
                cmd.Parameters.AddWithValue("@DESCRIPTION", transportRate.Description);
                cmd.Parameters.AddWithValue("@RATE_TYPE", transportRate.ChargeType);
                cmd.Parameters.AddWithValue("@UOM", transportRate.UOM);
                cmd.Parameters.AddWithValue("@MINIMUM_VALUE", transportRate.Minimum);
                cmd.Parameters.AddWithValue("@CURRENCY_CODE", transportRate.Currency);
                cmd.Parameters.AddWithValue("@REMARKS", transportRate.Remarks);
                if (transportRate.IsSectorDependent == true)
                    cmd.Parameters.AddWithValue("@IS_SECTOR_DEPENDENT", "T");
                else
                    cmd.Parameters.AddWithValue("@IS_SECTOR_DEPENDENT", "F");
                if (transportRate.IsStopDependent == true)
                    cmd.Parameters.AddWithValue("@IS_STOP_DEPENDENT", "T");
                else
                    cmd.Parameters.AddWithValue("@IS_STOP_DEPENDENT", "F");
                cmd.Parameters.AddWithValue("@START_SECTOR_CODE", transportRate.StartSectorCode);
                cmd.Parameters.AddWithValue("@END_SECTOR_CODE", transportRate.EndSectorCode);
                cmd.Parameters.AddWithValue("@START_STOP_CODE", transportRate.StartStop);
                cmd.Parameters.AddWithValue("@END_STOP_CODE", transportRate.EndStop);
                if (transportRate.IsContainerMovement)
                    cmd.Parameters.AddWithValue("@IS_CONTAINER_MOVEMENT", "T");
                else
                    cmd.Parameters.AddWithValue("@IS_CONTAINER_MOVEMENT", "F");
                cmd.Parameters.AddWithValue("@CONTAINER_CODE", transportRate.ContainerCode);
                if (transportRate.IsOverweight)
                    cmd.Parameters.AddWithValue("@IS_OVERWEIGHT", "T");
                else
                    cmd.Parameters.AddWithValue("@IS_OVERWEIGHT", "F");
                if (transportRate.IsTruckMovement)
                    cmd.Parameters.AddWithValue("@IS_TRUCK_MOVEMENT", "T");
                else
                    cmd.Parameters.AddWithValue("@IS_TRUCK_MOVEMENT", "F");

                cmd.Parameters.AddWithValue("@NoOfLeg", transportRate.NoOfLeg);

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                transportRateOut = GetTransportRate(quotation, transportRate.ChargeID, transportRate.SequenceNo, con, tran);

                temp = true;
            }
            catch (Exception ex)
            {
                temp = false;
                throw ex;
            }
            return temp;

        }

        internal static bool DeleteTransportRate(Quotation quotation, TransportRate transportRate, SqlConnection con, SqlTransaction tran)
        {
            bool temp = false;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_DELETE_TPT_MKT_QUOTATION_DETAIL_RATE_TBL", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@QUOTATION_ID", quotation.QuotationID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", transportRate.SequenceNo);

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                temp = true;

            }
            catch (Exception ex)
            {
                temp = false;
                throw new FMException(TptResourceDAL.ErrFailedToDeleteTransportRate);
            }
            return temp;
        }

        internal static bool AddQuotationHeader(Quotation quotation, SqlConnection con, SqlTransaction tran, out Quotation quotationOut)
        {
            bool temp = false;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                SqlCommand cmd2 = new SqlCommand("sp_Edit_Tpt_Quotation_Running_No", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                SqlParameter newReqNumber2 = new SqlParameter("@Value", SqlDbType.Int);
                cmd2.Parameters.Add(newReqNumber2);
                newReqNumber2.Direction = ParameterDirection.Output;
                cmd2.Transaction = tran;
                cmd2.ExecuteNonQuery();
                int runningno = Convert.ToInt32(newReqNumber2.Value);
                string srunningno = runningno.ToString();
                if (srunningno.Length == 1)
                {
                    srunningno = "0000" + srunningno;
                }
                else
                {
                    if (srunningno.Length == 2)
                    {
                        srunningno = "000" + srunningno;
                    }
                    else
                    {
                        if (srunningno.Length == 3)
                        {
                            srunningno = "00" + srunningno;
                        }
                        else
                        {
                            if (srunningno.Length == 4)
                            {
                                srunningno = "0" + srunningno;
                            }
                        }
                    }
                }



                SqlCommand cmd = new SqlCommand("sp_INSERT_TPT_MKT_QUOTATION_HEADER_TBL", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@QUOTATION_ID", quotation.QuotationID);
                string year = quotation.QuotationDate.Year.ToString();
                year = year.Substring(2, 2);
                string month = quotation.QuotationDate.Month.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                if (quotation.QuotationType == QuotationType.Customer)
                {
                    quotation.QuotationNo = "TR" + quotation.CustNo + year + month + srunningno;
                    cmd.Parameters.AddWithValue("@QUOTATION_TYPE", "C");
                }
                else
                {
                    quotation.QuotationNo = "TR" + quotation.CustomerGroup + year + month + srunningno;
                    cmd.Parameters.AddWithValue("@QUOTATION_TYPE", "G");
                }
                cmd.Parameters.AddWithValue("@QUOTATION_NO", quotation.QuotationNo);
                cmd.Parameters.AddWithValue("@CUST_CODE", quotation.CustNo);
                cmd.Parameters.AddWithValue("@QUOTATION_DATE", DateUtility.ConvertDateAndTimeForSQLPurpose(quotation.QuotationDate));
                cmd.Parameters.AddWithValue("@DATE_VALID_FROM", DateUtility.ConvertDateAndTimeForSQLPurpose(quotation.ValidFrom));
                cmd.Parameters.AddWithValue("@DATE_VALID_TO", DateUtility.ConvertDateAndTimeForSQLPurpose(quotation.ValidTo));
                cmd.Parameters.AddWithValue("@CREDIT_DAYS", quotation.CreditDays);
                cmd.Parameters.AddWithValue("@SALESMAN_NO", quotation.Salesman);
                cmd.Parameters.AddWithValue("@CONDITIONS", quotation.Conditions);
                cmd.Parameters.AddWithValue("@PAYMENT_TERMS", quotation.PaymentTerms);
                cmd.Parameters.AddWithValue("@REMARKS", quotation.Remarks);
                cmd.Parameters.AddWithValue("@REMARKS2", quotation.Remarks2);
                cmd.Parameters.AddWithValue("@REMARKS3", quotation.Remarks3);
                cmd.Parameters.AddWithValue("@REMARKS4", quotation.Remarks4);
                cmd.Parameters.AddWithValue("@REMARKS5", quotation.Remarks5);

                cmd.Parameters.AddWithValue("@CUSTOMER_GROUP_NO", quotation.CustomerGroup);
                if (quotation.IsValid == true)
                {
                    cmd.Parameters.AddWithValue("@ISVALID", "T");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ISVALID", "F");
                }
                cmd.Parameters.AddWithValue("@BRANCH_CODE", quotation.BranchCode);
                SqlParameter newReqNumber = new SqlParameter("@Value", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber);
                newReqNumber.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                quotation.QuotationID = Convert.ToInt32(newReqNumber.Value);

                quotationOut = GetQuotation(quotation.QuotationNo, con, tran);

                string SQLQuery = @"Update TPT_MKT_QUOTATION_HEADER_TBL
                                        set ISVALID ='F' 
                                        where QUOTATION_ID < (select MAX(QUOTATION_ID) from TPT_MKT_QUOTATION_HEADER_TBL 
                                                                where CUST_CODE = '{0}')
                                                AND CUST_CODE = '{0}'";
                SQLQuery = string.Format(SQLQuery, quotation.CustNo);

                SqlCommand cmd1 = new SqlCommand(SQLQuery, con);
                if (con.State == ConnectionState.Closed) { con.Open(); };
                cmd1.Transaction = tran;
                cmd1.ExecuteNonQuery();
                temp = true;
            }
            catch (Exception ex)
            {
                temp = false;
                throw new FMException(TptResourceDAL.ErrFailedToAddQuotationHeader);
            }
            return temp;
        }

        internal static bool EditQuotationHeader(Quotation quotation, SqlConnection con, SqlTransaction tran, out Quotation quotationOut)
        {
            bool success = false;
            try
            {
                byte[] originalRowVersion = new byte[8];
                string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL ";
                SQLString += "WHERE QUOTATION_ID = " + quotation.QuotationID;
                SqlCommand cmd1 = new SqlCommand(SQLString, con);
                cmd1.CommandType = CommandType.Text;
                cmd1.Transaction = tran;
                IDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    originalRowVersion = (byte[])reader["UPDATE_VERSION"];
                }
                reader.Close();
                if (SqlBinary.Equals(quotation.UpdateVersion, originalRowVersion) == false)
                {
                    success = false;
                    throw new FMException(TptResourceDAL.ErrMultiUserConflict + "\nTransportMarketDAL.EditQuotationHeader");

                }
                else
                {

                    SqlCommand cmd = new SqlCommand("sp_EDIT_TPT_MKT_QUOTATION_HEADER_TBL", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@QUOTATION_ID", quotation.QuotationID);
                    //                cmd.Parameters.AddWithValue("@QUOTATION_NO", quotation.QuotationNo);
                    //                cmd.Parameters.AddWithValue("@QUOTATION_TYPE", quotation.QuotationType);
                    //                cmd.Parameters.AddWithValue("@CUST_CODE", quotation.CustNo);
                    //                cmd.Parameters.AddWithValue("@QUOTATION_DATE", quotation.QuotationDate);
                    //                cmd.Parameters.AddWithValue("@DATE_VALID_FROM", quotation.ValidFrom);
                    cmd.Parameters.AddWithValue("@DATE_VALID_TO", DateUtility.ConvertDateAndTimeForSQLPurpose(quotation.ValidTo));
                    cmd.Parameters.AddWithValue("@CREDIT_DAYS", quotation.CreditDays);
                    cmd.Parameters.AddWithValue("@SALESMAN_NO", quotation.Salesman);
                    cmd.Parameters.AddWithValue("@CONDITIONS", quotation.Conditions);
                    cmd.Parameters.AddWithValue("@PAYMENT_TERMS", quotation.PaymentTerms);
                    cmd.Parameters.AddWithValue("@REMARKS", quotation.Remarks);
                    cmd.Parameters.AddWithValue("@REMARKS2", quotation.Remarks2);
                    cmd.Parameters.AddWithValue("@REMARKS3", quotation.Remarks3);
                    cmd.Parameters.AddWithValue("@REMARKS4", quotation.Remarks4);
                    cmd.Parameters.AddWithValue("@REMARKS5", quotation.Remarks5);
                    //                cmd.Parameters.AddWithValue("@CUSTOMER_GROUP_NO", quotation.CustomerGroup);

                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();

                    quotationOut = GetQuotation(quotation.QuotationNo, con, tran);

                    success = true;
                }
            }
            catch (Exception ex)
            {
                throw new FMException(TptResourceDAL.ErrFailedToEditQuotationHeader + "\n" + ex.ToString());
            }
            return success;
        }

        internal static bool DeleteQuotationHeader(Quotation quotation, SqlConnection con, SqlTransaction tran)
        {
            bool temp = false;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_DELETE_TPT_MKT_QUOTATION_HEADER_TBL", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@QUOTATION_ID", quotation.QuotationID);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                string SQLQuery = @"Update TPT_MKT_QUOTATION_HEADER_TBL
                                        set ISVALID ='T' 
                                        where QUOTATION_ID = (select MAX(QUOTATION_ID) from TPT_MKT_QUOTATION_HEADER_TBL 
                                                                where CUST_CODE = '{0}')
                                                AND CUST_CODE = '{0}'";
                SQLQuery = string.Format(SQLQuery, quotation.CustNo);

                SqlCommand cmd1 = new SqlCommand(SQLQuery, con);
                if (con.State == ConnectionState.Closed) { con.Open(); };
                cmd1.Transaction = tran;
                cmd1.ExecuteNonQuery();

                temp = true;
            }
            catch (Exception ex)
            {
                temp = false;
                throw ex;
                //throw new FMException(ResTransportMarketDAL.ErrorFailedToDeleteQuotationHeader);
            }
            return temp;
        }

        internal static bool AddPriceBreakRange(int quotationID, TransportRate transportrate, PriceBreaks priceBreakRange, SqlConnection con, SqlTransaction tran, out PriceBreaks priceBreakOut)
        {
            bool temp = false;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_INSERT_TPT_MKT_QUOTATION_DETAIL_RATE_PRICE_BREAKS_TBL", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@QUOTATION_ID", quotationID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO_RATE", transportrate.SequenceNo);
                cmd.Parameters.AddWithValue("@CHARGE_CODE", transportrate.ChargeID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", priceBreakRange.Seq_No);
                cmd.Parameters.AddWithValue("@BREAK_CEILING", priceBreakRange.End);
                cmd.Parameters.AddWithValue("@UNIT_RATE", priceBreakRange.Rate);
                if (priceBreakRange.IsLumpSum == true)
                {
                    cmd.Parameters.AddWithValue("@IS_LUMP_SUM", "Y");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@IS_LUMP_SUM", "N");
                }
                cmd.Parameters.AddWithValue("@LUMP_SUM_VALUE", priceBreakRange.LumpSumValue);
                cmd.Parameters.AddWithValue("@REMARKS", priceBreakRange.Remarks);

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                priceBreakOut = GetPriceBreakRange(quotationID, transportrate.SequenceNo, priceBreakRange.Seq_No, con, tran);

                temp = true;
            }
            catch (Exception ex)
            {
                temp = false;
                throw new FMException(TptResourceDAL.ErrFailedToAddPriceBreaks + "\n" + ex.ToString());
            }
            return temp;
        }

        internal static bool EditPriceBreakRange(int quotationID, TransportRate transportrate, PriceBreaks priceBreakRange, SqlConnection con, SqlTransaction tran, out PriceBreaks priceBreakOut)
        {
            bool temp = false;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_EDIT_TPT_MKT_QUOTATION_DETAIL_RATE_PRICE_BREAKS_TBL", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@QUOTATION_ID", quotationID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO_RATE", transportrate.SequenceNo);
                cmd.Parameters.AddWithValue("@CHARGE_CODE", transportrate.ChargeID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", priceBreakRange.Seq_No);
                cmd.Parameters.AddWithValue("@BREAK_CEILING", priceBreakRange.End);
                cmd.Parameters.AddWithValue("@UNIT_RATE", priceBreakRange.Rate);
                cmd.Parameters.AddWithValue("@IS_LUMP_SUM", priceBreakRange.IsLumpSum);
                cmd.Parameters.AddWithValue("@LUM_SUM_VALUE", priceBreakRange.LumpSumValue);
                cmd.Parameters.AddWithValue("@REMARKS", priceBreakRange.Remarks);

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                priceBreakOut = GetPriceBreakRange(quotationID, transportrate.SequenceNo, priceBreakRange.Seq_No, con, tran);

                temp = true;

            }
            catch (Exception ex)
            {
                temp = false;
                throw new FMException(TptResourceDAL.ErrFailedToEditPriceBreaks + "\n" + ex.ToString());
            }
            return temp;

        }

        internal static bool DeletePriceBreakRange(int quotationID, TransportRate transportrate, PriceBreaks priceBreakRange, SqlConnection con, SqlTransaction tran)
        {
            bool temp = false;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_DELETE_TPT_MKT_QUOTATION_DETAIL_RATE_PRICE_BREAKS_TBL", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@QUOTATION_ID", quotationID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO_RATE", transportrate.SequenceNo);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", priceBreakRange.Seq_No);

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                temp = true;
            }
            catch (Exception ex)
            {
                temp = false;
                throw new FMException(TptResourceDAL.ErrFailedToDeletePriceBreaks);
            }
            return temp;
        }

        internal static bool UpdatePriceBreakRange(int quotationID, TransportRate transportrate, PriceBreaks priceBreakRange, SqlConnection con, SqlTransaction tran)
        {
            bool temp = false;
            try
            {
                #region SQL Update Query String and Parameter
                string SQLString = @"update TPT_MKT_QUOTATION_DETAIL_RATE_PRICE_BREAKS_TBL
                                        set CHARGE_CODE = '{0}',
                                            BREAK_CEILING = '{1}',
                                            UNIT_RATE = '{2}',
                                            IS_LUMP_SUM = '{3}',
                                            LUMP_SUM_VALUE   = '{4}',
                                            REMARKS = '{5}'                                           
                                        where QUOTATION_ID = {6}
                                             AND SEQUENCE_NO_RATE = {7}
                                             AND SEQUENCE_NO = {8}
                                        ";
                string isLumpSum = "";
                if (priceBreakRange.IsLumpSum)
                    isLumpSum = "Y";
                else
                    isLumpSum = "N";
                SQLString = string.Format(SQLString,
                                            transportrate.ChargeID,
                                            priceBreakRange.End,
                                            priceBreakRange.Rate,
                                            isLumpSum,
                                            priceBreakRange.LumpSumValue,
                                            priceBreakRange.Remarks,
                                            quotationID,
                                            transportrate.SequenceNo,
                                            priceBreakRange.Seq_No);


                #endregion

                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.CommandType = CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                temp = true;
            }
            catch (Exception ex)
            {
                temp = false;
                throw new FMException(TptResourceDAL.ErrFailedToDeletePriceBreaks + "\n" + ex.ToString());
            }
            return temp;
        }
        internal static Quotation GetQuotation(string QuotationNo, SqlConnection con, SqlTransaction tran)
        {

            Quotation quotation = new Quotation();
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL ";
            SQLString += "WHERE QUOTATION_NO = '" + QuotationNo + "'";
            SqlCommand cmd = new SqlCommand(SQLString, con);
            cmd.Transaction = tran;
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                quotation = GetQuotation(reader);
            }
            reader.Close();
            return quotation;
        }

        internal static TransportRate GetTransportRate(Quotation quotation, string ChargeCode, int SeqNo, SqlConnection con, SqlTransaction tran)
        {

            TransportRate temp = new TransportRate();
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL ";
            SQLString += "Where QUOTATION_ID=" + quotation.QuotationID;
            SQLString += " AND CHARGE_CODE='" + ChargeCode + "'";
            SQLString += " AND SEQUENCE_NO=" + SeqNo;
            SqlCommand cmd = new SqlCommand(SQLString, con);
            if (con.State == ConnectionState.Closed) { con.Open(); }
            if (tran == null) { tran = con.BeginTransaction(); }
            cmd.Transaction = tran;
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                temp = GetTransportRate(reader);
            }
            reader.Close();
            return temp;
        }

        internal static PriceBreaks GetPriceBreakRange(int quotationid, int seqnorate, int seqno, SqlConnection con, SqlTransaction tran)
        {
            PriceBreaks pricebreak = new PriceBreaks();
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_PRICE_BREAKS_TBL ";
            SQLString += "WHERE QUOTATION_ID = " + quotationid;
            SQLString += " AND SEQUENCE_NO_RATE = " + seqnorate;
            SQLString += " AND SEQUENCE_NO = " + seqno;

            SqlCommand cmd = new SqlCommand(SQLString, con);
            cmd.Transaction = tran;
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pricebreak = GetPriceBreak(reader);
            }
            reader.Close();
            return pricebreak;
        }

        internal static Quotation GetAllQuotation(string QuotationNo)
        {
            Quotation quotation = new Quotation();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL ";
            SQLString += "WHERE QUOTATION_NO = '" + QuotationNo + "'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                quotation = GetQuotation(reader);
            }
            cn.Close();
            return quotation;
        }

        internal static SortableList<Quotation> GetQuotationHeaders(Quotation startQuotation, Quotation endQuotation)
        {
            string sqlWhere = "  Where ";
            sqlWhere += "QUOTATION_NO >= '" + startQuotation.QuotationNo + "' AND QUOTATION_NO <= '" + endQuotation.QuotationNo + "'";
            return GetQuotationHeaders(sqlWhere);
        }

        internal static SortableList<Quotation> GetAllQuotationHeader()
        {
            SortableList<Quotation> quotations = new SortableList<Quotation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL ";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                quotations.Add(GetQuotation(reader));
            }
            cn.Close();
            return quotations;
        }

        internal static SortableList<Quotation> GetAllQuotationHeaderExcept(int quotationID)
        {
            SortableList<Quotation> quotations = new SortableList<Quotation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL ";
            SQLString += "WHERE QUOTATION_ID <> " + quotationID;
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                quotations.Add(GetQuotation(reader));
            }
            cn.Close();
            return quotations;
        }

        private static SortableList<Quotation> GetQuotationHeaders(string sqlWhere)
        {
            SortableList<Quotation> quotations = new SortableList<Quotation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL ";
            SQLString += sqlWhere;
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                quotations.Add(GetQuotation(reader));
            }
            cn.Close();
            return quotations;
        }

        internal static SortableList<Quotation> GetQuotationByCustomer(string custNo)
        {
            SortableList<Quotation> quotations = new SortableList<Quotation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL ";
            SQLString += "WHERE CUST_CODE = '" + custNo + "'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            if (cn.State == ConnectionState.Closed) { cn.Open(); };
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                quotations.Add(GetQuotation(reader));
            }
            reader.Close();
            return quotations;
        }

        internal static Quotation GetQuotation(IDataReader reader)
        {
            QuotationType quotationType = QuotationType.Customer;
            SortableList<TransportRate> transportRates = new SortableList<TransportRate>();
            string temp = (string)reader["QUOTATION_TYPE"];
            if (temp == "G")
            {
                quotationType = QuotationType.CustomerGroup;
            }
            else
            {
                quotationType = QuotationType.Customer;
            }
            string isvalidYorN = (string)reader["ISVALID"];
            bool isvalid = true;
            if (isvalidYorN == "T")
            {
                isvalid = true;
            }
            else
            {
                isvalid = false;
            }
            Quotation quotation = new Quotation(
               (int)reader["QUOTATION_ID"],
               reader["QUOTATION_NO"] == DBNull.Value ? string.Empty : (string)reader["QUOTATION_NO"],
               quotationType,
               reader["CUST_CODE"] == DBNull.Value ? string.Empty : (string)reader["CUST_CODE"],
               reader["CUSTOMER_GROUP_NO"] == DBNull.Value ? string.Empty : (string)reader["CUSTOMER_GROUP_NO"],
               (DateTime)reader["QUOTATION_DATE"],
               (DateTime)reader["DATE_VALID_FROM"],
               (DateTime)reader["DATE_VALID_TO"],
               //20140521 correction- gerry change the null value to 0 instead of string for int type
               reader["CREDIT_DAYS"] == DBNull.Value ? 0 : (int)reader["CREDIT_DAYS"], //string.Empty : (int)reader["CREDIT_DAYS"]
               reader["SALESMAN_NO"] == DBNull.Value ? string.Empty : (string)reader["SALESMAN_NO"],
               reader["CONDITIONS"] == DBNull.Value ? string.Empty : (string)reader["CONDITIONS"],
               reader["PAYMENT_TERMS"] == DBNull.Value ? string.Empty : (string)reader["PAYMENT_TERMS"],
               (byte[])reader["UPDATE_VERSION"],
               reader["REMARKS"] == DBNull.Value ? string.Empty : (string)reader["REMARKS"],
               reader["REMARKS2"] == DBNull.Value ? string.Empty : (string)reader["REMARKS2"],
               reader["REMARKS3"] == DBNull.Value ? string.Empty : (string)reader["REMARKS3"],
               reader["REMARKS4"] == DBNull.Value ? string.Empty : (string)reader["REMARKS4"],
               reader["REMARKS5"] == DBNull.Value ? string.Empty : (string)reader["REMARKS5"],
               transportRates,
               isvalid,
               reader["BRANCH_CODE"] == DBNull.Value ? string.Empty : (string)reader["BRANCH_CODE"]
            );

            quotation.OldValidTo = quotation.ValidTo;
            quotation.TransportRates = GetTransportRatesAndPriceBreaksBasedOnQuotationID(quotation);
            return quotation;
        }

        internal static SortableList<PriceBreaks> GetAllPriceBreakRange(TransportRate transportrate)
        {
            SortableList<PriceBreaks> pricebreaks = new SortableList<PriceBreaks>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_PRICE_BREAKS_TBL ";
            SQLString += "WHERE QUOTATION_ID = " + transportrate.QuotationID;
            SQLString += " AND SEQUENCE_NO_RATE = " + transportrate.SequenceNo;
            SQLString += " AND CHARGE_CODE = '" + transportrate.ChargeID + "'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pricebreaks.Add(GetPriceBreak(reader));
            }
            cn.Close();
            return pricebreaks;
        }

        internal static PriceBreaks GetPriceBreakRange(int quotationid, int seqnorate, int seqno)
        {
            PriceBreaks pricebreak = new PriceBreaks();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_PRICE_BREAKS_TBL ";
            SQLString += "WHERE QUOTATION_ID = " + quotationid;
            SQLString += " AND SEQUENCE_NO_RATE = " + seqnorate;
            SQLString += " AND SEQUENCE_NO = " + seqno;

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            pricebreak = GetPriceBreak(reader);
            cn.Close();
            return pricebreak;
        }

        internal static PriceBreaks GetPriceBreak(IDataReader reader)
        {
            bool islumpsum = false;
            string temp = Convert.ToString(reader["IS_LUMP_SUM"]);
            if (temp.Equals("Y"))
            {
                islumpsum = true;
            }
            else
            {
                islumpsum = false;
            }

            PriceBreaks pricebreaks = new PriceBreaks(
               (int)reader["SEQUENCE_NO"],
               (decimal)reader["BREAK_CEILING"],
               islumpsum,
               (decimal)reader["LUMP_SUM_VALUE"],
               (decimal)reader["UNIT_RATE"],
               (byte[])reader["UPDATE_VERSION"],
               (string)reader["REMARKS"]
            );

            return pricebreaks;

        }

        internal static bool FindCustomerGroupInsideQuotation(CustomerGroup customerGroup)
        {

            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL ";
            SQLString += "WHERE CUSTOMER_GROUP_NO = " + " '" + customerGroup.CustomerGroupCode + "'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                count++;
                if (count > 0)
                {
                    break;
                }
            }
            cn.Close();
            if (count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //29 Aug 2011 - Gerry Modify QUERY added the truck booking table if quotation_no was used
        internal static bool FindQuotationInsideBooking(Quotation quotation)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT JOB_NUMBER FROM TPT_JOB_MAIN_Tbl 
                                    WHERE QUOTATION_NO = '{0}'
                                 UNION
                                 SELECT JOB_NUMBER FROM TRK_JOB_MAIN_Tbl 
                                    WHERE QUOTATION_NO = '{0}'";
            SQLString = string.Format(SQLString, quotation.QuotationNo);
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                count++;
                if (count > 0)
                {
                    break;
                }
            }
            cn.Close();
            if (count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        internal static SortableList<Quotation> GetQuotationBasedOnCustNo(string custno, string quotationNo)
        {
            SortableList<Quotation> quotations = new SortableList<Quotation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL ";
            SQLString += "WHERE CUST_CODE = '" + custno + "'";
            SQLString += " AND QUOTATION_NO <> '" + quotationNo + "'";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                quotations.Add(GetQuotation(reader));
            }
            cn.Close();

            if (quotations.Count == 0)
            {
                string customer_group = "";

                string SQLSearchString = "SELECT CUSTOMER_GROUP_CODE FROM TPT_CUSTOMER_CUSTOMER_GROUP_Tbl";
                SQLSearchString += " WHERE CUSTOMER_CODE = '" + custno + "'";
                cn.Open();
                cmd = new SqlCommand(SQLSearchString, cn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer_group = (string)reader["CUSTOMER_GROUP_CODE"];
                }
                cn.Close();

                SQLString = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL ";
                SQLString += "WHERE CUSTOMER_GROUP_NO = '" + customer_group + "'";
                SQLString += " AND QUOTATION_TYPE = 'G'";
                SQLString += " AND QUOTATION_NO <> '" + quotationNo + "'";

                cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    quotations.Add(GetQuotation(reader));
                }
                cn.Close();
            }


            return quotations;
        }

        internal static SortableList<Quotation> GetQuotationBasedOnCustNo(string custno, string quotationNo, DateTime date)
        {
            SortableList<Quotation> quotations = new SortableList<Quotation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL ";
            SQLString += "WHERE CUST_CODE = '" + custno + "'";
            if (quotationNo != "")
            {
                SQLString += " AND QUOTATION_NO <> '" + quotationNo + "'";
            }
            SQLString += " AND (DATE_VALID_FROM <= '" + DateUtility.ConvertDateForSQLPurpose(date) + "'";
            SQLString += " AND DATE_VALID_TO >= '" + DateUtility.ConvertDateForSQLPurpose(date) + "')";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                quotations.Add(GetQuotation(reader));
            }
            cn.Close();

            if (quotations.Count == 0)
            {
                string customer_group = "";

                string SQLSearchString = "SELECT CUSTOMER_GROUP_CODE FROM TPT_CUSTOMER_CUSTOMER_GROUP_Tbl";
                SQLSearchString += " WHERE CUSTOMER_CODE = '" + custno + "'";
                cn.Open();
                cmd = new SqlCommand(SQLSearchString, cn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer_group = (string)reader["CUSTOMER_GROUP_CODE"];
                }
                cn.Close();

                SQLString = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL ";
                SQLString += "WHERE CUSTOMER_GROUP_NO = '" + customer_group + "'";
                SQLString += " AND QUOTATION_TYPE = 'G'";
                if (quotationNo != "")
                {
                    SQLString += " AND QUOTATION_NO <> '" + quotationNo + "'";
                }
                //SQLString += " AND QUOTATION_NO <> '" + quotationNo + "'";
                SQLString += " AND (DATE_VALID_FROM <= '" + DateUtility.ConvertDateForSQLPurpose(date) + "'";
                SQLString += " AND DATE_VALID_TO >= '" + DateUtility.ConvertDateForSQLPurpose(date) + "')";

                cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    quotations.Add(GetQuotation(reader));
                }
                cn.Close();
            }


            return quotations;
        }

        internal static SortableList<Quotation> GetQuotationBasedOnCustomerGroup(string custgroupcode, string quotationNo)
        {
            SortableList<Quotation> quotations = new SortableList<Quotation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL ";
            SQLString += "WHERE CUSTOMER_GROUP_NO = '" + custgroupcode + "'";
            SQLString += " AND QUOTATION_TYPE = 'G'";
            SQLString += " AND QUOTATION_NO <> '" + quotationNo + "'";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                quotations.Add(GetQuotation(reader));
            }
            cn.Close();
            return quotations;
        }

        internal static SortableList<Quotation> GetQuotationBasedOnCustomerGroup(string custgroupcode, string quotationNo, DateTime date)
        {
            SortableList<Quotation> quotations = new SortableList<Quotation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL ";
            SQLString += "WHERE CUSTOMER_GROUP_NO = '" + custgroupcode + "'";
            SQLString += " AND QUOTATION_TYPE = 'G'";
            SQLString += " AND QUOTATION_NO <> '" + quotationNo + "'";
            SQLString += " AND (DATE_VALID_FROM <= '" + DateUtility.ConvertDateForSQLPurpose(date) + "'";
            SQLString += " AND DATE_VALID_TO > '" + DateUtility.ConvertDateForSQLPurpose(date) + "')";


            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                quotations.Add(GetQuotation(reader));
            }
            cn.Close();
            return quotations;
        }

        internal static ArrayList GetValidQuotationPeriod(Quotation quotation)
        {
            ArrayList validPeriods = new ArrayList();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string customer_group = "";
            try
            {
                if (quotation.QuotationType == QuotationType.Customer)
                {
                    string SQLSearchString = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL";
                    SQLSearchString += " WHERE CUST_CODE = '" + quotation.CustNo + "'";
                    SQLSearchString += " AND (DATE_VALID_TO >= '" + DateUtility.ConvertDateForSQLPurpose(quotation.ValidFrom) + "')";
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLSearchString, cn);
                    DataSet dsSearchResult = new DataSet();
                    daSearchCmd.Fill(dsSearchResult);
                    if (dsSearchResult.Tables[0].Rows.Count > 0)
                    {
                        string quotationFound = "";
                        for (int i = 0; i < dsSearchResult.Tables[0].Rows.Count; i++)
                        {
                            string dateValidFrom = dsSearchResult.Tables[0].Rows[i]["DATE_VALID_FROM"].ToString();
                            string dateValidTo = dsSearchResult.Tables[0].Rows[i]["DATE_VALID_TO"].ToString();
                            quotationFound += "\n" + CommonResource.QuotationNo + ":" + dsSearchResult.Tables[0].Rows[i]["QUOTATION_NO"].ToString() + "\t" + dateValidFrom.Substring(0, 10) + " - " + dateValidTo.Substring(0, 10);
                        }
                        foreach (DataRow dr in dsSearchResult.Tables[0].Rows)
                        {
                            validPeriods.Add(dr);
                        }
                        throw new FMException(TptResourceBLL.ErrDuplicateQuotationForCust + string.Format(quotationFound));
                    }
                    cn.Close();

                    SQLSearchString = "SELECT CUSTOMER_GROUP_CODE FROM TPT_CUSTOMER_CUSTOMER_GROUP_Tbl";
                    SQLSearchString += " WHERE CUSTOMER_CODE = '" + quotation.CustNo + "'";
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(SQLSearchString, cn);
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        customer_group = (string)reader["CUSTOMER_GROUP_CODE"];
                    }
                    cn.Close();
                }
                else
                {
                    customer_group = quotation.CustomerGroup;
                }

                if (customer_group != "")
                {
                    string SQLSearchString2 = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL";
                    SQLSearchString2 += " WHERE CUSTOMER_GROUP_NO = '" + customer_group + "'";
                    SQLSearchString2 += " AND DATE_VALID_TO >= '" + DateUtility.ConvertDateForSQLPurpose(quotation.ValidFrom) + "')";
                    cn.Open();

                    SqlDataAdapter daSearchCmd2 = new SqlDataAdapter(SQLSearchString2, cn);
                    DataSet dsSearchResult2 = new DataSet();
                    daSearchCmd2.Fill(dsSearchResult2);
                    if (dsSearchResult2.Tables[0].Rows.Count > 0)
                    {
                        string quotationFound = "";
                        for (int i = 0; i < dsSearchResult2.Tables[0].Rows.Count; i++)
                        {
                            string dateValidFrom = dsSearchResult2.Tables[0].Rows[i]["DATE_VALID_FROM"].ToString();
                            string dateValidTo = dsSearchResult2.Tables[0].Rows[i]["DATE_VALID_TO"].ToString();
                            quotationFound += "\n" + CommonResource.QuotationNo + ":" + dsSearchResult2.Tables[0].Rows[i]["QUOTATION_NO"].ToString() + "\t" + dateValidFrom.Substring(0, 10) + " - " + dateValidTo.Substring(0, 10);
                        }
                        foreach (DataRow dr in dsSearchResult2.Tables[0].Rows)
                        {
                            validPeriods.Add(dr);
                        }
                        throw new FMException(TptResourceBLL.ErrDuplicateQuotationForCustGroup + string.Format(quotationFound));
                    }
                    cn.Close();
                }
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return validPeriods;
        }

        internal static Quotation GetLatestQuotation(string CustCode)
        {
            Quotation quotation = new Quotation();
            string SQLString = @"Select * from TPT_MKT_QUOTATION_HEADER_TBL as a
                                    Where CUST_CODE = '{0}' AND 
                                         (select count(*) from TPT_MKT_QUOTATION_HEADER_TBL
                                               where CUST_CODE = a.CUST_CODE
	                                             and DATE_VALID_FROM > a.DATE_VALID_FROM) < 1
                                        ";
            try
            {
                SQLString = string.Format(SQLString, CustCode);
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = new SqlCommand(SQLString, con);

                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    quotation = GetQuotation(reader);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return quotation;

        }
        //Feb. 25, 2011 - Gerry Added to fetch the latest quotation
        internal static SortableList<Quotation> GetLatestQuotationHeader(string startQuotationNo, string endQuotationNo, bool latest)
        {
            string SQLString = " as a Where ";
            if (latest)
            {
                if (startQuotationNo.Equals(string.Empty) && !endQuotationNo.Equals(string.Empty))
                    SQLString += " QUOTATION_NO = '{1}' AND ";
                else if (!startQuotationNo.Equals(string.Empty) && endQuotationNo.Equals(string.Empty))
                    SQLString += " QUOTATION_NO = '{0}' AND ";
                else if (!startQuotationNo.Equals(string.Empty) && !endQuotationNo.Equals(string.Empty))
                    SQLString += " QUOTATION_NO >= '{0}' and QUOTATION_NO <= '{1}' AND";
                else if (startQuotationNo.Equals(string.Empty) && endQuotationNo.Equals(string.Empty))
                    SQLString += "";

                SQLString += @" (select count(*) from TPT_MKT_QUOTATION_HEADER_TBL
                                   where CUST_CODE = a.CUST_CODE
	                                 and DATE_VALID_FROM > a.DATE_VALID_FROM) < 1
                                        ";
            }
            else
            {
                if (startQuotationNo.Equals(string.Empty) && !endQuotationNo.Equals(string.Empty))
                    SQLString += " QUOTATION_NO = '{1}'";
                else if (!startQuotationNo.Equals(string.Empty) && endQuotationNo.Equals(string.Empty))
                    SQLString += " QUOTATION_NO = '{0}'";
                else if (!startQuotationNo.Equals(string.Empty) && !endQuotationNo.Equals(string.Empty))
                    SQLString += " QUOTATION_NO >= '{0}' and QUOTATION_NO <= '{1}'";
                else if (startQuotationNo.Equals(string.Empty) && endQuotationNo.Equals(string.Empty))
                    SQLString = "";
            }
            SQLString += "ORDER BY QUOTATION_ID DESC";

            SQLString = string.Format(SQLString, startQuotationNo, endQuotationNo);
            return GetQuotationHeaders(SQLString);
        }

        //Feb. 25, 2011 - Gerry Added to fetch the latest quotation_no for search combobox
        //Using List<string> to bound faster combobox for search.
        internal static List<string> GetQuotationNoForCriteria(bool latest)
        {
            List<string> quotationNos = new List<string>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT QUOTATION_NO FROM TPT_MKT_QUOTATION_HEADER_TBL as a";
            if (latest)
                SQLString += @" WHERE (select count(*) from TPT_MKT_QUOTATION_HEADER_TBL
                                   where CUST_CODE = a.CUST_CODE
	                                 and DATE_VALID_FROM > a.DATE_VALID_FROM) < 1
                                        ";

            SQLString += " ORDER BY QUOTATION_NO";
            con.Open();
            SqlCommand cmd = new SqlCommand(SQLString, con);
            try
            {
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string value = reader.GetString(0);
                    quotationNos.Add(value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return quotationNos;
        }

        internal static List<string> GetQuotationNoToCopy(string quotationNo)
        {
            List<string> quotationNos = new List<string>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT QUOTATION_NO FROM TPT_MKT_QUOTATION_HEADER_TBL Where QUOTATION_NO <> '{0}'";

            SQLString = string.Format(SQLString, quotationNo);
            SQLString += " ORDER BY QUOTATION_NO";
            con.Open();
            SqlCommand cmd = new SqlCommand(SQLString, con);
            try
            {
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string value = reader.GetString(0);
                    quotationNos.Add(value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return quotationNos;
        }


        #region Not yet implemented
        internal static List<string> GetUpdateQuotationNoForCriteria(bool latest, string startQuotationNo, string endQuotationNo)
        {
            List<string> quotationNos = new List<string>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT QUOTATION_NO FROM TPT_MKT_QUOTATION_HEADER_TBL as a
                                    Where QUOTATION_NO > '{0}'";
            if (latest)
                SQLString += @" WHERE (select count(*) from TPT_MKT_QUOTATION_HEADER_TBL
                                   where CUST_CODE = a.CUST_CODE
	                                 and DATE_VALID_FROM > a.DATE_VALID_FROM) < 1
                                        ";

            SQLString += " ORDER BY QUOTATION_NO";
            con.Open();
            SqlCommand cmd = new SqlCommand(SQLString, con);
            try
            {
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string value = reader.GetString(0);
                    quotationNos.Add(value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return quotationNos;
        }

        internal static SortableList<Quotation> GetQuotationsNoForCriteria(bool latest)
        {
            SortableList<Quotation> quotationNos = new SortableList<Quotation>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT QUOTATION_NO FROM TPT_MKT_QUOTATION_HEADER_TBL as a";
            if (latest)
                SQLString += @" WHERE (select count(*) from TPT_MKT_QUOTATION_HEADER_TBL
                                   where CUST_CODE = a.CUST_CODE
	                                 and DATE_VALID_FROM > a.DATE_VALID_FROM) < 1
                                        ";
            con.Open();
            SqlCommand cmd = new SqlCommand(SQLString, con);
            try
            {
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    quotationNos.Add(GetQuotation(reader));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return quotationNos;
        }
        #endregion

        internal static TransportRate GetTransportRatesBasedOnQuotationNoAndUOMandStopDependent(string QuotationNo, string uom, string chargeCode, bool stopDependent)
        {
            Quotation tempquotation = GetAllQuotation(QuotationNo);
            TransportRate templist = new TransportRate();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL 
                                    WHERE QUOTATION_ID = '{0}'
                                    AND UOM = '{1}'";
            if (chargeCode != "")
                SQLString += " AND CHARGE_CODE = '{2}'";

            if (stopDependent == true)
                SQLString += " AND IS_STOP_DEPENDENT = 'T'";
            else
                SQLString += " AND IS_STOP_DEPENDENT = 'F'";

            if (uom.Contains("'"))
            {
                int index = uom.IndexOf("'");
                uom = uom.Insert(index, "'");
            }


            SQLString = string.Format(SQLString, tempquotation.QuotationID, uom, chargeCode);
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                templist = GetTransportRate(reader);
            }
            cn.Close();
            return templist;
        }

        internal static TransportRate GetTransportRatesBasedOnQuotationNoAndUOMForInvoice(string QuotationNo, string uom, string chargeCode)
        {
            Quotation tempquotation = GetAllQuotation(QuotationNo);
            TransportRate templist = new TransportRate();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL 
                                    WHERE QUOTATION_ID = '{0}'
                                    AND UOM = '{1}'";
            if (chargeCode != "")
                SQLString += " AND CHARGE_CODE = '{2}'";

            if (uom.Contains("'"))
            {
                int index = uom.IndexOf("'");
                uom = uom.Insert(index, "'");
            }


            SQLString = string.Format(SQLString, tempquotation.QuotationID, uom, chargeCode);
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                templist = GetTransportRate(reader);
            }
            cn.Close();
            return templist;
        }

        internal static DataTable GetTransportRatesBasedOnQuotationNoAndUOM(string QuotationNo, string chargeCode, string uom)
        {
            Quotation tempquotation = GetAllQuotation(QuotationNo);
            DataTable templist = new DataTable();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT  qt.SEQUENCE_NO [SeqNo], qt.UOM [UOM], 
                                    case qr.UNIT_RATE when 0 then qr.LUMP_SUM_VALUE else qr.UNIT_RATE end [RATE],
                                    qt.MINIMUM_VALUE,
                                    case qr.IS_LUMP_SUM when 'Y' then 'LUMPSUM' else 'FIX RATE' end [Type]
                                    FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL as qt
                                    inner join TPT_MKT_QUOTATION_DETAIL_RATE_PRICE_BREAKS_TBL as qr
                                    on qr.QUOTATION_ID = qt.QUOTATION_ID and qr.SEQUENCE_NO_RATE = qt.SEQUENCE_NO


                                WHERE qt.QUOTATION_ID = '{0}'
                                    AND qt.CHARGE_CODE = '{1}'
                                    AND qt.UOM = '{2}'";

            if (uom.Contains("'"))
            {
                int index = uom.IndexOf("'");
                uom = uom.Insert(index, "'");
            }

            try
            {
                SQLString = string.Format(SQLString, tempquotation.QuotationID, chargeCode, uom);
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(templist);
                cn.Close();
            }
            catch (Exception ex) { throw ex; }

            return templist;
        }
        /*
        internal static SortableList<TransportRate>  GetTransportRatesBasedOnQuotationNoAndUOM(string QuotationNo, string uom, string chargeCode)
        {
            Quotation tempquotation = GetAllQuotation(QuotationNo);
            SortableList<TransportRate> templist = new SortableList<TransportRate>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL 
                                    WHERE QUOTATION_ID = '{0}'
                                    AND UOM = '{1}'";
            if (chargeCode != "")
                SQLString += " AND CHARGE_CODE = '{2}'";

            if (uom.Contains("'"))
            {
                int index = uom.IndexOf("'");
                uom = uom.Insert(index, "'");
            }

            try
            {
                SQLString = string.Format(SQLString, tempquotation.QuotationID, uom, chargeCode);
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                { 
                    
                }
                cn.Close();
            }
            catch (Exception ex) { throw ex; }

            return templist;
        }
        */
        internal static ArrayList GetJobsAndInvoicesForquotation(DateTime validFrom, DateTime validTo)
        {
            ArrayList tempList = new ArrayList();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SqlQuery = @"Select QUOTATION_NO, DATE_VALID_FROM, DATE_VALID_TO from TPT_MKT_QUOTATION_HEADER_TBL
                                     Where QUOTATION_NO in 
                                    (Select QUOTATION_NO from TPT_JOB_MAIN_Tbl union Select QUOTATION_NO from dbo.TRK_JOB_MAIN_Tbl)
                                    and DATE_VALID_FROM = '{0}'
                                    and DATE_VALID_TO ='{1}'";

                SqlQuery = string.Format(SqlQuery, DateUtility.ConvertDateForSQLPurpose(validFrom), DateUtility.ConvertDateForSQLPurpose(validTo));
                SqlCommand cmd = new SqlCommand(SqlQuery, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tempList.Add(string.Concat(reader.GetString(0) + reader.GetString(1).ToString() + " - " + reader.GetString(1).ToString()));
                }
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return tempList;
        }

        internal static string GetValidQuotationNo(string custCode, DateTime date, out string msg)
        {
            string quotationNo = "";
            msg = "";
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SqlQuery = @"Select QUOTATION_NO from TPT_MKT_QUOTATION_HEADER_TBL
                                     Where CUST_CODE = '{0}'
                                     AND DATE_VALID_FROM <= '{1}'
                                     AND DATE_VALID_TO >= '{1}'";

                SqlQuery = string.Format(SqlQuery, custCode, DateUtility.ConvertDateForSQLPurpose(date));

                SqlCommand cmd = new SqlCommand(SqlQuery, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    quotationNo = reader.GetString(0).Trim();
                }
                reader.Close();
                if (quotationNo == "")
                {
                    msg = "Selected Date is out of Quotation Range. ";
                }
            }
            catch (Exception ex)
            {
                quotationNo = "";
                throw new FMException(ex.ToString());
            }
            return quotationNo;

        }

        internal static ArrayList GetJobsAndInvoicesForQuotation(Quotation quotation, DateTime newValidTo)
        {
            ArrayList tempJobs = new ArrayList();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                //Sort for Truck And Haulier Jobs uses the quotation No
                string SqlQuery = @"select JOB_NUMBER, BOOKING_DATE as BOOKEDDate from TPT_JOB_MAIN_Tbl                                    
                                    where Job_Number in (select Job_Number from TPT_JOB_MAIN_Tbl where QUOTATION_NO = '{0}'
                                                               AND BOOKING_DATE > '{1}' AND BOOKING_DATE <= '{2}')
                                    union
                                    select JOB_NUMBER, BOOKING_DATE as BOOKEDDate from TRK_JOB_MAIN_Tbl
                                    where Job_Number in (select Job_Number from TRK_JOB_MAIN_Tbl where QUOTATION_NO = '{0}'
                                                               AND BOOKING_DATE > '{1}' AND BOOKING_DATE <= '{2}')
                                    ";

                SqlQuery = string.Format(SqlQuery, quotation.QuotationNo, DateUtility.ConvertDateForSQLPurpose(newValidTo), DateUtility.ConvertDateForSQLPurpose(quotation.ValidTo));
                SqlCommand cmd = new SqlCommand(SqlQuery, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string jobNo = (string)reader.GetString(0);
                    string bookedDate = ((DateTime)reader["BOOKEDDate"]).ToShortDateString();
                    tempJobs.Add(string.Concat(CommonResource.JobNo + ": " + jobNo + CommonResource.Date + bookedDate));
                }
                reader.Close();
                //Sort for Truck And Haulier Invoices uses the quotation No
                string SqlQuery1 = @"select Invoice_Number, Invoice_Date as INVDate from ACT_IV_Transport_Invoice_Master_Tbl
                                    where Posted_Y_N = 'F' AND Customer_Code = '{0}'
                                            AND Invoice_Date > '{1}' AND Invoice_Date <= '{2}'
                                    ";
                //30 December 2011 - Gerry Removed due to invoicing changes 
                //union
                //select Invoice_Number, Invoice_Date as INVDate from ACT_IV_Truck_Invoice_Master_Tbl
                //where Posted_Y_N = 'F' AND Customer_Code = '{0}'
                //        AND Invoice_Date > '{1}' AND Invoice_Date <= '{2}'
                //end removed 

                SqlQuery1 = string.Format(SqlQuery1, quotation.CustNo, DateUtility.ConvertDateForSQLPurpose(newValidTo), DateUtility.ConvertDateForSQLPurpose(quotation.ValidTo));
                cmd = new SqlCommand(SqlQuery1, cn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string invNo = (string)reader.GetString(0);
                    if (invNo.Equals(string.Empty))
                        invNo = "     -      ";
                    string invDate = ((DateTime)reader["INVDate"]).ToShortDateString();
                    tempJobs.Add(string.Concat("INV NO:" + invNo + "  INV DATE:" + invDate));
                }
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }

            return tempJobs;
        }

        internal static ArrayList GetJobsForQuotation(Quotation quotation)
        {
            ArrayList tempJobs = new ArrayList();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SqlQuery = @"select Job_Number, Booking_Date from TPT_JOB_MAIN_Tbl where QUOTATION_NO = '{0}'
                                    union
                                    select Job_Number, Booking_Date from TRK_JOB_MAIN_Tbl where QUOTATION_NO = '{0}'
                                    ";
                SqlQuery = string.Format(SqlQuery, quotation.QuotationNo);
                SqlCommand cmd = new SqlCommand(SqlQuery, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                SqlDataReader reader = cmd.ExecuteReader();
                string job = "";
                while (reader.Read())
                {
                    job = CommonResource.JobNo + ": " + reader.GetString(0) + "\t" + CommonResource.Date + ": " + ((DateTime)reader["Booking_Date"]).ToShortDateString();
                    tempJobs.Add(string.Format(job));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }

            return tempJobs;
        }
        //without stop dependent or none stop dependent
        internal static ArrayList GetTransportRatesUOMs(int QuotationID, string ChargeCode, bool isTruckMovement, bool isContainerMovement)
        {
            ArrayList temp = new ArrayList();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());


            string SQLString = @"SELECT DISTINCT UOM FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL 
                                    Where QUOTATION_ID= '{0}'
                                    AND CHARGE_CODE='{1}'";

            #region QUERY for None Truck and None COntainer Movements

            string SQLStringForNoneMovementCharges = "";
            SQLStringForNoneMovementCharges = SQLString + " AND IS_CONTAINER_MOVEMENT ='F' AND IS_TRUCK_MOVEMENT = 'F'";

            SQLStringForNoneMovementCharges = string.Format(SQLStringForNoneMovementCharges, QuotationID, ChargeCode);

            #endregion

            #region QUERY for Truck and COntainer Movements
            string SQLStringForMovementCharges = "";
            if (isContainerMovement && !isTruckMovement)
                SQLStringForMovementCharges = SQLString + " AND IS_CONTAINER_MOVEMENT ='T'";
            else if (!isContainerMovement && isTruckMovement)
                SQLStringForMovementCharges = SQLString + " AND IS_TRUCK_MOVEMENT ='T'";




            SQLStringForMovementCharges = string.Format(SQLStringForMovementCharges, QuotationID, ChargeCode);

            #endregion

            try
            {
                SqlCommand cmd = new SqlCommand(SQLStringForNoneMovementCharges, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    temp.Add(reader.GetString(0));
                }
                reader.Close();

                cmd = new SqlCommand(SQLStringForMovementCharges, cn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    temp.Add(reader.GetString(0));
                }
            }
            catch (Exception ex) { throw ex; }

            return temp;
        }
        //for stop dependent or none stop dependent
        internal static ArrayList GetTransportRatesUOMs(int QuotationID, string ChargeCode, bool isTruckMovement, bool isContainerMovement, bool isStopDependent)
        {
            ArrayList temp = new ArrayList();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());


            string SQLString = @"SELECT DISTINCT UOM FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL 
                                    Where QUOTATION_ID= '{0}'
                                    AND CHARGE_CODE='{1}'
                                    AND IS_STOP_DEPENDENT='{2}'";

            #region QUERY for None Truck and None COntainer Movements

            string SQLStringForNoneMovementCharges = "";
            SQLStringForNoneMovementCharges = SQLString + " AND IS_CONTAINER_MOVEMENT ='F' AND IS_TRUCK_MOVEMENT = 'F'";

            SQLStringForNoneMovementCharges = string.Format(SQLStringForNoneMovementCharges, QuotationID, ChargeCode, isStopDependent ? "T" : "F");

            #endregion

            #region QUERY for Truck and COntainer Movements
            string SQLStringForMovementCharges = "";
            if (isContainerMovement && !isTruckMovement)
                SQLStringForMovementCharges = SQLString + " AND IS_CONTAINER_MOVEMENT ='T'";
            else if (!isContainerMovement && isTruckMovement)
                SQLStringForMovementCharges = SQLString + " AND IS_TRUCK_MOVEMENT ='T'";




            SQLStringForMovementCharges = string.Format(SQLStringForMovementCharges, QuotationID, ChargeCode, isStopDependent ? "T" : "F");

            #endregion

            try
            {
                SqlCommand cmd = new SqlCommand(SQLStringForNoneMovementCharges, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    temp.Add(reader.GetString(0));
                }
                reader.Close();

                cmd = new SqlCommand(SQLStringForMovementCharges, cn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    temp.Add(reader.GetString(0));
                }
            }
            catch (Exception ex) { throw ex; }

            return temp;
        }

        internal static SortableList<TransportRate> GetNotMovementTransportRates(string quotationNo, bool isTruckMovement, bool isContainerMovement)
        {
            SortableList<TransportRate> tempList = new SortableList<TransportRate>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                Quotation tempQuotation = GetAllQuotation(quotationNo);
                string SQLString = @"SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL 
                                    Where QUOTATION_ID= '{0}'";

                #region QUERY for None Truck and None COntainer Movements

                if (!isContainerMovement && !isTruckMovement)
                    SQLString += @" AND IS_CONTAINER_MOVEMENT ='F'
                                    AND IS_TRUCK_MOVEMENT ='F'";

                #endregion

                #region QUERY for Truck and COntainer Movements
                if (isContainerMovement && !isTruckMovement)
                    SQLString += @" AND IS_CONTAINER_MOVEMENT ='T'
                                    AND IS_TRUCK_MOVEMENT ='F'";
                else if (!isContainerMovement && isTruckMovement)
                    SQLString += @" AND IS_CONTAINER_MOVEMENT ='F'
                                    AND IS_TRUCK_MOVEMENT ='T'";

                SQLString = string.Format(SQLString, tempQuotation.QuotationID);

                #endregion

                SQLString = string.Format(SQLString, tempQuotation.QuotationID, isTruckMovement);

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tempList.Add(GetTransportRate(reader));
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

        //this method is use for airjob integration
        internal static bool HaveTruckMovementTransportRatesForMT_CBM(string quotationNo, out string outMsg)
        {
            bool retValue = true;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                outMsg = string.Empty;
                Quotation tempquotation = GetAllQuotation(quotationNo);
                string SQLString = @"SELECT distinct UOM FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL
                                    WHERE QUOTATION_ID = {0}
                                     AND IS_TRUCK_MOVEMENT = 'T'
                                     AND UOM = 'CBM'
             
                                    UNION 
                                SELECT distinct UOM FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL
                                    WHERE QUOTATION_ID = {0}
                                     AND IS_TRUCK_MOVEMENT = 'T'
                                     AND (UOM = 'MT' OR UOM = 'KGM')"; //check for CMB and MT Rates
                SQLString = string.Format(SQLString, tempquotation.QuotationID);
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                int count = 0;
                string uom = string.Empty;
                while (reader.Read())
                {
                    count++;
                    uom = reader.GetString(0).ToString();
                }
                reader.Close();
                if (count < 2)
                {
                    retValue = false;
                    outMsg = TptResourceBLL.WarnNoQuotationForUOM;
                    if (uom.Equals(TruckMovementUOM_WtVol.KGM.ToString()) || uom.Equals(TruckMovementUOM_WtVol.MT.ToString()))
                        outMsg = string.Format(outMsg, CommonResource.Volume);
                    else if (uom.Equals(TruckMovementUOM_WtVol.CBM.ToString()))
                        outMsg = string.Format(outMsg, CommonResource.Weight);
                    else
                        outMsg = string.Format(outMsg, CommonResource.Weight + "or " + CommonResource.Volume);
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
            finally { cn.Close(); }
            return retValue;
        }

        #region "2014-03-10 Zhou Kai adds function"
        public static List<string> GetAllDeletedQuotationNumbers(Dictionary<string, string> dict)
        {
            List<string> tmpList = new List<string>();
            string sqlQuery = @"SELECT DISTINCT QuotationNo, FORM_NAME FROM 

                                           (SELECT PARENT_IDENTIFIER As QuotationNo, FORM_NAME FROM TPT_LOG_HEADER_Tbl WHERE 
                                           MODULE = 7 AND CHILD_IDENTIFIER = '')  AS QuotationNo

                                          WHERE  (QuotationNo NOT IN (SELECT  QUOTATION_NO FROM [TPT_MKT_QUOTATION_HEADER_TBL])" +
                                           LoggerDAL.FormatFormNamesForSql(dict) + ")";
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    tmpList.Add(r["QUOTATIONNO"].ToString());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { cn.Close(); }

            return tmpList;
        }

        // 2014-03-14 Zhou Kai adds
        /*
         * Get all the transport rate sequence numbers those are logged as deleted and also are not in
         * the TPT_MKT_QUOTATION_DETAIL_RATE_TBL by quotationNo
         */
        public static List<string> GetAllDeletedTransportRatesByQuotationNo(Dictionary<string, string> dict)
        {
            string quotationNo = dict[LogHeader.TOP_LEVEL_NO];
            List<string> tmpList = new List<string>();
            SortableList<TransportRate> tmpRateList = new SortableList<TransportRate>();
            string tripSeqNos = String.Empty;
            tmpRateList = Quotation.GetTransportRatesBasedOnQuotationNo(quotationNo);
            foreach (TransportRate rate in tmpRateList)
            {
                tripSeqNos = tripSeqNos + ",'" + rate.SequenceNo.ToString() + "'";
            }
            if (tripSeqNos.Equals(String.Empty)) // no deleted job trips 
            {
                tripSeqNos = "('')";
            }
            else
            {
                tripSeqNos = "(" + tripSeqNos.Substring(1, tripSeqNos.Length - 1) + ")";
            }
            string sqlQuery = "SELECT CHILD_IDENTIFIER, FORM_NAME FROM TPT_LOG_HEADER_Tbl " +
                                        "WHERE PARENT_IDENTIFIER = '{0}' AND MODULE = 7 " +
                                        "AND CHILD_IDENTIFIER <> '' AND FORM_ACTION = 3 AND " +
                                        "CHILD_IDENTIFIER NOT IN " + tripSeqNos;
            sqlQuery = String.Format(sqlQuery, dict[LogHeader.TOP_LEVEL_NO]);
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    tmpList.Add(r["CHILD_IDENTIFIER"].ToString().Trim());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { cn.Close(); }

            return tmpList;
        }

        /*
         * 2014-03-14 Zhou Kai writes this function to get all quotation numbers which has
         * deleted transport rates. 
         */
        public static List<string> GetAllQuotationNosWhichHasDeletedRates(Dictionary<string, string> dict)
        {
            List<string> tmpList = new List<string>();
            string sqlQuery = "SELECT DISTINCT PARENT_IDENTIFIER FROM TPT_LOG_HEADER_Tbl " +
                                       "WHERE FORM_ACTION = 3 AND " +
                                       "CHILD_IDENTIFIER <> '' AND CHARINDEX('&', PARENT_IDENTIFIER) = 0 " +
                                        "AND MODULE = 7 " +
                                        "AND PARENT_IDENTIFIER IN (SELECT QUOTATION_NO FROM TPT_MKT_QUOTATION_HEADER_TBL)";
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    tmpList.Add(sr["PARENT_IDENTIFIER"].ToString().Trim());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { cn.Close(); }

            return tmpList;
        }
        #endregion
    }
}
