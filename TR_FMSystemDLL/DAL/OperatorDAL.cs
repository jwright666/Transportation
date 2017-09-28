using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_FMSystemDLL.DAL
{
    internal class OperatorDAL
    { 
        // gets a list of Operators from database
        internal static List<OperatorDTO> GetAllOperators()
        {
            List<OperatorDTO> Operators = new List<OperatorDTO>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM CRT_Operation_Database_TBl ";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetOperator(reader));
                }
                cn.Close();
            }
            catch (Exception ex) { throw ex; }
            return Operators;
        }

        internal static List<OperatorDTO> GetAllLocalAndNotAirlinesOperators()
        {
            List<OperatorDTO> Operators = new List<OperatorDTO>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM CRT_Operation_Database_TBl,CRT_Operation_Database_Type_Tbl ";
                SQLString += "where CRT_Operation_Database_TBl.Operation_Type_Code=CRT_Operation_Database_Type_Tbl.Operation_Type_Code ";
                SQLString += "AND (CRT_Operation_Database_Type_Tbl.Operation_Type_Desc not like 'AIRLINES%' ";
                SQLString += "AND CRT_Operation_Database_Type_Tbl.Operation_Type_Desc not like 'OVERSEAS%')";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetOperator(reader));
                }
                cn.Close();
            }
            catch (Exception ex) { throw ex; }
            return Operators;
        }

        // creates OperatorDTO object from 1 row of the DataReader
        internal static OperatorDTO GetOperator(IDataReader reader)
        {
            string operatorCode = reader["Operation_Code"] == DBNull.Value ? string.Empty : (string)reader["Operation_Code"];
            string operatorName = reader["Operation_Name"] == DBNull.Value ? string.Empty : (string)reader["Operation_Name"];
            string add1 = reader["Op_Add1"] == DBNull.Value ? string.Empty : (string)reader["Op_Add1"];
            string add2 = reader["Op_Add2"] == DBNull.Value ? string.Empty : (string)reader["Op_Add2"];
            string add3 = reader["Op_Add3"] == DBNull.Value ? string.Empty : (string)reader["Op_Add3"];
            string add4 = reader["Op_Add4"] == DBNull.Value ? string.Empty : (string)reader["Op_Add4"]; ;
            string city = reader["City_Code"] == DBNull.Value ? string.Empty : (string)reader["City_Code"];

            #region Gerry Removed
            /*
            if (reader["Op_Add1"] is System.DBNull)
            {
                add1 = "";
            }
            else
            {
                add1 = (string)reader["Op_Add1"];
            }

            if (reader["Op_Add2"] is System.DBNull)
            {
                add2 = "";
            }
            else
            {
                add2 = (string)reader["Op_Add2"];
            }

            if (reader["Op_Add3"] is System.DBNull)
            {
                add3 = "";
            }
            else
            {
                add3 = (string)reader["Op_Add3"];
            } 

            if (reader["Op_Add4"] is System.DBNull)
            {
                add4 = "";
            }
            else
            {
                add4 = (string)reader["Op_Add4"];
            }

            if (reader["City_Code"] is System.DBNull)
            {
                city = "";
            }
            else
            {
                city = (string)reader["City_Code"];
            } 
            */
            #endregion

            OperatorDTO operatorDTO = new OperatorDTO(operatorCode, operatorName, add1, add2, add3, add4, city);

            operatorDTO.ZipCode = reader["Zip_Code"] == DBNull.Value ? string.Empty : (string)reader["Zip_Code"];
            operatorDTO.ZoneArea = reader["Zip_Code"] == DBNull.Value ? string.Empty : (string)reader["Zip_Code"];
            return operatorDTO;
        }

        //Chong Chin 22 April 2010 -Start -  Return string collection for faster loading
        internal static List<string> GetAllOperatorCodes()
        {
            List<string> OperatorCodes = new List<string>();   
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT Operation_Code FROM CRT_Operation_Database_TBl ";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    OperatorCodes.Add((string)reader["Operation_Code"]);
                }
                cn.Close();
            }
            catch (Exception ex) { throw ex; }
            return OperatorCodes;
        }

        internal static List<string> GetAllOperatorNames()
        {
            List<string> OperatorCodes = new List<string>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT Operation_Name FROM CRT_Operation_Database_TBl ";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    OperatorCodes.Add((string)reader["Operation_Name"]);
                }
                cn.Close();
            }
            catch (Exception ex) { throw ex; }
            return OperatorCodes;
        }


        internal static OperatorDTO GetOperatorDTO(string operatorDTOCode)
        {
            OperatorDTO operatorDTO = new OperatorDTO();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM CRT_Operation_Database_TBl WHERE Operation_Code = '" + operatorDTOCode + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    operatorDTO = (GetOperator(reader));
                }
                cn.Close();
            }
            catch (Exception ex) { throw ex; }
            return operatorDTO;
        }

        internal static OperatorDTO GetOperatorDTOByName(string operatorDTOName)
        {
            OperatorDTO operatorDTO = new OperatorDTO();   
            try
            {
                operatorDTOName = CommonUtilities.FormatString(operatorDTOName);
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM CRT_Operation_Database_TBl WHERE Operation_Name = '" + operatorDTOName + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    operatorDTO = (GetOperator(reader));
                }
                cn.Close();
            }
            catch (Exception ex) { throw ex; }
            return operatorDTO;
        }                                          
        // Chong Chin 22 April 2010 End

       
    }
}
