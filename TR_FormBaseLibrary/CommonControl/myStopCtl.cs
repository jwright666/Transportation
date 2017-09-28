using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FM.TR_FMSystemDLL.BLL;

namespace FormBaseLibrary.CommonControl
{
    /// <summary>
    /// 2014-09-30 Zhou Kai optimized this user 
    /// control, so that it has no dead loop of event
    /// handlers.
    /// The key point to avoid dead loop of event 
    /// handlers is: 
    /// (1) do only one thing in one function, or say 
    /// implement only one logic in a function. 
    /// (2) make sure the data flow is only one way 
    /// flow. For example, one possible data flow is:
    /// user input --> control.Text property 
    ///-->public property representing the control.Text
    /// property.
    /// You can't then add another dataflow:
    /// such as set public property in order to set the 
    /// Text property of a control.
    /// In that way, you got a dead loop.
    /// In stead, you can add a function solely for
    /// setting the Text property of that control:
    /// public void SetTbxName(string name);
    /// 
    /// </summary>
    public partial class myStopCtl : UserControl
    {
        #region "Public properties"
        private Stop current;
        public Stop Current 
        { get {return current;}
            private set
            {
                current = value;
                if (current != null)
                {
                    cboCode.Text = current.Code;
                    tbxName.Text = current.Description;
                    tbxAdr1.Text = current.Address1;
                    tbxAdr2.Text = current.Address2;
                    tbxAdr3.Text = current.Address3;
                    tbxAdr4.Text = current.Address4;
                    tbxCity.Text = current.City;
                }
                else
                {
                    cboCode.Text = String.Empty;
                    tbxName.Text = String.Empty;
                    tbxAdr1.Text = String.Empty;
                    tbxAdr2.Text = String.Empty;
                    tbxAdr3.Text = String.Empty;
                    tbxAdr4.Text = String.Empty;
                    tbxCity.Text = String.Empty;
                }
            }
        }
        private List<Stop> lstAllStops;
        public List<Stop> LstAllStops
        {
            get { return lstAllStops; }
            set
            {
                lstAllStops = value;
                if (lstAllStops != null)
                {
                    BindingSource bds = new BindingSource { DataSource = lstAllStops };
                    cboCode.DataSource = bds;
                    cboCode.DisplayMember = "Code";
                }
            }
        }

        public void SetCode(string code)
        { cboCode.Text = code; }
        public void SetName(string description)
        { tbxName.Text = description; }
        public void SetAdr1(string adr1)
        { tbxAdr1.Text = adr1; }
        public void SetAdr2(string adr2)
        { tbxAdr2.Text = adr2; }
        public void SetAdr3(string adr3)
        { tbxAdr3.Text = adr3; }
        public void SetAdr4(string adr4)
        { tbxAdr4.Text = adr4; }
        public void SetCity(string city)
        { tbxCity.Text = city; }

        #endregion

        #region "Constructors"
        public myStopCtl()
        {
            InitializeComponent();

            current = null;
            lstAllStops = null;

        }

        #endregion

        #region "Event handlers"
        public event EventHandler OnCodeChanged;

        private void cboCode_TextChanged(object sender, EventArgs e)
        {
            // perform control consumer's logic
            if (OnCodeChanged != null)
            {
               OnCodeChanged(this, EventArgs.Empty);
            }
            else
            {
                // perform control's own default logic
                CodeChanged(sender, e);
            }
            
        }

        // the default logic to handle the code_changed event
        private void CodeChanged(object sender, EventArgs e)
        {
            if (cboCode.Text != String.Empty && 
                (Stop)cboCode.SelectedItem is Stop)
            {
                current = (Stop)cboCode.SelectedItem;
                Current = current;
            }
            else { current = null; }

        }

        // control logic
        private void tbxName_TextChanged(
            object sender, EventArgs e)
        {
            if (Current != null)
                Current.Description = tbxName.Text;
        }

        private void tbxAdr1_TextChanged(object sender, EventArgs e)
        {
            if (Current != null)
                Current.Address1 = tbxAdr1.Text;
        }

        private void tbxAdr2_TextChanged(object sender, EventArgs e)
        {
            if (Current != null)
                Current.Address2 = tbxAdr2.Text;
        }

        private void tbxAdr3_TextChanged(object sender, EventArgs e)
        {
            if (Current != null)
                Current.Address3 = tbxAdr3.Text;
        }

        private void tbxAdr4_TextChanged(object sender, EventArgs e)
        {
            if (Current != null)
                Current.Address4 = tbxAdr4.Text;
        }

        private void tbxCity_TextChanged(object sender, EventArgs e)
        {
            if (Current != null)
                Current.City = tbxCity.Text;
        }
        // end of control logic

        public virtual void AutoCompleteComboBox(object sender, KeyPressEventArgs e, bool blnLimitToList)
        {
            ComboBox cb = (ComboBox)sender;
            cb.DropDownStyle = ComboBoxStyle.DropDown;
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
                if (e.KeyChar == '\r')
                    return;

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
                e.Handled = blnLimitToList;
            }
        }

        private void cboCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                ComboBox cbo = (ComboBox)sender;
                if (e.KeyChar == '\n')
                    return;
                else
                {
                    cbo.DroppedDown = true;
                    AutoCompleteComboBox(sender, e, true);
                    //if (cbo.Text == "")
                    //    cbo.SelectedIndex = 0;
                }
            }
            catch (Exception ex) { MessageBox.Show("Data error: " + ex.Message); }
        }

        #endregion 

    }
}
