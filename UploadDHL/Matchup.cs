using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UploadDHL.GetForwarderId;

namespace UploadDHL
{
    public partial class Matchup : Form
    {

        private List<XMLRecord> zFormBill;
        private List<ForwObj> zFromForw;
        private List<XMLRecord> zFilterFormBill;
        private List<ForwObj> zFilterFromForw;
        private XMLRecord zSelectedRec = null;
        public Matchup()
        {
            InitializeComponent();
        }

        private void Matchup_Load(object sender, EventArgs e)
        {

        }

        public void InitData(List<XMLRecord> formBill, List<ForwObj> fromForw)
        {
            zFormBill = formBill;
            zFromForw = fromForw;
        }
        public void ShowData()
        {
             zFilterFormBill = zFormBill.Where(x => x.ForwarderObj == null).ToList();
            var listint = zFormBill.Where(x => x.ForwarderObj != null).Select(x => x.ForwarderObj.Id).ToList();
             zFilterFromForw = zFromForw.Where(x => listint.All(y => y != x.Id)).ToList();
            XuMissingCalculation.DataSource = zFilterFormBill;
            XuForwPart.DataSource = zFilterFromForw;
          

        }

        private void XuMissingCalculation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            var row=XuMissingCalculation.Rows[e.RowIndex];

           zSelectedRec = zFilterFormBill.FirstOrDefault(x => x.Awb == row.Cells[0].Value.ToString());

            XuForwPart.DataSource= zFilterFromForw.Where(x => x.Zip == zSelectedRec.Zip ).ToList();

        }

        private void XuForwPart_CellDoblClick(object sender, DataGridViewCellEventArgs e)
        {

            var row = XuForwPart.Rows[e.RowIndex];

            var data = zFilterFromForw.FirstOrDefault(x => x.Id == int.Parse(row.Cells[0].Value.ToString()));

            if (zSelectedRec != null)
            {

                foreach (var rec in zFilterFormBill.Where(x => x.Awb == zSelectedRec.Awb))
                {
                    rec.ForwarderObj = data;
                }
                ShowData();


            }

        }

        private void XuNonMatch_Click(object sender, EventArgs e)
        {
            zSelectedRec = null;
            ShowData();
        }
    }
}
