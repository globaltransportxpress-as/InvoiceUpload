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

namespace UploadDHL
{
    public partial class Form1 : Form
    {

        private string actualTransFile = "";
        private List<GridData> zGridDataList;
        private int zOK;

        private List<TranslationRecord> zTranlationListFull;



        public Form1()
        {
            InitializeComponent();

            EditMode(false);
            FileDone.Text = "Welcome...............";

            var isShipx = Config.System == "ShipX";
            XuDHL.Enabled = !isShipx;
            XUGTX.Enabled = isShipx;
            XuGls.Enabled = !isShipx;
            XuFedex.Enabled = !isShipx;
            XuPdk.Enabled = !isShipx;

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
        }

        private void EditTransFile(string file)
        {
            EditMode(true);
            actualTransFile = file;
            var translation = new Translation(actualTransFile);
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
            EditTransFile(Config.TranslationFileDHL);
        }

        private void XuPdk_Click(object sender, EventArgs e)
        {



            Init();

            var path = Config.PDKRootFileDir + "\\In\\";
            var listfile = Directory.EnumerateFiles(path, "*.xlsx");
            var pdkHandler = new PdkHandler();
            if (pdkHandler.Error != "")
            {
                FileDone.Text = pdkHandler.Error;

                return;
            }

            foreach (string file in listfile.Where(x => !x.Contains("~")))
            {

                var nfile = ImportExceltoDatatable(file, "Fakturaspecifikation");
                pdkHandler = new PdkHandler();
                var filename = file.Replace(path, "");

                pdkHandler.GridData.Filename = filename;

                ShowMessage("Execution..." + filename);


                string[] columnNames = nfile.Columns.Cast<DataColumn>()
                    .Select(x => x.ColumnName)
                    .ToArray();
                pdkHandler.SetData(columnNames);


                foreach (DataRow row in nfile.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();

                    pdkHandler.SetData(fields);

                }

                if (pdkHandler.DataValidation())
                {

                    if (pdkHandler.MakeXmlAndWeight())
                    {
                        if (pdkHandler.FileFinish(file))
                        {
                            pdkHandler.Success();
                            zOK++;
                        }
                    }

                    pdkHandler.DropLines();
                }

                zGridDataList.Add(pdkHandler.GridData);

            }

            Finish();



        }

        private void XuEditTranslation_Click(object sender, EventArgs e)
        {

            EditTransFile(Config.TranslationFilePDK);
        }

        private void XuEditTransFedex_Click(object sender, EventArgs e)
        {
            EditTransFile(Config.TranslationFileFedex);
        }

        private void XuEditTransGLS_Click(object sender, EventArgs e)
        {
            EditTransFile(Config.TranslationFilePickupGLS);
        }

        private void XuGTXTrans_Click(object sender, EventArgs e)
        {
            EditTransFile(Config.TranslationFileGtx);
        }

        private void XuSaveTranslatioon_Click(object sender, EventArgs e)
        {
            var translation = new Translation(actualTransFile);

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




        private void XuFedex_Click(object sender, EventArgs e)
        {

            Init();
            var fedexHandler = new FedexHandler();
            if (fedexHandler.Error != "")
            {
                FileDone.Text = fedexHandler.Error;

                return;
            }
            var path = Config.FedexRootFileDir + "\\In\\";

            foreach (string file in Directory.EnumerateFiles(path, "*.csv"))
            {

                ;
                var filename = file.Replace(path, "");
                fedexHandler = new FedexHandler();
                fedexHandler.GridData.Filename = filename;

                ShowMessage("Execution..." + filename);


                try
                {

                    using (StreamReader fileStream = new StreamReader(file))
                    {

                        string header = fileStream.ReadLine();

                        if (fedexHandler.CheckHeader(header))
                        {
                            string line = fileStream.ReadLine();
                            while (line != null)
                            {
                                fedexHandler.Next(line);
                                line = fileStream.ReadLine();
                            }
                        }
                    }
                    if (fedexHandler.DataValidation())
                    {

                        if (fedexHandler.MakeXmlAndWeight())
                        {
                            if (fedexHandler.FileFinish(file))
                            {
                                fedexHandler.Success();
                                zOK++;
                            }
                        }

                        fedexHandler.DropLines();
                    }

                }
                catch (Exception ex)
                {
                    fedexHandler.GridData.Status = VendorHandler.E_ERROR;
                    fedexHandler.GridData.Comment = ex.Message;
                }
                zGridDataList.Add(fedexHandler.GridData);
            }

            Finish();

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

            Init();
            var path = Config.GLSRootFileDir + "\\In\\";
            var listfile = Directory.EnumerateFiles(path, "*.xlsx");
            var glsHandler = new GLSHandler();
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

        private void XUGTX_Click(object sender, EventArgs e)
        {
            Init();


            var path = Config.GTXRootFileDir + "\\In\\";
            var listfile = Directory.EnumerateFiles(path, "*.xlsx");
            foreach (string file in listfile.Where(x => !x.Contains("~")))
            {

                var nfile = ImportExceltoDatatable(file, "Ark1");
                var gtxHandler = new GTXHandler();
                gtxHandler.Error = "";
                var griddata = new GridData();
                griddata.Filename = file.Replace(path, "");
                griddata.JumpLineData = new List<string>();
                ShowMessage("Execution..." + griddata.Filename);

                string[] columnNames = nfile.Columns.Cast<DataColumn>()
                    .Select(x => x.ColumnName)
                    .ToArray();
                var head = string.Join(",", columnNames).Replace(" ", "")
                    .Replace("#", "");
                if (gtxHandler.Header(head))
                {
                    foreach (DataRow row in nfile.Rows)
                    {
                        string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();

                        gtxHandler.SetData(fields);
                    }
                    if (gtxHandler.DataValidation())
                    {
                        var invoces = gtxHandler.Records.Select(x => x.InvoiceNumber).Distinct().ToList();
                        var err = false;
                        foreach (var inv in invoces)
                        {
                            if (!gtxHandler.MakeXmlAndWeight(inv))
                            {
                                err = true;
                            }


                        }
                        if (!err && gtxHandler.FileFinish(file))
                        {
                            gtxHandler.Success();
                            zOK++;
                        }

                        gtxHandler.DropLines();
                    }
                }
                zGridDataList.Add(gtxHandler.GridData);

            }

            Finish();






        }




        private void XuClose_Click(object sender, EventArgs e)
        {
            XuDataGridError.Visible = false;
            XuClose.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {


            var path = Config.ConvertFolder + "\\";
            var listfile = Directory.EnumerateFiles(path, "*.xlsx");
            foreach (string file in listfile.Where(x => !x.Contains("~")))
            {

                var nfile = ImportExceltoDatatable(file, "Ark1");
                nfile.DefaultView.Sort = "INVOICEID";
                string priv = "";
                var datatb = nfile.Clone();
                var tblst = new List<DataTable>();
                var outfile = "";
                var rowcount = 0;
                foreach (DataRow row in nfile.Rows)
                {
                    if (priv != "" && priv != row["INVOICEID"].ToString())
                    {
                        tblst = new List<DataTable>();
                        tblst.Add(datatb);
                        outfile = Config.ConvertFolder + "Done\\" + priv + "_" + DateTime.Now.ToString("yyyy-MM-dd");
                        if (rowcount > 146)
                        {
                            Export(outfile,
                                "Fakturadata",
                                "Normal",
                                datatb);
                        }
                        else
                        {
                            ExportToExcel(datatb, outfile + ".xlsx", true);
                        }

                        rowcount = 0;
                        datatb = nfile.Clone();

                    }
                    priv = row["INVOICEID"].ToString();
                    datatb.Rows.Add(row.ItemArray);
                    rowcount++;


                }
                tblst = new List<DataTable>();
                tblst.Add(datatb);
                outfile = Config.ConvertFolder + "Done\\" + priv + "_" + DateTime.Now.ToString("yyyy-MM-dd");
                if (rowcount > 146)
                {
                    Export(outfile,
                        "Fakturadata",
                        "Normal",
                        datatb);
                }
                else
                {
                    ExportToExcel(datatb, outfile + ".xlsx", true);
                }
            }
            FileDone.Text = "Done ...";



        }






        public static string Export(string excelFileName,
            string excelWorksheetName,
            string tableStyle,
             System.Data.DataTable dt)
        {
            var excel = new Excel.Application();
            excel.DisplayAlerts = false;
            excel.Visible = false;
            excel.ScreenUpdating = false;

            Workbooks workbooks = excel.Workbooks;
            Workbook workbook = workbooks.Add(Type.Missing);

            // Count of data tables provided.


            Sheets worksheets = workbook.Sheets;
            Worksheet worksheet = (Worksheet)worksheets[1];
            worksheet.Name = excelWorksheetName;

            int rows = dt.Rows.Count;
            int columns = dt.Columns.Count;
            // Add the +1 to allow room for column headers.
            var data = new object[rows + 1, columns];

            // Insert column headers.
            for (var column = 0; column < columns; column++)
            {
                data[0, column] = dt.Columns[column].ColumnName;
            }

            // Insert the provided records.
            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    data[row + 1, column] = dt.Rows[row][column];
                }
            }

            // Write this data to the excel worksheet.
            Range beginWrite = (Range)worksheet.Cells[1, 1];
            Range endWrite = (Range)worksheet.Cells[rows + 1, columns];
            Range sheetData = worksheet.Range[beginWrite, endWrite];
            sheetData.Value2 = data;

            // Additional row, column and table formatting.
            worksheet.Select();
            sheetData.Worksheet.ListObjects.Add(XlListObjectSourceType.xlSrcRange,
                sheetData,
                System.Type.Missing,
                XlYesNoGuess.xlYes,
                System.Type.Missing).Name = excelWorksheetName;
            sheetData.Select();
            //   sheetData.Worksheet.ListObjects[excelWorksheetName[i]].TableStyle = tableStyle;
            excel.Application.Range["2:2"].Select();
            excel.ActiveWindow.FreezePanes = true;
            excel.ActiveWindow.DisplayGridlines = false;
            excel.Application.Cells.EntireColumn.AutoFit();
            excel.Application.Cells.EntireRow.AutoFit();

            // Select the first cell in the worksheet.
            excel.Application.Range["$A$2"].Select();


            // Turn off alerts to prevent asking for 'overwrite existing' and 'save changes' messages.
            excel.DisplayAlerts = false;

            // Save our workbook and close excel.
            string SaveFilePath = string.Format(@"{0}.xlsx", excelFileName);
            workbook.SaveAs(SaveFilePath, XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing);
            workbook.Close(false, Type.Missing, Type.Missing);
            excel.Quit();

            // Release our resources.
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(workbooks);
            Marshal.ReleaseComObject(excel);
            Marshal.FinalReleaseComObject(excel);

            return SaveFilePath;
        }

        private void XuZeroCost_Click(object sender, EventArgs e)
        {

            var path = Config.ConvertFolder;
            var listfile = Directory.EnumerateFiles(path, "*.xml");
            foreach (string file in listfile.Where(x => !x.Contains("~")))
            {

                var doc = new XmlDocument();
                doc.Load(file);

                foreach (XmlNode d in doc.SelectNodes("//ShipmentList/Shipment/AXDetails/AXLine/CostPrice"))
                {
                    d.InnerText = "0,010";
                }

                doc.Save(file.Replace(path, path + "Done\\"));
            }
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

        private void XuSplitWeigth_Click(object sender, EventArgs e)
        {
            var path = Config.ConvertFolder;
            var listfile = Directory.EnumerateFiles(path, "*.csv");
            foreach (string file in listfile.Where(x => !x.Contains("~")))
            {


                using (StreamReader fileStream = new StreamReader(file))
                {

                    string cap = fileStream.ReadLine();


                    var datalist = new List<string>();
                    string line = fileStream.ReadLine();
                    var tp = line.Split(';')[2];
                    var count = 0;
                    var fno = 0;
                    while (line != null)
                    {
                        var ntp = line.Split(';')[2];
                        if (tp != ntp || count > 500)

                        {
                            if (count < 100)
                            {
                                tp = ntp;
                            }
                            else
                            {
                                string pno = fno.ToString();
                                if (fno == 0)
                                {
                                    pno = "";
                                }
                                var filename = file.Replace(path, path + "Done\\").Replace(".csv", tp + "_" + pno + ".csv");

                                WriteFile(filename, cap, datalist);
                                datalist = new List<string>();
                                if (tp == ntp)
                                {
                                    fno++;
                                }
                                else
                                {
                                    fno = 0;
                                }
                                tp = ntp;
                                count = 0;
                            }





                        }

                        datalist.Add(line);
                        count++;


                        line = fileStream.ReadLine();
                    }



                }
            }

        }


        public static void ExportToExcel(DataTable dataTable, String filePath, bool overwiteFile = true)
        {
            // If using Professional version, put your serial key below.
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("Fakturadata");



            // Insert DataTable to an Excel worksheet.
            worksheet.InsertDataTable(dataTable,
                new InsertDataTableOptions()
                {
                    ColumnHeaders = true,
                    StartRow = 0
                });

            workbook.Save(filePath);

        }





        private void XuRasFileMaker_Click(object sender, EventArgs e)
        {

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

        private void XuPalleData_Click(object sender, EventArgs e)
        {
            EditMode(false);
            FileDone.Text = "Start...........>>>>";

            var msglist = new List<GridData>();
            var cok = 0;

            var con = new DFEEntities();


            var path = Config.PDKRootFileDir + "\\Pallet\\Report";
            var listfile = Directory.EnumerateFiles(path, "*.xls");
            foreach (string file in listfile.Where(x => !x.Contains("~")))
            {

                var nfile = ImportExceltoDatatable(file, "Results");
                var palletHandler = new PalletHandler();
                palletHandler.Error = "";
                palletHandler.ErrorHandler.File = file;
                var griddata = new GridData();
                griddata.Filename = file.Replace(path, "");
                griddata.JumpLineData = new List<string>();
                ShowMessage("Execution..." + griddata.Filename);

                string[] columnNames = nfile.Columns.Cast<DataColumn>()
                    .Select(x => x.ColumnName)
                    .ToArray();



                foreach (DataRow row in nfile.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                    var record = palletHandler.SetData(fields);

                    var ex = con.PDKPalletReport.Where(x => x.Fakturanummer == record.Fakturanummer && x.Forsendelsenummer == record.Forsendelsenummer);
                    con.PDKPalletReport.RemoveRange(ex);
                    con.PDKPalletReport.Add(record.Convert());
                }
                con.SaveChanges();
                try
                {

                    File.Move(file, file.Replace("\\Pallet\\Report", "\\Done\\"));
                }
                catch (Exception ex)
                {

                    File.Move(file, file.Replace("\\Pallet\\Report", "\\Done\\" + DateTime.Now.ToString("ddHHmm")));
                }
            }
            ShowMessage("Done Reports");
            this.Refresh();
            path = Config.PDKRootFileDir + "\\Pallet\\Factura";
            listfile = Directory.EnumerateFiles(path, "*.xls");
            foreach (string file in listfile.Where(x => !x.Contains("~")))
            {

                var nfile = ImportExceltoDatatable(file, "faktura");

                ShowMessage("Execution..." + file.Replace(path, ""));




                string factura = "";
                bool okhead = false;
                foreach (DataRow row in nfile.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                    var record = new PalletRecord(fields, factura, okhead);
                    factura = record.Factura;
                    if (okhead && !string.IsNullOrWhiteSpace(record.Afsender))
                    {
                        var ex = con.PDKPalletrecord.Where(x => x.Factura == record.Factura && x.Sendingsnummer == record.Sendingsnummer);
                        con.PDKPalletrecord.RemoveRange(ex);
                        con.PDKPalletrecord.Add(record.Convert());

                    }
                    okhead = record.HeaderOK;
                }
                con.SaveChanges();
                try
                {

                    File.Move(file, file.Replace("\\Pallet\\Factura", "\\Done\\"));
                }
                catch (Exception ex)
                {

                    File.Move(file, file.Replace("\\Pallet\\Factura", "\\Done\\" + DateTime.Now.ToString("ddHHmm")));
                }



            }
            FileDone.Text = "Done Factura";
            this.Refresh();
            path = Config.PDKRootFileDir + "\\Pallet\\Colli";
            listfile = Directory.EnumerateFiles(path, "*.xls");
            foreach (string file in listfile.Where(x => !x.Contains("~")))
            {

                var nfile = ImportExceltoDatatable(file, "faktura");

                ShowMessage("Execution..." + file.Replace(path, ""));




                string factura = "";
                bool okhead = false;
                foreach (DataRow row in nfile.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                    var record = new PalletColliRecord(fields, factura, okhead);
                    factura = record.Factura;
                    if (okhead && !string.IsNullOrWhiteSpace(record.Afsender))
                    {
                        var ex = con.PDKPalletCollirecord.Where(x => x.Factura == record.Factura && x.Sendingsnummer == record.Sendingsnummer);
                        con.PDKPalletCollirecord.RemoveRange(ex);
                        con.PDKPalletCollirecord.Add(record.Convert());

                    }
                    okhead = record.HeaderOK;
                }
                con.SaveChanges();
                try
                {

                    File.Move(file, file.Replace("\\Pallet\\Colli", "\\Done\\"));
                }
                catch (Exception ex)
                {

                    File.Move(file, file.Replace("\\Pallet\\Colli", "\\Done\\" + DateTime.Now.ToString("ddHHmm")));
                }
            }
            FileDone.Text = "Finish";
            this.Refresh();
        }
    }
}
