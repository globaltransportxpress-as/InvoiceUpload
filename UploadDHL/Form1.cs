using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using GemBox.Spreadsheet;
using Microsoft.Office.Interop.Excel;
using UploadDHL.DataConnections;
using Excel = Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

using UploadDHL.DataUploadWeb;
using UploadDHL.GetForwarderId;
using UploadDHL.GTX;
using UploadDHL.Properties.DataSources;
using Label = System.Windows.Forms.Label;

namespace UploadDHL
{
    public partial class Form1 : Form
    {

        private string actualTransFile = "";
        private List<GridData> zGridDataList;
        private int zOK;
        private List<ForwObj> zForwarderObjs;

        private List<TranslationRecord> zTranlationListFull =null;
        private List<XMLRecord> zBillRecordsList;


        public Form1()
        {
            InitializeComponent();

            EditMode(false);
            FileDone.Text = "Welcome...............";

           
            var date = DateTime.Now.AddMonths(-1);

            XuMinDate.Text = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-dd");
            var lastDayOfMonth = DateTime.DaysInMonth(date.Year, date.Month);
            XuMaxDate.Text  = new DateTime(date.Year, date.Month, lastDayOfMonth).ToString("yyyy-MM-dd");


        }

        private void ShowMessage(string msg)
        {
            FileDone.Text = msg;
            this.Refresh();
            System.Windows.Forms.Application.DoEvents();
        }

        private void XuDHL_Click(object sender, EventArgs e)
        {
            // Append text to an existing file named "WriteLines.txt".
            Init();
            var dhlHandler = new DHLHandler();
            if (XuNoInvoice.Checked)
            {
                

                SelectFromForwarderPickup( dhlHandler, XuTranslationErrorDHL);
                return;


            }
           
            if (dhlHandler.Error != "")
            {
                FileDone.Text = dhlHandler.Error;

                return;
            }

            var path = Config.DHLRootFileDir + "\\In\\";
            foreach (string file in Directory.EnumerateFiles(path, "*.xlsx"))
            {


                var nfile = ImportExceltoDatatable(file, "Sheet1");
               
              
                dhlHandler.GridData.Filename = file;
                dhlHandler.InvoiceName = file.Replace(path, "").Replace(" ", "").Replace(".", "").Replace("xlsx", "");

                ShowMessage("Execution..." + file);

                string[] columnNames = nfile.Columns.Cast<DataColumn>()
                    .Select(x => x.ColumnName)
                    .ToArray();


                dhlHandler.CheckHeader(string.Join(";", columnNames));


                    foreach (DataRow row in nfile.Rows) {

                        string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();

                        dhlHandler.Next(fields);


                     

                      

                    }
                    if (dhlHandler.DataValidation())
                    {

                        if (dhlHandler.MakeXmlAndWeight())
                        {
                            if (dhlHandler.FileFinish(file))
                            {
                                dhlHandler.Success();
                                zOK++;
                            }
                        }



                        dhlHandler.DropLines();
                    }
                
                zGridDataList.Add(dhlHandler.GridData);

            }


            Finish();
        }


        private DataTable ErrorToTable(List<string> errorlines)
        {
            DataTable result = new DataTable();
            var li = 0;
            foreach (var line in errorlines)
            {


                string[] fields = line.Split('|');
                if (li == 0)
                {
                    foreach (var f in fields)
                    {

                        result.Columns.Add(f);
                    }
                }
                // If first line is data then add it
                li++;
                result.Rows.Add(fields);
            }

            return result;
        }




        private void EditMode(bool show)
        {
            FileDone.Text = "";
            XuMsgGrid.Visible = !show;
            XuEditTranslationGrid.Visible = show;
            XuSaveTranslatioon.Visible = show;
            XuDataGridError.Visible = false;
            XuClose.Visible = false;
            XuFilterKey.Visible = show;
            XuLabelFilter.Visible = show;
            if (show)
            {
                XuFilterKey.Text = "";
            }
            XuTranslationErrorGLS.Visible = false;
            XuTranslationErrorHS.Visible = false;
            XuTranslationErrorDHL.Visible = false;
        }

        private void EditTransFile(VendorHandler handler)
        {
            EditMode(true);
           
            var translation =  handler.Translation;
            
            var list = translation.TranDictionary.Select(x => x.Value).ToList();
            foreach (var added in translation.AddList)
            {
                var arr = added.Split(';');
                list.Add(new TranslationRecord(arr));
            }

            zTranlationListFull = list.OrderBy(x => x.KeyType).ThenBy(x => x.GTXName).ToList();
            XuEditTranslationGrid.DataSource = zTranlationListFull;
        }

        private void Xu_EditDHL_Click(object sender, EventArgs e)
        {
            EditTransFile(new DHLHandler());
        }

       
       

        private void XuEditTransGLS_Click(object sender, EventArgs e)
        {
            EditTransFile(new GLSHandler());
        }

        private void XuSaveTranslatioon_Click(object sender, EventArgs e)
        {
            var translation = new GLSHandler().Translation;

            translation.SaveAll((List<TranslationRecord>)XuEditTranslationGrid.DataSource);

            EditMode(false);

        }

        private void XuMsgGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (
                e.RowIndex >= 0)
            {
                var gd = (List<GridData>)XuMsgGrid.DataSource;

                XuDataGridError.Visible = true;
                XuClose.Visible = true;

                XuDataGridError.DataSource = ErrorToTable(gd[e.RowIndex].ErrorLines
                    .Select(x => x.Status + "|" + x.Reason + "|" + x.Raw).ToList());



            }
        }

        public string ExcelToCSV(string filename)
        {
            Type officeType = Type.GetTypeFromProgID("Excel.Application");

            if (officeType == null)
            {
                // Excel is not installed.
                // Show message or alert that Excel is not installed.
            }
            else
            {
                // Excel is installed.
                // Let us continue our work on Excel file conversion. 
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();

                // While saving, it asks for the user confirmation, whether we want to save or not.
                // By setting DisplayAlerts to false, we just skip this alert.
                app.DisplayAlerts = false;

                // Now we open the upload file in Excel Workbook. 
                Microsoft.Office.Interop.Excel.Workbook excelWorkbook = app.Workbooks.Open(filename);

                string newFileName = filename.Replace(".xlsx", ".csv").Replace(" ", "_");

                // Now save this file as CSV file.
                excelWorkbook.SaveAs(newFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlCSV);

                // Close the Workbook and Quit the Excel Application at the end. 
                excelWorkbook.Close();
                app.Quit();
                return newFileName;
            }
            return "";
        }

        public DataTable ImportExceltoDatatable(string filepath, string tabname)
        {
            // string sqlquery= "Select * From [SheetName$] Where YourCondition";

            var arkname = tabname;
            if (XU_ArkName.Text != "")
            {
                arkname = XU_ArkName.Text;
            }
            string sqlquery = "Select * From [" + arkname + "$]";
            DataSet ds = new DataSet();
            string constring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath +
                               ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
            OleDbConnection con = new OleDbConnection(constring + "");
            OleDbDataAdapter da = new OleDbDataAdapter(sqlquery, con);
            da.Fill(ds);
            DataTable dt = ds.Tables[0];


            return dt;
        }




       

        private void Init()
        {
            EditMode(false);
            FileDone.Text = "Start...........>>>>";
            zOK = 0;
            zGridDataList = new List<GridData>();

        }

        private void Finish()
        {
            ShowMessage("Done ..." + zOK + " files");
            
            XuMsgGrid.DataSource = zGridDataList;

        }



        private void XuGls_Click(object sender, EventArgs e)
        {
            var glsHandler = new GLSHandler();
            if (XuNoInvoice.Checked)
            {


                SelectFromForwarderPickup(glsHandler,XuTranslationErrorGLS);
                return;


            }
            Init();
            var path = Config.GLSRootFileDir + "\\In\\";
            var listfile = Directory.EnumerateFiles(path, "*.xlsx");
           
            if (glsHandler.Error != "")
            {
                FileDone.Text = glsHandler.Error;

                return;
            }
            foreach (string file in listfile.Where(x => !x.Contains("~")))
            {
                var nfile = ImportExceltoDatatable(file, "Fakturaspecifikation");

                var filename = file.Replace(path, "");
                glsHandler = new GLSHandler();
                glsHandler.GridData.Filename = filename;

                ShowMessage("Execution..." + filename);

                string[] columnNames = nfile.Columns.Cast<DataColumn>()
                    .Select(x => x.ColumnName)
                    .ToArray();


                if (glsHandler.Header(columnNames))
                {
                    foreach (DataRow row in nfile.Rows)
                    {
                        string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();

                        glsHandler.Next(fields);

                    }





                    if (glsHandler.DataValidation())
                    {

                        if (glsHandler.MakeXmlAndWeight())
                        {
                            if (glsHandler.FileFinish(file))
                            {
                                glsHandler.Success();
                                zOK++;
                            }
                        }



                        glsHandler.DropLines();
                    }
                }
                zGridDataList.Add(glsHandler.GridData);

            }


            Finish();


        }

        private void SelectFromForwarderPickup(VendorHandler vhandler, Label errorLabel)
        {
            Init();

            DateTime st;
            DateTime en;




            if (DateTime.TryParse(XuMinDate.Text, out st) && DateTime.TryParse(XuMaxDate.Text, out en))
            {

                XuErrorMessage.Text = "";

                var lst = new List<XMLRecord>();
                var xmlRec = new XMLRecord();
                xmlRec.Awb = "xxxxx";
                xmlRec.InvoiceNumber = XuInvoiceNumber.Text;
                xmlRec.InvoiceDate = en;
                lst.Add(xmlRec);




                if (vhandler.MakeCompairPic(lst, st, en.Date) != "")
                {
                    errorLabel.Visible = true;
                }

            }
            else
            {
                XuErrorMessage.Text = "Dates not legal !!!!!!!!!!";
            }

            
        }

    

        private void XUGTX_Click(object sender, EventArgs e)
        {
          
            SelectFromForwarderPickup(new HSHandler(),XuTranslationErrorHS);


        }




        private void XuClose_Click(object sender, EventArgs e)
        {
            XuDataGridError.Visible = false;
            XuClose.Visible = false;
        }

       





     


        private void WriteFile(string filename, string cap, List<string> datalist)
        {
            using (StreamWriter outputFile =
                new StreamWriter(filename))
            {
                outputFile.WriteLine(cap);
                foreach (var rec in datalist)
                {

                    outputFile.WriteLine(rec);




                }
            }
        }

       



       

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void XuFilterChange(object sender, EventArgs e)
        {
            XuEditTranslationGrid.CurrentCell = null;


            for (int i = 0; i < zTranlationListFull.Count; i++)
            {
                var tr = zTranlationListFull[i];
                XuEditTranslationGrid.Rows[i].Visible = string.IsNullOrEmpty(XuFilterKey.Text) || tr.Key.ToLower().Contains(XuFilterKey.Text.ToLower()) || tr.GTXName.ToLower().Contains(XuFilterKey.Text.ToLower());
            }



        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void XuHSTranslation_Click(object sender, EventArgs e)
        {
            EditTransFile(new HSHandler());
        }

        private void XuGetFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "csv files (*.csv)|*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    var filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();
                    var datalist = new List<PriceObject>();
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string line = reader.ReadLine();
                        line = reader.ReadLine();
                        while (line != null)
                        {
                            datalist.Add(new PriceObject(line));
                            line = reader.ReadLine();

                        }
                    }
                    VendorHandler vhand = null;
                    var carrier = datalist[0].Carrier;
                    if (carrier  == "GLS")
                    {
                      vhand=new GLSHandler();
                    }
                    if (carrier == "HS")
                    {
                        vhand = new HSHandler();

                    }
                    if (carrier == "DHL")
                    {
                        vhand = new DHLHandler();

                    }
                    var form = new Matchup();
                    form.zVendorHandler = vhand;
                    form.Show();
                    form.zOutData = datalist;
                    form.DisplayGrid();
                }
                

            }
        }

        private void XuShipX_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "csv files (*.csv)|*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    var filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();
                    var datalist = new List<PriceObject>();
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string line = reader.ReadLine();
                        line = reader.ReadLine();
                        while (line != null)
                        {
                            var po = new PriceObject(line);
                            po.Payer = "?";
                            po.Account = "-1";
                            po.CostPrice = po.Price;
                            po.CostEstimated = po.Price;
                            po.Price = -1;
                            datalist.Add(po);
                            line = reader.ReadLine();

                        }
                    }
                    if (datalist.Count > 0)
                    {

                        var d = datalist.Where(x => x.ForwId > 0).Select(x => x.ForwId).ToList();
                       var custandacc = new VendorHandler().GetCustomerAndAccount(d);
                        var reslist = custandacc.Select(x => new AccountMatch(x));
                        foreach (var c in reslist)
                        {

                            var found = datalist.Where(x => x.ForwId == c.ForwId);
                            if (reslist.Count(x => x.ForwId == c.ForwId) > 1)
                            {
                                 found = found.Where(x=>  c.CompanyAddress.Contains(x.Customer) ).ToList();
                            }
                           
                           
                            found.ToList().ForEach(x => { x.Account = c.Account  ;
                                x.Payer = c.Payer;
                            });    
   

                        }

                        var form = new Matchup();
                        form.zVendorHandler = new ShipXHandler();
                        form.Show();
                        form.zOutData = datalist;
                        form.DisplayGrid();
                    }

                }

               
            }
        }
    }
}
