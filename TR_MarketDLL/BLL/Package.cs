using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MarketDLL.DAL;

namespace FM.TR_MarketDLL.BLL
{
    public class Package
    {
        public string PackType { get; set; }
        public string PackDescription { get; set; }
        public byte[] updateVersion;
        public SortableList<PackageUOM> PackageUOMs;
        
        public Package()
        {
            this.PackType = string.Empty;
            this.PackDescription = string.Empty;
            this.PackageUOMs = new SortableList<PackageUOM>();
        }

        public bool AddPackageType()
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                if (IsValidToAddPackage())
                {
                    retValue = PackageDAL.AddPackage(this, con, tran);
                }
                tran.Commit();
            }
            catch (FMException fmEx)
            {
                tran.Rollback();
                throw fmEx;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            return retValue;
        }

        public bool EditPackageType()
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { con.BeginTransaction(); }

                //TODO Edit
                //retValue = PackageDAL.AddPackage(this, con, tran);

                tran.Commit();
            }
            catch (FMException fmEx)
            {
                tran.Rollback();
                throw fmEx;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            return retValue;
        }   

        public bool DeletePackageType()
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { con.BeginTransaction(); }
                
                //TODO Delete package
                //retValue = PackageDAL.AddPackage(this, con, tran);

                tran.Commit();
            }
            catch (FMException fmEx)
            {
                tran.Rollback();
                throw fmEx;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            return retValue;
        }

        public static SortableList<Package> GetAllPackageTypes()
        {
            return PackageDAL.GetAllPackageType();
        }

        public static Package GetPackageByType(string packType)
        {
            return PackageDAL.GetPackageType(packType);
        }

        public bool IsValidToAddPackage()
        {
            try
            {
                foreach (Package pack in GetAllPackageTypes())
                { 
                   if(this.PackType.Equals(pack.PackType))
                   {
                       throw new FMException("Package Type already exist. ");                    
                   }
                }      
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return true;
        }
    }
}
