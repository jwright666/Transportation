namespace FM.TransportMarket.UI
{
    partial class FrmPriceBreakNew
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
            this.lblEnd = new System.Windows.Forms.Label();
            this.txtEnd = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRate = new System.Windows.Forms.TextBox();
            this.txtLumpSumValue = new System.Windows.Forms.TextBox();
            this.rdbLumpSum = new System.Windows.Forms.RadioButton();
            this.rdbRate = new System.Windows.Forms.RadioButton();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Location = new System.Drawing.Point(54, 15);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(32, 13);
            this.lblEnd.TabIndex = 3;
            this.lblEnd.Text = "End :";
            // 
            // txtEnd
            // 
            this.txtEnd.Location = new System.Drawing.Point(92, 12);
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.Size = new System.Drawing.Size(100, 20);
            this.txtEnd.TabIndex = 2;
            this.txtEnd.Text = "999";
            this.txtEnd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEnd_KeyPress);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtRate);
            this.groupBox1.Controls.Add(this.txtLumpSumValue);
            this.groupBox1.Controls.Add(this.rdbLumpSum);
            this.groupBox1.Controls.Add(this.rdbRate);
            this.groupBox1.Location = new System.Drawing.Point(92, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(286, 84);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // txtRate
            // 
            this.txtRate.Location = new System.Drawing.Point(87, 14);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new System.Drawing.Size(115, 20);
            this.txtRate.TabIndex = 4;
            this.txtRate.Text = "0";
            this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEnd_KeyPress);
            // 
            // txtLumpSumValue
            // 
            this.txtLumpSumValue.Location = new System.Drawing.Point(87, 52);
            this.txtLumpSumValue.Name = "txtLumpSumValue";
            this.txtLumpSumValue.Size = new System.Drawing.Size(115, 20);
            this.txtLumpSumValue.TabIndex = 3;
            this.txtLumpSumValue.Text = "0";
            this.txtLumpSumValue.Visible = false;
            this.txtLumpSumValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEnd_KeyPress);
            // 
            // rdbLumpSum
            // 
            this.rdbLumpSum.AutoSize = true;
            this.rdbLumpSum.Location = new System.Drawing.Point(6, 53);
            this.rdbLumpSum.Name = "rdbLumpSum";
            this.rdbLumpSum.Size = new System.Drawing.Size(75, 17);
            this.rdbLumpSum.TabIndex = 0;
            this.rdbLumpSum.Text = "Lump Sum";
            this.rdbLumpSum.UseVisualStyleBackColor = true;
            this.rdbLumpSum.CheckedChanged += new System.EventHandler(this.rdbLumpSum_CheckedChanged);
            // 
            // rdbRate
            // 
            this.rdbRate.AutoSize = true;
            this.rdbRate.Checked = true;
            this.rdbRate.Location = new System.Drawing.Point(6, 15);
            this.rdbRate.Name = "rdbRate";
            this.rdbRate.Size = new System.Drawing.Size(48, 17);
            this.rdbRate.TabIndex = 1;
            this.rdbRate.TabStop = true;
            this.rdbRate.Text = "Rate";
            this.rdbRate.UseVisualStyleBackColor = true;
            // 
            // lblRemarks
            // 
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Location = new System.Drawing.Point(31, 131);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(55, 13);
            this.lblRemarks.TabIndex = 6;
            this.lblRemarks.Text = "Remarks :";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(92, 128);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(286, 20);
            this.txtRemarks.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 172);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(403, 43);
            this.panel1.TabIndex = 7;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(219, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(92, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FrmPriceBreakNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(403, 215);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblRemarks);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblEnd);
            this.Controls.Add(this.txtEnd);
            this.Name = "FrmPriceBreakNew";
            this.Text = "New Price Break";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.TextBox txtEnd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtRate;
        private System.Windows.Forms.TextBox txtLumpSumValue;
        private System.Windows.Forms.RadioButton rdbRate;
        private System.Windows.Forms.RadioButton rdbLumpSum;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
    }
}