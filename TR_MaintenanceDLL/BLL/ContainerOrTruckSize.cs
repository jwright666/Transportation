using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MaintenanceDLL.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.TR_MaintenanceDLL.BLL
{
    public class ContainerOrTruckSize
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal InnerVolume { get; set; }
        public string Type { get; set; }

        public ContainerOrTruckSize()
        {
            this.Code = string.Empty;
            this.Description = string.Empty;
            this.Length = 0;
            this.Width = 0;
            this.Height = 0;
            this.InnerVolume = 0;
            this.Type = "Truck";
        }

        public static ContainerOrTruckSize GetContainerOrTruckSize(string code)
        {
            try
            {
                //TODO  
                return ContainerOrTruckSizeDAL.GetContainerOrTruckSize(code);
            }
            catch (FMException ex) { throw; }
            catch (Exception ex) { throw; }
            return null;
        }
        public static ContainerOrTruckSize GetContainerOrTruckSize(string code, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                //TODO  
                return ContainerOrTruckSizeDAL.GetContainerOrTruckSize(code, con, tran);
            }
            catch (FMException ex) { throw; }
            catch (Exception ex) { throw; }
            return null;
        }
        public static bool DeleteContainerOrTruckSize(string code, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                //TODO  
               return ContainerOrTruckSizeDAL.DeleteContainerOrTruckSize(code, con, tran);
            }
            catch (FMException ex) { throw; }
            catch (Exception ex) { throw; }
            return true;
        }
        public static bool AddContainerOrTruckSize(ContainerOrTruckSize containerOrTruckSize, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                //TODO  
                return ContainerOrTruckSizeDAL.AddContainerOrTruckSize(containerOrTruckSize, con, tran);
            }
            catch (FMException ex) { throw; }
            catch (Exception ex) { throw; }
            return true;
        }
        public static bool EditContainerOrTruckSize(ContainerOrTruckSize containerOrTruckSize, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                //TODO  
                return ContainerOrTruckSizeDAL.EditContainerOrTruckSize(containerOrTruckSize, con, tran);
            }
            catch (FMException ex) { throw; }
            catch (Exception ex) { throw; }
            return true;
        }

        public override string ToString()
        {
            return Length.ToString("#0") + "x" + Width.ToString("#0") + "x" + Height.ToString("#0");
        }
    }
}
