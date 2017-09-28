using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;
using FM.TransportMaintenanceDLL.BLL;
using FM.FMSystem.BLL;
using FM.FMSystem.DAL;
using TR_LanguageResource.Resources;

namespace FM.TransportMaintenanceDLL.DAL
{
    public class DriverIncentiveDAL
    {
        internal static DriverIncentive GetDriverIncentive(IDataReader reader)
        {
            DriverIncentive driverIncentive = new DriverIncentive(
                    (string)reader["Incentive_Reference"],
                    (DateTime)reader["Valid_From"],
                    (DateTime)reader["Valid_To"],
                    new SortableList<DriverIncentiveDetail>()
                    );
            return driverIncentive;
        }

        internal static DriverIncentiveDetail GetDriverIncentiveDetail(IDataReader reader)
        {

            string temp = (string)reader["Incentive_Type"];
            DriverIncentiveType dit = new DriverIncentiveType();
            if (temp == "1")
            {
                dit = DriverIncentiveType.Haulage;
            }
            else
            {
                dit = DriverIncentiveType.Truck;
            }


            temp = (string)reader["IsLaden"];
            bool isladen = false;
            if (temp=="T")
            {
                isladen = true;
            } else
            {
                isladen = false;
            }

            DriverIncentiveDetail driverIncentivedetail = new DriverIncentiveDetail(
                    dit,
                    isladen,
                    (string)reader["Container_Size_Code"],
                    (string)reader["Incentive_Description"],
                    (decimal)reader["Incentive_Per_Trip"]
                    );
            return driverIncentivedetail;
        }

        internal static SortableList<DriverIncentive> GetAllDriverIncentives()
        {
            SortableList<DriverIncentive> driverIncentives = new SortableList<DriverIncentive>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_DRIVER_INCENTIVE_TBL";
            SQLString += " ORDER BY Valid_From ";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                driverIncentives.Add(GetDriverIncentive(reader));
            }
            cn.Close();
            return driverIncentives;
        }

        internal static SortableList<DriverIncentiveDetail> GetDriverIncentiveDetail(DriverIncentive d)
        {
            SortableList<DriverIncentiveDetail> driverIncentivedetails = new SortableList<DriverIncentiveDetail>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_DRIVER_INCENTIVE_DETAIL_TBL";
            SQLString += " WHERE Incentive_Reference='" + CommonUtilities.FormatString(d.IncentiveReference) + "'";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                driverIncentivedetails.Add(GetDriverIncentiveDetail(reader));
            }
            cn.Close();
            return driverIncentivedetails;
        }

        internal static SortableList<DriverIncentiveDetail> GetDriverIncentiveDetail(string incentiveRef)
        {
            SortableList<DriverIncentiveDetail> driverIncentivedetails = new SortableList<DriverIncentiveDetail>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_DRIVER_INCENTIVE_DETAIL_TBL";
            SQLString += " WHERE Incentive_Reference='" + CommonUtilities.FormatString(incentiveRef) + "'";


            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                driverIncentivedetails.Add(GetDriverIncentiveDetail(reader));
            }
            cn.Close();
            return driverIncentivedetails;
        }
        internal static bool VerifyValidDate(DateTime date)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            string SQLString = "SELECT * FROM TPT_DRIVER_INCENTIVE_TBL ";
            SQLString += "WHERE Valid_From <= '" + DateUtility.ConvertDateForSQLPurpose(date) + "'";
            SQLString += " AND '" + DateUtility.ConvertDateForSQLPurpose(date) + "' <= Valid_To";
            SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLString, cn);
            DataSet dsSearchResult = new DataSet();
            daSearchCmd.Fill(dsSearchResult);
            cn.Close();
            if (dsSearchResult.Tables[0].Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }


        internal static bool AddDriverIncentive(DriverIncentive driverincentive,SqlConnection cn,SqlTransaction tran)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            try
            {
                string SQLInstertString = "INSERT INTO TPT_DRIVER_INCENTIVE_TBL (Incentive_Reference,Valid_From,Valid_To) VALUES ('" + driverincentive.IncentiveReference.Trim();
                SQLInstertString += "'" + "," + "'" + DateUtility.ConvertDateForSQLPurpose(driverincentive.ValidFrom);
                SQLInstertString += "'" + "," + "'" + DateUtility.ConvertDateForSQLPurpose(driverincentive.ValidTo) + "')";
                SqlCommand cmd = new SqlCommand(SQLInstertString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new FMException(TptResourceDAL.ErrEditFailed + " DriverIncentive\n" + ex.ToString());
            }

            return true;
        }

        internal static bool EditDriverIncentive(DriverIncentive driverincentive, SqlConnection cn, SqlTransaction tran)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            try
            {
                string SQLUpdateDriverString = " Update TPT_DRIVER_INCENTIVE_TBL set " +
                                               " Valid_To ='" + DateUtility.ConvertDateForSQLPurpose(driverincentive.ValidTo) + "'" + 
                                               " Where Incentive_Reference = '" + CommonUtilities.FormatString(driverincentive.IncentiveReference) + "'";

                SqlCommand cmd = new SqlCommand(SQLUpdateDriverString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new FMException(TptResourceDAL.ErrEditFailed + " DriverIncentive");
            }

            return true;
        }
  
        internal static bool AddDriverIncentiveDetail(DriverIncentive driverincentive, DriverIncentiveDetail driverincentivedetail, SqlConnection cn, SqlTransaction tran)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLInstertString = @"INSERT INTO TPT_DRIVER_INCENTIVE_DETAIL_TBL 
                                                    (Incentive_Reference,
                                                    Incentive_Type,
                                                    IsLaden,
                                                    Container_Size_Code,
                                                    Incentive_Description,
                                                    Incentive_Per_Trip) 
                                            VALUES('{0}','{1}','{2}','{3}','{4}',{5})";
            //4 Feb 2012 - Gerry Modified
            string containerSize = driverincentivedetail.ContainerSizeCode.ToString().Trim();
            //containerSize = FormatStringForSQLQUERY(containerSize);
            string desc = driverincentivedetail.IncentiveDescription.ToString().Trim();
            ///desc = FormatStringForSQLQUERY(desc);

            SQLInstertString = string.Format(SQLInstertString,
                                                CommonUtilities.FormatString(driverincentive.IncentiveReference.Trim()),
                                                CommonUtilities.FormatString(driverincentivedetail.IncentiveType.GetHashCode().ToString()),
                                                (driverincentivedetail.IsLaden ? "T" : "F"),
                                                CommonUtilities.FormatString(containerSize),
                                                CommonUtilities.FormatString(desc),
                                                driverincentivedetail.IncentivePerTrip);

            //4 Feb 2012 - Gerry Removed
            #region Removed
            /*
            SQLInstertString += "VALUES ('" + driverincentive.IncentiveReference.Trim();
            SQLInstertString += "'" + "," + "'" + driverincentivedetail.IncentiveType.GetHashCode().ToString();
            if (driverincentivedetail.IsLaden)
                SQLInstertString += "'" + "," + "'T";
            else
                SQLInstertString += "'" + "," + "'F";
            if ((driverincentivedetail.ContainerSizeCode == "20'") ||
                (driverincentivedetail.ContainerSizeCode == "40'"))
            {
                SQLInstertString += "'" + "," + "'" + driverincentivedetail.ContainerSizeCode + "'";
            }
            else
            {
                SQLInstertString += "'" + "," + "'" + driverincentivedetail.ContainerSizeCode;
            }
            SQLInstertString += "'" + "," + "'" + driverincentivedetail.IncentiveDescription;
            SQLInstertString += "'" + "," + driverincentivedetail.IncentivePerTrip + ")";
            */
            #endregion

            //end
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = new SqlCommand(SQLInstertString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new FMException(TptResourceDAL.ErrInsertFailed + " DriverIncentiveDetail" + "\n" + ex.ToString());
            }

            return true;
        }


        internal static bool DeleteDriverIncentiveDetail(DriverIncentive driverincentive, DriverIncentiveDetail driverincentivedetail, SqlConnection cn, SqlTransaction tran)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (con.State == ConnectionState.Closed) { con.Open(); }
            try
            {
                string SQLDeleteString = "DELETE FROM TPT_DRIVER_INCENTIVE_DETAIL_TBL";
                SQLDeleteString +=  " Where Incentive_Reference = '" + CommonUtilities.FormatString(driverincentive.IncentiveReference) + "'" +
                                    " AND Incentive_Type = '" + driverincentivedetail.IncentiveType.GetHashCode().ToString() + "'";
                
                if (driverincentivedetail.IsLaden)
                    SQLDeleteString += " AND IsLaden = 'T'";
                else
                    SQLDeleteString += " AND IsLaden = 'F'";
                
                //4 Feb 2012 - Gerry Added
                string contSize = driverincentivedetail.ContainerSizeCode;
                //contSize = FormatStringForSQLQUERY(contSize);

                if (!contSize.Equals(string.Empty))
                {
                    SQLDeleteString += " AND Container_Size_Code = '" + CommonUtilities.FormatString(contSize) + "'";
                }

                //end


                SqlCommand cmd = new SqlCommand(SQLDeleteString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new FMException(TptResourceDAL.ErrDeleteFailed + " DriverIncentiveDetail\n"+ ex.ToString());
            }

            return true;
        }


        internal static bool EditDriverIncentiveDetail(DriverIncentive driverincentive, DriverIncentiveDetail driverincentivedetail,SqlConnection cn, SqlTransaction tran)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (con.State == ConnectionState.Closed) { con.Open(); }
            try
            {
                string desc = driverincentivedetail.IncentiveDescription.ToString().Trim();
                //desc = FormatStringForSQLQUERY(desc);

                string SQLUpdateDriverString = " Update TPT_DRIVER_INCENTIVE_DETAIL_TBL set " +
                                               " Incentive_Per_Trip =" + driverincentivedetail.IncentivePerTrip +
                                               ", Incentive_Description ='" + CommonUtilities.FormatString(desc) +"'"+

                                               " Where Incentive_Reference = '" + CommonUtilities.FormatString(driverincentive.IncentiveReference) + "'" +
                                               " AND Incentive_Type = '" + driverincentivedetail.IncentiveType.GetHashCode().ToString() + "'";
                if (driverincentivedetail.IsLaden)
                    SQLUpdateDriverString  +=  " AND IsLaden = 'T'";
                else
                    SQLUpdateDriverString  +=  " AND IsLaden = 'F'";

                //4 Feb 2012 - Gerry Added
                string contSize = driverincentivedetail.ContainerSizeCode;
                //contSize = FormatStringForSQLQUERY(contSize);

                if (!contSize.Equals(string.Empty))
                {
                    SQLUpdateDriverString += " AND Container_Size_Code = '" + CommonUtilities.FormatString(contSize) + "'";
                }
                //end



                SqlCommand cmd = new SqlCommand(SQLUpdateDriverString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new FMException(TptResourceDAL.ErrEditFailed + " DriverIncentiveDetail\n"+ ex.ToString());
            }

            return true;
        }

 


    }
}
