using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using System.Data.SqlClient;
using System.Data;
using FM.TR_MarketDLL.BLL;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_MarketDLL.DAL
{
    public class PackageDAL
    {
        internal static bool AddPackage(Package pack, SqlConnection con, SqlTransaction tran)
        {
            //TODO add package UOM
            try
            {
                string query = @"insert into TPT_PACKTYPE(PACK_TYPE, PACK_DESCRIPTION)
                                    values('{0}','{1}')";

                query = string.Format(query, pack.PackType, pack.PackDescription);

                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tran;

                cmd.ExecuteNonQuery();
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return true;
        }

        internal static SortableList<Package> GetAllPackageType()
        {
            SortableList<Package> packages = new SortableList<Package>();
            try
            {
                //TODO get all package UOM
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_PACKTYPE ";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Package pack = new Package();
                    pack.PackType = reader["PACK_TYPE"] == null ? string.Empty : (string)reader["PACK_TYPE"];
                    pack.PackDescription = reader["PACK_DESCRIPTION"] == null ? string.Empty : (string)reader["PACK_DESCRIPTION"];
                    pack.updateVersion = (byte[])reader["UPDATE_VERSION"];

                    packages.Add(pack);
                }
                reader.Close();
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return packages;
        }
        internal static Package GetPackageType(string packType)
        {
            Package package = new Package();
            try
            {
                //TODO get all package UOM
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_PACKTYPE WHERE PACK_TYPE ='" + packType +"'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    package.PackType = reader["PACK_TYPE"] == null ? string.Empty : (string)reader["PACK_TYPE"];
                    package.PackDescription = reader["PACK_DESCRIPTION"] == null ? string.Empty : (string)reader["PACK_DESCRIPTION"];
                    package.updateVersion = (byte[])reader["UPDATE_VERSION"];

                }
                reader.Close();
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return package;
        }     
    }
}
