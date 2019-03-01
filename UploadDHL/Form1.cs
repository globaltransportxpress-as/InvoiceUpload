using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nu.gtx.DbMain.Standard.PM;

namespace UploadDHL
{
    public partial class Form1 : Form
    {

        private string actualTransFile = "";
        public Form1()
        {
            InitializeComponent();

            EditMode(false);
            FileDone.Text = "Welcome...............";
        }



        private void XuDHL_Click(object sender, EventArgs e)
        {
            // Append text to an existing file named "WriteLines.txt".
            EditMode(false);
            FileDone.Text = "Start...........>>>>";
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
                    var jumpline = 0;
                    FileDone.Text = "Execution..." + griddata.Filename;
                    var dhlHandler = new DHLHandler();
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

                            griddata.Comment = dhlHandler.ReasonError.ToString();
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

        private void EditMode(bool show)
        {
            FileDone.Text = "";
            XuMsgGrid.Visible = !show;
            XuEditTranslationGrid.Visible = show;
            XuSaveTranslatioon.Visible = show;
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
            XuEditTranslationGrid.DataSource = list.OrderBy(x => x.KeyType).ThenBy(x => x.GTXName).ToList();
        }

        private void Xu_EditDHL_Click(object sender, EventArgs e)
        {
            EditTransFile(Config.TranslationFileDHL);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            EditMode(false);
            FileDone.Text = "Start...........>>>>";
            var msglist = new List<GridData>();
            var cok = 0;




            var path = Config.PDKRootFileDir + "\\In\\";
            var listfile = Directory.EnumerateFiles(path, "*.xlsx");
            foreach (string file in listfile.Where(x => !x.Contains("~")))
            {

                var nfile = ImportExceltoDatatable(file);
                var pdkHandler = new PdkHandler();
                pdkHandler.Error = "";
                var griddata = new GridData();
                griddata.Filename = file.Replace(path, "");
                FileDone.Text = "Execution..." + griddata.Filename;
                string[] columnNames = nfile.Columns.Cast<DataColumn>()
                    .Select(x => x.ColumnName)
                    .ToArray();
                pdkHandler.SetData(columnNames);

                foreach (DataRow row in nfile.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => field.ToString()).
                        ToArray();

                    pdkHandler.SetData(fields);

                }





                //using (StreamReader fileStream = new StreamReader(file))
                //{




                //    string line = fileStream.ReadLine();
                //    while (line != null)
                //    {
                //        pdkHandler.SetData(line.Split(';'));


                //        line = fileStream.ReadLine();

                //    }

                //}
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
                        WeightFile.CreateFile(Config.PDKRootFileDir, pdkHandler.Records.Select(x => x.Convert()).ToList(), pdkHandler.Factura);
                        pdkHandler.MakeXML2();

                        var context = new DbMainStandard();
                        context.InvoiceShipment.RemoveRange(context.InvoiceShipment.Where(x => x.Invoice == pdkHandler.Factura && x.InvoiceDate == pdkHandler.FacturaDate));
                        foreach (var record in pdkHandler.Records.Where(x => x.Price > 0).ToList())
                        {

                            context.InvoiceShipment.Add(record.StdConvert());

                        }
                        context.SaveChanges();



                        griddata.Status = "OK";
                        griddata.Comment = "Success";
                        griddata.JumpLines = pdkHandler.DropLines;

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

        private void XuSaveTranslatioon_Click(object sender, EventArgs e)
        {
            var translation = new Translation(actualTransFile);

            translation.SaveAll((List<TranslationRecord>)XuEditTranslationGrid.DataSource);

            EditMode(false);

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

        public DataTable ImportExceltoDatatable(string filepath)
        {
            // string sqlquery= "Select * From [SheetName$] Where YourCondition";
            string sqlquery = "Select * From [Fakturaspecifikation$]";
            DataSet ds = new DataSet();
            string constring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
            OleDbConnection con = new OleDbConnection(constring + "");
            OleDbDataAdapter da = new OleDbDataAdapter(sqlquery, con);
            da.Fill(ds);
            DataTable dt = ds.Tables[0];


            return dt;
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
                    var jumpline = 0;
                    FileDone.Text = "Execution..." + griddata.Filename;
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
                                   jumpline = jumpline + 1;
                               }


                               line = fileStream.ReadLine();
                           }
                            fedexHandler.MakeXmlAndWeightfile(griddata.Filename.Replace(".csv",""));


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

                var nfile = ImportExceltoDatatable(file);
                var glsHandler = new GLSHandler();
                glsHandler.Error = "";
                var griddata = new GridData();
                griddata.Filename = file.Replace(path, "");
                FileDone.Text = "Execution..." + griddata.Filename;
                string[] columnNames = nfile.Columns.Cast<DataColumn>()
                    .Select(x => x.ColumnName)
                    .ToArray();
              //  glsHandler.ReadHeadData(columnNames);

                foreach (DataRow row in nfile.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => field.ToString()).
                        ToArray();

                  //  glsHandler.SetData(fields);

                }





                //using (StreamReader fileStream = new StreamReader(file))
                //{




                //    string line = fileStream.ReadLine();
                //    while (line != null)
                //    {
                //        pdkHandler.SetData(line.Split(';'));


                //        line = fileStream.ReadLine();

                //    }

                //}
                if (glsHandler.Error != "")
                {
                    Message.Text = "Translation missing ";
                    griddata.Status = "TRANSLATION";


                }
                else
                {
                    if (glsHandler.Records.Count == 0)
                    {
                        if (glsHandler.Dic == null)
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
                        WeightFile.CreateFile(Config.PDKRootFileDir, glsHandler.Records.Select(x => x.Convert()).ToList(), glsHandler.Factura);
                        glsHandler.MakeXML2();

                        var context = new DbMainStandard();
                        context.InvoiceShipment.RemoveRange(context.InvoiceShipment.Where(x => x.Invoice == glsHandler.Factura && x.InvoiceDate == glsHandler.FacturaDate));
                        foreach (var record in glsHandler.Records.Where(x => x.Price > 0).ToList())
                        {

                            context.InvoiceShipment.Add(record.StdConvert());

                        }
                        context.SaveChanges();



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


                }
                msglist.Add(griddata);


                cok++;

            }
            FileDone.Text = "Done ..." + cok + " files";








        }
    }
}
