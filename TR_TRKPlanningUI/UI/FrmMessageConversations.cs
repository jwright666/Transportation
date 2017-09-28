
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using FM.TR_TRKBookDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_TRKPlanDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using TR_LanguageResource.Resources;
using TR_FormBaseLibrary;
using TR_MessageDLL.BLL;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_TRKPlanningUI.UI
{
    public partial class FrmMessageConversation : Form
    {
        //get textbox line count 
        private const int EM_GETLINECOUNT = 0xba;
        [DllImport("user32", EntryPoint = "SendMessageA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);


        System.Timers.Timer _timer;
        SortableList<Driver> _drivers = new SortableList<Driver>();
        FrmTruckPlanningEntry _parent;
        Dictionary<string, string> _linkDict = new Dictionary<string, string>();
        List<Control> _controls;
        StringBuilder _commentHistory = new StringBuilder();
        List<Task_Comment> _taskCommentList = new List<Task_Comment>();
        delegate void ShowUpdatedMessageDelegate(List<Task_Comment> commentList, string replayText);
        ShowUpdatedMessageDelegate _showUpdatedMessage;
        string _selectedTaskID = string.Empty;
        string _truckCommBaseAddress = string.Empty;
        Task_Tracking _selectedTask_Tracking;
        PlanTruckTrip _selectedPlanTrip;
        string _deptCode;
        SortableList<PlanTruckTrip> _planTruckTrips;
        string _userID;


        public FrmMessageConversation(string userID)
        {
            InitializeComponent();
            _userID = userID;
            _deptCode = TptDept.GetAllTptDepts(DeptType.Trucking)[0].TptDeptCode.ToString();
            //_drivers = PlanDateDept.GetDriversForPlanDateDept(DateTime.Today.Date, _deptCode);
            //_planHaulierTrips = PlanHaulierTrip.GetAllPlanHaulierTripsByDayAndDept(DateTime.Today.Date, _deptCode);
            InitializeDisplay();
        }

        public FrmMessageConversation(FrmTruckPlanningEntry parent)
        {
            InitializeComponent();
            this._parent = parent;
            _userID = parent.user.UserID.ToString();
            _drivers = parent.drivers;
            _planTruckTrips = parent.planTruckTrips;
            InitializeDisplay();
            dtpScheduleDate.Value = parent.chosenDate;
        }

        void InitializeDisplay()
        {
            dtpScheduleDate.Value = DateTime.Today;
            PopulateDrivers();
            _showUpdatedMessage = new ShowUpdatedMessageDelegate(UpdateDisplayComments);
            StringBuilder wsAddress = new StringBuilder(255);
            FMGlobalSettings.GetPrivateProfileString("TruckCommAPI", "BaseAddress", "", wsAddress, 255, Application.StartupPath + "//System_Manager.ini");
            _truckCommBaseAddress = wsAddress.ToString();

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.Enabled = true;
            string statusMsg = string.Empty;
            this.MouseWheel += new MouseEventHandler(pnlConvo_MouseWheel);
        }
        void PopulateDrivers()
        {
            try 
            {
                tvDrivers.Nodes.Clear();
                foreach (Driver drv in _drivers)
                {
                    TreeNode node = new TreeNode();
                    node.Name = drv.Code.ToString().Trim();
                    //node.ImageIndex = 0;
                    PlanTruckTrip planTrip = PlanTruckTrip.FindPlanTruckTripByDriver(_planTruckTrips, drv);
                    if (planTrip != null)
                    {
                        foreach (PlanTruckSubTrip subTrip in planTrip.planTruckSubTrips)
                        {
                            if (subTrip.Task_ID == null || subTrip.Task_ID == string.Empty || subTrip.Task_ID == "0")
                            {
                                //do nothing
                            }
                            else
                            {   //add the trackComm taskID
                                TreeNode childNode = new TreeNode();
                                childNode.Name = subTrip.Task_ID.Trim();
                                childNode.Text = "#" + subTrip.Task_ID.Trim() +" - "+ subTrip.StartStop.Code.ToString() + " to " + subTrip.EndStop.Code.ToString();
                                childNode.ImageIndex = 1;
                                childNode.SelectedImageIndex = 1;
                                node.Nodes.Add(childNode);
                            }
                        }
                    }
                    node.Text = "(" + node.Nodes.Count.ToString() + ")" + drv.DescriptionForPlanningPurpose.ToString().Trim();
                    tvDrivers.Nodes.Add(node);
                }
            }
            catch (FMException) { throw; }
            catch (Exception) { throw; }
        }

        private void UpdateDisplayComments(List<Task_Comment> commentList, string replyText)
        {
            if (commentList != null)
            {
                if (commentList.Count > 0)
                {
                    if (_taskCommentList.Count < commentList.Count)
                    {
                        _linkDict = new Dictionary<string, string>();
                        _controls = new List<Control>();
                        commentList = commentList.OrderBy(m => m.Comment_ID).ToList();
                        int attachmentCount = 1;
                        foreach (Task_Comment comment in commentList)
                        {
                            bool isFromPlanner = comment.User_ID.Trim() == _userID.Trim(); //planTruckSubTrip.DriverNumber.Trim();
                            if (comment.Attachments.Count == 0)
                            {
                                string tempComment = comment.Description.ToString();
                                if (!tempComment.Equals(string.Empty))
                                {
                                    if (!_taskCommentList.Contains(comment))
                                    {
                                        DisplayMessage(comment.User_ID.Trim(), comment.Added_DateTime, tempComment, false, isFromPlanner);

                                        if (!_commentHistory.ToString().Contains(tempComment))
                                            _commentHistory.Append(tempComment + Environment.NewLine);

                                        pnlConvo.Controls.Clear();
                                        _controls.Reverse();
                                        pnlConvo.Controls.AddRange(_controls.ToArray());
                                        pnlConvo.VerticalScroll.Value = pnlConvo.VerticalScroll.Maximum;
                                        pnlConvo.AutoScroll = true;
                                        _controls.Reverse();
                                    }
                                }
                            }
                            else
                            {
                                //string podString = "pod.png";
                                for (int i = 0; i < comment.Attachments.Count; i++)
                                {
                                    string tempComment = comment.Attachments[i].CMT_FileName;
                                    if (!_taskCommentList.Contains(comment))
                                    {
                                        DisplayMessage(comment.User_ID.Trim(), comment.Added_DateTime, tempComment, true, isFromPlanner);

                                        _linkDict.Add(tempComment, comment.Attachments[i].CMT_File.ToString());//comment.Attachments[i].FilePath.ToString());

                                        pnlConvo.Controls.Clear();
                                        _controls.Reverse();
                                        pnlConvo.Controls.AddRange(_controls.ToArray());
                                        pnlConvo.VerticalScroll.Value = pnlConvo.VerticalScroll.Maximum;
                                        pnlConvo.AutoScroll = true;
                                        _controls.Reverse();
                                    }
                                }
                            }
                        }
                        _taskCommentList = commentList;
                    }
                }
            }
            _timer.Enabled = true;
        }
        private void DisplayMessage(string sender, string sendTime, string strMesssage, bool isLink, bool fromPlanner)
        {
            int LineCount = (strMesssage.Length / 20) + 1;
            int PanelHeight = (LineCount * 18) + 15;
            Panel panel1 = new Panel();
            panel1.Dock = DockStyle.Top;
            //panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Height = PanelHeight;
            panel1.MinimumSize = new Size(PanelHeight, 65);

            GroupBox gb1 = new GroupBox();
            gb1.Text = sender;
            gb1.Dock = fromPlanner ? DockStyle.Right : DockStyle.Left;
            gb1.BackColor = fromPlanner ? Color.LightYellow : Color.LightCyan;
            gb1.Font = new Font("Arial", 8);
            int maxWidth =  pnlConvo.Width - (pnlConvo.Width / 4);
            gb1.MaximumSize = new Size(maxWidth, 0);
            gb1.MinimumSize = new Size(PanelHeight, 50);
            gb1.MouseEnter += new System.EventHandler(this.pnlConvo_MouseEnter);

            TextBox txt1 = new TextBox();
            txt1.AutoSize = true;
            txt1.Font = new Font("Arial", 10);
            txt1.Multiline = true;
            txt1.ReadOnly = true;
            txt1.Text = strMesssage;
            txt1.Dock = DockStyle.Fill;
            txt1.BackColor = fromPlanner ? Color.LightYellow : Color.LightCyan;
            txt1.BorderStyle = BorderStyle.None;
            txt1.TextAlign = HorizontalAlignment.Left;
            txt1.WordWrap = true;

            //to fit group box
            int txtWidth = 0;
            if (LineCount == 1)
            {
                txtWidth = Convert.ToInt32(Convert.ToDouble(strMesssage.Length) * 7.2);
                txt1.Width = txtWidth;
            }
            if (isLink)
            {
                txt1.ForeColor = Color.Blue;
                txt1.Font = new Font(txt1.Font.Name, txt1.Font.SizeInPoints, FontStyle.Underline);
                txt1.Tag = strMesssage;
                txt1.Click += Attachment_Click;
                txt1.Width = maxWidth - 10;
            }
            gb1.Controls.Add(txt1);

            Label label1a = new Label();
            label1a.AutoSize = true;
            label1a.Text = sendTime.Substring(sendTime.IndexOf(" "), sendTime.Length - sendTime.IndexOf(" "));
            label1a.BringToFront();
            label1a.Dock = DockStyle.Top;
            label1a.Font = new Font("Arial", 8);
            label1a.TextAlign = ContentAlignment.BottomRight;
            label1a.ForeColor = Color.DimGray;
            gb1.Controls.Add(label1a);

            //to fit group box
            if (LineCount == 1)
            {
                int MaxWidth = 0;
                int TitleWidth = Convert.ToInt32(Convert.ToDouble(gb1.Text.Length) * 7.2) + 10;
                if (TitleWidth > txtWidth)
                    MaxWidth = TitleWidth;
                else
                    MaxWidth = txtWidth;

                gb1.Width = MaxWidth + 15;
                panel1.Height = PanelHeight;
            }
            else
            {
                int width = 100; //default width
                Font font = new Font(txt1.Font.Name, txt1.Font.Size);
                Size s = TextRenderer.MeasureText(txt1.Text, font);
                if (s.Width > width)
                {
                    gb1.Width = s.Width + 10;
                    //txt1.Width = s.Width + 10;
                    var numberOfLines = SendMessage(txt1.Handle.ToInt32(), EM_GETLINECOUNT, 0, 0);
                    int txtH = (txt1.Font.Height + 2) * numberOfLines;

                    panel1.Height = (txtH - label1a.Height - 10) + gb1.Height;
                }
            }
            panel1.Controls.Add(gb1);
            panel1.Dock = DockStyle.Top;
            _controls.Add(panel1);
        }
 
        private void Attachment_Click(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            var tag = control.Tag;
            string linkValue = null;
            _linkDict.TryGetValue(tag.ToString(), out linkValue);
            if (linkValue != null)
            {
                //var pix = Encoding.ASCII.GetBytes(linkValue);
                var pix = Convert.FromBase64String(linkValue);
                using (var memoryStream = new MemoryStream(pix))
                {
                    memoryStream.Write(pix, 0, pix.Length);
                    Image attachment = Image.FromStream(memoryStream);
                    String tmpFile = Path.GetTempFileName();//default filename
                    tmpFile = Path.ChangeExtension(tmpFile, "png");
                    attachment.Save(tmpFile);

                    //view file
                    Process.Start("IExplore.exe","file:///" + tmpFile);
                    //delay 2 sec to take time loading the file
                    Thread.Sleep(1000);
                    //delete after viewing
                    File.Delete(tmpFile);
                }
            }
        }
        private void tvDrivers_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try 
            {
                _controls = new List<Control>();
                pnlConvo.Controls.Clear();
                if (e.Node.Parent != null)
                {
                    //Driver drv = Driver.GetDriver(e.Node.Parent.Text.ToString());
                    //_selectedPlanTrip = PlanHaulierTrip.FindPlanHaulierTripByDriver(_planHaulierTrips, drv);

                    _taskCommentList = new List<Task_Comment>();
                    _selectedTaskID = e.Node.Name.ToString();
                    _timer.Enabled = false;
                    ShowConversation();
                }
                else
                {
                    Label lbl = new Label();
                    if(e.Node.Nodes.Count == 0)
                        lbl.Text = "There is no task assigned to this driver during the selected date. ";
                    else if (e.Node.Nodes.Count == 1)
                        lbl.Text = "There is " + e.Node.Nodes.Count.ToString() + " conversation between the driver during the selected date.";
                    else
                        lbl.Text = "There are " + e.Node.Nodes.Count.ToString() + " conversations between the driver during the selected date.";

                    lbl.ForeColor = Color.Red;
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.Dock = DockStyle.Fill;
                    pnlConvo.Controls.Add(lbl);
                }
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void ShowConversation()
        {
            var access_token = await GetTC_AuthTokenWithLoginAsync().ConfigureAwait(false);
            _selectedTask_Tracking = await GetTC_TaskFromVehicleAsync(new Task_Tracking()
            {
                Task_ID = _selectedTaskID.ToString().Trim(),
                Server_Name = FMGlobalSettings.TheInstance.serverName.Replace("\\", "*"),
                DB_Name = FMGlobalSettings.TheInstance.dbName
            }, access_token.ToString()).ConfigureAwait(false);
        }
        //Comments
        private async Task<string> GetTC_AuthTokenWithLoginAsync()
        {
            try
            {
                HttpResponseMessage response = null;
                StringBuilder Security_Code = new StringBuilder(255);
                StringBuilder With_Login = new StringBuilder(255);
                FMGlobalSettings.GetPrivateProfileString("TruckCommAPI", "SecurityCode", "", Security_Code, 255, Application.StartupPath + "//System_Manager.ini");
                FMGlobalSettings.GetPrivateProfileString("TruckCommAPI", "WithLogin", "", Security_Code, 255, Application.StartupPath + "//System_Manager.ini");
                bool withLogin = With_Login.ToString() == "T";
                AuthToken token = new AuthToken()
                {
                    User_Name = FMGlobalSettings.TheInstance.loginUserName,
                    Password = FMGlobalSettings.TheInstance.loginPassword,
                    Security_Code = Security_Code.ToString(),
                    Server_Name = FMGlobalSettings.TheInstance.serverName.Replace("\\", "*"),
                    DB_Name = FMGlobalSettings.TheInstance.dbName
                }; 
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (withLogin)
                    {
                        response = await client.PostAsJsonAsync<AuthToken>(_truckCommBaseAddress + "/AuthToken/", token);
                    }
                    else //if no login needed
                    {
                        response = await client.PostAsJsonAsync<AuthToken>(_truckCommBaseAddress + "/AuthToken_NoLogin/", token);
                    }
                    var responseString = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        JArray jArr = JArray.Parse(responseString);
                        JObject jObj = JObject.Parse(jArr[0].ToString());
                        var access_Token = jObj.GetValue("Auth_Token").ToString();
                        DateTime expiry = Convert.ToDateTime(jObj.GetValue("Auth_Expire").ToString());
                        if (expiry.Date < DateTime.Today)
                            throw new Exception("Authentication token was expired last " + expiry.Date.ToShortDateString());

                        return access_Token;
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return string.Empty;
        }
        private async Task<Task_Tracking> GetTC_TaskFromVehicleAsync(Task_Tracking task, string access_token)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.BaseAddress = new Uri(_truckCommBaseAddress);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //TODO replace the correct API
                    task.Auth_Token = access_token;
                    HttpResponseMessage response = await client.PostAsJsonAsync<Task_Tracking>(client.BaseAddress.ToString() + "/GetJobTrip/", task);
                    var taskResponseString = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        JArray jArr = JArray.Parse(taskResponseString);
                        JObject jObj = JObject.Parse(jArr[1].ToString());
                        task.Status = jObj.GetValue("Status").ToString();
                        task.Description = jObj.GetValue("Description").ToString();
                        task.Vehicle_Number = jObj.GetValue("Vehicle_Number").ToString();
                        task.Driver_Number = jObj.GetValue("Driver_Number").ToString();
                        //comments
                        #region Comments and Attachments
                        Task_Comment comment = new Task_Comment() { Task_ID = task.Task_ID, Comment_ID = "", DB_Name = task.DB_Name, Server_Name = task.Server_Name.Replace("\\", "*"), Auth_Token = access_token };
                        HttpResponseMessage commentResponse = await client.PostAsJsonAsync<Task_Comment>(client.BaseAddress.ToString() + "/GetComment/", comment);
                        var commentResponseString = await commentResponse.Content.ReadAsStringAsync();
                        if (commentResponse.IsSuccessStatusCode)
                        {
                            List<Task_Comment> commentList = new List<Task_Comment>();
                            JArray jArrComment = JArray.Parse(commentResponseString);
                            if (jArrComment.Count > 1)
                            {
                                jArrComment.RemoveAt(0);
                                for (int i = 0; i < jArrComment.Count; i++)
                                {
                                    var jObjComment = JObject.Parse(jArrComment[i].ToString());
                                    Task_Comment tempComment = new Task_Comment();
                                    tempComment.Task_ID = jObjComment.GetValue("Task_ID").ToString();
                                    tempComment.Comment_ID = jObjComment.GetValue("Comment_ID").ToString();
                                    tempComment.Description = jObjComment.GetValue("Description").ToString();
                                    tempComment.Driver_Number = jObjComment.GetValue("Driver_Number").ToString();
                                    tempComment.Vehicle_Number = jObjComment.GetValue("Vehicle_Number").ToString();
                                    tempComment.Added_DateTime = jObjComment.GetValue("Added_DateTime").ToString();
                                    tempComment.User_ID = jObjComment.GetValue("User_ID").ToString();
                                    //get attachments if any
                                    #region Attachments
                                    ///*
                                    Comment_Attachment paramAttachment = new Comment_Attachment() { Comment_ID = tempComment.Comment_ID, DB_Name = task.DB_Name, Server_Name = task.Server_Name.Replace("\\", "*"), Auth_Token = access_token };
                                    HttpResponseMessage attachmentResponse = await client.PostAsJsonAsync<Comment_Attachment>(client.BaseAddress.ToString() + "/GetCommentImage/", paramAttachment);
                                    var attachmentResponseString = await attachmentResponse.Content.ReadAsStringAsync();
                                    if (attachmentResponse.IsSuccessStatusCode)
                                    {
                                        tempComment.Attachments = new List<Comment_Attachment>();
                                        JArray jArrAttachment = JArray.Parse(attachmentResponseString);
                                        jArrAttachment.RemoveAt(0);
                                        for (int a = 0; a < jArrAttachment.Count; a++)
                                        {
                                            var jObjAttachment = JObject.Parse(jArrAttachment[a].ToString());
                                            Comment_Attachment tempAttachment = new Comment_Attachment();
                                            //tempAttachment.Comment_ID = jObjAttachment.GetValue("Comment_ID").ToString();
                                            tempAttachment.CMT_FileName = jObjAttachment.GetValue("CMT_FileName").ToString();
                                            tempAttachment.CMT_FilePath = jObjAttachment.GetValue("CMT_FilePath").ToString();
                                            tempAttachment.CMT_File = jObjAttachment.GetValue("CMT_File").ToString();
                                            tempAttachment.Description = jObjAttachment.GetValue("Description").ToString();
                                            if (!tempAttachment.CMT_FileName.Equals(string.Empty))
                                            {
                                                var attach = tempComment.Attachments.Where(at => at.CMT_FileName == tempAttachment.CMT_FileName).ToList();
                                                if (attach.Count == 0)
                                                    tempComment.Attachments.Add(tempAttachment);
                                            }
                                        }
                                    }
                                    //*/
                                    #endregion

                                    commentList.Add(tempComment);
                                }
                                if (_taskCommentList.Count != commentList.Count)
                                {
                                    if (this.IsHandleCreated)
                                    {
                                        BeginInvoke(_showUpdatedMessage, commentList, string.Empty);
                                    }
                                }
                            }
                        }
                        #endregion
                        return task;
                    }
                }
                return task;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private async void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timer.Enabled = false;
                var access_token = await GetTC_AuthTokenWithLoginAsync().ConfigureAwait(false);
                if (_selectedTaskID != null)
                {
                    if (_selectedTaskID != string.Empty)
                    {
                        var task = await GetTC_TaskFromVehicleAsync(new Task_Tracking()
                                                                        {
                                                                            Task_ID = _selectedTaskID,
                                                                            Server_Name = FMGlobalSettings.TheInstance.serverName.Replace("\\", "*"),
                                                                            DB_Name = FMGlobalSettings.TheInstance.dbName
                                                                        }, access_token.ToString());
                        //Thread.Sleep(3000); //pause for 3 seconds to allow request tobe executed
                    }
                }
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }

        private void dtpScheduleDate_ValueChanged(object sender, EventArgs e)
        {
            if (_parent == null)
            {
                _drivers = PlanDateDept.GetDriversForPlanDateDeptForTrucking(dtpScheduleDate.Value.Date, _deptCode);
                _planTruckTrips = PlanTruckTrip.GetAllPlanTruckTripByDayAndDept(dtpScheduleDate.Value.Date, _deptCode);
                PopulateDrivers();
            }
        }

        private void pnlConvo_MouseEnter(object sender, EventArgs e)
        {
            pnlConvo.Focus();
        }
        private void pnlConvo_MouseWheel(object sender, EventArgs e)
        {
            pnlConvo.Focus();
        }
    }
}
