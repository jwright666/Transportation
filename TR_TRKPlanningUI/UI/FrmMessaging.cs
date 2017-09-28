
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
    public partial class FrmMessaging : Form
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

        public FrmMessaging(FrmTruckPlanningEntry parent)
        {
            InitializeComponent();
            this._parent = parent;
            PopulateDrivers();
            _showUpdatedMessage = new ShowUpdatedMessageDelegate(UpdateDisplayComments);
            _truckCommBaseAddress = parent.truckCommBaseAddress;

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.Enabled = true;
            string statusMsg = string.Empty;
        }

        void PopulateDrivers()
        {
            try 
            {
                tvDrivers.Nodes.Clear();
                foreach (Driver drv in _parent.drivers)
                {
                    string nodeName = drv.DescriptionForPlanningPurpose.ToString().Trim();
                    TreeNode node = new TreeNode();
                    node.Name = nodeName;
                    //node.ImageIndex = 0;
                    PlanTruckTrip planTrip = PlanTruckTrip.FindPlanTruckTripByDriver(_parent.planTruckTrips, drv);
                    foreach (PlanTruckSubTrip subTrip in planTrip.planTruckSubTrips)
                    {
                        if (subTrip.Task_ID == null || subTrip.Task_ID == string.Empty)
                        {
                            //do nothing
                        }
                        else
                        {   //add the trackComm taskID
                            TreeNode childNode = new TreeNode();
                            childNode.Name = subTrip.Task_ID.Trim();
                            childNode.Text = "#" + subTrip.Task_ID.Trim() + " - " + subTrip.StartStop.Code.ToString() + " to " + subTrip.EndStop.Code.ToString();
                            childNode.ImageIndex = 1;
                            childNode.SelectedImageIndex = 1;
                            node.Nodes.Add(childNode);
                        }
                    }
                    node.Text = "(" + node.Nodes.Count.ToString() + ")" + node.Name;
                    tvDrivers.Nodes.Add(node);
                }
            }
            catch (FMException) { throw; }
            catch (Exception) { throw; }
        }

        private void UpdateCurrentStatusDisplay(Task_Tracking taskObject, string[] status)
        {
            //status = new string[1];
            //if (taskObject != null)
            //{
            //    _currentTaskStatus = taskObject.Status.ToString();
            //    gbxJobInVehicle.Text = "#" + taskObject.Task_ID.ToString() + " " + taskObject.Description.ToString();
            //    lblPerformer.Text = "";
            //    switch (taskObject.Status.ToString())
            //    {
            //        case "P":
            //            lblMessageStatus.Text = "In progress";
            //            lblMessageStatus.BackColor = Color.SkyBlue;
            //            break;
            //        case "H":
            //            lblMessageStatus.Text = "Held";
            //            lblMessageStatus.BackColor = Color.OrangeRed;
            //            break;
            //        case "R":
            //            lblMessageStatus.Text = "Rejected";
            //            lblMessageStatus.BackColor = Color.Red;
            //            break;
            //        case "S":
            //            lblMessageStatus.Text = "Completed";
            //            lblMessageStatus.BackColor = Color.YellowGreen;
            //            //update job status
            //            //currentPlanTruckSubTripJob = planTruckSubTrip.planTruckSubTripJobs.FirstOrDefault(stj => stj.cloudTrack_JobId == taskObject.task_id.ToString());
            //            SetPlanTruckSubTripToComplete();
            //            break;
            //        default:
            //            lblMessageStatus.Text = "Sent Out";
            //            lblMessageStatus.BackColor = Color.Gray;
            //            break;
            //    }
            //    status[0] = lblMessageStatus.Text.ToString();
            //}
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
                            bool isFromPlanner = comment.User_ID.Trim() == _parent.user.UserID.Trim(); //planTruckSubTrip.DriverNumber.Trim();
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
        }
        private void DisplayMessage(string sender, string sendTime, string strMesssage, bool isLink, bool fromPlanner)
        {
            int LineCount = (strMesssage.Length / 20) + 1;
            int PanelHeight = (LineCount * 18) + 15;
            Panel panel1 = new Panel();
            panel1.Dock = DockStyle.Top;
            //panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Height = PanelHeight;
            panel1.MinimumSize = new Size(PanelHeight, 50);

            GroupBox gb1 = new GroupBox();
            gb1.Text = sender;
            gb1.Dock = fromPlanner ? DockStyle.Right : DockStyle.Left;
            gb1.BackColor = fromPlanner ? Color.LightYellow : Color.LightCyan;
            gb1.Font = new Font("Arial", 8);
            int maxWidth =  pnlConvo.Width - (pnlConvo.Width / 4);
            gb1.MaximumSize = new Size(maxWidth, 0);
            gb1.MinimumSize = new Size(PanelHeight, 50);

            TextBox txt1 = new TextBox();
            txt1.AutoSize = true;
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
            label1a.Font = new Font("Arial", 6);
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
                    Process.Start(tmpFile);
                    //delay 2 sec to take time loading the file
                    Thread.Sleep(1000);
                    //delete after viewing
                    File.Delete(tmpFile);
                }
            }
        }
        private async void tvDrivers_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try 
            {
                _controls = new List<Control>();
                pnlConvo.Controls.Clear();
                if (e.Node.Parent != null)
                {
                    Driver drv = Driver.GetDriver(e.Node.Parent.Text.ToString());
                    _selectedPlanTrip = PlanTruckTrip.FindPlanTruckTripByDriver(_parent.planTruckTrips, drv);

                    _taskCommentList = new List<Task_Comment>();
                    _selectedTaskID = e.Node.Text.ToString();
                    var access_token = await GetTC_AuthTokenWithLoginAsync(true).ConfigureAwait(false);
                    _selectedTask_Tracking = await GetTC_TaskFromVehicleAsync(new Task_Tracking()
                    {
                        Task_ID = e.Node.Text.ToString().Trim(),
                        Server_Name = FMGlobalSettings.TheInstance.serverName.Replace("\\", "*"),
                        DB_Name = FMGlobalSettings.TheInstance.dbName
                    }, access_token.ToString());
                }
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        //Comments
        Task_Comment CreateComment()
        {
            var dbName = FMGlobalSettings.TheInstance.dbName.ToString();
            var serverName = FMGlobalSettings.TheInstance.serverName.Replace("\\", "*").ToString();
            Task_Comment comment = new Task_Comment()
            {
                Comment_ID = "0",
                Task_ID = _selectedTaskID == string.Empty ? "0" : _selectedTaskID,
                Description = txtReplyText.Text.Trim().ToString(),
                Driver_Number = _selectedTask_Tracking == null ? string.Empty : _selectedTask_Tracking.Driver_Number,
                Vehicle_Number = _selectedTask_Tracking == null ? string.Empty : _selectedTask_Tracking.Vehicle_Number,
                User_ID = _parent.user.UserID.ToString(),
                Added_DateTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
                Send_Notification = "T",
                DB_Name = dbName,
                Server_Name = serverName
            };
            return comment;
        }
        private async Task<string> GetTC_AuthTokenWithLoginAsync(bool withLogin)
        {
            try
            {
                HttpResponseMessage response = null;
                StringBuilder Security_Code = new StringBuilder(255);
                FMGlobalSettings.GetPrivateProfileString("TruckCommAPI", "SecurityCode", "", Security_Code, 255, Application.StartupPath + "//System_Manager.ini");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (withLogin)
                    {
                        AuthToken token = new AuthToken()
                        {
                            User_Name = FMGlobalSettings.TheInstance.loginUserName,
                            Password = FMGlobalSettings.TheInstance.loginPassword,
                            Security_Code = Security_Code.ToString(),
                            Server_Name = FMGlobalSettings.TheInstance.serverName.Replace("\\", "*"),
                            DB_Name = FMGlobalSettings.TheInstance.dbName
                        };
                        response = await client.PostAsJsonAsync<AuthToken>(_truckCommBaseAddress + "/AuthToken/", token);
                    }
                    else //if no login needed
                    {
                        response = await client.PostAsJsonAsync<string>(_truckCommBaseAddress + "/AuthToken_NoLogin/", Security_Code.ToString());
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
        private async Task<bool> PostTC_TaskCommentAsync(Task_Comment comment, string access_token)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //TODO replace the correct API
                    comment.Auth_Token = access_token;
                    HttpResponseMessage response = await client.PostAsJsonAsync<Task_Comment>(_truckCommBaseAddress + "/AddComment/", comment);
                    var accessResponseString = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        JArray jArrAccess = JArray.Parse(accessResponseString);
                        var status = JObject.Parse(jArrAccess[0].ToString()).GetValue("access").ToString();
                        if (status == "Success")
                        {
                            response = await client.PostAsJsonAsync<Task_Comment>(_truckCommBaseAddress + "/GetComment/", comment);
                            var commentsResponseString = await response.Content.ReadAsStringAsync();
                            if (response.IsSuccessStatusCode)
                            {
                                JArray jArrComments = JArray.Parse(accessResponseString);
                                jArrComments.RemoveAt(0);
                                List<Task_Comment> commentList = new List<Task_Comment>();
                                for (int i = 0; i < jArrComments.Count; i++)
                                {
                                    var jObjComment = JObject.Parse(jArrComments[i].ToString());
                                    Task_Comment tempComment = new Task_Comment();
                                    tempComment.Task_ID = jObjComment.GetValue("Task_ID").ToString();
                                    tempComment.Comment_ID = jObjComment.GetValue("Comment_ID").ToString();
                                    tempComment.Description = jObjComment.GetValue("Description").ToString();
                                    tempComment.Driver_Number = jObjComment.GetValue("Driver_Number").ToString();
                                    tempComment.Vehicle_Number = jObjComment.GetValue("Vehicle_Number").ToString();
                                    tempComment.Added_DateTime = jObjComment.GetValue("Added_DateTime").ToString();
                                    tempComment.User_ID = jObjComment.GetValue("User_ID").ToString();
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
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (_selectedTaskID != string.Empty)
            {
                var task_Comment = CreateComment();
                var access_token = await GetTC_AuthTokenWithLoginAsync(true).ConfigureAwait(false);
                var posted = await PostTC_TaskCommentAsync(task_Comment, access_token.ToString()).ConfigureAwait(false);
                if (posted)
                {
                    if (this.IsHandleCreated)
                    {
                        txtReplyText.BeginInvoke(new MethodInvoker(delegate
                        {
                            txtReplyText.Text = string.Empty;
                        }));
                    }
                }
            }
        }

        private async void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timer.Enabled = false;
                var access_token = await GetTC_AuthTokenWithLoginAsync(true).ConfigureAwait(false);
                if (_selectedTaskID != string.Empty)
                {
                    var task = await GetTC_TaskFromVehicleAsync(new Task_Tracking()
                                                                    {
                                                                        Task_ID = _selectedTaskID,
                                                                        Server_Name = FMGlobalSettings.TheInstance.serverName.Replace("\\", "*"),
                                                                        DB_Name = FMGlobalSettings.TheInstance.dbName
                                                                    }, access_token.ToString());
                    Thread.Sleep(3000); //pause for 3 seconds to allow request tobe executed
                }
                _timer.Enabled = true;
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }
    }
}
