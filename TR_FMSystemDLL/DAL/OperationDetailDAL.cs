using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_FMSystemDLL.DAL
{
    internal class OperationDetailDAL
    {

        internal static OperationDetail GetOperationDetail(string operationCode)
        {
            try
            {
                OperationDetail operationDetail = new OperationDetail(); 
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                //string sqlGetOperationDetail = " Select * from CRT_Operation_Database_Tbl where Operation_Code ='" + operationCode + "'";
                string sqlGetOperationDetail = @" Select country.Country_Name,opdb.Country_Code, opDB.* from CRT_Operation_Database_Tbl opDB
                                                     inner join CRT_Country_Tbl country
                                                     on country.Country_Code = opDB.Country_Code where Operation_Code ='" + operationCode + "'";
                SqlCommand cmd = new SqlCommand(sqlGetOperationDetail, cn);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlGetOperationDetail, cn);
                DataSet ds = new DataSet();
                adapter.Fill(ds); 
                
                cn.Open();
                //IDataReader reader = cmd.ExecuteReader();
                //while (reader.Read())
                foreach (DataRow dr in ds.Tables[0].Rows) 
                {
                    operationDetail.addedBy = dr["Added_By"].ToString();
                    if (dr["Added_DateTime"].ToString() != "")
                    {
                        operationDetail.addedDateTime = Convert.ToDateTime(dr["Added_DateTime"]);
                    }
                   
                    operationDetail.changedBy = dr["Changed_By"].ToString();
                    if (dr["Changed_DateTime"].ToString() != "")
                    {
                        operationDetail.changedDateTime = Convert.ToDateTime(dr["Changed_DateTime"]); 
                    }
                    

                    //operationDetail.addedBy = (string)reader["Added_By"];
                    //operationDetail.addedDateTime = (DateTime)reader["Added_DateTime"];
                    //operationDetail.changedBy = (string)reader["Changed_By"];
                    //operationDetail.changedDateTime = (DateTime)reader["Changed_DateTime"];
                    operationDetail.cityCode = dr["City_Code"].ToString();
                    operationDetail.contact1 = dr["Contact1"].ToString();
                    operationDetail.contact2 = dr["Contact2"].ToString();
                    operationDetail.coRegNo = dr["Co_Reg_No"].ToString();
                    operationDetail.countryCode = dr["Country_Code"].ToString();
                    operationDetail.countryName = dr["Country_Name"].ToString(); 
                    //operationDetail.cityCode = (string)reader["City_Code"];
                    //operationDetail.contact1 = (string)reader["Contact1"];
                    //operationDetail.contact2 = (string)reader["Contact2"];
                    //operationDetail.coRegNo = (string)reader["Co_Reg_No"];
                    //operationDetail.countryCode = (string)reader["Country_Code"];
                    operationDetail.customerCode = dr["Customer_Code"].ToString();
                    operationDetail.email1 = dr["Email1"].ToString();
                    operationDetail.email2 = dr["Email2"].ToString();
                    operationDetail.fax1 = dr["Fax1"].ToString();
                    operationDetail.fax2 = dr["Fax2"].ToString(); 

                    //operationDetail.customerCode = (string)reader["Customer_Code"];
                    //operationDetail.email1 = (string)reader["Email1"];
                    //operationDetail.email2 = (string)reader["Email2"];
                    //operationDetail.fax1 = (string)reader["Fax1"];
                    //operationDetail.fax2 = (string)reader["Fax2"];
                    
                    //operationDetail.iataCode = (string)reader["IATA_Code"];
                    //if ((string)reader["Misc_Y_N"] == "T")
                    //{
                    //    operationDetail.miscYN = true;
                    //}
                    //else
                    //{
                    //    operationDetail.miscYN = false;
                    //}
                    if (dr["IATA_Code"].ToString() == "T")
                    {
                        operationDetail.miscYN = true;
                    }
                    else
                    {
                        operationDetail.miscYN = false;
                    }

                    //operationDetail.oldOpCode = (string)reader["Old_Op_Code"];
                    //operationDetail.opAdd1 = (string)reader["Op_Add1"];
                    //operationDetail.opAdd1Add = (string)reader["op_Add1_Add"];
                    //operationDetail.opAdd2 = (string)reader["Op_Add2"];
                    //operationDetail.opAdd2Add = (string)reader["Op_Add2_Add"];

                    //operationDetail.opAdd3 = (string)reader["Op_Add3"];
                    //operationDetail.opAdd3Add = (string)reader["Op_Add3_Add"];
                    //operationDetail.opAdd4 = (string)reader["Op_Add4"];
                    //operationDetail.opAdd4Add = (string)reader["Op_Add4_Add"];
                    operationDetail.oldOpCode = dr["Old_Op_Code"].ToString();
                    operationDetail.opAdd1 = dr["Op_Add1"].ToString();
                    operationDetail.opAdd1Add = dr["op_Add1_Add"].ToString();
                    operationDetail.opAdd2 = dr["Op_Add2"].ToString();
                    operationDetail.opAdd2Add = dr["Op_Add2_Add"].ToString();
                    operationDetail.opAdd3 = dr["Op_Add3"].ToString();
                    operationDetail.opAdd3Add = dr["Op_Add3_Add"].ToString();
                    operationDetail.opAdd4 = dr["Op_Add4"].ToString();
                    operationDetail.opAdd4Add = dr["Op_Add4_Add"].ToString(); 
                    
                    //operationDetail.operationCode = (string)reader["Operation_Code"];
                    //operationDetail.operationName = (string)reader["Operation_Name"];
                    //operationDetail.operationNameAdd = (string)reader["Operation_Name_Add"];
                    //operationDetail.operationTypeCode = (string)reader["Operation_Type_Code"];
                    //operationDetail.otherCode = (string)reader["Other_Code"];

                    operationDetail.operationCode = dr["Operation_Code"].ToString();
                    operationDetail.operationName = dr["Operation_Name"].ToString();
                    operationDetail.operationNameAdd = dr["Operation_Name_Add"].ToString();
                    operationDetail.operationTypeCode = dr["Operation_Type_Code"].ToString();
                    operationDetail.otherCode = dr["Other_Code"].ToString(); 

                    //operationDetail.routingCode = (string)reader["Routing_Code"];
                    //operationDetail.salesManCode = (string)reader["Salesman_Code"];
                    //operationDetail.specialInstruction = (string)reader["Special_Instruction"];
                    //operationDetail.telephone1 = (string)reader["Telephone1"];
                    //operationDetail.telephone2 = (string)reader["Telephone2"];

                    operationDetail.routingCode = dr["Routing_Code"].ToString();
                    operationDetail.salesManCode = dr["Salesman_Code"].ToString();
                    operationDetail.specialInstruction = dr["Special_Instruction"].ToString();
                    operationDetail.telephone1 = dr["Telephone1"].ToString();
                    operationDetail.telephone2 = dr["Telephone2"].ToString();

                    //operationDetail.uenNO = dr["UEN_No"].ToString();
                    operationDetail.zipCode = dr["Zip_Code"].ToString(); 
                    //operationDetail.uenNO = (string)reader["UEN_No"];
                    //operationDetail.zipCode = (string)reader["Zip_Code"];
                }


                cn.Close();
                return operationDetail; 
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }

        internal static OperationDetail GetOperationDetailBySpecialRef(string special_Ref)
        {
            try
            {
                OperationDetail operationDetail = new OperationDetail();
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                //string sqlGetOperationDetail = " Select * from CRT_Operation_Database_Tbl where Operation_Code ='" + operationCode + "'";
                string sqlGetOperationDetail = @" Select country.Country_Name,opdb.Country_Code, opDB.* from CRT_Operation_Database_Tbl opDB
                                                     inner join CRT_Country_Tbl country
                                                     on country.Country_Code = opDB.Country_Code where Special_Ref='" + special_Ref + "'";
                SqlCommand cmd = new SqlCommand(sqlGetOperationDetail, cn);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlGetOperationDetail, cn);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                cn.Open();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    operationDetail.addedBy = dr["Added_By"].ToString();
                    if (dr["Added_DateTime"].ToString() != "")
                    {
                        operationDetail.addedDateTime = Convert.ToDateTime(dr["Added_DateTime"]);
                    }

                    operationDetail.changedBy = dr["Changed_By"].ToString();
                    if (dr["Changed_DateTime"].ToString() != "")
                    {
                        operationDetail.changedDateTime = Convert.ToDateTime(dr["Changed_DateTime"]);
                    }
                    operationDetail.cityCode = dr["City_Code"].ToString();
                    operationDetail.contact1 = dr["Contact1"].ToString();
                    operationDetail.contact2 = dr["Contact2"].ToString();
                    operationDetail.coRegNo = dr["Co_Reg_No"].ToString();
                    operationDetail.countryCode = dr["Country_Code"].ToString();
                    operationDetail.countryName = dr["Country_Name"].ToString();
                    operationDetail.customerCode = dr["Customer_Code"].ToString();
                    operationDetail.email1 = dr["Email1"].ToString();
                    operationDetail.email2 = dr["Email2"].ToString();
                    operationDetail.fax1 = dr["Fax1"].ToString();
                    operationDetail.fax2 = dr["Fax2"].ToString();
                    if (dr["IATA_Code"].ToString() == "T")
                    {
                        operationDetail.miscYN = true;
                    }
                    else
                    {
                        operationDetail.miscYN = false;
                    }
                    operationDetail.oldOpCode = dr["Old_Op_Code"].ToString();
                    operationDetail.opAdd1 = dr["Op_Add1"].ToString();
                    operationDetail.opAdd1Add = dr["op_Add1_Add"].ToString();
                    operationDetail.opAdd2 = dr["Op_Add2"].ToString();
                    operationDetail.opAdd2Add = dr["Op_Add2_Add"].ToString();
                    operationDetail.opAdd3 = dr["Op_Add3"].ToString();
                    operationDetail.opAdd3Add = dr["Op_Add3_Add"].ToString();
                    operationDetail.opAdd4 = dr["Op_Add4"].ToString();
                    operationDetail.opAdd4Add = dr["Op_Add4_Add"].ToString();
                    operationDetail.operationCode = dr["Operation_Code"].ToString();
                    operationDetail.operationName = dr["Operation_Name"].ToString();
                    operationDetail.operationNameAdd = dr["Operation_Name_Add"].ToString();
                    operationDetail.operationTypeCode = dr["Operation_Type_Code"].ToString();
                    operationDetail.otherCode = dr["Other_Code"].ToString();
                    operationDetail.routingCode = dr["Routing_Code"].ToString();
                    operationDetail.salesManCode = dr["Salesman_Code"].ToString();
                    operationDetail.specialInstruction = dr["Special_Instruction"].ToString();
                    operationDetail.telephone1 = dr["Telephone1"].ToString();
                    operationDetail.telephone2 = dr["Telephone2"].ToString();
                    operationDetail.zipCode = dr["Zip_Code"].ToString();
                }


                cn.Close();
                return operationDetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        internal static SortableList<OperationDetail> GetAllOperationDetail()
        {
            try
            {
                SortableList<OperationDetail> ops = new SortableList<OperationDetail>();
                OperationDetail operationDetail = new OperationDetail();
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string sqlGetOperationDetail = " Select * from CRT_Operation_Database_Tbl";
                SqlCommand cmd = new SqlCommand(sqlGetOperationDetail, cn);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlGetOperationDetail, cn);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                cn.Open();
                //IDataReader reader = cmd.ExecuteReader();
                //while (reader.Read())
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    operationDetail.addedBy = dr["Added_By"].ToString();
                    if (dr["Added_DateTime"].ToString() != "")
                    {
                        operationDetail.addedDateTime = Convert.ToDateTime(dr["Added_DateTime"]);
                    }

                    operationDetail.changedBy = dr["Changed_By"].ToString();
                    if (dr["Changed_DateTime"].ToString() != "")
                    {
                        operationDetail.changedDateTime = Convert.ToDateTime(dr["Changed_DateTime"]);
                    }


                    //operationDetail.addedBy = (string)reader["Added_By"];
                    //operationDetail.addedDateTime = (DateTime)reader["Added_DateTime"];
                    //operationDetail.changedBy = (string)reader["Changed_By"];
                    //operationDetail.changedDateTime = (DateTime)reader["Changed_DateTime"];
                    operationDetail.cityCode = dr["City_Code"].ToString();
                    operationDetail.contact1 = dr["Contact1"].ToString();
                    operationDetail.contact2 = dr["Contact2"].ToString();
                    operationDetail.coRegNo = dr["Co_Reg_No"].ToString();
                    operationDetail.countryCode = dr["Country_Code"].ToString();
                    operationDetail.countryName = dr["Country_Name"].ToString(); 
                    //operationDetail.cityCode = (string)reader["City_Code"];
                    //operationDetail.contact1 = (string)reader["Contact1"];
                    //operationDetail.contact2 = (string)reader["Contact2"];
                    //operationDetail.coRegNo = (string)reader["Co_Reg_No"];
                    //operationDetail.countryCode = (string)reader["Country_Code"];
                    operationDetail.customerCode = dr["Customer_Code"].ToString();
                    operationDetail.email1 = dr["Email1"].ToString();
                    operationDetail.email2 = dr["Email2"].ToString();
                    operationDetail.fax1 = dr["Fax1"].ToString();
                    operationDetail.fax2 = dr["Fax2"].ToString();

                    //operationDetail.customerCode = (string)reader["Customer_Code"];
                    //operationDetail.email1 = (string)reader["Email1"];
                    //operationDetail.email2 = (string)reader["Email2"];
                    //operationDetail.fax1 = (string)reader["Fax1"];
                    //operationDetail.fax2 = (string)reader["Fax2"];

                    //operationDetail.iataCode = (string)reader["IATA_Code"];
                    //if ((string)reader["Misc_Y_N"] == "T")
                    //{
                    //    operationDetail.miscYN = true;
                    //}
                    //else
                    //{
                    //    operationDetail.miscYN = false;
                    //}
                    if (dr["IATA_Code"].ToString() == "T")
                    {
                        operationDetail.miscYN = true;
                    }
                    else
                    {
                        operationDetail.miscYN = false;
                    }

                    //operationDetail.oldOpCode = (string)reader["Old_Op_Code"];
                    //operationDetail.opAdd1 = (string)reader["Op_Add1"];
                    //operationDetail.opAdd1Add = (string)reader["op_Add1_Add"];
                    //operationDetail.opAdd2 = (string)reader["Op_Add2"];
                    //operationDetail.opAdd2Add = (string)reader["Op_Add2_Add"];

                    //operationDetail.opAdd3 = (string)reader["Op_Add3"];
                    //operationDetail.opAdd3Add = (string)reader["Op_Add3_Add"];
                    //operationDetail.opAdd4 = (string)reader["Op_Add4"];
                    //operationDetail.opAdd4Add = (string)reader["Op_Add4_Add"];
                    operationDetail.oldOpCode = dr["Old_Op_Code"].ToString();
                    operationDetail.opAdd1 = dr["Op_Add1"].ToString();
                    operationDetail.opAdd1Add = dr["op_Add1_Add"].ToString();
                    operationDetail.opAdd2 = dr["Op_Add2"].ToString();
                    operationDetail.opAdd2Add = dr["Op_Add2_Add"].ToString();
                    operationDetail.opAdd3 = dr["Op_Add3"].ToString();
                    operationDetail.opAdd3Add = dr["Op_Add3_Add"].ToString();
                    operationDetail.opAdd4 = dr["Op_Add4"].ToString();
                    operationDetail.opAdd4Add = dr["Op_Add4_Add"].ToString();

                    //operationDetail.operationCode = (string)reader["Operation_Code"];
                    //operationDetail.operationName = (string)reader["Operation_Name"];
                    //operationDetail.operationNameAdd = (string)reader["Operation_Name_Add"];
                    //operationDetail.operationTypeCode = (string)reader["Operation_Type_Code"];
                    //operationDetail.otherCode = (string)reader["Other_Code"];

                    operationDetail.operationCode = dr["Operation_Code"].ToString();
                    operationDetail.operationName = dr["Operation_Name"].ToString();
                    operationDetail.operationNameAdd = dr["Operation_Name_Add"].ToString();
                    operationDetail.operationTypeCode = dr["Operation_Type_Code"].ToString();
                    operationDetail.otherCode = dr["Other_Code"].ToString();

                    //operationDetail.routingCode = (string)reader["Routing_Code"];
                    //operationDetail.salesManCode = (string)reader["Salesman_Code"];
                    //operationDetail.specialInstruction = (string)reader["Special_Instruction"];
                    //operationDetail.telephone1 = (string)reader["Telephone1"];
                    //operationDetail.telephone2 = (string)reader["Telephone2"];

                    operationDetail.routingCode = dr["Routing_Code"].ToString();
                    operationDetail.salesManCode = dr["Salesman_Code"].ToString();
                    operationDetail.specialInstruction = dr["Special_Instruction"].ToString();
                    operationDetail.telephone1 = dr["Telephone1"].ToString();
                    operationDetail.telephone2 = dr["Telephone2"].ToString();

                    //operationDetail.uenNO = dr["UEN_No"].ToString();
                    operationDetail.zipCode = dr["Zip_Code"].ToString();
                    //operationDetail.uenNO = (string)reader["UEN_No"];
                    //operationDetail.zipCode = (string)reader["Zip_Code"];

                    ops.Add(operationDetail);
                }


                cn.Close();
                return ops;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
