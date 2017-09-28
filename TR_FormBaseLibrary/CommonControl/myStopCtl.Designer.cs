namespace FormBaseLibrary.CommonControl
{
    partial class myStopCtl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cboCode = new System.Windows.Forms.ComboBox();
            this.tbxName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxAdr1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxAdr2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxAdr3 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxAdr4 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbxCity = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Code";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboCode
            // 
            this.cboCode.FormattingEnabled = true;
            this.cboCode.Location = new System.Drawing.Point(62, 2);
            this.cboCode.Name = "cboCode";
            this.cboCode.Size = new System.Drawing.Size(134, 21);
            this.cboCode.TabIndex = 1;
            this.cboCode.TextChanged += new System.EventHandler(this.cboCode_TextChanged);
            this.cboCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCode_KeyPress);
            // 
            // tbxName
            // 
            this.tbxName.Location = new System.Drawing.Point(62, 23);
            this.tbxName.Name = "tbxName";
            this.tbxName.ReadOnly = true;
            this.tbxName.Size = new System.Drawing.Size(291, 20);
            this.tbxName.TabIndex = 2;
            this.tbxName.TextChanged += new System.EventHandler(this.tbxName_TextChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(5, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Address1";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbxAdr1
            // 
            this.tbxAdr1.Location = new System.Drawing.Point(62, 43);
            this.tbxAdr1.Name = "tbxAdr1";
            this.tbxAdr1.ReadOnly = true;
            this.tbxAdr1.Size = new System.Drawing.Size(291, 20);
            this.tbxAdr1.TabIndex = 2;
            this.tbxAdr1.TextChanged += new System.EventHandler(this.tbxAdr1_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(5, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Address2";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbxAdr2
            // 
            this.tbxAdr2.Location = new System.Drawing.Point(62, 63);
            this.tbxAdr2.Name = "tbxAdr2";
            this.tbxAdr2.ReadOnly = true;
            this.tbxAdr2.Size = new System.Drawing.Size(291, 20);
            this.tbxAdr2.TabIndex = 2;
            this.tbxAdr2.TextChanged += new System.EventHandler(this.tbxAdr2_TextChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(5, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Address3";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbxAdr3
            // 
            this.tbxAdr3.Location = new System.Drawing.Point(62, 83);
            this.tbxAdr3.Name = "tbxAdr3";
            this.tbxAdr3.ReadOnly = true;
            this.tbxAdr3.Size = new System.Drawing.Size(291, 20);
            this.tbxAdr3.TabIndex = 2;
            this.tbxAdr3.TextChanged += new System.EventHandler(this.tbxAdr3_TextChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(5, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Address4";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbxAdr4
            // 
            this.tbxAdr4.Location = new System.Drawing.Point(62, 103);
            this.tbxAdr4.Name = "tbxAdr4";
            this.tbxAdr4.ReadOnly = true;
            this.tbxAdr4.Size = new System.Drawing.Size(291, 20);
            this.tbxAdr4.TabIndex = 2;
            this.tbxAdr4.TextChanged += new System.EventHandler(this.tbxAdr4_TextChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(5, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "City";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbxCity
            // 
            this.tbxCity.Location = new System.Drawing.Point(62, 123);
            this.tbxCity.Name = "tbxCity";
            this.tbxCity.ReadOnly = true;
            this.tbxCity.Size = new System.Drawing.Size(291, 20);
            this.tbxCity.TabIndex = 2;
            this.tbxCity.TextChanged += new System.EventHandler(this.tbxCity_TextChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(5, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Name";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // myStopCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbxCity);
            this.Controls.Add(this.tbxAdr4);
            this.Controls.Add(this.tbxAdr3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbxAdr2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbxAdr1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbxName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(356, 147);
            this.MinimumSize = new System.Drawing.Size(222, 90);
            this.Name = "myStopCtl";
            this.Size = new System.Drawing.Size(356, 147);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboCode;
        private System.Windows.Forms.TextBox tbxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxAdr1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxAdr2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxAdr3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbxAdr4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbxCity;
        private System.Windows.Forms.Label label7;
    }
}
