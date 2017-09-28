using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using System.Data;
using System.Data.SqlClient;
using FM.TR_MarketDLL.BLL;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_MarketDLL.DAL
{
    public class PackageUOMDAL
    {
        internal static bool AddPackageUOM(PackageUOM packUOM, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string query = @"insert into TPT_PACK_UOM(PACK_UOM, PACK_TYPE, PACK_UOM_DESCRIPTION, LENGTH, WIDTH, HEIGHT, UNIT_WEIGHT)
                                    values('{0}','{1}','{2}','{3}','{4}', '{5}', '{6}')";

                query = string.Format(query, CommonUtilities.FormatString(packUOM.packUOMCode), 
                                           CommonUtilities.FormatString(packUOM.packType), 
                                           CommonUtilities.FormatString(packUOM.packUOMDescription),
                                           packUOM.length,
                                           packUOM.width,
                                           packUOM.height,
                                           packUOM.unitWeight);     
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

        internal static bool EditPackageUOM(PackageUOM packUOM, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string query = @"update TPT_PACK_UOM
                                    set
                                        PACK_UOM_DESCRIPTION= '{1}', 
                                        LENGTH= '{2}',
                                        WIDTH= '{3}', 
                                        HEIGHT= '{4}',
                                        UNIT_WEIGHT = '{5}'  

                                    where PACK_UOM = '{0}' 
                                    and PACK_TYPE = '{6}'                 
                                    ";

                query = string.Format(query, CommonUtilities.FormatString(packUOM.packUOMCode), 
                                           CommonUtilities.FormatString(packUOM.packUOMDescription),
                                           packUOM.length,
                                           packUOM.width,
                                           packUOM.height,
                                           packUOM.unitWeight,
                                           CommonUtilities.FormatString(packUOM.packType));     
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
        internal static bool DeletePackageUOM(PackageUOM packUOM, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string query = @"delete from TPT_PACK_UOM
                                    where PACK_UOM = '{0}' 
                                    and PACK_TYPE = '{1}'
                                    ";

                query = string.Format(query, CommonUtilities.FormatString(packUOM.packUOMCode),
                                           CommonUtilities.FormatString(packUOM.packType));
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
        ///Gerry - Note
        ///this method can be use either getting all the collection package UOM inside the same package type
        ///2nd can get all collection of package uom independent with the package type (just pass null to the parameter) 
        internal static SortableList<PackageUOM> GetAllPackageUOM(Package package)
        {
            SortableList<PackageUOM> packUOMs = new SortableList<PackageUOM>();
            try
            {
                //TODO get all package UOM
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT TPTuom.PACK_UOM, TPTuom.PACK_TYPE, TPTuom.PACK_UOM_DESCRIPTION, 
	                                    TPTuom.LENGTH, TPTuom.WIDTH, TPTuom.HEIGHT, TPTuom.UNIT_WEIGHT, tptuom.UPDATE_VERSION
                                    FROM TPT_PACK_UOM AS TPTuom
                                    INNER JOIN  TPT_PACKTYPE AS packType
                                    on TPTuom.PACK_TYPE = packType.PACK_TYPE
                                    INNER JOIN CRT_Measurement_Tbl AS FMuom
                                    on FMuom.Measurement_Code = TPTuom.PACK_UOM   
                                    "; 

                if (package != null)
                {       
                    SQLString +=" where packType.PACK_TYPE = '{0}'";
                    SQLString = string.Format(SQLString, package.PackType);
                }
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PackageUOM packUOM = new PackageUOM();
                    packUOM.packUOMCode = reader["PACK_UOM"] == null ? string.Empty : (string)reader["PACK_UOM"];
                    packUOM.packType = reader["PACK_TYPE"] == null ? string.Empty : (string)reader["PACK_TYPE"];
                    packUOM.packUOMDescription = reader["PACK_UOM_DESCRIPTION"] == null ? string.Empty : (string)reader["PACK_UOM_DESCRIPTION"];
                    packUOM.length = reader["LENGTH"] == null ? 0 : (decimal)reader["LENGTH"];
                    packUOM.width = reader["WIDTH"] == null ? 0 : (decimal)reader["WIDTH"];
                    packUOM.height = reader["HEIGHT"] == null ? 0 : (decimal)reader["HEIGHT"];
                    packUOM.unitWeight = reader["UNIT_WEIGHT"] == null ? 0 : (decimal)reader["UNIT_WEIGHT"];
                    packUOM.updateVersion = (byte[])reader["UPDATE_VERSION"];

                    packUOMs.Add(packUOM);
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
            return packUOMs;     
        }

        internal static bool IsPackageUOMInUsed(PackageUOM packUOM)
        {
            try
            {
                string query = @"select * from TPT_CHARGE_TBL as charge
                                    inner join TPT_CHARGE_UOM_Tbl as uom
                                    on charge.CHARGE_CODE = uom.CHARGE_CODE
                                    and charge.BRANCH_CODE = uom.BRANCH_CODE
                                    and charge.CUST_VEND_TYPE_CODE = uom.CUST_VEND_TYPE_CODE
                                where uom.UOM ='{0}'
                                and  charge.PACK_TYPE='{1}'
                                    ";

                query = string.Format(query, CommonUtilities.FormatString(packUOM.packUOMCode),
                                           CommonUtilities.FormatString(packUOM.packType));

                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                cn.Open();
                SqlDataAdapter daSearchCmd = new SqlDataAdapter(query, cn);
                DataSet dsSearchResult = new DataSet();
                daSearchCmd.Fill(dsSearchResult);
                cn.Close();
                if (dsSearchResult.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                return false;

            }
            catch (FMException ex)
            {
                throw ex;
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
            
        }

        internal static PackageUOM GetPackageUOM(string packType,string packUOMCode)
        {
            PackageUOM packUOM = new PackageUOM();
            try
            {
                //TODO get all package UOM
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT TPTuom.PACK_UOM, TPTuom.PACK_TYPE, TPTuom.PACK_UOM_DESCRIPTION, 
	                                    TPTuom.LENGTH, TPTuom.WIDTH, TPTuom.HEIGHT, TPTuom.UNIT_WEIGHT, tptuom.UPDATE_VERSION
                                    FROM TPT_PACK_UOM AS TPTuom
                                    INNER JOIN  TPT_PACKTYPE AS packType
                                    on TPTuom.PACK_TYPE = packType.PACK_TYPE
                                    INNER JOIN CRT_Measurement_Tbl AS FMuom
                                    on FMuom.Measurement_Code = TPTuom.PACK_UOM   
                                    where packType.PACK_TYPE = '{0}' 
                                            and PACK_UOM = '{1}'";
                SQLString = string.Format(SQLString, packType, packUOMCode);
                
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    packUOM.packUOMCode = reader["PACK_UOM"] == null ? string.Empty : (string)reader["PACK_UOM"];
                    packUOM.packType = reader["PACK_TYPE"] == null ? string.Empty : (string)reader["PACK_TYPE"];
                    packUOM.packUOMDescription = reader["PACK_UOM_DESCRIPTION"] == null ? string.Empty : (string)reader["PACK_UOM_DESCRIPTION"];
                    packUOM.length = reader["LENGTH"] == null ? 0 : (decimal)reader["LENGTH"];
                    packUOM.width = reader["WIDTH"] == null ? 0 : (decimal)reader["WIDTH"];
                    packUOM.height = reader["HEIGHT"] == null ? 0 : (decimal)reader["HEIGHT"];
                    packUOM.unitWeight = reader["UNIT_WEIGHT"] == null ? 0 : (decimal)reader["UNIT_WEIGHT"];
                    packUOM.updateVersion = (byte[])reader["UPDATE_VERSION"];     
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
            return packUOM;     
            
        }
    }
}
