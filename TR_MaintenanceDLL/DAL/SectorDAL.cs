using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using TR_LanguageResource.Resources;

namespace FM.TR_MaintenanceDLL.DAL
{
    internal class SectorDAL
    {
        internal static bool AddSector(Sector sector,SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string SQLInstertString = "INSERT INTO TPT_SECTOR_TBL (Sector_Code,Sector_Description) VALUES ('" + sector.SectorCode.Trim();
                SQLInstertString += "'" + "," + "'" + sector.SectorDescription + "')";
                SqlCommand cmd = new SqlCommand(SQLInstertString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new FMException(TptResourceDAL.ErrInsertFailed + " Sector");
            }

            return true;

        }

        internal static bool EditSector(Sector sector, SqlConnection cn, SqlTransaction tran)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            try
            {
                string SQLUpdateDriverString = " Update TPT_SECTOR_TBL set " +
                                               " Sector_Description ='" + sector.SectorDescription + "'" +
                                               " Where Sector_Code = '" + sector.SectorCode.Trim() + "'";

                SqlCommand cmd = new SqlCommand(SQLUpdateDriverString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new FMException(TptResourceDAL.ErrEditFailed + " Sector");
            }

            return true;
        }

        internal static bool DeleteSector(Sector sector, SqlConnection cn, SqlTransaction tran)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            try
            {
                string SQLDeleteString = "DELETE FROM TPT_SECTOR_TBL";
                SQLDeleteString += " Where Sector_Code = '" + sector.SectorCode.Trim() + "'";

                SqlCommand cmd = new SqlCommand(SQLDeleteString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new FMException(TptResourceDAL.ErrDeleteFailed + " Sector");
            }

            return true;
        }

        internal static SortableList<Sector> GetAllSectors()
        {
            SortableList<Sector> sectors = new SortableList<Sector>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_SECTOR_TBL";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                sectors.Add(GetSector(reader));
            }
            cn.Close();
            return sectors;
        }

        internal static Sector GetSector(IDataReader reader)
        {
            Sector sector = new Sector(
                    (string)reader["Sector_Code"],
                    (string)reader["Sector_Description"]
                    );
            return sector;
        }

        internal static bool IsSectorUsed(Sector sector)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL ";
            SQLString += "WHERE START_SECTOR_CODE = '" + sector.SectorCode + "'";
            SQLString += " OR END_SECTOR_CODE = '" + sector.SectorCode + "'";
            SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLString, cn);
            DataSet dsSearchResult = new DataSet();
            daSearchCmd.Fill(dsSearchResult);
            cn.Close();
            if (dsSearchResult.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static Sector GetSectorsByCode(string sectorCode)
        {
            Sector sector = new Sector();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT * FROM TPT_SECTOR_TBL
                                    Where  Sector_Code = '{0}'";

            SQLString = string.Format(SQLString, sectorCode);
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                sector = GetSector(reader);
            }
            cn.Close();
            return sector;
        }

        internal static List<string> GetSectors()
        {
            List<string> sectors = new List<string>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT Sector_Code FROM TPT_SECTOR_TBL ";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                sectors.Add(reader.GetString(0).Trim());
            }
            cn.Close();
            return sectors;
        }


    }
}
