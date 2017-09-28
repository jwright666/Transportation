namespace FM.TR_TRKPlanningUI.UI
{
    partial class FrmMessageConversation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMessageConversation));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gbxDrivers = new System.Windows.Forms.GroupBox();
            this.tvDrivers = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.gbxConvo = new System.Windows.Forms.GroupBox();
            this.pnlConvo = new System.Windows.Forms.Panel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.dtpScheduleDate = new System.Windows.Forms.DateTimePicker();
            this.lblDate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbxDrivers.SuspendLayout();
            this.gbxConvo.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 38);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.gbxDrivers);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.gbxConvo);
            this.splitContainer1.Size = new System.Drawing.Size(671, 398);
            this.splitContainer1.SplitterDistance = 297;
            this.splitContainer1.TabIndex = 0;
            // 
            // gbxDrivers
            // 
            this.gbxDrivers.AutoSize = true;
            this.gbxDrivers.Controls.Add(this.tvDrivers);
            this.gbxDrivers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxDrivers.Location = new System.Drawing.Point(0, 0);
            this.gbxDrivers.Name = "gbxDrivers";
            this.gbxDrivers.Size = new System.Drawing.Size(297, 398);
            this.gbxDrivers.TabIndex = 1;
            this.gbxDrivers.TabStop = false;
            this.gbxDrivers.Text = "DRIVERS";
            // 
            // tvDrivers
            // 
            this.tvDrivers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvDrivers.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvDrivers.ImageIndex = 0;
            this.tvDrivers.ImageList = this.imageList1;
            this.tvDrivers.Location = new System.Drawing.Point(3, 16);
            this.tvDrivers.Name = "tvDrivers";
            this.tvDrivers.SelectedImageIndex = 0;
            this.tvDrivers.ShowLines = false;
            this.tvDrivers.Size = new System.Drawing.Size(291, 379);
            this.tvDrivers.TabIndex = 0;
            this.tvDrivers.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvDrivers_NodeMouseClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "MessageICO.ico");
            this.imageList1.Images.SetKeyName(1, "incomingMsg.ico");
            this.imageList1.Images.SetKeyName(2, "letter-48.png.ico");
            // 
            // gbxConvo
            // 
            this.gbxConvo.AutoSize = true;
            this.gbxConvo.Controls.Add(this.pnlConvo);
            this.gbxConvo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxConvo.Location = new System.Drawing.Point(0, 0);
            this.gbxConvo.Name = "gbxConvo";
            this.gbxConvo.Size = new System.Drawing.Size(370, 398);
            this.gbxConvo.TabIndex = 0;
            this.gbxConvo.TabStop = false;
            this.gbxConvo.Text = "Conversation";
            // 
            // pnlConvo
            // 
            this.pnlConvo.AutoSize = true;
            this.pnlConvo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConvo.Location = new System.Drawing.Point(3, 16);
            this.pnlConvo.Name = "pnlConvo";
            this.pnlConvo.Size = new System.Drawing.Size(364, 379);
            this.pnlConvo.TabIndex = 2;
            this.pnlConvo.MouseEnter += new System.EventHandler(this.pnlConvo_MouseEnter);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.dtpScheduleDate);
            this.panelTop.Controls.Add(this.lblDate);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(671, 38);
            this.panelTop.TabIndex = 0;
            // 
            // dtpScheduleDate
            // 
            this.dtpScheduleDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpScheduleDate.Location = new System.Drawing.Point(119, 9);
            this.dtpScheduleDate.Name = "dtpScheduleDate";
            this.dtpScheduleDate.Size = new System.Drawing.Size(98, 20);
            this.dtpScheduleDate.TabIndex = 1;
            this.dtpScheduleDate.ValueChanged += new System.EventHandler(this.dtpScheduleDate_ValueChanged);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(12, 13);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(101, 13);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "Conversation Date :";
            // 
            // FrmMessageConversation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 436);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panelTop);
            this.Name = "FrmMessageConversation";
            this.Text = "Messaging";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gbxDrivers.ResumeLayout(false);
            this.gbxConvo.ResumeLayout(false);
            this.gbxConvo.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox gbxDrivers;
        private System.Windows.Forms.TreeView tvDrivers;
        private System.Windows.Forms.GroupBox gbxConvo;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel pnlConvo;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.DateTimePicker dtpScheduleDate;
        private System.Windows.Forms.Label lblDate;
    }
}