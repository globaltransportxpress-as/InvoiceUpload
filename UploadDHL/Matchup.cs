using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using UploadDHL.GetForwarderId;
using UploadDHL.Properties.DataSources;

namespace UploadDHL
{
    public partial class Matchup : Form
    {
        private class ComboObj
        {
            public string Text { get; set; }
            public string Account { get; set; }
        }
        private List<XMLRecord> zFormBill;
        private List<ForwarderRecord> zFromForw;
        private List<XMLRecord> zFilterFormBill;
        private List<XMLRecord> zNonAssigned;
        private List<ForwarderRecord> zFilterFromForw;
        private List<ForwarderRecord> zAssignCostForw;
        private List<ForwarderRecord> zNoMatchFromForw;
        private XMLRecord zSelectedRec = null;
        public List<PriceObject>  zOutData = new List<PriceObject>();
        private List<PriceObject> zFilterOut;
        public VendorHandler zVendorHandler;
        public Translation TranslationHandler;
        private Dictionary<string,bool> zSortDir= new Dictionary<string, bool>();
        private int awbcount = 1;
        public string Carrier;
        
        private Dictionary<string, string > AccountCustomer= new Dictionary<string, string>();
        public Matchup()
        {
            InitializeComponent();
        }

        private void Matchup_Load(object sender, EventArgs e)
        {

        }

        public void InitData(List<XMLRecord> formBill, List<ForwarderRecord> fromForw)
        {
            zFormBill = formBill;
            zFromForw = fromForw;

            XuFilterRecordType.DataSource = formBill.Select(x => x.KeyType).Distinct().ToList();
            XuFilterRecordType.Text = "";
         var lp=   TranslationHandler.AccountDictionary.Select(
                x => new ComboObj { Account = x.Key, Text = string.Format("{0} ({1})", x.Value, x.Key)}).OrderBy(x=>x.Text).ToList();

            XuCompanyAccountSel.DataSource = lp;
            XuCompanyAccountSel.DisplayMember = "Text";


        }


        public void DisplayGrid()
        {

            XuFilterDate.DataSource = zOutData.Select(x => x.Date).Distinct().OrderBy(x => x).ToList();
            XuBillTypeFilter.DataSource = zOutData.Select(x => x.BillType).Distinct().OrderBy(x => x).ToList();

           



            XuValueGrid.DataSource = zOutData;

            zFilterOut = zOutData;
            XuAssignPanel.Visible = false;
            XuPricePanel.Visible = true;
            XuBillTypeFilter.Text = "";
            XuFilterDate.Text = "";
        }
        public void ShowData()
        {
            XuMissingCalculation.MultiSelect = true;
            XuMissingCalculation.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            zNonAssigned = zFormBill.Where(x => x.ForwarderObj == null).ToList();
            var listint = zFormBill.Where(x => x.ForwarderObj != null).Select(x => x.ForwarderObj.Id).ToList();
            zNoMatchFromForw = zFromForw.Where(x => listint.All(y => y != x.Id)).ToList();

            if (zNonAssigned.Count == 0)
            {
                var soapClient = new GetForwarderIdSoapClient("GetForwarderIdSoap");


                

                 zOutData = new List<PriceObject>();
               
                var awblist = zFormBill.Select(x =>x.Awb ).Distinct().ToList();
                foreach (var awb in awblist)
                {

                    var da = zFormBill.First(x => x.Awb == awb).ForwarderObj.PriceList;
                    var accounts = da.Select(x => x.AccountId).Distinct().ToList();

                    
                    foreach (var bill in zFormBill.Where(x => x.Awb == awb ))
                    {
                        var pre = 0;
                        
                        foreach (var acc in accounts)
                        {
                            var ourawb = "FW" + bill.ForwarderObj.Id;
                            
                            if (bill.ForwarderObj.Id == 0)
                            {
                                 ourawb = "AW" + DateTime.Now.ToString("ffffdmy")+ awbcount.ToString();
                                awbcount++;
                            }
                            
                            var pO = new PriceObject();
                            pO.ForwId = bill.ForwarderObj.Id;
                            pO.Factura = bill.InvoiceNumber;
                            pO.Awb = MarkPre(ourawb, pre);
                            pO.CarrierAwb = bill.Awb;
                            pO.Account = acc;
                            pO.Date = bill.Shipdate;
                           

                            pO.Items = bill.NumberofCollies;

                            

                            pO.Customer = bill.ForwarderObj.CompanyName;
                            pO.BillCustomer = bill.CompanyName;
                            pO.DataType = bill.KeyType;
                            pO.TypeCostId =bill.Transport.ToString();
                            pO.TypeSalesId = bill.Product.ToString();
                            pO.CostPrice = bill.Price/accounts.Count();
                            pO.BillType = bill.GTXName;
                            pO.Note = bill.ForwarderObj.NoteInternal;
                            
                            pO.Link = bill.ForwarderObj.Link;
                            pO.Address = bill.Address1;
                            pO.Zip = bill.Zip;
                            pO.City = bill.City;
                            pO.Price = -1M;
                            pO.Carrier = bill.CarrierCode;
                            
                           

                            var lin = da.FirstOrDefault(x => x.LineName == bill.GTXName && x.AccountId == acc);


                           
                            if (lin != null)
                            {

                                pO.Payer = lin.CompanyName;
                                pO.ForwType = lin.LineName;
                                pO.Price = lin.Price;
                                pO.CostEstimated = lin.EstimatedCost;
                                
                                
                            }
                            else
                            {

                                pO.Payer = "UnKnown";
                                pO.ForwType = "None";
                                pO.Price = -1;
                                pO.CostEstimated = -1;
                               
                            }
                            pre++;

                            zOutData.Add(pO);

                        }


                    }
                    






                }


                DisplayGrid();



            }
            else
            {
                FilterFormBill();
                XuMissingCalculation.DataSource = zFilterFormBill;
               
                XuFilterForw.DataSource= zNoMatchFromForw.Select(x => x.CompanyName).Distinct().ToList();
                XuFilterForw.Text = "";
               
                XuForwPart.DataSource = zNoMatchFromForw;
                zFilterFromForw = zNoMatchFromForw;
               XuFilterForw.Enabled= true;
            }

           
          

        }

        private void FilterForw()
        {
            zFilterFromForw = zNoMatchFromForw;
            if (ComboVal(XuFilterForw))
            {
                zFilterFromForw = zNoMatchFromForw.Where(x => x.CompanyName == XuFilterForw.SelectedValue.ToString()).ToList();
            }
          
            
            XuForwPart.DataSource = zFilterFromForw;

        }
        private void FilterFormBill()
        {
            zFilterFormBill = zNonAssigned;
            if (ComboVal(XuFilterRecordType))
            {
                zFilterFormBill = zNonAssigned.Where(x => x.KeyType == XuFilterRecordType.SelectedValue.ToString()).ToList();
            }
           


        }
        public void AssignOK( )
        {

            var res = GetSeleted();
            res.Where(x=>x.Account!="-1").ToList().ForEach(x=>x.OK ="OK");

            FilterList();
           




        }


        private List<PriceObject> GetSeleted()
        {
            var rows = XuValueGrid.SelectedRows
                .OfType<DataGridViewRow>()
                .Where(row => !row.IsNewRow)
                .Select(x => x.Index).ToList();

            var res = zFilterOut.Select((item, i) => new
            {
                Item = item,
                inx = i
            }).Where(x => rows.Contains(x.inx)).Select(x => x.Item);
            return res.ToList();
        }



        public void AssignPrice(decimal price, bool recaloil, bool makeok)
        {

            var res = GetSeleted();

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
                    oil.Price = decimal.Round(oil.Price / oldp * price, 2, MidpointRounding.AwayFromZero);

                    if (makeok  && record.Account !="-1")
                    {
                        oil.OK = "OK";
                    }
                }
                }
                record.Price = price;
                if (makeok && record.Account!= "-1")
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

        private IEnumerable<PriceObject> GenericFilter(IEnumerable<PriceObject> q, ComboBox searchType,
            string searchText)
        {
            
             var searchList = searchText.Split(',');
                
                if (searchType.SelectedItem.ToString().StartsWith("Customer"))
                {
                    q = q.Where(x =>searchList.Any(y=>x.Customer.StartsWith(y, StringComparison.InvariantCultureIgnoreCase)));
                }
                if (searchType.SelectedItem.ToString().StartsWith("Payer"))
                {
                    q = q.Where(x => searchList.Any(y => x.Payer.StartsWith(y, StringComparison.InvariantCultureIgnoreCase)));
                }
                if (searchType.SelectedItem.ToString().StartsWith("BillCustomer"))
                {
                    q = q.Where(x => searchList.Any(y => x.BillCustomer.StartsWith(y, StringComparison.InvariantCultureIgnoreCase)));
                }
                if (searchType.SelectedItem.ToString().StartsWith("Address"))
                {
                    q = q.Where(x => searchList.Any(y => x.Address.StartsWith(y, StringComparison.InvariantCultureIgnoreCase)));
                }
                if (searchType.SelectedItem.ToString().StartsWith("Zip"))
                {
                    q = q.Where(x => searchList.Any(y => x.Zip.StartsWith(y, StringComparison.InvariantCultureIgnoreCase)));
                }
                if (searchType.SelectedItem.ToString().StartsWith("City"))
                {
                    q = q.Where(x => searchList.Any(y => x.City.StartsWith(y, StringComparison.InvariantCultureIgnoreCase)));
                }
                if (searchType.SelectedItem.ToString().StartsWith("Account"))
                {
                    q = q.Where(x => searchList.Contains(x.Account.ToString()));
                }
                if (searchType.SelectedItem.ToString().StartsWith( "Awb"))
                {
                    q = q.Where(x => searchList.Any(y => x.Awb.StartsWith(y, StringComparison.InvariantCultureIgnoreCase))); 
                }
                if (searchType.SelectedItem.ToString().StartsWith("CarrierAwb"))
                {
                    q = q.Where(x => searchList.Any(y => x.CarrierAwb.StartsWith(y, StringComparison.InvariantCultureIgnoreCase)));
                }

                if (searchType.SelectedItem.ToString().StartsWith("Date"))
                {
                    q = q.Where(x => searchList.Any(y => x.Date.ToString("ddMMyyyy").StartsWith(y, StringComparison.InvariantCultureIgnoreCase)));
                }

            if (searchType.SelectedItem.ToString().StartsWith("TypeSalesId"))
            {
                q = q.Where(x => searchList.Contains(x.TypeSalesId.ToString()));
            }
            if (searchType.SelectedItem.ToString().StartsWith("DataType"))
            {
                q = q.Where(x => searchList.Contains(x.DataType.ToString()));
            }
            if (searchType.SelectedItem.ToString().StartsWith("TypeCostId"))
            {
                q = q.Where(x => searchList.Contains(x.TypeCostId.ToString()));
            }
            if (searchType.SelectedItem.ToString().StartsWith("BillType"))
            {
                q = q.Where(x => searchList.Any(y => x.BillType.StartsWith(y, StringComparison.InvariantCultureIgnoreCase)));
            }
            if (searchType.SelectedItem.ToString().StartsWith("ForwType"))
            {
                q = q.Where(x => searchList.Any(y => x.ForwType.StartsWith(y, StringComparison.InvariantCultureIgnoreCase)));
            }
            int n;
            if (int.TryParse(searchText, out n))
            {
                if (searchType.SelectedItem.ToString().StartsWith("Price"))
                {
                    q = q.Where(x => (int)x.Price == n);
                }
                if (searchType.SelectedItem.ToString().StartsWith("CostPrice"))
                {
                    q = q.Where(x => (int)x.CostPrice == n);
                }
                if (searchType.SelectedItem.ToString().StartsWith("CostEstimated"))
                {
                    q = q.Where(x => (int)x.CostEstimated == n);
                }
            }

            return q;



        }
        private void FilterList()
        {

            if (zOutData.All(x => !string.IsNullOrEmpty(x.OK)))
            {
               
                zVendorHandler.ExportData(zOutData);
                Close();
                return;

            }

            IEnumerable<PriceObject> q = zOutData;
            if (XuSearch.Text != "" && XuSearchType.SelectedItem != null)
            {
                q = GenericFilter(q, XuSearchType, XuSearch.Text);


            }
            
            if (XuSearch2.Text != "" && XuSearchType2.SelectedItem != null)
            {
                q = GenericFilter(q, XuSearchType2, XuSearch2.Text);
            }

            if (ComboVal(XuBillTypeFilter))
            {
                q = q.Where(x => x.BillType == (string)XuBillTypeFilter.SelectedItem);
            }

            DateTime date;
            if (ComboVal(XuFilterDate))
            {
                q = q.Where(x => x.Date ==(DateTime) XuFilterDate.SelectedItem);
            }
           
            int acc;
           
           
            if (XuSpecial.SelectedItem != null && XuSpecial.SelectedItem.ToString().StartsWith("1."))
            {

                q = q.Where(x => x.CostEstimated < x.CostPrice);



            }
            if (XuSpecial.SelectedItem != null && XuSpecial.SelectedItem.ToString().StartsWith("2."))
            {

                q = q.Where(x => x.Price <0 );



            }
            if (XuSpecial.SelectedItem != null && XuSpecial.SelectedItem.ToString().StartsWith("3."))
            {

                q = q.Where(x => zOutData.Where(y=>y.Customer== x.Customer).Select(y=>y.Account).Distinct().Count()>1);



            }
            if (XuSpecial.SelectedItem != null && XuSpecial.SelectedItem.ToString().StartsWith("4."))
            {

                q = q.Where(x => zOutData.Count(y => y.Awb == x.Awb && y.TypeSalesId == x.TypeSalesId) > 1);



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
            zFilterOut = q.ToList();
            XuValueGrid.DataSource = zFilterOut;
            foreach (DataGridViewColumn column in XuValueGrid.Columns)
            {

                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
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
            var sp = new List<PickUpPriceLine>();
            
            foreach (var rec in selcRec)
            {

                var spl = new PickUpPriceLine()
                {
                    EstimatedCost = -1M,
                    Price = -1M,
                    LineName = rec.GTXName,
                    AccountId = XuAccount.Text,
                    CompanyName = XuCustomerName.Text


                };
                sp.Add(spl);


                var fw = new ForwObj();
                fw.NoteInternal = "Created by accounting";
               
                fw.PriceList = sp.ToArray();
                fw.CompanyName = rec.CompanyName;
         

                rec.ForwarderObj =new ForwarderRecord(fw);
              
               
            }
            
            zSelectedRec = null;
            ShowData();

        }

        private void XuMissingCalculation_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var row = XuMissingCalculation.Rows[e.RowIndex];

            zSelectedRec = zFilterFormBill.FirstOrDefault(x => x.Awb == row.Cells[0].Value.ToString());
           
             var da  = zNoMatchFromForw.Where(x => x.Zip == zSelectedRec.Zip).ToList();
            
            var da1 = da.Where(x => x.PickupDate == zSelectedRec.Shipdate).ToList();
            if (da1.Count > 0)
            {
               
                da = da1;

            }
            zFilterFromForw = da;
            XuForwPart.DataSource = da;
            if (e.Button == MouseButtons.Right && da.Count==1)
            {
                foreach (var rec in zFormBill.Where(x => x.Awb == zSelectedRec.Awb))
                {
                    rec.ForwarderObj = da[0];
                }
                ShowData();

            }

        }

        private void XuForwPart_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
          
            if (e.Button == MouseButtons.Right )
            {
                var rows = XuForwPart.SelectedRows
                    .OfType<DataGridViewRow>()
                    .Where(row => !row.IsNewRow)
                    .Select(x => x.Index).ToList();

                zAssignCostForw = zFilterFromForw.Select((item, i) => new
                {
                    Item = item,
                    inx = i
                }).Where(x => rows.Contains(x.inx)).Select(x => x.Item).ToList();

                XuAssignCostPanel.Visible = true;

               


            }
            else
            {
                var row = XuForwPart.Rows[e.RowIndex];
                var data = zFilterFromForw.Select((item, i) => new
                {
                    Item = item,
                    inx = i
                }).Where(x => row.Index ==x.inx).Select(x => x.Item).FirstOrDefault();

                if (zSelectedRec != null)
                {

                    foreach (var rec in zFormBill.Where(x => x.Awb == zSelectedRec.Awb))
                    {
                        rec.ForwarderObj = data;
                    }
                    ShowData();


                }
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
            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "csv|*.csv";
            saveFileDialog1.Title = "Save data to csv";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
               
                using (StreamWriter outputFile =
                    new StreamWriter(new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.Write),
                        Encoding.UTF8))
                {
                    
                  


                        outputFile.Write(ToCsv(zOutData));
                }

                Close();
            }
           
        }



        private string ToCsv<T>( IList<T> list)
        {
            Type elementType = typeof(T);

            var sb = new StringBuilder();
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            var column = new List<string>();
            foreach (var propInfo in elementType.GetProperties())
            {
               

                column.Add(propInfo.Name);
            }
            sb.AppendLine(string.Join(";", column));
            //go through each property on T and add each value to the table
            foreach (T item in list)
            {
                var row = new List<string>();

                foreach (var propInfo in elementType.GetProperties())
                {

                    var d = propInfo.GetValue(item, null);
                    var val = "";
                    if (d != null)
                    {
                        val = d.ToString();
                    }
                    row.Add(string.Format("\"{0}\"",val));
                }
                sb.AppendLine(string.Join(";", row));

            }

            return sb.ToString();
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

                var y = d.Y - XuDialogPanel.Height;
                if (y < 10)
                {
                    y = 10;
                }

                XuDialogPanel.Location = new Point(d.X+20,y);
                var res = GetSeleted();
                XuAccountPossible.DataSource = res.Select(x => x.Account).Distinct().ToList();

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
                AssignOK();
                XuDialogPanel.Visible = false;
            }


        }

        private void XuValueGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            XuDialogPanel.Visible = false;
        }

       

        private void XuFilterForw_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterForw();
        }

        private void XuForwPart_Click(object sender, EventArgs e)
        {
            XuAssignCostPanel.Visible = false;
            zAssignCostForw = null;
        }

        private void XuAssignCost_Click(object sender, EventArgs e)
        {
            decimal cost;
           var rec= zFormBill.FirstOrDefault(x => x.Awb.StartsWith("xx"));
            if (rec!=null && zAssignCostForw != null && zAssignCostForw.Count > 0 && decimal.TryParse(XuCost.Text, out cost))
            {

                foreach (var ass in zAssignCostForw)
                {
                    foreach (var a in ass.PriceList.Select(x => x.LineName).Distinct())
                    {
                        var transrec = TranslationHandler.TranDictionary.FirstOrDefault(x => x.Value.GTXName == a);
                        var fob = new XMLRecord();
                        fob.CompanyName = ass.CompanyName;
                        fob.Awb = "FW" + ass.Id;
                        fob.Address1 = ass.Street;
                        fob.CarrierCode = Carrier;
                        fob.City = ass.City;
                        fob.Costprice = cost;
                        fob.ForwarderObj = ass;
                        fob.GTXName = a;
                        fob.Product = transrec.Value.GTXProduct;
                        fob.Transport = (byte) transrec.Value.GTXTransp;
                        fob.KeyType = transrec.Value.KeyType;
                        fob.InvoiceDate = rec.InvoiceDate;
                        fob.Shipdate = ass.PickupDate;
                        fob.Zip = ass.Zip;
                        fob.Costprice = cost;
                        fob.Price = cost;
                        fob.NumberofCollies = 1;
                        zFormBill.Add(fob);
                        fob.InvoiceNumber = rec.InvoiceNumber;
                    }
                   

                }






            }
            XuAssignCostPanel.Visible = false;
            ShowData();
        }

        private void XuDeleteXX_Click(object sender, EventArgs e)
        {
            var rec = zFormBill.FirstOrDefault(x => x.Awb.StartsWith("xx"));
            if (rec != null)
            {
                zFormBill.Remove(rec);
                ShowData();
            }
        }

        private void XuCopy_Click(object sender, EventArgs e)
        {
            var res = GetSeleted();

            foreach(var re in res) { 
                var np = new PriceObject(re);
               
                zOutData.Add(np);


            }
            FilterList();
            XuDialogPanel.Visible = false;
        }

        private void XuSetOK_Click(object sender, EventArgs e)
        {
            AssignOK();
            XuDialogPanel.Visible = false;

        }

        private void XuSetAccount_Click(object sender, EventArgs e)
        {
            if (XuAccountPossible.SelectedItem != null)
            {
                string acc = (string) XuAccountPossible.SelectedItem;
                var res = GetSeleted();
              var f=  res.FirstOrDefault(x => x.Account ==acc );
                foreach(var r in res)
                {
                    r.Account = acc;
                    r.Payer = f.Payer;
                }
               
                FilterList();
                XuDialogPanel.Visible = false;


            }

        }

        private void XuDialogPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void XuValueGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var txt=XuValueGrid.Columns[e.ColumnIndex].HeaderText;

            if (!zSortDir.ContainsKey(txt))
            {
                zSortDir.Add(txt,true);
            }
            
            PriceObject po = new PriceObject();
            Type t = po.GetType();
            PropertyInfo prop = t.GetProperty(txt);
            if (zSortDir[txt])
            {
                zFilterOut = zFilterOut.OrderBy(x => prop.GetValue(x, null)).ToList();
            }
            else
            {
                zFilterOut = zFilterOut.OrderByDescending(x => prop.GetValue(x, null)).ToList();
            }
            zSortDir[txt] = !zSortDir[txt];
            XuValueGrid.DataSource = zFilterOut;
        }

        
        private void XuFilterRecordType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowData();
        }

        private void Action_Filter(object sender, EventArgs e)
        {
            FilterList();
        }

        private void XuDelete_Click(object sender, EventArgs e)
        {
           
                var res = GetSeleted();

                

                zOutData.RemoveAll(x=>res.Contains(x));


               

                FilterList();
                XuDialogPanel.Visible = false;
            }

        private void XuShipX_Click(object sender, EventArgs e)
        {


            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "csv|*.csv";
            saveFileDialog1.Title = "Save data to csv";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.

                using (StreamWriter outputFile =
                    new StreamWriter(new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.Write),
                        Encoding.UTF8))
                {




                    outputFile.Write(ToCsv(zOutData.Where(x=>x.Payer.StartsWith("ShipX")).ToList()));
                }

               
            }

        }

        private void XuCompanyAccountSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (XuCompanyAccountSel.SelectedItem != null)
            {
                var d = (ComboObj) XuCompanyAccountSel.SelectedItem;
                XuAccount.Text = d.Account;
            }
        }
    }
}
