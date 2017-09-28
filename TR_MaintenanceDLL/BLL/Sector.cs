using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_MaintenanceDLL.DAL;
using FM.TR_FMSystemDLL.BLL;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using FM.TR_FMSystemDLL.DAL;
using TR_LanguageResource.Resources;

namespace FM.TR_MaintenanceDLL.BLL
{
    public class Sector
    {
        private string sectorCode;
        private string sectorDescription;

        public Sector()
        {
            this.sectorCode = "";
            this.sectorDescription = "";
        }

        public Sector(string sectorCode, string sectorDescription)
        {
            this.sectorCode = sectorCode;
            this.sectorDescription = sectorDescription;
        }

        public string SectorCode
        {
            get { return sectorCode; }
            set { sectorCode = value; }
        }

        public string SectorDescription
        {
            get { return sectorDescription; }
            set { sectorDescription = value; }
        }

        public static SortableList<Sector> GetAllSectors()
        {
            return SectorDAL.GetAllSectors();
        }

        public bool AddSector()
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                SectorDAL.AddSector(this, con, tran);

                tran.Commit();
            }
            catch (FMException ex)
            {
                tran.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return true;
        }

        public bool EditSector()
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                SectorDAL.EditSector(this, con, tran);

                tran.Commit();
            }
            catch (FMException ex)
            {
                tran.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return true;
        }

        public bool DeleteSector()
        {
            if (SectorDAL.IsSectorUsed(this) == false)
            {

                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SectorDAL.DeleteSector(this, con, tran);

                    tran.Commit();
                }
                catch (FMException ex)
                {
                    tran.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new FMException(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return true;
        }

        public static Sector GetSectorByCode(string sectorCode)
        {
            return SectorDAL.GetSectorsByCode(sectorCode);
        }

        public static List<string> GetSectors()
        {
            return SectorDAL.GetSectors();
        }
    }
}
