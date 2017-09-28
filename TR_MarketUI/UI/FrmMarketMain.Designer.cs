namespace FM.TransportMarket.UI
{
    partial class frmMarketMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMarketMain));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.quotationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quotationEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quotationReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sectorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chargeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tlsbQuotation = new System.Windows.Forms.ToolStripButton();
            this.packageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quotationToolStripMenuItem,
            this.reportToolStripMenuItem,
            this.sectorToolStripMenuItem,
            this.chargeToolStripMenuItem,
            this.packageToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(632, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // quotationToolStripMenuItem
            // 
            this.quotationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quotationEntryToolStripMenuItem});
            this.quotationToolStripMenuItem.Name = "quotationToolStripMenuItem";
            this.quotationToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.quotationToolStripMenuItem.Text = "&Quotation";
            // 
            // quotationEntryToolStripMenuItem
            // 
            this.quotationEntryToolStripMenuItem.Name = "quotationEntryToolStripMenuItem";
            this.quotationEntryToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.quotationEntryToolStripMenuItem.Text = "Quotation Entry";
            this.quotationEntryToolStripMenuItem.Click += new System.EventHandler(this.quotationEntryToolStripMenuItem_Click);
            // 
            // reportToolStripMenuItem
            // 
            this.reportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quotationReportToolStripMenuItem});
            this.reportToolStripMenuItem.Name = "reportToolStripMenuItem";
            this.reportToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.reportToolStripMenuItem.Text = "Report";
            // 
            // quotationReportToolStripMenuItem
            // 
            this.quotationReportToolStripMenuItem.Name = "quotationReportToolStripMenuItem";
            this.quotationReportToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.quotationReportToolStripMenuItem.Text = "Quotation Report";
            this.quotationReportToolStripMenuItem.Click += new System.EventHandler(this.quotationReportToolStripMenuItem_Click);
            // 
            // sectorToolStripMenuItem
            // 
            this.sectorToolStripMenuItem.Name = "sectorToolStripMenuItem";
            this.sectorToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.sectorToolStripMenuItem.Text = "Sector";
            this.sectorToolStripMenuItem.Visible = false;
            this.sectorToolStripMenuItem.Click += new System.EventHandler(this.sectorToolStripMenuItem_Click);
            // 
            // chargeToolStripMenuItem
            // 
            this.chargeToolStripMenuItem.Name = "chargeToolStripMenuItem";
            this.chargeToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.chargeToolStripMenuItem.Text = "Charge";
            this.chargeToolStripMenuItem.Click += new System.EventHandler(this.chargeToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 431);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(632, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Status";
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.DodgerBlue;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlsbQuotation});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(77, 407);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tlsbQuotation
            // 
            this.tlsbQuotation.Image = ((System.Drawing.Image)(resources.GetObject("tlsbQuotation.Image")));
            this.tlsbQuotation.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tlsbQuotation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsbQuotation.Name = "tlsbQuotation";
            this.tlsbQuotation.Size = new System.Drawing.Size(75, 66);
            this.tlsbQuotation.Text = "Quotation";
            this.tlsbQuotation.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tlsbQuotation.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // packageToolStripMenuItem
            // 
            this.packageToolStripMenuItem.Name = "packageToolStripMenuItem";
            this.packageToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.packageToolStripMenuItem.Text = "Package";
            this.packageToolStripMenuItem.Click += new System.EventHandler(this.packageToolStripMenuItem_Click);
            // 
            // frmMarketMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 453);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "frmMarketMain";
            this.Text = "Freight Master - Transport System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMarketMain_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMarketMain_FormClosed);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem quotationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quotationEntryToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tlsbQuotation;
        private System.Windows.Forms.ToolStripMenuItem reportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quotationReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sectorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chargeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem packageToolStripMenuItem;
    }
}



