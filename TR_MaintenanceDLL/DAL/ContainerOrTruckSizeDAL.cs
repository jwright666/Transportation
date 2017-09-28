using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MaintenanceDLL.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.TR_MaintenanceDLL.DAL
{
    public class ContainerOrTruckSizeDAL
    {

        internal static bool AddContainerOrTruckSize(ContainerOrTruckSize truckSize, SqlConnection cn, SqlTransaction tran)
        { 
            try
            {
                #region Query
                string query = @"INSERT INTO [dbo].[TPT_CONTAINER_OR_TRUCK_SIZE_Tbl]
                                       ([CODE]
                                       ,[DESCRIPTION]
                                       ,[LENGTH]
                                       ,[WIDTH]
                                       ,[HEIGHT]
                                       ,[INNER_VOLUME]
                                       ,[TYPE])
                                 VALUES
                                       ('{0}'
                                       ,'{1}'
                                       ,'{2}'
                                       ,'{3}'
                                       ,'{4}'
                                       ,'{5}'
                                       ,'{6}')";
                query = string.Format(query, truckSize.Code, truckSize.Description, truckSize.Length, truckSize.Width, truckSize.Height, truckSize.InnerVolume, truckSize.Type);
                #endregion

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw; }
            catch (InvalidOperationException ex) { throw new FMException("DAL AddContainerOrTruckSize\n" + ex.ToString()); }
            catch (SqlException ex) { throw new FMException("DAL AddContainerOrTruckSize\n" + ex.ToString()); }
            catch (Exception ex) { throw new FMException("DAL AddContainerOrTruckSize\n" + ex.ToString()); }
            return true;
        }
        internal static bool EditContainerOrTruckSize(ContainerOrTruckSize truckSize, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                #region Query
                string query = @"UPDATE [dbo].[TPT_CONTAINER_OR_TRUCK_SIZE_Tbl]
                                 SET [DESCRIPTION] = '{1}'
                                       ,[LENGTH] = '{2}'
                                       ,[WIDTH] = '{3}'
                                       ,[HEIGHT] = '{4}'
                                       ,[INNER_VOLUME] = '{5}'
                                       ,[TYPE] = '{6}'
                                WHERE [CODE] = '{0}' ";
                query = string.Format(query, truckSize.Code, truckSize.Description, truckSize.Length, truckSize.Width, truckSize.Height, truckSize.InnerVolume, truckSize.Type);
                #endregion

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw; }
            catch (InvalidOperationException ex) { throw new FMException("DAL AddContainerOrTruckSize\n" + ex.ToString()); }
            catch (SqlException ex) { throw new FMException("DAL AddContainerOrTruckSize\n" + ex.ToString()); }
            catch (Exception ex) { throw new FMException("DAL AddContainerOrTruckSize\n" + ex.ToString()); }
            return true;
        }
        internal static bool DeleteContainerOrTruckSize(string code, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                #region Query
                string query = @"DELETE FROM [dbo].[TPT_CONTAINER_OR_TRUCK_SIZE_Tbl] WHERE [CODE] = '{0}'";
                query = string.Format(query, code);
                #endregion

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw; }
            catch (InvalidOperationException ex) { throw new FMException("DAL AddContainerOrTruckSize\n" + ex.ToString()); }
            catch (SqlException ex) { throw new FMException("DAL AddContainerOrTruckSize\n" + ex.ToString()); }
            catch (Exception ex) { throw new FMException("DAL AddContainerOrTruckSize\n" + ex.ToString()); }
            return true;
        }
        internal static ContainerOrTruckSize GetContainerOrTruckSize(string code, SqlConnection cn, SqlTransaction tran)
        {
            ContainerOrTruckSize retValue = null;
            try
            {
                #region Query
                string query = @"SELECT * FROM [dbo].[TPT_CONTAINER_OR_TRUCK_SIZE_Tbl] WHERE [CODE] = '{0}'";
                query = string.Format(query, code);
                #endregion

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue = new ContainerOrTruckSize();
                    retValue.Code = reader["CODE"] == DBNull.Value ? "" : (string)reader["CODE"];
                    retValue.Description = reader["DESCRIPTION"] == DBNull.Value ? "" : (string)reader["DESCRIPTION"];
                    retValue.Length = reader["LENGTH"] == DBNull.Value ? 0 : (decimal)reader["LENGTH"];
                    retValue.Width = reader["WIDTH"] == DBNull.Value ? 0 : (decimal)reader["WIDTH"];
                    retValue.Height = reader["HEIGHT"] == DBNull.Value ? 0 : (decimal)reader["HEIGHT"];
                    retValue.InnerVolume = reader["INNER_VOLUME"] == DBNull.Value ? 0 : (decimal)reader["INNER_VOLUME"];
                    retValue.Type = reader["TYPE"] == DBNull.Value ? "" : (string)reader["TYPE"];
                }
                reader.Close();
            }
            catch (FMException ex) { throw; }
            catch (InvalidOperationException ex) { throw new FMException("DAL AddContainerOrTruckSize\n" + ex.ToString()); }
            catch (SqlException ex) { throw new FMException("DAL AddContainerOrTruckSize\n" + ex.ToString()); }
            catch (Exception ex) { throw new FMException("DAL AddContainerOrTruckSize\n" + ex.ToString()); }
            return retValue;
        }
        internal static ContainerOrTruckSize GetContainerOrTruckSize(string code)
        {
            ContainerOrTruckSize retValue = null;
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    #region Query
                    string query = @"SELECT * FROM [dbo].[TPT_CONTAINER_OR_TRUCK_SIZE_Tbl] WHERE [CODE] = '{0}'";
                    query = string.Format(query, code);
                    #endregion

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        retValue = new ContainerOrTruckSize();
                        retValue.Code = reader["CODE"] == DBNull.Value ? "" : (string)reader["CODE"];
                        retValue.Description = reader["DESCRIPTION"] == DBNull.Value ? "" : (string)reader["DESCRIPTION"];
                        retValue.Length = reader["LENGTH"] == DBNull.Value ? 0 : (decimal)reader["LENGTH"];
                        retValue.Width = reader["WIDTH"] == DBNull.Value ? 0 : (decimal)reader["WIDTH"];
                        retValue.Height = reader["HEIGHT"] == DBNull.Value ? 0 : (decimal)reader["HEIGHT"];
                        retValue.InnerVolume = reader["INNER_VOLUME"] == DBNull.Value ? 0 : (decimal)reader["INNER_VOLUME"];
                        retValue.Type = reader["TYPE"] == DBNull.Value ? "" : (string)reader["TYPE"];
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw; }
                catch (InvalidOperationException ex) { throw new FMException("DAL AddContainerOrTruckSize\n" + ex.ToString()); }
                catch (SqlException ex) { throw new FMException("DAL AddContainerOrTruckSize\n" + ex.ToString()); }
                catch (Exception ex) { throw new FMException("DAL AddContainerOrTruckSize\n" + ex.ToString()); }
            }
            return retValue;
        }
    }
}
