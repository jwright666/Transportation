using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace FormBaseLibrary.CommonControl
{
    public partial class IPLComboBox : ComboBox
    {   
        public String[] DisplayColumns { get; set; }
        public IPLComboBox()
        {
            InitializeComponent();
            this.DrawItem += new DrawItemEventHandler(TwoColCombo_DrawItem);
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.KeyPress += new KeyPressEventHandler(ComboBox_KeyPress);
        }

        private void ComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                AutoComplete_DataSource(sender, e);
            }
            catch { }
        }    

        void TwoColCombo_DrawItem(object sender, DrawItemEventArgs args)
        {
            args.DrawBackground();
            //DataRowView drv = (DataRowView)(((ComboBox)sender).Items[args.Index]);
            //string id = string.Empty;
            //string name = string.Empty;
            var obj = (((ComboBox)sender).Items[args.Index]);
            Rectangle rect = args.Bounds;//for first column
            int indx = 0;
            int colCount = (new List<string>(DisplayColumns)).Count;
            foreach (string col in DisplayColumns)
            {
                string colValue = obj.GetType().GetProperty(col).GetValue(obj, null).ToString();
                using (SolidBrush sb = new SolidBrush(args.ForeColor))
                {
                    if (indx == 0)
                    {   //assume the first column is Code, usually codes are 10-12 characters
                        rect.Width = colCount == 2 ? (args.Bounds.Width/2) -75 : args.Bounds.Width / colCount;
                        
                        args.Graphics.DrawString(colValue, args.Font, sb, rect);
                        args.Graphics.DrawLine(new Pen(Color.Black), rect.Right, 0, rect.Right, rect.Bottom);
                    }
                    else
                    {
                        if (new List<string>(DisplayColumns).Count > 2)
                        {
                            Rectangle rect2 = args.Bounds;
                            rect2.Width = (args.Bounds.Width - rect.Width) / 2;
                            rect2.X = rect.Right;
                            if (indx == 1)
                            {
                                args.Graphics.DrawString(colValue, args.Font, sb, rect2);
                                args.Graphics.DrawLine(new Pen(Color.Black), rect2.Right, 0, rect2.Right, rect2.Bottom);
                            }
                            else
                            {
                                Rectangle r3 = args.Bounds;
                                r3.X = rect2.Right;
                                args.Graphics.DrawString(colValue, args.Font, sb, r3);
                            }
                        }
                        else
                        {
                            Rectangle rect2 = args.Bounds;
                            rect2.X = rect.Width;
                            args.Graphics.DrawString(colValue, args.Font, sb, rect2);
                        }
                    }
                }
                indx++;
            }
            //display horizontal line
            args.Graphics.DrawLine(new Pen(Color.Black), new Point(args.Bounds.Left, args.Bounds.Bottom - 1),
                                       new Point(args.Bounds.Right, args.Bounds.Bottom - 1));
        }
        void AutoComplete_DataSource(object sender, KeyPressEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            cb.DroppedDown = true;
            string strFindStr = "";
            if (e.KeyChar == (char)8)
            {
                if (cb.SelectionStart <= 1)
                {
                    cb.Text = "";
                    return;
                }
                if (cb.SelectionLength == 0)
                    strFindStr = cb.Text.Substring(0, cb.Text.Length - 1);
                else
                    strFindStr = cb.Text.Substring(0, cb.SelectionStart - 1);
            }
            else
            {
                if (cb.SelectionLength == 0)
                    strFindStr = cb.Text + e.KeyChar;
                else
                    strFindStr = cb.Text.Substring(0, cb.SelectionStart) + e.KeyChar;
            }

            int intIdx = -1;
            // Search the string in the ComboBox list. 
            intIdx = cb.FindString(strFindStr);
            if (intIdx != -1)
            {
                cb.SelectedText = "";
                cb.SelectedIndex = intIdx;
                cb.SelectionStart = strFindStr.Length;
                cb.SelectionLength = cb.Text.Length;
                e.Handled = true;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
