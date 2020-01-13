using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UploadDHL
{
    public partial class Dialog : Form
    {
        public Dialog()
        {
            InitializeComponent();
        }

        private Matchup zMainForm;
        public Dialog(Form caller)
        {
            zMainForm = caller as Matchup;
            InitializeComponent();
            
        }

        public void PriceAssign()
        {
            XuMakeOK.Checked = true;
            XuReaclOil.Checked = true;
            XuNewPrice.Focus();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void XuAssignPrice_Click(object sender, EventArgs e)
        {
            decimal price;
            if (decimal.TryParse(XuNewPrice.Text, out price))
            {

               zMainForm.AssignPrice(price, XuReaclOil.Checked, XuMakeOK.Checked);
               Close();

            }
            if (XuMakeOK.Checked && !XuReaclOil.Checked)
            {
                zMainForm.AssignPrice( XuMakeOK.Checked );
                Close();
            }

           


        }

        private void Dialog_Shown(object sender, EventArgs e)
        {
            XuNewPrice.Text = "";
        }

        private void Dialog_Leave(object sender, EventArgs e)
        {
            Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void XuNewPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void XuReaclOil_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
