using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using nu.gtx.Common1.Constants;
using Newtonsoft.Json.Linq;
using UploadDHL.DataConnections;
using UploadDHL.GetForwarderId;
using UploadDHL.Properties.DataSources;


namespace UploadDHL
{
    public partial class VendorHandler
    {


        public string Error { get; set; }
        public string ErrorWeight { get; set; }

        public string RootDir { get; set; }
        public string CarrierName { get; set; }

        public GridData GridData = new GridData();
        public Translation Translation = null;

        public List<XMLRecord> Records = new List<XMLRecord>();
       

        public ErrorHandler ErrorHandler { get; set; }
        public List<InvoiceLine> InvoiceLineList { get; set; }
        private InvoiceShipmentLoad zInvoiceShipmentLoad = new InvoiceShipmentLoad();

        public int LineNumber { get; set; }

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
           


            return true;


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
            GridData.JumpLines = InvoiceLineList.Count(x => x.Status == VendorHandler.DROP);
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















        public bool RecordOK(DataRecord record, InvoiceLine iLine)
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
            var chkYear = DateTime.Now.AddYears(-2);
            if (record.XmlRecord.Shipdate < chkYear || record.XmlRecord.InvoiceDate < chkYear)
            {
                iLine.Status = E_DATE;


            }






            if (record.GTXTranslate.KeyType == FRAGT && record.XmlRecord.BilledWeight == 0)
            {


                iLine.Status = E_WEIGHT;

            }
            if (record.XmlRecord.Costprice == 0)
            {


                iLine.Status = DROP;

            }




            return true;


        }

        public InvoiceLine AddInvoiceLine(string data, int lno, string status)
        {
            LineNumber = LineNumber + lno;
            var line = new InvoiceLine
            {


                Line = LineNumber,
                Raw = data,
                Fixed = "",
                Status = status

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




           // try
           // {

                if (listRecords.Count == 0)
                {
                    return "No records";
                }



               

                var minDate = listRecords.Min(x => x.Shipdate);
                var maxDate = listRecords.Max(x => x.Shipdate);
            return MakeCompairPic(listRecords, minDate,maxDate);

        }

        public List<ForwarderRecord> TranslateRecords(List<ForwObj> foreobjs)
        {
           
                var forwarderRecordList = new List<ForwarderRecord>();
                foreach (var fw in foreobjs)
                {

                    forwarderRecordList.Add(new ForwarderRecord(fw, Translation));





                }
                if (forwarderRecordList.Any(x => !string.IsNullOrEmpty(x.Error)))
                {
                    
                    return null;
                }

            var plist = forwarderRecordList.SelectMany(x => x.PriceList).Select(x => x.AccountId + "|" + x.CompanyName)
                .Distinct().ToList();

            Translation.AddAccounts(plist);
            return forwarderRecordList;



        }

        public List<ForwObj> GetForwarderList( DateTime minDate, DateTime maxDate)
        {
            var soapClient = new GetForwarderIdSoapClient("GetForwarderIdSoap");
           
                var d = soapClient.GetForwarderList(CarrierName, minDate, maxDate);
                return  d.ToList();

        }

        public List<string> GetCustomerAndAccount(List<int> forwIds)
        {
            var soapClient = new GetForwarderIdSoapClient("GetForwarderIdSoap");

            return soapClient.GetOriginalAccount(forwIds.ToArray()).ToList();
            

        }



        public string MakeCompairPic(List<XMLRecord> listRecords,  DateTime minDate, DateTime maxDate)
        {
           
            try
            {

                var forwlist = GetForwarderList(minDate, maxDate).Where(x=>x.PriceList!=null && x.PriceList.Length >0).ToList();


           var forwarderList = TranslateRecords(forwlist);
                if (forwarderList == null)
                {
                    return "TRANSLATIONERROR";
                }
  




             var  forwList = forwarderList.Where(x => x.PriceList.Length > 0).ToList();
               
            string oldAwb = "";
            ForwarderRecord fwo = null;
            foreach (var record in listRecords.OrderBy(x => x.Awb))
            {
                if (oldAwb != record.Awb)
                {
                    oldAwb = record.Awb;
                    var fw = forwList.Where(x => x.PickupDate == record.Shipdate);

                    if (record.CarrierCode == "GLS")
                    {
                        fwo = fw.FirstOrDefault(x => x.OperatorFeedback.Contains(record.Awb) && ( x.CompanyName.ToUpper().Contains(record.CompanyName.ToUpper()) ||
                            record.CompanyName.ToUpper().Contains(x.CompanyName.ToUpper())));
                        if (fwo == null)
                        {
                            fwo = fw.FirstOrDefault(
                                x => x.Street.ToUpper().Contains(record.Address1.ToUpper()) ||
                                     record.Address1.ToUpper().Contains(x.Street.ToUpper()));
                        }
                        
                        
                    }
                    if (record.CarrierCode == "DHL")
                    {
                        var fwd = fw.Where(x => x.PickupDate == record.Shipdate);

                        fwo = fwd.FirstOrDefault(
                            x => x.Street.ToUpper().Contains(record.Address1.ToUpper()) ||
                                 record.Address1.ToUpper().Contains(x.Street.ToUpper()));
                        if (fwo == null)
                        {

                            fwo = fwd.FirstOrDefault(
                                x => x.CompanyName.ToUpper().Contains(record.CompanyName.ToUpper()) ||
                                     record.CompanyName.ToUpper().Contains(x.CompanyName.ToUpper()));

                        }


                    }
                    if (fwo!=null && fwo.PriceList.Select(x => x.AccountId).Distinct().Count() > 1)
                    {
                        fwo = null;
                    }
                    }



                record.ForwarderObj = fwo;
            }
            var form = new Matchup();


            form.Carrier = CarrierName;
            form.zVendorHandler = this;
            form.TranslationHandler = Translation;
            form.Show();
            form.InitData(listRecords, forwList);
            form.ShowData();


            return "Error";




            if (listRecords.Count > 0)
            {
                WeightFileObj.CreateFile(Config.GLSRootFileDir, listRecords, listRecords[0].InvoiceNumber);
            }

        }
        catch (Exception ex)
        {
            return ex.Message;
        }



        return "";
        }


        public bool MakeXmlAndWeight(string invoicename)
        {


            return MakeGridComment(MakeXmlAndWeightfile(Records.Where(x => x.InvoiceNumber == invoicename).ToList()),
                "MakeXmlAndWeight");



        }
        public bool MakeXmlAndWeight()
        {


            return MakeGridComment(MakeXmlAndWeightfile(Records),
                "MakeXmlAndWeight");



        }
        public bool FileFinish(string file)
        {
            return MakeGridComment(MoveAllFiles(file), "MoveFiles");
        }

        public string MoveAllFiles(string file)
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
            return MoveFiles(RootDir + "\\XML\\");
        }



        public bool ExportData(List<PriceObject> pricelist)
        {

            var acclist = pricelist.Select(x => x.Account).Distinct().ToList();
            var reslist = new List<JObject>();
            foreach (var d in acclist)
            {
                var thisawb = "";
                JArray thisArr = null;
                foreach (var plst in pricelist.Where(x => x.Account == d).OrderBy(x => x.Awb))
                {
                    if (plst.Awb != thisawb)
                    {
                        var obj = plst.ToJSonHead();
                            

                        thisArr =(JArray)obj["order_lines"]  ;
                        reslist.Add(obj);

                    }
                    thisArr.Add(plst.ToJSonLine());

                }
            }
            using (StreamWriter outputFile =
                new StreamWriter(new FileStream(RootDir + "\\XML\\"+ CarrierName + DateTime.Now.ToString("ddHHmmfff")+".json", FileMode.OpenOrCreate, FileAccess.Write),
                    Encoding.UTF8))
            {


                foreach (var lin in reslist)
                {

                    outputFile.WriteLine(lin.ToString());

                }

              
            }

            using (StreamWriter outputFile =
                new StreamWriter(new FileStream(RootDir + "\\XML\\Cost_" + CarrierName + DateTime.Now.ToString("ddHHmmfff") + ".csv", FileMode.OpenOrCreate, FileAccess.Write),
                    Encoding.UTF8))
            {
                outputFile.WriteLine("Factura;Awb;Account;Product;Id;Price;Estimated");

                foreach (var lin in pricelist.Select(x => string.Format("{0};{1};{2};{3};{4};{5:#.00};{6:#.00}", x.Factura,
                    x.Awb, x.Account,
                    x.DataType, x.TypeCostId, x.CostPrice, x.CostEstimated)))
                {
                    outputFile.WriteLine(lin);
                }

                
            }

            return true;


        }



        private string MoveFiles(string from)
        {




            string destPath = Config.EndDir(CarrierName);


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
