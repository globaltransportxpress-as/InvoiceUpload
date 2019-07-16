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

using Excel = Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

using UploadDHL.DataUploadWeb;

namespace UploadDHL
{
    public partial class Form1 : Form
    {

        private string actualTransFile = "";
        private List<GridData> _currentStatusList;
        private List<string> JumpLines { get; set; }
        private Excel.Application excel = new Excel.Application();
        private InvoiceShipmentLoad invoiceShipmentLoad = new InvoiceShipmentLoad();
        private ErrorHandler zErrorHandler = new ErrorHandler();
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



        private void XuDHL_Click(object sender, EventArgs e)
        {
            // Append text to an existing file named "WriteLines.txt".
            EditMode(false);
            FileDone.Text = "Start...........>>>>";
            zErrorHandler = new ErrorHandler();
            JumpLines = new List<string>();
            using (StreamWriter logFile = new StreamWriter(Config.LogFile, true))
            {



                var errors = 0;
                var oks = 0;

                var OK = true;
                var msglist = new List<GridData>();
                // XuMessageBox.Items.Clear();
                var path = Config.DHLRootFileDir + "\\In\\";
                foreach (string file in Directory.EnumerateFiles(path, "*.csv"))
                {

                    OK = true;
                    var griddata = new GridData();
                    griddata.Filename = file.Replace(path, "");
                    griddata.JumpLineData = new List<string>();
                    zErrorHandler.File = file;
                    var jumpline = 0;
                    FileDone.Text = "Execution..." + griddata.Filename;
                    this.Refresh();
                    System.Windows.Forms.Application.DoEvents();
                    var dhlHandler = new DHLHandler();
                    dhlHandler.ErrorHandler = zErrorHandler;
                    try
                    {

                        using (StreamReader fileStream = new StreamReader(file))
                        {

                            string header = fileStream.ReadLine();

                            dhlHandler.Start(header);

                            string line = fileStream.ReadLine();
                            while (line != null)
                            {

                                if (!dhlHandler.Next(line))
                                {
                                    griddata.JumpLineData.Add(line);
                                    jumpline = jumpline + 1;
                                }


                                line = fileStream.ReadLine();
                            }
                            dhlHandler.MakeXmlAndWeightfile();


                        }

                        griddata.JumpLines = jumpline;
                        if (dhlHandler.Error)
                        {
                            griddata.Status = "ERROR";
                            if (dhlHandler.TranslationError)
                            {

                                griddata.Status = "TRANSLATION";

                            }
                            if (dhlHandler.FormatError)
                            {

                                griddata.Status = "FORMAT";

                            }

                            griddata.Comment = dhlHandler.zReasonError.ToString();
                        }
                        else
                        {
                            var fname = Path.GetFileName(file);
                            try
                            {

                                File.Move(file, Config.DHLRootFileDir + "\\Done\\" + fname);
                            }
                            catch (Exception ex)
                            {

                                File.Move(file,
                                    Config.DHLRootFileDir + "\\Done\\" + DateTime.Now.ToString("ddHHmm") + fname);
                            }
                            MoveFiles(Config.DHLRootFileDir + "\\XML\\", "DHL");
                            griddata.Status = "OK";
                            griddata.Comment = "";



                        }
                        oks++;
                       
                    }
                    catch (Exception ex)
                    {
                        zErrorHandler.Add("FileRead",ex.Message, "XuDHL_Click");
                        griddata.Status = "Error";
                        griddata.Comment = ex.Message;
                    }

                    msglist.Add(griddata);










                }
                FileDone.Text = "Done " + oks + " files";
             
               
                logFile.Flush();
                logFile.Close();

                XuMsgGrid.DataSource = msglist;
            }

        }

        private void EditMode(bool show)
        {
            FileDone.Text = "";
            XuMsgGrid.Visible = !show;
            XuEditTranslationGrid.Visible = show;
            XuSaveTranslatioon.Visible = show;
            XuJumpLines.Visible = false;
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

            zTranlationListFull= list.OrderBy(x => x.KeyType).ThenBy(x => x.GTXName).ToList();
            XuEditTranslationGrid.DataSource = zTranlationListFull;
        }

        private void Xu_EditDHL_Click(object sender, EventArgs e)
        {
            EditTransFile(Config.TranslationFileDHL);
        }

        private void XuPdk_Click(object sender, EventArgs e)
        {
            EditMode(false);
            FileDone.Text = "Start...........>>>>";
            zErrorHandler = new ErrorHandler();
            var msglist = new List<GridData>();
            var cok = 0;


           

            var path = Config.PDKRootFileDir + "\\In\\";
            var listfile = Directory.EnumerateFiles(path, "*.xlsx");
            foreach (string file in listfile.Where(x => !x.Contains("~")))
            {
                zErrorHandler.File = file;
                var nfile = ImportExceltoDatatable(file, "Fakturaspecifikation");
                var pdkHandler = new PdkHandler();
                pdkHandler.Error = "";
                pdkHandler.ErrorHandler = zErrorHandler;
                var griddata = new GridData();
                griddata.Filename = file.Replace(path, "");
                griddata.JumpLineData = new List<string>();
                FileDone.Text = "Execution..." + griddata.Filename;
                this.Refresh();
                System.Windows.Forms.Application.DoEvents();
                string[] columnNames = nfile.Columns.Cast<DataColumn>()
                    .Select(x => x.ColumnName)
                    .ToArray();
                pdkHandler.SetData(columnNames);


                foreach (DataRow row in nfile.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();

                    if (pdkHandler.SetData(fields) == null)
                    {
                        griddata.JumpLineData.Add(string.Join(";", fields));
                    }

                }





              
                if (pdkHandler.ErrorWeight != "")
                {
                   
                    Message.Text = pdkHandler.ErrorWeight;
                    griddata.Status = "Weight Zerro";
                }
                else {
                if (pdkHandler.Error != "")
                {
                    Message.Text = "Translation missing ";
                    griddata.Status = "TRANSLATION";


                }
                else
                {
                    if (pdkHandler.Records.Count == 0)
                    {
                        if (pdkHandler.Dic == null)
                        {
                            Message.Text = "Translation missing ";
                            griddata.Status = "ERROR";
                            griddata.Comment = "Header not found";

                        }
                        else
                        {
                            griddata.Status = "ERROR";
                            griddata.Comment = "No data records found";

                            Message.Text = "Data have wrong format";

                        }

                    }
                    else
                    {
                        WeightFileObj.CreateFile(Config.PDKRootFileDir,
                            pdkHandler.Records.Select(x => x.Convert()).ToList(), pdkHandler.Factura);
                        pdkHandler.MakeXML2();
                        var listInvShip = new List<InvoiceShipmentHolder>();
                        var ccount = 0;
                        var errorlist = new List<string>();
                        foreach (var record in pdkHandler.Records.Where(x => x.Price > 0).ToList())
                        {

                           
                           invoiceShipmentLoad.AddShipment(record.StdConvert());

                        }
                       
                       
                        if (invoiceShipmentLoad.Run()=="OK")
                        {

                            MoveFiles(Config.PDKRootFileDir + "\\XML\\", "PDK");
                            griddata.Status = "OK";
                            griddata.Comment = "Success";
                            griddata.JumpLines = pdkHandler.DropLines;



                            try
                            {

                                File.Move(file, file.Replace("\\In\\", "\\Done\\"));
                            }
                            catch (Exception ex)
                            {

                                File.Move(file, file.Replace("\\In\\", "\\Done\\" + DateTime.Now.ToString("ddHHmm")));
                            }


                        }
                        else
                        {
                            griddata.Status = "Error";
                            griddata.Comment = "Upload data to web error";
                            }
                       

                      


                    }


                }
                }
                msglist.Add(griddata);


                cok++;

            }
          
            FileDone.Text = "Done ..." + cok + " files";

            XuMsgGrid.DataSource = msglist;



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
            EditTransFile(Config.TranslationFileGLS);
        }

        private void XuGTXTrans_Click(object sender, EventArgs e)
        {
            EditTransFile(Config.TranslationFileGtx);
        }

        private void XuSaveTranslatioon_Click(object sender, EventArgs e)
        {
            var translation = new Translation(actualTransFile);

            translation.SaveAll((List<TranslationRecord>) XuEditTranslationGrid.DataSource);

            EditMode(false);

        }

        private void XuMsgGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView) sender;

            if (
                e.RowIndex >= 0)
            {
                var gd = (List<GridData>) XuMsgGrid.DataSource;
                XuJumpLines.Text = string.Join(Environment.NewLine, gd[e.RowIndex].JumpLineData);
                XuJumpLines.Visible = true;
                XuClose.Visible = true;
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


        private void MoveFiles(string from , string carrier)
        {

            

                
                string destPath = Config.EndDir(carrier);


           


                if (System.IO.Directory.Exists(destPath))
                {
                    string[] files = System.IO.Directory.GetFiles(from );


                    foreach (string s in files)
                    {

                        try
                        {
                            // Use static Path methods to extract only the file name from the path.
                            var fileName = System.IO.Path.GetFileName(s);
                            var destFile = System.IO.Path.Combine(destPath, fileName);
                            System.IO.File.Move(s, destFile);


                        }
                        catch (Exception)
                        {



                        }


                    }
                }
                else
                {

                    FileDone.Text = "Destination path does not exist!";
               
                }
            
            


        }

        private void XuFedex_Click(object sender, EventArgs e)
        {
            EditMode(false);
            FileDone.Text = "Start...........>>>>";
            using (StreamWriter logFile = new StreamWriter(Config.LogFile, true))
            {



                var errors = 0;
                var oks = 0;

                var OK = true;
                var msglist = new List<GridData>();
                // XuMessageBox.Items.Clear();
                var path = Config.FedexRootFileDir + "\\In\\";
                foreach (string file in Directory.EnumerateFiles(path, "*.csv"))
                {

                    OK = true;
                    var griddata = new GridData();
                    griddata.Filename = file.Replace(path, "");
                    griddata.JumpLineData = new List<string>();
                    var jumpline = 0;
                    FileDone.Text = "Execution..." + griddata.Filename;
                    this.Refresh();
                    System.Windows.Forms.Application.DoEvents();
                    var fedexHandler = new FedexHandler();
                    try
                    {

                        using (StreamReader fileStream = new StreamReader(file))
                        {

                            string header = fileStream.ReadLine();

                            fedexHandler.Start(header);

                            string line = fileStream.ReadLine();
                            while (line != null)
                            {

                                if (!fedexHandler.Next(line))
                                {
                                    griddata.JumpLineData.Add(line);
                                    jumpline = jumpline + 1;
                                }


                                line = fileStream.ReadLine();
                            }
                            if (!fedexHandler.Error)
                            {
                                fedexHandler.MakeXmlAndWeightfile(griddata.Filename.Replace(".csv", ""));
                            }
                            


                        }

                        griddata.JumpLines = jumpline;
                        if (fedexHandler.Error)
                        {
                            griddata.Status = "ERROR";
                            if (fedexHandler.TranslationError)
                            {

                                griddata.Status = "TRANSLATION";

                            }
                            if (fedexHandler.FormatError)
                            {

                                griddata.Status = "FORMAT";

                            }

                            griddata.Comment = fedexHandler.ReasonError.ToString();
                        }
                        else
                        {
                            var fname = Path.GetFileName(file);
                            try
                            {

                                File.Move(file, Config.FedexRootFileDir + "\\Done\\" + fname);
                            }
                            catch (Exception ex)
                            {

                                File.Move(file,
                                    Config.FedexRootFileDir + "\\Done\\" + DateTime.Now.ToString("ddHHmm") + fname);
                            }
                            MoveFiles(Config.FedexRootFileDir + "\\XML\\", "Fedex");
                            griddata.Status = "OK";
                            griddata.Comment = "";



                        }
                        oks++;
                    }
                    catch (Exception ex)
                    {
                        griddata.Status = "Error";
                        griddata.Comment = ex.Message;
                    }

                    msglist.Add(griddata);

                }

               
                FileDone.Text = "Done " + oks + " files";
                logFile.Flush();
                logFile.Close();

                XuMsgGrid.DataSource = msglist;
            }

        }



        private void XuGls_Click(object sender, EventArgs e)
        {

            EditMode(false);
            FileDone.Text = "Start...........>>>>";
            var msglist = new List<GridData>();
            var cok = 0;




            var path = Config.GLSRootFileDir + "\\In\\";
            var listfile = Directory.EnumerateFiles(path, "*.xlsx");
            foreach (string file in listfile.Where(x => !x.Contains("~")))
            {

                var nfile = ImportExceltoDatatable(file, "Fakturaspecifikation");
                var glsHandler = new GLSHandler();
                glsHandler.Error = "";
                var griddata = new GridData();
                griddata.Filename = file.Replace(path, "");
                griddata.JumpLineData = new List<string>();
                FileDone.Text = "Execution..." + griddata.Filename;
                this.Refresh();
                System.Windows.Forms.Application.DoEvents();
                string[] columnNames = nfile.Columns.Cast<DataColumn>()
                    .Select(x => x.ColumnName)
                    .ToArray();
                if (glsHandler.Header(string.Join(";", columnNames).Replace(" ", "").Replace("#", "")))
                {
                    foreach (DataRow row in nfile.Rows)
                    {
                        string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();

                        if (glsHandler.SetData(fields) == null)
                        {


                            griddata.JumpLineData.Add(string.Join(";", fields));


                        }

                    }
                }
                else
                {


                    griddata.Status = "ERROR";
                    griddata.Comment = "Header format not matching";



                }



                if (glsHandler.Error != "")
                {
                    Message.Text = "Translation missing ";
                    griddata.Status = "TRANSLATION";


                }

                else
                {
                    if (glsHandler.Records.Count == 0)
                    {

                        griddata.Status = "ERROR";
                        griddata.Comment = "No data records found";





                    }
                    else
                    {
                        WeightFileObj.CreateFile(Config.GLSRootFileDir,
                            glsHandler.Records.Select(x => x.Convert()).ToList(), glsHandler.Factura);
                        glsHandler.MakeXML2();
                        

                      
                      
                       
                        foreach (var record in glsHandler.Records
                            .Where(x => x.Beløb > 0 && x.GTXTranslate.KeyType == "FRAGT").ToList())
                        {

                            invoiceShipmentLoad.AddShipment(record.StdConvert());

                        }


                        if (invoiceShipmentLoad.Run() == "OK")
                        {
                            MoveFiles(Config.GLSRootFileDir + "\\XML\\", "GLS");
                            griddata.Status = "OK";
                            griddata.Comment = "Success";
                            griddata.JumpLines = glsHandler.DropLines;

                            Message.Text = "Done";

                            try
                            {

                                File.Move(file, file.Replace("\\In\\", "\\Done\\"));
                            }
                            catch (Exception ex)
                            {

                                File.Move(file, file.Replace("\\In\\", "\\Done\\" + DateTime.Now.ToString("ddHHmm")));
                            }

                        }
                        else
                        {
                            griddata.Status = "ERROR";
                            griddata.Comment = "Upload web data error";
                            griddata.JumpLines = glsHandler.DropLines;

                            Message.Text = "Done";
                        }


                    }


                }
                msglist.Add(griddata);


                cok++;

            }
            FileDone.Text = "Done ..." + cok + " files";


           


            XuMsgGrid.DataSource = msglist;


        }

        private void XUGTX_Click(object sender, EventArgs e)
        {
            EditMode(false);
            FileDone.Text = "Start...........>>>>";
            var msglist = new List<GridData>();
            var cok = 0;




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
                FileDone.Text = "Execution..." + griddata.Filename;
                this.Refresh();
                System.Windows.Forms.Application.DoEvents();
                string[] columnNames = nfile.Columns.Cast<DataColumn>()
                    .Select(x => x.ColumnName)
                    .ToArray();
                var noHeaderError = gtxHandler.Header(string.Join(";", columnNames).Replace(" ", "")
                    .Replace("#", ""));
                if (noHeaderError)
                {
                    foreach (DataRow row in nfile.Rows)
                    {
                        string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();

                        if (gtxHandler.SetData(fields) == null)
                        {


                            griddata.JumpLineData.Add(string.Join(";", fields));


                        }

                    }
                }

                if (!noHeaderError)
                {


                    griddata.Status = "ERROR";
                    griddata.Comment = "Header format not matching";



                }
                if (gtxHandler.Error != "" && griddata.Status != "ERROR")
                {
                    if (gtxHandler.Error == "TRANSLATION")
                    {
                        griddata.Status = "TRANSLATION";
                        griddata.Comment = "Translation missing";
                    }
                    else
                    {
                        griddata.Status = "ERROR";
                        griddata.Comment = gtxHandler.Error;
                    }



                }
                if (gtxHandler.Records.Count == 0 && string.IsNullOrWhiteSpace(griddata.Status))
                {

                    griddata.Status = "ERROR";
                    griddata.Comment = "No data records found";





                }


                if (string.IsNullOrWhiteSpace(griddata.Status))
                {

                   var invoces = gtxHandler.Records.Select(x=>x.INVOICEID).Distinct().ToList();
                    foreach (var inv in invoces)
                    {

                        WeightFileObj.CreateFile(Config.GTXRootFileDir,
                            gtxHandler.Records.Where(x=>x.INVOICEID==inv).Select(x => x.Convert()).ToList(), inv);


                    }
                   
                    gtxHandler.MakeXML2();


                    MoveFiles(Config.GTXRootFileDir + "\\XML\\", "GTX");

                    griddata.Status = "OK";
                    griddata.Comment = "Success";
                    griddata.JumpLines = gtxHandler.DropLines;

                    Message.Text = "Done";

                    try
                    {

                        File.Move(file, file.Replace("\\In\\", "\\Done\\"));
                    }
                    catch (Exception ex)
                    {

                        File.Move(file, file.Replace("\\In\\", "\\Done\\" + DateTime.Now.ToString("ddHHmm")));
                    }


                }

                msglist.Add(griddata);
                cok++;
            }




           

            FileDone.Text = "Done ..." + cok + " files";





            XuMsgGrid.DataSource = msglist;
        }

        private void XuClose_Click(object sender, EventArgs e)
        {
            XuJumpLines.Visible = false;
            XuClose.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            

            var path = Config.ConvertFolder+"\\";
            var listfile = Directory.EnumerateFiles(path, "*.xlsx");
            foreach (string file in listfile.Where(x => !x.Contains("~")))
            {

                var nfile = ImportExceltoDatatable(file, "Ark1");
                nfile.DefaultView.Sort = "INVOICEID";
                string priv = "";
                var datatb  = nfile.Clone();
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
            FileDone.Text = "Done ..." ;



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
                Worksheet worksheet = (Worksheet) worksheets[ 1];
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
                Range beginWrite = (Range) worksheet.Cells[1, 1];
                Range endWrite = (Range) worksheet.Cells[rows + 1, columns];
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
                            else {
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


      public static void ExportToExcel( DataTable dataTable, String filePath, bool overwiteFile = true)
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
    }
}
