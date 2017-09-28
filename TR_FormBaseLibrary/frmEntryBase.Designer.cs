namespace TR_FormBaseLibrary
{
    partial class frmEntryBase
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnDeleteMaster = new System.Windows.Forms.Button();
            this.btnEditMaster = new System.Windows.Forms.Button();
            this.btnNewMaster = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbcEntry = new System.Windows.Forms.TabControl();
            this.tabPageSearch = new System.Windows.Forms.TabPage();
            this.pnlQuery = new System.Windows.Forms.Panel();
            this.btnQuery = new System.Windows.Forms.Button();
            this.tabPageMaster = new System.Windows.Forms.TabPage();
            this.pnlMaster = new System.Windows.Forms.Panel();
            this.bdsMaster = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox4.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.tbcEntry.SuspendLayout();
            this.tabPageSearch.SuspendLayout();
            this.pnlQuery.SuspendLayout();
            this.tabPageMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.groupBox4.Controls.Add(this.btnDeleteMaster);
            this.groupBox4.Controls.Add(this.btnEditMaster);
            this.groupBox4.Controls.Add(this.btnNewMaster);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(663, 44);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            // 
            // btnDeleteMaster
            // 
            this.btnDeleteMaster.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDeleteMaster.Location = new System.Drawing.Point(172, 13);
            this.btnDeleteMaster.Name = "btnDeleteMaster";
            this.btnDeleteMaster.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteMaster.TabIndex = 7;
            this.btnDeleteMaster.Text = "Delete";
            this.btnDeleteMaster.UseVisualStyleBackColor = false;
            this.btnDeleteMaster.Click += new System.EventHandler(this.btnDeleteMaster_Click);
            // 
            // btnEditMaster
            // 
            this.btnEditMaster.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnEditMaster.Location = new System.Drawing.Point(91, 13);
            this.btnEditMaster.Name = "btnEditMaster";
            this.btnEditMaster.Size = new System.Drawing.Size(75, 23);
            this.btnEditMaster.TabIndex = 6;
            this.btnEditMaster.Text = "Edit";
            this.btnEditMaster.UseVisualStyleBackColor = false;
            this.btnEditMaster.Click += new System.EventHandler(this.btnEditMaster_Click);
            // 
            // btnNewMaster
            // 
            this.btnNewMaster.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnNewMaster.Location = new System.Drawing.Point(10, 13);
            this.btnNewMaster.Name = "btnNewMaster";
            this.btnNewMaster.Size = new System.Drawing.Size(75, 23);
            this.btnNewMaster.TabIndex = 5;
            this.btnNewMaster.Text = "New";
            this.btnNewMaster.UseVisualStyleBackColor = false;
            this.btnNewMaster.Click += new System.EventHandler(this.btnNewMaster_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Menu;
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnPrint);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 435);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(663, 47);
            this.pnlBottom.TabIndex = 9;
            this.pnlBottom.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(167, 17);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(261, 17);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(91, 17);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(10, 17);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 24);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbcEntry
            // 
            this.tbcEntry.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tbcEntry.Controls.Add(this.tabPageSearch);
            this.tbcEntry.Controls.Add(this.tabPageMaster);
            this.tbcEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcEntry.Location = new System.Drawing.Point(0, 44);
            this.tbcEntry.Name = "tbcEntry";
            this.tbcEntry.SelectedIndex = 0;
            this.tbcEntry.Size = new System.Drawing.Size(663, 391);
            this.tbcEntry.TabIndex = 10;
            // 
            // tabPageSearch
            // 
            this.tabPageSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.tabPageSearch.Controls.Add(this.pnlQuery);
            this.tabPageSearch.Location = new System.Drawing.Point(4, 25);
            this.tabPageSearch.Name = "tabPageSearch";
            this.tabPageSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSearch.Size = new System.Drawing.Size(655, 362);
            this.tabPageSearch.TabIndex = 0;
            this.tabPageSearch.Text = "Search";
            // 
            // pnlQuery
            // 
            this.pnlQuery.Controls.Add(this.btnQuery);
            this.pnlQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQuery.Location = new System.Drawing.Point(3, 3);
            this.pnlQuery.Name = "pnlQuery";
            this.pnlQuery.Size = new System.Drawing.Size(649, 55);
            this.pnlQuery.TabIndex = 2;
            // 
            // btnQuery
            // 
            this.btnQuery.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnQuery.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnQuery.Location = new System.Drawing.Point(425, 22);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 1;
            this.btnQuery.Text = "Query";
            this.btnQuery.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnQuery.UseVisualStyleBackColor = false;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // tabPageMaster
            // 
            this.tabPageMaster.BackColor = System.Drawing.Color.LightBlue;
            this.tabPageMaster.Controls.Add(this.pnlMaster);
            this.tabPageMaster.Location = new System.Drawing.Point(4, 25);
            this.tabPageMaster.Name = "tabPageMaster";
            this.tabPageMaster.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMaster.Size = new System.Drawing.Size(655, 362);
            this.tabPageMaster.TabIndex = 1;
            this.tabPageMaster.Text = "Master";
            // 
            // pnlMaster
            // 
            this.pnlMaster.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.pnlMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMaster.Enabled = false;
            this.pnlMaster.Location = new System.Drawing.Point(3, 3);
            this.pnlMaster.Name = "pnlMaster";
            this.pnlMaster.Size = new System.Drawing.Size(649, 356);
            this.pnlMaster.TabIndex = 0;
            // 
            // frmEntryBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 482);
            this.Controls.Add(this.tbcEntry);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.groupBox4);
            this.Name = "frmEntryBase";
            this.Text = "Freight Master - Transport System ";
            this.Load += new System.EventHandler(this.frmEntryBase_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEntryBase_FormClosing);
            this.groupBox4.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.tbcEntry.ResumeLayout(false);
            this.tabPageSearch.ResumeLayout(false);
            this.pnlQuery.ResumeLayout(false);
            this.tabPageMaster.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bdsMaster)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.GroupBox groupBox4;
        protected System.Windows.Forms.Button btnDeleteMaster;
        protected System.Windows.Forms.Button btnEditMaster;
        protected System.Windows.Forms.Button btnNewMaster;
        protected System.Windows.Forms.Button btnCancel;
        protected System.Windows.Forms.Button btnSave;
        protected System.Windows.Forms.TabControl tbcEntry;
        protected System.Windows.Forms.Button btnQuery;
        protected System.Windows.Forms.TabPage tabPageSearch;
        protected System.Windows.Forms.GroupBox pnlBottom;
        protected System.Windows.Forms.Button btnPrint;
        protected System.Windows.Forms.Panel pnlQuery;
        protected System.Windows.Forms.TabPage tabPageMaster;
        protected System.Windows.Forms.Panel pnlMaster;
        protected System.Windows.Forms.BindingSource bdsMaster;
        protected System.Windows.Forms.Button btnClose;
    }
}

