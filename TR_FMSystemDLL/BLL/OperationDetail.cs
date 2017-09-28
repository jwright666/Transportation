using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.BLL
{
     public class OperationDetail
    {
         public string operationCode { get; set; }
         public string operationName { get; set; }
         public string oldOpCode { get; set; }
         public string iataCode { get; set; }
         public string customerCode { get; set; }
         public string opAdd1 { get; set; }
         public string opAdd2 { get; set; }
         public string opAdd3 { get; set; }
         public string opAdd4 { get; set; }
         public string zipCode { get; set;}
         public string cityCode { get; set;}
         public string countryCode { get; set; }
         public string countryName { get; set; }
         public string telephone1 { get; set; }
         public string telephone2 { get; set; }
         public string fax1 { get; set; }
         public string fax2 { get; set; }
         public string contact1 { get; set; }
         public string contact2 { get; set; }
         public string email1 { get; set; }
         public string email2 { get; set; }
         public string coRegNo { get; set; }
         public string salesManCode { get; set; }
         public string specialInstruction { get; set; }
         public string operationTypeCode { get; set; }
         public bool miscYN { get; set; }
         public string otherCode { get; set; }
         public string operationNameAdd { get; set; }
         public string opAdd1Add { get; set; }
         public string opAdd2Add { get; set; }
         public string opAdd3Add { get; set; }
         public string opAdd4Add { get; set; }
         public string addedBy { get; set; }
         public DateTime addedDateTime { get; set; }
         public string changedBy { get; set; }
         public DateTime changedDateTime { get; set; }
         public string routingCode { get; set; }
         public string uenNO { get; set; }

         public OperationDetail ()
         {
            this.zipCode =string.Empty; 
            this.uenNO =string.Empty ;
            this.addedBy =string .Empty;
            this.addedDateTime =DateTime.Now; 
             this.changedBy =string.Empty; 
             this.changedDateTime =DateTime.Now;
             this.cityCode =string.Empty; 
             this.contact1 =string.Empty; 
             this.contact2 =string.Empty ; 
             this.coRegNo =string.Empty ; 
             this.countryCode =string.Empty; 
             this.customerCode =string.Empty ; 
             this.email1 =string.Empty ; 
             this.email2 =string.Empty ;
             this.fax1=string.Empty ; 
             this.fax2 =string.Empty ; 
             this.iataCode =string .Empty; 
             this.miscYN =true; 
             this.oldOpCode =string.Empty; 
             this .opAdd1 =string.Empty ; 
             this.opAdd1Add =string.Empty ; 
             this .opAdd2 =string.Empty ; 
             this.opAdd2Add =string.Empty ; 
             this.opAdd3 =string.Empty ; 
             this.opAdd3Add =string.Empty;
             this.opAdd4 =string.Empty ;
             this.opAdd4Add =string.Empty ;
             this .operationCode =string.Empty ; 
             this.operationName =string.Empty ;
             this.operationNameAdd =string.Empty ; 
             this .operationTypeCode =string .Empty; 
             this.otherCode =string .Empty ;
             this.routingCode =string .Empty ;
             this .salesManCode =string .Empty ; 
             this.specialInstruction =string.Empty; 
             this.telephone1 =string.Empty ; 
             this.telephone2 =string .Empty ;
             this.countryName = string.Empty;
         }

         public static OperationDetail GetOperationDetail(string operationCode)
         {
             return OperationDetailDAL.GetOperationDetail(operationCode.Trim());
         }

         public static OperationDetail GetOperationDetailBySpecialRef(string specialRef)
         {
             return OperationDetailDAL.GetOperationDetailBySpecialRef(specialRef.Trim());
         }

         public static SortableList<OperationDetail> GetAllOperationDetail()
         {
             return OperationDetailDAL.GetAllOperationDetail();
         }
     }
}
