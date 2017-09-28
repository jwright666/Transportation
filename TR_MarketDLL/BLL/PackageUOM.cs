using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_MarketDLL.DAL;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_MarketDLL.BLL
{
    public class PackageUOM
    {
        public string packUOMCode { get; set; }
        public string packType { get; set; }
        public string packUOMDescription { get; set; }
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal height { get; set; }
        public decimal unitWeight { get; set; }
        public byte[] updateVersion;

        public PackageUOM()
        {
            this.packUOMCode = string.Empty;
            this.packType = string.Empty;
            this.packUOMDescription = string.Empty;
            this.length = 0;
            this.width = 0;
            this.height = 0;
            this.unitWeight = 0;
        }

        public bool AddPackageUOM(Package package)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                if (validatepackUOM(package))
                {
                    retValue = PackageUOMDAL.AddPackageUOM(this, con, tran);
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

        public bool EditPackageUOM()
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran =con.BeginTransaction(); }
                if (!IsPackageUOMInUsed())
                {
                    retValue = PackageUOMDAL.EditPackageUOM(this, con, tran);
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

        public bool DeletePackageUOM()
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                if (!IsPackageUOMInUsed())
                {
                    retValue = PackageUOMDAL.DeletePackageUOM(this, con, tran);
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
        public static SortableList<PackageUOM> GetAllPackageUOM(Package package)
        {
            return PackageUOMDAL.GetAllPackageUOM(package);          
        }

        public bool validatepackUOM(Package package)
        {  
            //validate fieds
            if (packUOMCode.Length < 1)
                throw new FMException("PackageUOM code cannot be blank or empty. ");
            if (packType.Length < 1)
                throw new FMException("PackageUOM type cannot be blank or empty. ");
            if (length <= 0)
                throw new FMException("PackageUOM length cannot be 0 or empty. ");
            if (width <= 0)
                throw new FMException("PackageUOM width cannot be 0 or empty. ");
            if (height <= 0)
                throw new FMException("PackageUOM height cannot be 0 or empty. ");
            if (unitWeight <= 0)
                throw new FMException("PackageUOM unit weight cannot be 0 or empty. ");

            //validate duplicate
            foreach (PackageUOM packUOM in package.PackageUOMs)
            {
                if (this.packType.Equals(packUOM.packType) && this.packUOMCode.Equals(packUOM.packUOMCode))
                {
                    throw new FMException("PackageUOM already exist. ");                    
                }
            }

            return true;
        }

        public bool IsPackageUOMInUsed()
        {
            if (PackageUOMDAL.IsPackageUOMInUsed(this))
            {
                throw new FMException("PackageUOM is currently used in transport charge. ");
            }  
            return false;
        }
        public static PackageUOM GetPackageUOM(string packType, string packUOM)
        {
            return PackageUOMDAL.GetPackageUOM(packType, packUOM);
        }

    }
}
