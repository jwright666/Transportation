using FM.TR_TRKPlanDLL.BLL;
namespace FM.TR_TRKPlanningUI.UI
{
    partial class FrmAutoCreateRoute
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gbxListOfDestination = new System.Windows.Forms.GroupBox();
            this.dgvAssignments = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDOWN = new System.Windows.Forms.Button();
            this.btnUP = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.bdsDestinationRoutes = new System.Windows.Forms.BindingSource(this.components);
            this.priorityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jobTripDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbxListOfDestination.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssignments)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDestinationRoutes)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxListOfDestination
            // 
            this.gbxListOfDestination.Controls.Add(this.dgvAssignments);
            this.gbxListOfDestination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxListOfDestination.Location = new System.Drawing.Point(0, 0);
            this.gbxListOfDestination.Name = "gbxListOfDestination";
            this.gbxListOfDestination.Size = new System.Drawing.Size(488, 213);
            this.gbxListOfDestination.TabIndex = 30;
            this.gbxListOfDestination.TabStop = false;
            this.gbxListOfDestination.Text = "List of Job Trips";
            // 
            // dgvAssignments
            // 
            this.dgvAssignments.AllowDrop = true;
            this.dgvAssignments.AllowUserToAddRows = false;
            this.dgvAssignments.AllowUserToOrderColumns = true;
            this.dgvAssignments.AutoGenerateColumns = false;
            this.dgvAssignments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAssignments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAssignments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAssignments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.priorityDataGridViewTextBoxColumn,
            this.jobTripDataGridViewTextBoxColumn});
            this.dgvAssignments.DataSource = this.bdsDestinationRoutes;
            this.dgvAssignments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAssignments.Location = new System.Drawing.Point(3, 16);
            this.dgvAssignments.Name = "dgvAssignments";
            this.dgvAssignments.ReadOnly = true;
            this.dgvAssignments.RowHeadersVisible = false;
            this.dgvAssignments.RowHeadersWidth = 20;
            this.dgvAssignments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAssignments.Size = new System.Drawing.Size(482, 194);
            this.dgvAssignments.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDOWN);
            this.panel1.Controls.Add(this.btnUP);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(488, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(59, 213);
            this.panel1.TabIndex = 32;
            // 
            // btnDOWN
            // 
            this.btnDOWN.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDOWN.Location = new System.Drawing.Point(3, 104);
            this.btnDOWN.Name = "btnDOWN";
            this.btnDOWN.Size = new System.Drawing.Size(51, 39);
            this.btnDOWN.TabIndex = 30;
            this.btnDOWN.Text = "▼";
            this.btnDOWN.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDOWN.UseVisualStyleBackColor = true;
            this.btnDOWN.Click += new System.EventHandler(this.btnDOWN_Click);
            // 
            // btnUP
            // 
            this.btnUP.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUP.Location = new System.Drawing.Point(4, 61);
            this.btnUP.Name = "btnUP";
            this.btnUP.Size = new System.Drawing.Size(51, 39);
            this.btnUP.TabIndex = 29;
            this.btnUP.Text = "▲";
            this.btnUP.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnUP.UseVisualStyleBackColor = true;
            this.btnUP.Click += new System.EventHandler(this.btnUP_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 213);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(547, 30);
            this.panel2.TabIndex = 33;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(265, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(184, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // bdsDestinationRoutes
            // 
            this.bdsDestinationRoutes.DataSource = typeof(DestinationRoute);
            // 
            // priorityDataGridViewTextBoxColumn
            // 
            this.priorityDataGridViewTextBoxColumn.DataPropertyName = "priority";
            this.priorityDataGridViewTextBoxColumn.HeaderText = "Priority";
            this.priorityDataGridViewTextBoxColumn.Name = "priorityDataGridViewTextBoxColumn";
            this.priorityDataGridViewTextBoxColumn.ReadOnly = true;
            this.priorityDataGridViewTextBoxColumn.Width = 67;
            // 
            // jobTripDataGridViewTextBoxColumn
            // 
            this.jobTripDataGridViewTextBoxColumn.DataPropertyName = "routeDescription";
            this.jobTripDataGridViewTextBoxColumn.HeaderText = "Job Trip Destinations";
            this.jobTripDataGridViewTextBoxColumn.Name = "jobTripDataGridViewTextBoxColumn";
            this.jobTripDataGridViewTextBoxColumn.ReadOnly = true;
            this.jobTripDataGridViewTextBoxColumn.Width = 124;
            // 
            // FrmAutoCreateRoute
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(547, 243);
            this.Controls.Add(this.gbxListOfDestination);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.MaximumSize = new System.Drawing.Size(563, 282);
            this.MinimumSize = new System.Drawing.Size(441, 282);
            this.Name = "FrmAutoCreateRoute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auto Create Route";
            this.Load += new System.EventHandler(this.FrmAutoCreateRoute_Load);
            this.gbxListOfDestination.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssignments)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bdsDestinationRoutes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxListOfDestination;
        public System.Windows.Forms.DataGridView dgvAssignments;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDOWN;
        private System.Windows.Forms.Button btnUP;
        private System.Windows.Forms.BindingSource bdsDestinationRoutes;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DataGridViewTextBoxColumn priorityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jobTripDataGridViewTextBoxColumn;
    }
}