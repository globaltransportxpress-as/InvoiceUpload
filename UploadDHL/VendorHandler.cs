using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UploadDHL.DataConnections;

namespace UploadDHL
{
    public partial class VendorHandler
    {


        public string Error { get; set; }
        public string ErrorWeight { get; set; }

        public string RootDir { get; set; }
        public string CarrierName { get; set; }

        public GridData GridData = new GridData();

        public List<XMLRecord> Records = new List<XMLRecord>();
        private DHLXML zXml = new DHLXML();

        public ErrorHandler ErrorHandler { get; set; }
        public List<InvoiceLine> InvoiceLineList { get; set; }
        private InvoiceShipmentLoad zInvoiceShipmentLoad = new InvoiceShipmentLoad();

        public int  LineNumber { get; set; }

        public static string HEAD = "HEAD";
        public static string SUMHED = "SUMHEAD";
        public static string DROP = "DROP";
        public static string FRAGT = "FRAGT";
        public static string E_ERROR = "E_ERROR";
        public static string E_HEAD = "E_HEAD";
        public static string E_TRANS = "E_TRANS";
        public static string E_DATE = "E_DATE";
        public static string E_DECIMAL = "E_DECIMAL";

        public static string GEBYR = "GEBYR";
        public static string E_INI = "E_INI";
        public static string E_DIC = "E_DIC";
        public static string E_WEIGHT = "E_WEIGHT";
        public static string E_COUNTRY = "E_COUNTRY";
        public static string OVERWEIGHT = "OVERWEIGHT";
        public static string E_OW = "E_OW";


        public VendorHandler()
        {

            Error = "";
           
            InvoiceLineList = new List<InvoiceLine>();
            LineNumber = 0;
            




        }

        public bool MatchHeader(string head, string refhead, string sep)
        {

            

            AddInvoiceLine(head.Replace(sep, "|"), 1, HEAD);
            if (head.Contains(refhead) )
            {
                return true;
            }
            AddInvoiceLine(refhead.Replace(sep, "|"), 0, E_HEAD);
            GridData.Status = "ERROR";
            GridData.Comment = "Header format not matching";
            GridData.ErrorLines = InvoiceLineList;


            return false;


        }


        public bool DataValidation()
        {

            var errorList = InvoiceLineList.Where(x => x.Status.StartsWith("E_")).ToList();
            GridData.ErrorLines = InvoiceLineList.Where(x => x.Status != VendorHandler.FRAGT && x.Status != VendorHandler.GEBYR).ToList();
            if (errorList.Count > 0)
            {

                GridData.Status = "ERROR";
                GridData.Comment = "Antal:" + errorList.Count + " Koder:" + string.Join("; ", errorList.Select(x => x.Status).Distinct().ToArray());
              
                return false;

            }
            
            return true;




        }

        public bool AddServiceToShipment(List<XMLRecord> records, XMLRecord rec)
        {
            if (rec.KeyType == GEBYR)
            {
                var record = records.FirstOrDefault(x => x.Awb == rec.Awb && x.KeyType == FRAGT);

                if (record == null)
                {



                  
                    records.Add(rec);

                }
                else
                {
                    var sv = new Service
                    {
                        OrigalName = rec.CarrierService,
                        GTXCode = rec.GTXName,
                        Price = rec.Price,
                        Tax = rec.Vat,
                        InvoiceLineNumber = LineNumber
                    };

                    record.Services.Add(sv);
                }

                return true;


            }
            return false;







        }

        public void DropLines()
        {
            GridData.JumpLines = InvoiceLineList.Count(x => x.Status==VendorHandler.DROP);
        }

        public void Success()
        {
            GridData.Status = "OK";
            GridData.Comment = "Success";

        }

        public bool MakeGridComment(string msg, string ty)
        {
            if (msg != "")
            {

                GridData.Status = ty;
                GridData.Comment = msg;
                return false;

            }
            return true;
        }
        









        




        public bool RecordOK(DataRecord record,InvoiceLine iLine )
        {
            if (record.Awb == "")
            {

                iLine.Status = DROP;


                return false;
            }

            if (record.XmlRecord == null)
            {
                return false;
            }
            if (record.RecordStatus.StartsWith("E_"))
            {
                return false;
            }
            var chkYear = DateTime.Now.AddYears( - 2);
            if (record.XmlRecord.Shipdate < chkYear || record.XmlRecord.InvoiceDate < chkYear)
            {
                iLine.Status = E_DATE;
               

            }



            
            
            
            if (record.GTXTranslate.KeyType == FRAGT && record.XmlRecord.BilledWeight ==0)
            {


                iLine.Status = E_WEIGHT;

            }
            if ( record.XmlRecord.Price == 0)
            {


                iLine.Status = DROP;

            }




            return true;


        }
       
        public InvoiceLine AddInvoiceLine(string data, int lno, string status)
        {
            LineNumber=LineNumber+lno;
            var line = new InvoiceLine
            {


                Line = LineNumber,
                Raw =data,
                Fixed = "",
                Status =status

            };

            InvoiceLineList.Add(line);
            return line;
        }


        public void RegisterIvoceLine(XMLRecord rec, InvoiceLine iLine)
        {
            iLine.Status = rec.KeyType;
            iLine.GTXName = rec.GTXName;
            iLine.TransportProduct = rec.Transport;
            iLine.Product = rec.Product;
            iLine.Awb = rec.Awb;
            iLine.Factura = rec.InvoiceNumber;
        }
        public string MakeXmlAndWeightfile(List<XMLRecord> listRecords)
        {
            decimal oil = 0;
            decimal sumTotal = 0;
            decimal sumTotal_Tax = 0;
            XMLRecord lastrecord = null;
            var sb = new StringBuilder();
            var wfList = new List<WeightFileRecord>();
            var records = listRecords.Where(x => x.KeyType == VendorHandler.FRAGT || x.KeyType == VendorHandler.GEBYR).ToList();


            foreach (var record in records)
            {
                var sb2 = new StringBuilder();
                sb2.AppendLine(zXml.FillServicesXml(record.GTXName, record.BilledWeight));

                foreach (var service in record.Services)
                {

                    sb2.AppendLine(zXml.FillServicesXml(service.GTXCode, service.Price));

                }
                int pic = 1;

                sb.AppendLine(zXml.FillShipmentXml(record.Awb, record.Reference,
                    record.Shipdate,
                    record.GTXName, pic, record.BilledWeight, record.Price,
                    record.Zip, record.Country_Iata, record.Reciever_Zip,
                    record.Reciever_Country_Iata, sb2.ToString()));





                lastrecord = record;
                wfList.Add(record.Convert());


            }
            oil = records.Sum(x => x.Oil());

            var rec = listRecords.FirstOrDefault(x => x.KeyType == VendorHandler.HEAD);
            var invoiceno = "";
            var xml = "";
            if (rec != null)
            {
                invoiceno = rec.InvoiceNumber;
                xml = zXml.FillFacturaXml(rec.InvoiceNumber, rec.InvoiceDate, rec.Due_Date, rec.VendorAccount,
                    rec.Price, oil, rec.Vat, sb.ToString());
            }
            else
            {
                if (lastrecord != null)
                {
                    sumTotal = records.Sum(x => x.Total_Price());
                    sumTotal_Tax = records.Sum(x => x.Total_Vat());
                    invoiceno = lastrecord.InvoiceNumber;
                    xml = zXml.FillFacturaXml(lastrecord.InvoiceNumber, lastrecord.InvoiceDate,
                        lastrecord.Due_Date, lastrecord.VendorAccount, sumTotal, oil, sumTotal_Tax,
                        sb.ToString());
                }


            }

            if (xml != "")
            {
                using (StreamWriter xmlout =
                    new StreamWriter(
                        RootDir + "\\Xml\\X" + invoiceno + "_" +
                        DateTime.Now.ToString("yyyyMMddmms") + ".xml", false))
                {
                    xmlout.Write(xml);
                }
            }

            //if (1 != 1)
            if (lastrecord != null)
            {
                WeightFileObj.CreateFile(RootDir, wfList, lastrecord.InvoiceNumber);

                foreach (var record in records)
                {

                    zInvoiceShipmentLoad.AddShipment(record.StdConvert());

                }
                var status = zInvoiceShipmentLoad.Run();

                if (status != "OK")
                {


                    return status;


                }
            }


            return "";

        }
    

    public bool MakeXmlAndWeight(string invoicename)
        {

           
            return MakeGridComment(MakeXmlAndWeightfile(Records.Where(x=>x.InvoiceNumber==invoicename).ToList()),
                "MakeXmlAndWeight");



        }
        public bool MakeXmlAndWeight()
        {


            return MakeGridComment(MakeXmlAndWeightfile(Records),
                "MakeXmlAndWeight");



        }
        public bool FileFinish(string file)
        {
            return MakeGridComment(MoveAllFiles( file, CarrierName), "MoveFiles");
        }

        public string MoveAllFiles( string file, string carrier)
        {
            var fname = Path.GetFileName(file);
            try
            {

                File.Move(file, RootDir + "\\Done\\" + fname);
            }
            catch (Exception ex)
            {

                File.Move(file,
                    RootDir + "\\Done\\" + DateTime.Now.ToString("ddHHmm") + fname);
            }
            return MoveFiles(RootDir + "\\XML\\", carrier);
        }
        private string MoveFiles(string from, string carrier)
        {




            string destPath = Config.EndDir(carrier);


            if (System.IO.Directory.Exists(destPath))
            {
                string[] files = System.IO.Directory.GetFiles(from);


                foreach (string s in files)
                {

                    try
                    {
                        // Use static Path methods to extract only the file name from the path.
                        var fileName = System.IO.Path.GetFileName(s);
                        var destFile = System.IO.Path.Combine(destPath, fileName);
                        System.IO.File.Move(s, destFile);


                    }
                    catch (Exception ex)
                    {

                        return ex.Message;

                    }


                }
            }
            else
            {

                return "Destination path does not exist!";

            }


            return "";

        }

    }

}
