using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormBaseLibrary.CommonControl
{
    public partial class IPLMessageBox : Form
    {
        Label message = new Label();
        Button b1 = new Button();
        Button b2 = new Button();

        public IPLMessageBox()
        {
            InitializeComponent();
        }

        public IPLMessageBox(string title, string body, string button1, string button2)
        {
            this.ClientSize = new System.Drawing.Size(390, 120);
            this.Text = title;
            this.ControlBox = false;
            this.MinimumSize = new System.Drawing.Size(390, 120);
            this.MaximumSize = new System.Drawing.Size(390, 120);

            b1.Location = new System.Drawing.Point(10, 50);
            b1.Size = new System.Drawing.Size(75, 23);
            b1.Text = button1;
            b1.BackColor = Control.DefaultBackColor;
            b1.Click +=b1_Click;

            b2.Location = new System.Drawing.Point(90, 50);
            b2.Size = new System.Drawing.Size(75, 23);
            b2.Text = button2;
            b2.BackColor = Control.DefaultBackColor;
            b2.Click += b2_Click;

            message.Location = new System.Drawing.Point(10, 10);
            message.Text = body;
            message.Font = Control.DefaultFont;
            message.AutoSize = true;

            this.BackColor = Color.White;
            this.ShowIcon = false;

            this.Controls.Add(b1);
            this.Controls.Add(b2);
            this.Controls.Add(message);
        }
        private void b1_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Yes;            
            this.Close();
        }
        private void b2_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
