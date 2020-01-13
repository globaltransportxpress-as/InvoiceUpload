using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UploadDHL.GetForwarderId;
using UploadDHL.Properties.DataSources;

namespace UploadDHL
{
    public partial class Matchup : Form
    {

        private List<XMLRecord> zFormBill;
        private List<ForwObj> zFromForw;
        private List<XMLRecord> zFilterFormBill;
        private List<XMLRecord> zNonAssigned;
        private List<ForwObj> zFilterFromForw;
        private XMLRecord zSelectedRec = null;
        private List<PriceObject>  zOutData = new List<PriceObject>();
        private Dialog zDialog = null;
        private Dictionary<int, string > AccountCustomer= new Dictionary<int, string>();
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
            XuMissingCalculation.MultiSelect = true;
            XuMissingCalculation.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            zNonAssigned = zFormBill.Where(x => x.ForwarderObj == null).ToList();
            var listint = zFormBill.Where(x => x.ForwarderObj != null).Select(x => x.ForwarderObj.Id).ToList();
             zFilterFromForw = zFromForw.Where(x => listint.All(y => y != x.Id)).ToList();

            if (zNonAssigned.Count == 0)
            {
                var soapClient = new GetForwarderIdSoapClient("GetForwarderIdSoap");


                

                 zOutData = new List<PriceObject>();
                XuAssignPanel.Visible = false;
                XuPricePanel.Visible = true;
                var awblist = zFormBill.Select(x =>x.Awb ).Distinct().ToList();
                foreach (var awb in awblist)
                {

                    var da = zFormBill.First(x => x.Awb == awb).ForwarderObj.PriceList;
                    var accounts = da.Select(x => x.FK_Account_Id).Distinct().ToList();

                    
                    foreach (var bill in zFormBill.Where(x => x.Awb == awb ))
                    {
                        var pre = 0;
                        
                        foreach (var acc in accounts)
                        {
                            
                            

                            var pO = new PriceObject();
                            pO.Factura = bill.InvoiceNumber;
                            pO.Awb = MarkPre(awb,pre);
                            pO.Account = acc;
                            pO.Date = bill.Shipdate;
                            pO.Items = bill.NumberofCollies;

                            if (!AccountCustomer.ContainsKey(acc))
                            {
                                var d = soapClient.CustomerName(acc);
                                AccountCustomer.Add(acc, d);

                            }
                            pO.Customer= AccountCustomer[acc];
                            



                            pO.CostPrice = bill.Price/accounts.Count();
                            pO.BillType = bill.GTXName;
                            pO.Note = bill.ForwarderObj.NoteInternal;
                            pO.Link = bill.ForwarderObj.Link;
                            pO.Address = string.Format("{0},{1} {2}", bill.Address1, bill.Zip, bill.City);
                            pO.Price = -1M;
                            SalePriceLine lin = null;
                            if (bill.KeyType == "FRAGT")
                            {
                                 lin = da.FirstOrDefault(x => x.LineName != "Oil" && x.FK_Account_Id == acc);
                              

                            }
                            else
                            {

                                 lin = da.FirstOrDefault(x => x.LineName == bill.GTXName && x.FK_Account_Id == acc);


                            }
                            if (lin != null)
                            {


                                pO.ForwType = lin.LineName;
                                pO.Price = lin.Price;
                                pO.CostEstimated = lin.EstimatedCost;
                                pO.Log = lin.Log;
                            }
                            else
                            {
                                pO.ForwType = "None";
                                pO.Price = -1;
                                pO.CostEstimated = -1;
                                pO.Log = "No forwarder";
                            }
                            pre++;

                            zOutData.Add(pO);

                        }


                    }
                    






                }


                XuFilterForwType.DataSource  = zOutData.Select(x => x.ForwType?.ToString() ?? "None").Distinct().OrderBy(x=>x).ToList();

                XuFilterBillType.DataSource = zOutData.Select(x => x.BillType).Distinct().OrderBy(x => x).ToList();

                XuFilterDate.DataSource = zOutData.Select(x => x.Date).Distinct().OrderBy(x => x).ToList();
              
                XuFilterCustomer.DataSource = zOutData.Select(x => x.Customer).Distinct().OrderBy(x => x).ToList();
                XuFilterAddress.DataSource = zOutData.Select(x => x.Address).Distinct().OrderBy(x => x).ToList();

                XuFilterCustomer.Text = "";
                XuFilterForwType.Text = "";
                XuFilterBillType.Text = "";
                XuFilterAddress.Text = "";
                XuFilterDate.Text = "";



                XuValueGrid.DataSource = zOutData;




            }
            else
            {
                FilterFormBill();
                XuMissingCalculation.DataSource = zFilterFormBill;
                XuForwPart.DataSource = zFilterFromForw;
            }

           
          

        }

        private void FilterFormBill()
        {
            zFilterFormBill = zNonAssigned;
            if (XuOnlyFragt.Checked)
            {
                zFilterFormBill = zNonAssigned.Where(x => x.KeyType == "FRAGT").ToList();
            }
           


        }
        public void AssignPrice( bool makeok)
        {

            var rows = XuValueGrid.SelectedRows
                .OfType<DataGridViewRow>()
                .Where(row => !row.IsNewRow)
                .Select(x => x.Index).ToList();
            foreach (var inx in rows)
            {

                zOutData[inx].OK = "OK";





            }

            FilterList();




        }
        public void AssignPrice(decimal price, bool recaloil, bool makeok)
        {

            var rows = XuValueGrid.SelectedRows
                .OfType<DataGridViewRow>()
                .Where(row => !row.IsNewRow)
                .Select(x => x.Index).ToList();

            var res = zOutData.Select((item, i) => new
            {
                Item = item,
                inx = i
            }).Where(x=>rows.Contains(x.inx)).Select(x=>x.Item);

            if (recaloil)
            {
              res= res.Where(x => x.BillType != "Oil").ToList();
            }
           
            foreach (var record in res)
            {
                var oldp = record.Price;
                if (recaloil) {
                var oil = zOutData.FirstOrDefault(x => x.Awb == record.Awb && x.BillType == "Oil");
                if (oil != null && oldp!=0 )
                {
                    oil.Price = oil.Price / oldp * price;
                    if (makeok)
                    {
                        oil.OK = "OK";
                    }
                }
                }
                record.Price = price;
                if (makeok)
                {
                    record.OK = "OK";
                }


            }
            FilterList();


        }

        private string MarkPre(string awb , int no)
        {
            if (no > 0)
            {
                return awb + "-" + no;
            }
            return awb;
        }
        private void FilterList()
        {
           

            IEnumerable<PriceObject> q = zOutData;
            if (XuSearch.Text != "" && XuSearchType.SelectedItem != null)
            {
                var searchList = XuSearch.Text.Split(',');
                if (XuSearchType.SelectedItem.ToString().StartsWith("Customer"))
                {
                    q = q.Where(x =>searchList.Any(y=>x.Customer.StartsWith(y, StringComparison.InvariantCultureIgnoreCase)));
                }

                if (XuSearchType.SelectedItem.ToString().StartsWith("Address"))
                {
                    q = q.Where(x => searchList.Any(y => x.Address.StartsWith(y, StringComparison.InvariantCultureIgnoreCase)));
                }
                if (XuSearchType.SelectedItem.ToString().StartsWith("Account"))
                {
                    q = q.Where(x => searchList.Contains(x.Account.ToString()));
                }
                if (XuSearchType.SelectedItem.ToString().StartsWith( "Awb"))
                {
                    q = q.Where(x => searchList.Any(y => x.Awb.StartsWith(y, StringComparison.InvariantCultureIgnoreCase))); 
                }

            }
            DateTime date;
            if (ComboVal(XuFilterDate))
            {
                q = q.Where(x => x.Date ==(DateTime) XuFilterDate.SelectedItem);
            }
           
            int acc;
           
            if (ComboVal(XuFilterBillType))
            {
                q = q.Where(x => x.BillType == XuFilterBillType.SelectedValue.ToString());
            }
            if (ComboVal(XuFilterForwType))
            {
                q = q.Where(x => x.ForwType == XuFilterForwType.SelectedValue.ToString());
            }

            if (XuSpecial.SelectedItem != null && XuSpecial.SelectedItem.ToString().StartsWith("1."))
            {

                q = q.Where(x => x.CostEstimated < x.CostPrice);



            }
            if (XuSpecial.SelectedItem != null && XuSpecial.SelectedItem.ToString().StartsWith("2."))
            {

                q = q.Where(x => x.Price <0 );



            }
            if (ComboVal(XuFilterCustomer))
            {
                q = q.Where(x => x.Customer == XuFilterCustomer.SelectedValue.ToString());
            }
            if (ComboVal(XuFilterAddress))
            {
                q = q.Where(x => x.Address == XuFilterAddress.SelectedValue.ToString());
            }
            decimal n;
            if (decimal.TryParse(XuFilterCost.Text, out n))
            {
                q = q.Where(x => Math.Abs(x.CostPrice-n)<0.4M);
            }
            if (decimal.TryParse(XuFilterPrice.Text, out n))
            {
                q = q.Where(x => Math.Abs(x.Price - n)< 0.4M);
            }

            if (!XuShowOK.Checked)
            {
                q = q.Where(x => x.OK != "OK");
            }

            if (XuSort.SelectedItem != null)
            {

                var sel = XuSort.SelectedItem.ToString();

                if (sel.StartsWith("1"))
                {
                    q = q.OrderBy(x => x.Date).ThenBy(x => x.Customer);
                }
                if (sel.StartsWith("2"))
                {
                    q = q.OrderBy(x => x.Customer).ThenBy(x => x.Date);
                }
                if (sel.StartsWith("3"))
                {
                    q = q.OrderBy(x => x.Address).ThenBy(x => x.Date);
                }

            }

            XuValueGrid.DataSource = q.ToList();
        }

        private bool ComboVal(ComboBox com)
        {
            return com.Text != "" && com.SelectedValue != null;
        }

        private void XuNonMatch_Click(object sender, EventArgs e)
        {
            zSelectedRec = null;
            ShowData();
        }

        private void XuAssignSeleted_Click(object sender, EventArgs e)
        {
            var selectedAwb = XuMissingCalculation.SelectedRows
                .OfType<DataGridViewRow>()
                .Where(row => !row.IsNewRow)
                .Select(x => x.Cells[0].Value.ToString()).ToList();

           // var row = XuMissingCalculation.Rows[e.RowIndex];

            var selcRec = zNonAssigned.Where(x => selectedAwb.Contains(x.Awb)).ToList();
            var sp = new List<SalePriceLine>();
            var spl = new SalePriceLine
            {
                EstimatedCost = -1M,
                FK_Account_Id = int.Parse(XuAccount.Text),

            };
            sp.Add(spl);
            foreach (var rec in selcRec)
            {
                var fw = new ForwObj();
                fw.NoteInternal = "Created by accounting";
                fw.PriceList = sp.ToArray();
                rec.ForwarderObj = fw;
               
            }
            zSelectedRec = null;
            ShowData();

        }

        private void XuMissingCalculation_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var row = XuMissingCalculation.Rows[e.RowIndex];

            zSelectedRec = zFilterFormBill.FirstOrDefault(x => x.Awb == row.Cells[0].Value.ToString());
           
             var da  = zFilterFromForw.Where(x => x.Zip == zSelectedRec.Zip).ToList();
            if (da.Count > 4)
            {
                da = da.Where(x => x.PickupDate == zSelectedRec.Shipdate).ToList();
            }
            XuForwPart.DataSource = da;
            if (e.Button == MouseButtons.Right && da.Count==1)
            {
                foreach (var rec in zNonAssigned.Where(x => x.Awb == zSelectedRec.Awb))
                {
                    rec.ForwarderObj = da[0];
                }
                ShowData();

            }

        }

        private void XuForwPart_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var row = XuForwPart.Rows[e.RowIndex];

            var data = zFilterFromForw.FirstOrDefault(x => x.Id == int.Parse(row.Cells[0].Value.ToString()));

            if (zSelectedRec != null)
            {

                foreach (var rec in zNonAssigned.Where(x => x.Awb == zSelectedRec.Awb))
                {
                    rec.ForwarderObj = data;
                }
                ShowData();


            }
        }

        private void XuAccount_TextChanged(object sender, EventArgs e)
        {

            int acc;
            if (int.TryParse(XuAccount.Text, out acc))
            {


                var soapClient = new GetForwarderIdSoapClient("GetForwarderIdSoap");


                var d = soapClient.CustomerName(acc);
                XuCustomerName.Text = d;

            }
        }

        
        private void XuFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterList();
        }

        private void XuFilterBillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterList();
        }

        

        private void XuValueGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            XuDialogPanel.Visible = false;
            if (XuValueGrid.Columns[XuValueGrid.CurrentCell.ColumnIndex].HeaderText.Contains("Link"))
            {
                if (!String.IsNullOrWhiteSpace(XuValueGrid.CurrentCell.EditedFormattedValue.ToString()))
                {
                    System.Diagnostics.Process.Start("" + XuValueGrid.CurrentCell.EditedFormattedValue);
                }
            }
        }

        private void XuFilter_Click(object sender, EventArgs e)
        {
           
                  FilterList();
              



           
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ShowData();
        }

      

        private void XuValueGrid_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Rectangle d = XuValueGrid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                XuDialogPanel.Location = new Point(d.X, d.Y);
               
                XuDialogPanel.Visible = true;
            }
        }

        private void XuValueGrid_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void XuFilterCustomer_TextUpdate(object sender, EventArgs e)
        {
            var combo = (ComboBox) sender;
            if (combo.Text == "")
            {

                combo.SelectedIndex = -1;
                FilterList();
            }
        }

        private void XuAssignPrice_Click(object sender, EventArgs e)
        {
            decimal price;
            if (decimal.TryParse(XuNewPrice.Text, out price))
            {

                AssignPrice(price, XuReaclOil.Checked, XuMakeOK.Checked);
                XuDialogPanel.Visible = false;

            }
            if (XuMakeOK.Checked && !XuReaclOil.Checked)
            {
                AssignPrice(XuMakeOK.Checked);
                XuDialogPanel.Visible = false;
            }


        }

        private void XuValueGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            XuDialogPanel.Visible = false;
        }
    }
}
