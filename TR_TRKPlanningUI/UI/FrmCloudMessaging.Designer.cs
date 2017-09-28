namespace FM.TruckPlanning.UI
{
    partial class FrmCloudMessaging
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
            this.gbxJobInfo = new System.Windows.Forms.GroupBox();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.gbxJobInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxJobInfo
            // 
            this.gbxJobInfo.Controls.Add(this.treeView2);
            this.gbxJobInfo.Location = new System.Drawing.Point(469, 102);
            this.gbxJobInfo.Name = "gbxJobInfo";
            this.gbxJobInfo.Size = new System.Drawing.Size(200, 100);
            this.gbxJobInfo.TabIndex = 2;
            this.gbxJobInfo.TabStop = false;
            this.gbxJobInfo.Text = "Job Info";
            // 
            // treeView2
            // 
            this.treeView2.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView2.Location = new System.Drawing.Point(3, 16);
            this.treeView2.Name = "treeView2";
            this.treeView2.Size = new System.Drawing.Size(121, 81);
            this.treeView2.TabIndex = 0;
            // 
            // FrmCloudMessaging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 407);
            this.Controls.Add(this.gbxJobInfo);
            this.Name = "FrmCloudMessaging";
            this.Text = "Cloud Messaging";
            this.gbxJobInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxJobInfo;
        private System.Windows.Forms.TreeView treeView2;
    }
}