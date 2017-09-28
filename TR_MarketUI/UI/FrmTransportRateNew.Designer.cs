namespace FM.TransportMarket.UI
{
    partial class FrmTransportRateNew
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.grbNoOfLegs = new System.Windows.Forms.GroupBox();
            this.rdoRoundTrip = new System.Windows.Forms.RadioButton();
            this.rdoOneLeg = new System.Windows.Forms.RadioButton();
            this.grpStops = new System.Windows.Forms.GroupBox();
            this.cboEndStop_Sector = new System.Windows.Forms.ComboBox();
            this.cboStartStop_Sector = new System.Windows.Forms.ComboBox();
            this.txtEndStop = new System.Windows.Forms.TextBox();
            this.lblendStop = new System.Windows.Forms.Label();
            this.txtStartStop = new System.Windows.Forms.TextBox();
            this.lblStartStop = new System.Windows.Forms.Label();
            this.chkboxOverweight = new System.Windows.Forms.CheckBox();
            this.txtContainerCode = new System.Windows.Forms.TextBox();
            this.lblContainerCode = new System.Windows.Forms.Label();
            this.chkboxTruckMovement = new System.Windows.Forms.CheckBox();
            this.chkboxContainerMovement = new System.Windows.Forms.CheckBox();
            this.grpDistanceRelated = new System.Windows.Forms.GroupBox();
            this.rdbStopRelated = new System.Windows.Forms.RadioButton();
            this.rdbSectorRelated = new System.Windows.Forms.RadioButton();
            this.rdbNone = new System.Windows.Forms.RadioButton();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.lblCurrencyCode = new System.Windows.Forms.Label();
            this.cboCurrencyCode = new System.Windows.Forms.ComboBox();
            this.lblMinimumValue = new System.Windows.Forms.Label();
            this.txtMinimum = new System.Windows.Forms.TextBox();
            this.lblUOM = new System.Windows.Forms.Label();
            this.cboUOM = new System.Windows.Forms.ComboBox();
            this.lblChargeType = new System.Windows.Forms.Label();
            this.cboChargeType = new System.Windows.Forms.ComboBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblChargeCode = new System.Windows.Forms.Label();
            this.cboChargeCode = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblPriceBreak = new System.Windows.Forms.Label();
            this.dgvPriceBreaks = new System.Windows.Forms.DataGridView();
            this.seqNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isLumpSumDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lumpSumValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bdsPriceBreaks = new System.Windows.Forms.BindingSource(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.grbNoOfLegs.SuspendLayout();
            this.grpStops.SuspendLayout();
            this.grpDistanceRelated.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPriceBreaks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsPriceBreaks)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.panel1.Controls.Add(this.grbNoOfLegs);
            this.panel1.Controls.Add(this.grpStops);
            this.panel1.Controls.Add(this.chkboxOverweight);
            this.panel1.Controls.Add(this.txtContainerCode);
            this.panel1.Controls.Add(this.lblContainerCode);
            this.panel1.Controls.Add(this.chkboxTruckMovement);
            this.panel1.Controls.Add(this.chkboxContainerMovement);
            this.panel1.Controls.Add(this.grpDistanceRelated);
            this.panel1.Controls.Add(this.txtRemarks);
            this.panel1.Controls.Add(this.lblRemarks);
            this.panel1.Controls.Add(this.lblCurrencyCode);
            this.panel1.Controls.Add(this.cboCurrencyCode);
            this.panel1.Controls.Add(this.lblMinimumValue);
            this.panel1.Controls.Add(this.txtMinimum);
            this.panel1.Controls.Add(this.lblUOM);
            this.panel1.Controls.Add(this.cboUOM);
            this.panel1.Controls.Add(this.lblChargeType);
            this.panel1.Controls.Add(this.cboChargeType);
            this.panel1.Controls.Add(this.txtDescription);
            this.panel1.Controls.Add(this.lblDescription);
            this.panel1.Controls.Add(this.lblChargeCode);
            this.panel1.Controls.Add(this.cboChargeCode);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(646, 343);
            this.panel1.TabIndex = 0;
            // 
            // grbNoOfLegs
            // 
            this.grbNoOfLegs.Controls.Add(this.rdoRoundTrip);
            this.grbNoOfLegs.Controls.Add(this.rdoOneLeg);
            this.grbNoOfLegs.Location = new System.Drawing.Point(449, 149);
            this.grbNoOfLegs.Name = "grbNoOfLegs";
            this.grbNoOfLegs.Size = new System.Drawing.Size(192, 60);
            this.grbNoOfLegs.TabIndex = 37;
            this.grbNoOfLegs.TabStop = false;
            this.grbNoOfLegs.Text = "No of Legs";
            // 
            // rdoRoundTrip
            // 
            this.rdoRoundTrip.AutoSize = true;
            this.rdoRoundTrip.Checked = true;
            this.rdoRoundTrip.Location = new System.Drawing.Point(7, 40);
            this.rdoRoundTrip.Name = "rdoRoundTrip";
            this.rdoRoundTrip.Size = new System.Drawing.Size(149, 17);
            this.rdoRoundTrip.TabIndex = 1;
            this.rdoRoundTrip.TabStop = true;
            this.rdoRoundTrip.Text = "2 Legs (Round Trip Price) ";
            this.rdoRoundTrip.UseVisualStyleBackColor = true;
            // 
            // rdoOneLeg
            // 
            this.rdoOneLeg.AutoSize = true;
            this.rdoOneLeg.Location = new System.Drawing.Point(7, 16);
            this.rdoOneLeg.Name = "rdoOneLeg";
            this.rdoOneLeg.Size = new System.Drawing.Size(76, 17);
            this.rdoOneLeg.TabIndex = 0;
            this.rdoOneLeg.Text = "1 Leg Only";
            this.rdoOneLeg.UseVisualStyleBackColor = true;
            // 
            // grpStops
            // 
            this.grpStops.Controls.Add(this.cboEndStop_Sector);
            this.grpStops.Controls.Add(this.cboStartStop_Sector);
            this.grpStops.Controls.Add(this.txtEndStop);
            this.grpStops.Controls.Add(this.lblendStop);
            this.grpStops.Controls.Add(this.txtStartStop);
            this.grpStops.Controls.Add(this.lblStartStop);
            this.grpStops.Location = new System.Drawing.Point(111, 254);
            this.grpStops.Name = "grpStops";
            this.grpStops.Size = new System.Drawing.Size(530, 82);
            this.grpStops.TabIndex = 31;
            this.grpStops.TabStop = false;
            this.grpStops.Text = "Stop";
            this.grpStops.Visible = false;
            // 
            // cboEndStop_Sector
            // 
            this.cboEndStop_Sector.FormattingEnabled = true;
            this.cboEndStop_Sector.Location = new System.Drawing.Point(87, 44);
            this.cboEndStop_Sector.Name = "cboEndStop_Sector";
            this.cboEndStop_Sector.Size = new System.Drawing.Size(121, 21);
            this.cboEndStop_Sector.TabIndex = 7;
            this.cboEndStop_Sector.SelectedIndexChanged += new System.EventHandler(this.cboEndStop_SelectedValueChanged);
            // 
            // cboStartStop_Sector
            // 
            this.cboStartStop_Sector.FormattingEnabled = true;
            this.cboStartStop_Sector.Location = new System.Drawing.Point(87, 18);
            this.cboStartStop_Sector.Name = "cboStartStop_Sector";
            this.cboStartStop_Sector.Size = new System.Drawing.Size(121, 21);
            this.cboStartStop_Sector.TabIndex = 6;
            this.cboStartStop_Sector.SelectedIndexChanged += new System.EventHandler(this.cboStartStop_SelectedValueChanged);
            // 
            // txtEndStop
            // 
            this.txtEndStop.Location = new System.Drawing.Point(210, 44);
            this.txtEndStop.Name = "txtEndStop";
            this.txtEndStop.Size = new System.Drawing.Size(314, 20);
            this.txtEndStop.TabIndex = 5;
            // 
            // lblendStop
            // 
            this.lblendStop.Location = new System.Drawing.Point(4, 48);
            this.lblendStop.Name = "lblendStop";
            this.lblendStop.Size = new System.Drawing.Size(82, 13);
            this.lblendStop.TabIndex = 4;
            this.lblendStop.Text = "End Stop:";
            this.lblendStop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStartStop
            // 
            this.txtStartStop.Location = new System.Drawing.Point(210, 18);
            this.txtStartStop.Name = "txtStartStop";
            this.txtStartStop.Size = new System.Drawing.Size(314, 20);
            this.txtStartStop.TabIndex = 2;
            // 
            // lblStartStop
            // 
            this.lblStartStop.Location = new System.Drawing.Point(1, 22);
            this.lblStartStop.Name = "lblStartStop";
            this.lblStartStop.Size = new System.Drawing.Size(85, 13);
            this.lblStartStop.TabIndex = 1;
            this.lblStartStop.Text = "Start Stop:";
            this.lblStartStop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkboxOverweight
            // 
            this.chkboxOverweight.AutoSize = true;
            this.chkboxOverweight.Enabled = false;
            this.chkboxOverweight.Location = new System.Drawing.Point(316, 192);
            this.chkboxOverweight.Name = "chkboxOverweight";
            this.chkboxOverweight.Size = new System.Drawing.Size(80, 17);
            this.chkboxOverweight.TabIndex = 36;
            this.chkboxOverweight.Text = "Overweight";
            this.chkboxOverweight.UseVisualStyleBackColor = true;
            // 
            // txtContainerCode
            // 
            this.txtContainerCode.Enabled = false;
            this.txtContainerCode.Location = new System.Drawing.Point(111, 189);
            this.txtContainerCode.Name = "txtContainerCode";
            this.txtContainerCode.Size = new System.Drawing.Size(94, 20);
            this.txtContainerCode.TabIndex = 35;
            // 
            // lblContainerCode
            // 
            this.lblContainerCode.Location = new System.Drawing.Point(3, 192);
            this.lblContainerCode.Name = "lblContainerCode";
            this.lblContainerCode.Size = new System.Drawing.Size(102, 13);
            this.lblContainerCode.TabIndex = 34;
            this.lblContainerCode.Text = "Container Code :";
            this.lblContainerCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkboxTruckMovement
            // 
            this.chkboxTruckMovement.AutoSize = true;
            this.chkboxTruckMovement.Enabled = false;
            this.chkboxTruckMovement.Location = new System.Drawing.Point(316, 171);
            this.chkboxTruckMovement.Name = "chkboxTruckMovement";
            this.chkboxTruckMovement.Size = new System.Drawing.Size(107, 17);
            this.chkboxTruckMovement.TabIndex = 33;
            this.chkboxTruckMovement.Text = "Truck Movement";
            this.chkboxTruckMovement.UseVisualStyleBackColor = true;
            // 
            // chkboxContainerMovement
            // 
            this.chkboxContainerMovement.AutoSize = true;
            this.chkboxContainerMovement.Enabled = false;
            this.chkboxContainerMovement.Location = new System.Drawing.Point(111, 171);
            this.chkboxContainerMovement.Name = "chkboxContainerMovement";
            this.chkboxContainerMovement.Size = new System.Drawing.Size(124, 17);
            this.chkboxContainerMovement.TabIndex = 32;
            this.chkboxContainerMovement.Text = "Container Movement";
            this.chkboxContainerMovement.UseVisualStyleBackColor = true;
            this.chkboxContainerMovement.CheckedChanged += new System.EventHandler(this.chkboxContainerMovement_CheckedChanged);
            // 
            // grpDistanceRelated
            // 
            this.grpDistanceRelated.Controls.Add(this.rdbStopRelated);
            this.grpDistanceRelated.Controls.Add(this.rdbSectorRelated);
            this.grpDistanceRelated.Controls.Add(this.rdbNone);
            this.grpDistanceRelated.Enabled = false;
            this.grpDistanceRelated.Location = new System.Drawing.Point(111, 206);
            this.grpDistanceRelated.Name = "grpDistanceRelated";
            this.grpDistanceRelated.Size = new System.Drawing.Size(530, 42);
            this.grpDistanceRelated.TabIndex = 29;
            this.grpDistanceRelated.TabStop = false;
            // 
            // rdbStopRelated
            // 
            this.rdbStopRelated.AutoSize = true;
            this.rdbStopRelated.Location = new System.Drawing.Point(95, 19);
            this.rdbStopRelated.Name = "rdbStopRelated";
            this.rdbStopRelated.Size = new System.Drawing.Size(87, 17);
            this.rdbStopRelated.TabIndex = 2;
            this.rdbStopRelated.Text = "Stop Related";
            this.rdbStopRelated.UseVisualStyleBackColor = true;
            this.rdbStopRelated.CheckedChanged += new System.EventHandler(this.rdbStopRelated_Click);
            // 
            // rdbSectorRelated
            // 
            this.rdbSectorRelated.AutoSize = true;
            this.rdbSectorRelated.Location = new System.Drawing.Point(232, 19);
            this.rdbSectorRelated.Name = "rdbSectorRelated";
            this.rdbSectorRelated.Size = new System.Drawing.Size(96, 17);
            this.rdbSectorRelated.TabIndex = 1;
            this.rdbSectorRelated.Text = "Sector Related";
            this.rdbSectorRelated.UseVisualStyleBackColor = true;
            this.rdbSectorRelated.Visible = false;
            this.rdbSectorRelated.CheckedChanged += new System.EventHandler(this.rdbSectorRelated_Click);
            // 
            // rdbNone
            // 
            this.rdbNone.AutoSize = true;
            this.rdbNone.Checked = true;
            this.rdbNone.Location = new System.Drawing.Point(9, 19);
            this.rdbNone.Name = "rdbNone";
            this.rdbNone.Size = new System.Drawing.Size(51, 17);
            this.rdbNone.TabIndex = 0;
            this.rdbNone.TabStop = true;
            this.rdbNone.Text = "None";
            this.rdbNone.UseVisualStyleBackColor = true;
            this.rdbNone.CheckedChanged += new System.EventHandler(this.rdbNone_Click);
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(111, 149);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(318, 20);
            this.txtRemarks.TabIndex = 27;
            // 
            // lblRemarks
            // 
            this.lblRemarks.Location = new System.Drawing.Point(3, 152);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(102, 13);
            this.lblRemarks.TabIndex = 26;
            this.lblRemarks.Text = "Remarks :";
            this.lblRemarks.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.Location = new System.Drawing.Point(3, 129);
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Size = new System.Drawing.Size(102, 13);
            this.lblCurrencyCode.TabIndex = 25;
            this.lblCurrencyCode.Text = "Currency Code :";
            this.lblCurrencyCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboCurrencyCode
            // 
            this.cboCurrencyCode.AllowDrop = true;
            this.cboCurrencyCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCurrencyCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCurrencyCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.cboCurrencyCode.FormattingEnabled = true;
            this.cboCurrencyCode.Location = new System.Drawing.Point(111, 126);
            this.cboCurrencyCode.Name = "cboCurrencyCode";
            this.cboCurrencyCode.Size = new System.Drawing.Size(94, 21);
            this.cboCurrencyCode.TabIndex = 24;
            this.cboCurrencyCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboChargeCode_KeyPress);
            // 
            // lblMinimumValue
            // 
            this.lblMinimumValue.Location = new System.Drawing.Point(3, 107);
            this.lblMinimumValue.Name = "lblMinimumValue";
            this.lblMinimumValue.Size = new System.Drawing.Size(102, 13);
            this.lblMinimumValue.TabIndex = 23;
            this.lblMinimumValue.Text = "Minimum Value :";
            this.lblMinimumValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMinimum
            // 
            this.txtMinimum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.txtMinimum.Location = new System.Drawing.Point(111, 104);
            this.txtMinimum.MaxLength = 15;
            this.txtMinimum.Name = "txtMinimum";
            this.txtMinimum.Size = new System.Drawing.Size(94, 20);
            this.txtMinimum.TabIndex = 22;
            this.txtMinimum.Text = "0";
            this.txtMinimum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMinimum_KeyPress);
            // 
            // lblUOM
            // 
            this.lblUOM.Location = new System.Drawing.Point(3, 85);
            this.lblUOM.Name = "lblUOM";
            this.lblUOM.Size = new System.Drawing.Size(102, 13);
            this.lblUOM.TabIndex = 21;
            this.lblUOM.Text = "UOM :";
            this.lblUOM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboUOM
            // 
            this.cboUOM.AllowDrop = true;
            this.cboUOM.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboUOM.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboUOM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.cboUOM.FormattingEnabled = true;
            this.cboUOM.Location = new System.Drawing.Point(111, 81);
            this.cboUOM.Name = "cboUOM";
            this.cboUOM.Size = new System.Drawing.Size(94, 21);
            this.cboUOM.TabIndex = 20;
            this.cboUOM.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboChargeCode_KeyPress);
            // 
            // lblChargeType
            // 
            this.lblChargeType.Location = new System.Drawing.Point(3, 60);
            this.lblChargeType.Name = "lblChargeType";
            this.lblChargeType.Size = new System.Drawing.Size(102, 13);
            this.lblChargeType.TabIndex = 19;
            this.lblChargeType.Text = "Charge Type :";
            this.lblChargeType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboChargeType
            // 
            this.cboChargeType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboChargeType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboChargeType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.cboChargeType.FormattingEnabled = true;
            this.cboChargeType.Location = new System.Drawing.Point(111, 57);
            this.cboChargeType.Name = "cboChargeType";
            this.cboChargeType.Size = new System.Drawing.Size(60, 21);
            this.cboChargeType.TabIndex = 18;
            this.cboChargeType.Text = "R";
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.txtDescription.Location = new System.Drawing.Point(111, 35);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(318, 20);
            this.txtDescription.TabIndex = 17;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(3, 38);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(102, 13);
            this.lblDescription.TabIndex = 16;
            this.lblDescription.Text = "Description :";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblChargeCode
            // 
            this.lblChargeCode.Location = new System.Drawing.Point(3, 15);
            this.lblChargeCode.Name = "lblChargeCode";
            this.lblChargeCode.Size = new System.Drawing.Size(102, 13);
            this.lblChargeCode.TabIndex = 15;
            this.lblChargeCode.Text = "Charge Code :";
            this.lblChargeCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboChargeCode
            // 
            this.cboChargeCode.AllowDrop = true;
            this.cboChargeCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboChargeCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboChargeCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.cboChargeCode.FormattingEnabled = true;
            this.cboChargeCode.Location = new System.Drawing.Point(111, 12);
            this.cboChargeCode.Name = "cboChargeCode";
            this.cboChargeCode.Size = new System.Drawing.Size(124, 21);
            this.cboChargeCode.TabIndex = 14;
            this.cboChargeCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboChargeCode_KeyPress);
            this.cboChargeCode.SelectedValueChanged += new System.EventHandler(this.cboChargeCode_SelectedValueChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Teal;
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Controls.Add(this.lblPriceBreak);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.ForeColor = System.Drawing.Color.Yellow;
            this.panel2.Location = new System.Drawing.Point(0, 343);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(646, 30);
            this.panel2.TabIndex = 2;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(84, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAdd.ForeColor = System.Drawing.Color.Black;
            this.btnAdd.Location = new System.Drawing.Point(3, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblPriceBreak
            // 
            this.lblPriceBreak.AutoSize = true;
            this.lblPriceBreak.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPriceBreak.Location = new System.Drawing.Point(339, 6);
            this.lblPriceBreak.Name = "lblPriceBreak";
            this.lblPriceBreak.Size = new System.Drawing.Size(100, 17);
            this.lblPriceBreak.TabIndex = 0;
            this.lblPriceBreak.Text = "Price Breaks";
            // 
            // dgvPriceBreaks
            // 
            this.dgvPriceBreaks.AllowUserToAddRows = false;
            this.dgvPriceBreaks.AllowUserToDeleteRows = false;
            this.dgvPriceBreaks.AutoGenerateColumns = false;
            this.dgvPriceBreaks.BackgroundColor = System.Drawing.Color.DodgerBlue;
            this.dgvPriceBreaks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPriceBreaks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.seqNoDataGridViewTextBoxColumn,
            this.endDataGridViewTextBoxColumn,
            this.isLumpSumDataGridViewCheckBoxColumn,
            this.lumpSumValueDataGridViewTextBoxColumn,
            this.rateDataGridViewTextBoxColumn,
            this.remarksDataGridViewTextBoxColumn});
            this.dgvPriceBreaks.DataSource = this.bdsPriceBreaks;
            this.dgvPriceBreaks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPriceBreaks.Location = new System.Drawing.Point(0, 373);
            this.dgvPriceBreaks.Name = "dgvPriceBreaks";
            this.dgvPriceBreaks.ReadOnly = true;
            this.dgvPriceBreaks.Size = new System.Drawing.Size(646, 142);
            this.dgvPriceBreaks.TabIndex = 3;
            // 
            // seqNoDataGridViewTextBoxColumn
            // 
            this.seqNoDataGridViewTextBoxColumn.DataPropertyName = "Seq_No";
            this.seqNoDataGridViewTextBoxColumn.HeaderText = "No";
            this.seqNoDataGridViewTextBoxColumn.Name = "seqNoDataGridViewTextBoxColumn";
            this.seqNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.seqNoDataGridViewTextBoxColumn.Width = 50;
            // 
            // endDataGridViewTextBoxColumn
            // 
            this.endDataGridViewTextBoxColumn.DataPropertyName = "End";
            this.endDataGridViewTextBoxColumn.HeaderText = "End";
            this.endDataGridViewTextBoxColumn.Name = "endDataGridViewTextBoxColumn";
            this.endDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isLumpSumDataGridViewCheckBoxColumn
            // 
            this.isLumpSumDataGridViewCheckBoxColumn.DataPropertyName = "IsLumpSum";
            this.isLumpSumDataGridViewCheckBoxColumn.HeaderText = "IsLumpSum";
            this.isLumpSumDataGridViewCheckBoxColumn.Name = "isLumpSumDataGridViewCheckBoxColumn";
            this.isLumpSumDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // lumpSumValueDataGridViewTextBoxColumn
            // 
            this.lumpSumValueDataGridViewTextBoxColumn.DataPropertyName = "LumpSumValue";
            this.lumpSumValueDataGridViewTextBoxColumn.HeaderText = "LumpSumValue";
            this.lumpSumValueDataGridViewTextBoxColumn.Name = "lumpSumValueDataGridViewTextBoxColumn";
            this.lumpSumValueDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // rateDataGridViewTextBoxColumn
            // 
            this.rateDataGridViewTextBoxColumn.DataPropertyName = "Rate";
            this.rateDataGridViewTextBoxColumn.HeaderText = "Rate";
            this.rateDataGridViewTextBoxColumn.Name = "rateDataGridViewTextBoxColumn";
            this.rateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            this.remarksDataGridViewTextBoxColumn.ReadOnly = true;
            this.remarksDataGridViewTextBoxColumn.Width = 200;
            // 
            // bdsPriceBreaks
            // 
            this.bdsPriceBreaks.DataSource = typeof(FM.TransportMarket.BLL.PriceBreaks);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 515);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(646, 30);
            this.panel3.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(342, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 24);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(141, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 24);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FrmTransportRateNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.ClientSize = new System.Drawing.Size(646, 545);
            this.Controls.Add(this.dgvPriceBreaks);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FrmTransportRateNew";
            this.Text = "New Transport Rate & Price Breaks";
            this.Load += new System.EventHandler(this.FrmTransportRateNew_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grbNoOfLegs.ResumeLayout(false);
            this.grbNoOfLegs.PerformLayout();
            this.grpStops.ResumeLayout(false);
            this.grpStops.PerformLayout();
            this.grpDistanceRelated.ResumeLayout(false);
            this.grpDistanceRelated.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPriceBreaks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsPriceBreaks)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.Label lblCurrencyCode;
        private System.Windows.Forms.ComboBox cboCurrencyCode;
        private System.Windows.Forms.Label lblMinimumValue;
        private System.Windows.Forms.TextBox txtMinimum;
        private System.Windows.Forms.Label lblUOM;
        private System.Windows.Forms.ComboBox cboUOM;
        private System.Windows.Forms.Label lblChargeType;
        private System.Windows.Forms.ComboBox cboChargeType;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblChargeCode;
        private System.Windows.Forms.ComboBox cboChargeCode;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblPriceBreak;
        private System.Windows.Forms.DataGridView dgvPriceBreaks;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.BindingSource bdsPriceBreaks;
        private System.Windows.Forms.DataGridViewTextBoxColumn seqNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn endDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isLumpSumDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lumpSumValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.CheckBox chkboxContainerMovement;
        private System.Windows.Forms.CheckBox chkboxTruckMovement;
        private System.Windows.Forms.CheckBox chkboxOverweight;
        private System.Windows.Forms.TextBox txtContainerCode;
        private System.Windows.Forms.Label lblContainerCode;
        private System.Windows.Forms.GroupBox grpStops;
        private System.Windows.Forms.TextBox txtEndStop;
        private System.Windows.Forms.Label lblendStop;
        private System.Windows.Forms.TextBox txtStartStop;
        private System.Windows.Forms.Label lblStartStop;
        private System.Windows.Forms.GroupBox grpDistanceRelated;
        private System.Windows.Forms.RadioButton rdbStopRelated;
        private System.Windows.Forms.RadioButton rdbSectorRelated;
        private System.Windows.Forms.RadioButton rdbNone;
        private System.Windows.Forms.GroupBox grbNoOfLegs;
        private System.Windows.Forms.RadioButton rdoRoundTrip;
        private System.Windows.Forms.RadioButton rdoOneLeg;
        private System.Windows.Forms.ComboBox cboStartStop_Sector;
        private System.Windows.Forms.ComboBox cboEndStop_Sector;

    }
}