
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_TRKBookDLL.BLL;
using FM.TR_TRKBookUI.UI;
using FM.TR_MaintenanceDLL.BLL;
using TR_FormBaseLibrary;
using TR_LanguageResource.Resources;
using FM.TR_TRKBookDLL.Facade;

namespace FM.TR_TRKBookUI.UI
{
    public partial class FrmConsignmentNote : AbstractForm
    {
        FrmTruckJob _parent;
        string trxID;
        List<string> _consignmentNoteNos = new List<string>();
        List<string> _trxIDs = new List<string>();
        delegate void CloseFormDelegate();
        CloseFormDelegate _delegateCloseForm;
        public ArrayList consignmentJob_Ids;


        public FrmConsignmentNote(FrmTruckJob parent)
        {
            InitializeComponent();
            //cboCustomerCode.DataSource = TruckingFacadeIn.GetConsignmentNoteHouseNos();
            this._parent = parent; 
            this._delegateCloseForm = new CloseFormDelegate(CloseForm);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void CloseForm()
        {
            this.Dispose();
            this.Close();
        }
        private async void btnRetrieveJob_Click(object sender, EventArgs e)
        {
            try
            {
                //_parent.truckJob = await GetConsignmentNoteByCustomerCodeAsync(cboCustomerCode.Text.ToString()).ConfigureAwait(false);
                List<TruckJob> truckJobList = await ConvertConsolidateDataToTruckJobsAsync(cboTrx_Id.Text.ToString()).ConfigureAwait(false);
                //string Auth_Token, User_ID, Trx_ID, Transport_Flag;
                if (truckJobList.Count > 0)
                {
                    bool saved = TruckJob.AddTruckJobs(new SortableList<TruckJob>(truckJobList), this.Name, _parent.user.UserID.ToString());
                    if (saved)
                    {
                        //string token = await GetAccessTokenAsync().ConfigureAwait(false);
                        //var data = new { Auth_Token = token, User_ID = _parent.user.UserID, Trx_ID = trxID.ToString(), Transport_Flag = "T", DBName = jsParameter.paramInfo.DatabaseName, ServerName = jsParameter.paramInfo.ServerName };
                        //update Transport Flag field to remove the processed trxID from the list                       
                        await UpdateTransportFlagAsync("T");//set to true

                        MessageBox.Show(truckJobList.Count.ToString() + " customer jobs being booked", CommonResource.Alert);
                        this.DialogResult = DialogResult.OK;
                    }
                }
            }
            catch (FMException ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }

        private void cboTrxNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cboConsignmentNoteNo.DataSource = TruckingFacadeIn.GetConsignmentNoteJobNos(cboTrxNo.Text.ToString());
        }

        private async void FrmConsignmentNote_Load(object sender, EventArgs e)
        {
            try
            {
                //_consignmentNoteNos = await GetCustomerCodesAsync().ConfigureAwait(false); //
                //cboCustomerCode.DataSource = _trxIDs;
                _trxIDs = await GetTrxIDsAsync();
                Thread.Sleep(200);
                cboTrx_Id.DataSource = _trxIDs.ToArray();
            }
            catch (FMException ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }
        private void PopulateTrxNosFromServer()
        {
            try
            {
               //_parent.truckJob = TruckingFacadeIn.ConvertConsignmentNoteToTruckJob(cboConsignmentNoteNo.Text.ToString());
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            
        }
        private async Task<List<String>> GetCustomerCodesAsync()
        {
            List<String> consignmentNotes = new List<String>();
            try
            {
                //TODO replace the correct API
                JSONParameter jsParameter = new JSONParameter();
                jsParameter.paramInfo = new ParamInfo(); // { DatabaseName = "FX_LOMING_TESTING", ServerName = "LAPTOP-P5S6NM39*SQLEXPRESS", Customer_Code = customerCode };
                jsParameter.paramInfo.SetParamInfo();
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    //client.BaseAddress = new Uri("http://192.168.44.165:8998/datasnap/rest/TServerMethods1");
                    client.BaseAddress = new Uri("http://" + jsParameter.paramInfo.ServerIP + ":" + jsParameter.paramInfo.ServerPort + "/datasnap/rest/TConsignmentMethods");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsJsonAsync<JSONParameter>(client.BaseAddress.ToString() + "/GetCustomersJSONArray/", jsParameter);
                    var taskResponseString = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        JArray jArr = JArray.Parse(taskResponseString);
                        consignmentNotes = TruckingFacadeIn.GetCustomerCodes(jArr);
                    }
                    else
                        throw new Exception(response.ReasonPhrase + "\n" + taskResponseString.Replace("/",""));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return consignmentNotes;
        }
        private async Task<TruckJob> GetConsignmentNoteByCustomerCodeAsync(string customerCode)
        {
            TruckJob truckJob = null;
            try
            {
                //TODO replace the correct API
                JSONParameter jsParameter = new JSONParameter();
                jsParameter.paramInfo = new ParamInfo(); // { DatabaseName = "FX_LOMING_TESTING", ServerName = "LAPTOP-P5S6NM39*SQLEXPRESS", Customer_Code = customerCode };
                jsParameter.paramInfo.SetParamInfo();
                jsParameter.paramInfo.filter = new Filter() { Customer_Code = customerCode };
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    //client.BaseAddress = new Uri("http://192.168.44.165:8998/datasnap/rest/TServerMethods1");
                    client.BaseAddress = new Uri("http://" + jsParameter.paramInfo.ServerIP + ":" + jsParameter.paramInfo.ServerPort + "/datasnap/rest/TConsignmentMethods");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsJsonAsync<JSONParameter>(client.BaseAddress.ToString() + "/GetConsignmentNoteJSONArray/", jsParameter);
                    var taskResponseString = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        JObject jObj = JObject.Parse(taskResponseString);
                        truckJob = new TruckJob();
                        truckJob = TruckingFacadeIn.ConvertConsignmentNoteToTruckJob(jObj, _parent.user, out consignmentJob_Ids);
                    }
                    else
                        throw new Exception(response.ReasonPhrase + "\n" + taskResponseString.Replace("/", ""));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return truckJob;
        }
        
        //currently authentication not in use for local service
        private async Task<String> GetAccessTokenAsync()
        {
            String accessToken = string.Empty;
            try
            {
                //TODO replace the correct API
                JSONParameter jsParameter = new JSONParameter();
                jsParameter.paramInfo = new ParamInfo(); // { DatabaseName = "FX_LOMING_TESTING", ServerName = "LAPTOP-P5S6NM39*SQLEXPRESS", Customer_Code = customerCode };
                jsParameter.paramInfo.SetParamInfo();
                string User_ID, Password, Company_ID;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    //client.BaseAddress = new Uri("http://192.168.44.165:8998/datasnap/rest/TServerMethods1");
                    client.BaseAddress = new Uri("http://42.1.62.183:5757/datasnap/rest/TCloudServerMethod/AuthToken/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var data = new { User_ID = "ipl", Password = "support", Company_ID = "Loming" };
                    HttpResponseMessage response = await client.PostAsJsonAsync<object>(client.BaseAddress.ToString(), data);
                    var taskResponseString = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        JArray jArr = JArray.Parse(taskResponseString);
                        JObject jObj = JObject.Parse(jArr[0].ToString());
                        accessToken = jObj.GetValue("Auth_Token").ToString();
                    }
                    else
                        throw new Exception(response.ReasonPhrase + "\n" + taskResponseString.Replace("/", ""));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accessToken;
        }

        private async Task<List<String>> GetTrxIDsAsync()
        {
            List<String> trxIDs = new List<String>();
            try
            {
                //TODO replace the correct API
                JSONParameter jsParameter = new JSONParameter();
                jsParameter.paramInfo = new ParamInfo(); // { DatabaseName = "FX_LOMING_TESTING", ServerName = "LAPTOP-P5S6NM39*SQLEXPRESS", Customer_Code = customerCode };
                jsParameter.paramInfo.SetParamInfo();
                //string token = await GetAccessTokenAsync().ConfigureAwait(false);
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    //client.BaseAddress = new Uri("http://192.168.44.165:5657/datasnap/rest/TConsignmentMethods");
                    client.BaseAddress = new Uri("http://" + jsParameter.paramInfo.ServerIP + ":" + jsParameter.paramInfo.ServerPort + "/datasnap/rest/TConsignmentMethods");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var data = new { Auth_Token = "", DBName = jsParameter.paramInfo.DatabaseName, ServerName = jsParameter.paramInfo.ServerName };

                    HttpResponseMessage response = await client.PostAsJsonAsync<object>(client.BaseAddress.ToString() + "/Get_ConsolidateTrxIDs/", data);
                    var taskResponseString = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        JArray jArr = JArray.Parse(taskResponseString);
                        for (int i = 1; i < jArr.Count; i++)
                        {
                            trxIDs.Add(((JObject)jArr[i]).GetValue("Trx_ID").ToString());
                        }
                    }
                    else
                        throw new Exception(response.ReasonPhrase + "\n" + taskResponseString.Replace("/", ""));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return trxIDs;
        }        
        private async Task<List<TruckJob>> ConvertConsolidateDataToTruckJobsAsync(string trxKey)
        {
            List<TruckJob> truckJobList = new List<TruckJob>();
            try
            {
                //TODO replace the correct API
                JSONParameter jsParameter = new JSONParameter();
                jsParameter.paramInfo = new ParamInfo(); // { DatabaseName = "FX_LOMING_TESTING", ServerName = "LAPTOP-P5S6NM39*SQLEXPRESS", Customer_Code = customerCode };
                jsParameter.paramInfo.SetParamInfo();
                //string token = await GetAccessTokenAsync().ConfigureAwait(false);
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    //client.BaseAddress = new Uri("http://192.168.44.165:5657/datasnap/rest/TConsignmentMethods");
                    client.BaseAddress = new Uri("http://" + jsParameter.paramInfo.ServerIP + ":" + jsParameter.paramInfo.ServerPort + "/datasnap/rest/TConsignmentMethods");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //string Auth_Token, Trx_ID, Transport_Mode = string.Empty;
                    var data = new { Auth_Token = "", Trx_ID = trxKey, Transport_Mode = "SEA", DBName = jsParameter.paramInfo.DatabaseName, ServerName = jsParameter.paramInfo.ServerName };

                    HttpResponseMessage response = await client.PostAsJsonAsync<object>(client.BaseAddress.ToString() + "/Get_ConsolidateByIDForTransport/", data);
                    var taskResponseString = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        JArray jObjArr = JArray.Parse(taskResponseString);
                        if (jObjArr.Count() > 1 && ((JObject)jObjArr[0]).GetValue("access").ToString() == "success")
                        {
                            JObject jObj = JObject.Parse(jObjArr[1].ToString());
                            JArray jConsignmentNoteArr = JArray.Parse(jObj.GetValue("ConsolDetail").ToString());
                            if (jConsignmentNoteArr.Count() > 0)
                                truckJobList = TruckingFacadeIn.ConvertCosolidatedConsignmentNoteToTruckJobs(jConsignmentNoteArr, _parent.user);                           
                            else
                                throw new FMException("No consignment note details found.");
                        }
                        else
                            throw new FMException("No consignment note Data found. Please check consignment data for the selected transaction number.");
                    }
                    else
                        throw new Exception(response.ReasonPhrase + "\n" + taskResponseString.Replace("/", ""));
                }
            }
            catch (Exception ex) { throw ex; }
            return truckJobList;
        }
        private async Task<bool> UpdateTransportFlagAsync(string flagValue)
        {
            try
            {
                //TODO replace the correct API
                JSONParameter jsParameter = new JSONParameter();
                jsParameter.paramInfo = new ParamInfo(); // { DatabaseName = "FX_LOMING_TESTING", ServerName = "LAPTOP-P5S6NM39*SQLEXPRESS", Customer_Code = customerCode };
                jsParameter.paramInfo.SetParamInfo();
                //string token = await GetAccessTokenAsync().ConfigureAwait(false);
                var data = new { Auth_Token = "", User_ID = _parent.user.UserID, Trx_ID = trxID.ToString(), Transport_Flag = flagValue, DBName = jsParameter.paramInfo.DatabaseName, ServerName = jsParameter.paramInfo.ServerName };
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    //client.BaseAddress = new Uri("http://192.168.44.165:5657/datasnap/rest/TConsignmentMethods");
                    client.BaseAddress = new Uri("http://" + jsParameter.paramInfo.ServerIP + ":" + jsParameter.paramInfo.ServerPort + "/datasnap/rest/TConsignmentMethods");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));                    
                    HttpResponseMessage response = await client.PostAsJsonAsync<object>(client.BaseAddress.ToString() + "/Edit_ConsolidateTransportFlag/", data);
                    var taskResponseString = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        JArray jArr = JArray.Parse(taskResponseString);
                        if (((JObject)jArr[0]).GetValue("access").ToString() == "success")
                        {
                            return true;
                        }                        
                    }
                    else
                        throw new Exception(response.ReasonPhrase + "\n" + taskResponseString.Replace("/", ""));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }        
      

        private void cboCustomerCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.AutoCompleteComboBox(sender, e, true);
        }

        private void cboTrx_Id_SelectedIndexChanged(object sender, EventArgs e)
        {
            trxID = cboTrx_Id.Text.ToString().Trim();
        }
    }
}
