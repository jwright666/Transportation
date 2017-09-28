namespace FM.TransportMarket.UI
{
    partial class FrmAddTruckMovementUOM
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cboUOMType = new System.Windows.Forms.ComboBox();
            this.lblUOMType = new System.Windows.Forms.Label();
            this.txtUOMDesc = new System.Windows.Forms.TextBox();
            this.lblUOMDesc = new System.Windows.Forms.Label();
            this.cboUOMCode = new System.Windows.Forms.ComboBox();
            this.lblUOMCode = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.cboUOMType);
            this.panel1.Controls.Add(this.lblUOMType);
            this.panel1.Controls.Add(this.txtUOMDesc);
            this.panel1.Controls.Add(this.lblUOMDesc);
            this.panel1.Controls.Add(this.cboUOMCode);
            this.panel1.Controls.Add(this.lblUOMCode);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(316, 106);
            this.panel1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(161, 76);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(80, 76);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cboUOMType
            // 
            this.cboUOMType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.cboUOMType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUOMType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboUOMType.FormattingEnabled = true;
            this.cboUOMType.Location = new System.Drawing.Point(80, 51);
            this.cboUOMType.Name = "cboUOMType";
            this.cboUOMType.Size = new System.Drawing.Size(102, 21);
            this.cboUOMType.TabIndex = 5;
            // 
            // lblUOMType
            // 
            this.lblUOMType.AutoSize = true;
            this.lblUOMType.Location = new System.Drawing.Point(8, 55);
            this.lblUOMType.Name = "lblUOMType";
            this.lblUOMType.Size = new System.Drawing.Size(65, 13);
            this.lblUOMType.TabIndex = 4;
            this.lblUOMType.Text = "UOM Type :";
            // 
            // txtUOMDesc
            // 
            this.txtUOMDesc.Location = new System.Drawing.Point(80, 30);
            this.txtUOMDesc.Name = "txtUOMDesc";
            this.txtUOMDesc.Size = new System.Drawing.Size(228, 20);
            this.txtUOMDesc.TabIndex = 3;
            // 
            // lblUOMDesc
            // 
            this.lblUOMDesc.AutoSize = true;
            this.lblUOMDesc.Location = new System.Drawing.Point(8, 34);
            this.lblUOMDesc.Name = "lblUOMDesc";
            this.lblUOMDesc.Size = new System.Drawing.Size(66, 13);
            this.lblUOMDesc.TabIndex = 2;
            this.lblUOMDesc.Text = "Description :";
            // 
            // cboUOMCode
            // 
            this.cboUOMCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.cboUOMCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUOMCode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboUOMCode.FormattingEnabled = true;
            this.cboUOMCode.Location = new System.Drawing.Point(80, 8);
            this.cboUOMCode.Name = "cboUOMCode";
            this.cboUOMCode.Size = new System.Drawing.Size(102, 21);
            this.cboUOMCode.TabIndex = 1;
            this.cboUOMCode.SelectedIndexChanged += new System.EventHandler(this.cboUOMCode_SelectedIndexChanged);
            // 
            // lblUOMCode
            // 
            this.lblUOMCode.AutoSize = true;
            this.lblUOMCode.Location = new System.Drawing.Point(8, 12);
            this.lblUOMCode.Name = "lblUOMCode";
            this.lblUOMCode.Size = new System.Drawing.Size(66, 13);
            this.lblUOMCode.TabIndex = 0;
            this.lblUOMCode.Text = "UOM Code :";
            // 
            // FrmAddTruckMovementUOM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 106);
            this.Controls.Add(this.panel1);
            this.MaximumSize = new System.Drawing.Size(332, 144);
            this.MinimumSize = new System.Drawing.Size(332, 144);
            this.Name = "FrmAddTruckMovementUOM";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Truck Movement UOM";
            this.Shown += new System.EventHandler(this.FrmAddTruckMovementUOM_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtUOMDesc;
        private System.Windows.Forms.Label lblUOMDesc;
        private System.Windows.Forms.ComboBox cboUOMCode;
        private System.Windows.Forms.Label lblUOMCode;
        private System.Windows.Forms.ComboBox cboUOMType;
        private System.Windows.Forms.Label lblUOMType;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
    }
}