using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using nu.gtx.DbMain.Standard.PM;
using UploadDHL.DataUploadWeb;

namespace UploadDHL
{
    class FedexHandler
    {
       
        
        private Translation zTranslation = new Translation(Config.TranslationFileFedex);
        private DHLXML zDhlXml;
        private List<FedexRecord> FedexRecords;
        private int zCount;
        public bool Error { get; set; }
        public bool TranslationError { get; set; }
        public bool FormatError { get; set; }
        public StringBuilder ReasonError = new StringBuilder();
        private static string refhead = "Master EDI No,Invoice Number,Invoice Date,Type,Settle,Inv Charge,Trans Cnt,Bill-To Account,Cntry,Total VAT Amt,VAT Billed Curr,Total Tax Amt,Tax Billed Curr,Total Non-Tax Amt,Non-Tax Billed Curr,Consolidated Acct,Co Cd,Ground Tracking Number,Tracking Number,Rebill,Non-Dup,Ship Date,Svc,Pkg,Grd Svc,IPD Adr,Msg Cd 1,Ref 1,Ref 2,Ref 3,Store No,Cust PO No,Cust Dept No,Cust Inv No,RMA No,Device No,Device,Payor,Net Chrg,Curr,Chrg 1,Freight Amt,Chrg 2,Vol Disc,Chrg 3,Earned Disc,Chrg 4,Auto Disc,Chrg 5,Perf Price Disc,Chrg 6,Fuel Amt,Chrg 7,Resi Amt,Chrg 8,DAS Amt,Chrg 9,On-Call Amt,Chrg 10,D.V. Amt,Chrg 11,Sign Svc Amt,Chrg 12,Sat Amt,Chrg 13,Addn Hndlg Amt,Chrg 14,Adr Corr Amt,Chrg 15,GST Amt,Chrg 16,Duty Amt,Chrg 17,Adv Fee Amt,Chrg 18,Orig VAT Amt,Chrg 19,Misc 1 Amt,Chrg 20,Misc 2 Amt,Chrg 21,Misc 3 Amt,Exchg Rate,Exc Curr,Fuel Pct,EU Bd,Count,Call Tag,Dec Value,Customs Value,DV-Cus Curr,Entry Number,MTWT No,Scale,Pcs,Bill Wt,Orig Wt,Multi Wt,Wt Unit,Length,Width,Height,Dim Unit,Divisor,Misc 1,Misc 2,Misc 3,Shipper Name,Shipper Company,Shipper Dept,Shipper Address 1,Shipper Address 2,Shipper City,ST,Postal,Origin Zip,Cntry1,Region,Recipient Name,Recipient Company,Recipient Address 1,Recipient Address 2,Recipient City,ST2,Postal2,Cntry2,Handling,Delivery,Time,Final,Exceptn,Attempt Date,Attempt Time,Signature,Svc Area,COD Amt,COD Trkg No,PDue,PDue Inv,Svc Pct,Rev Threshold Amt,Orig Recip Adr 1,Orig Recip Adr 2,Original City,ST3,Postal3,VAT No,FedEx VAT No,Cross Ref No,Intl Ground Ship No";
        private List<string> refList= refhead.Split(',').Select(x => x.Replace(" ", "")).ToList();

        private decimal zTotalPrice;
        private decimal zTotalWeight;
        private decimal zTotalTax;
        private decimal zTotalFee;
       

        public bool Start(string header)
        {
            FormatError = false;
            if (CheckHeader(header))
            {
                TranslationError = false;
               

               
               
               
            }

            zDhlXml = new DHLXML();
            FedexRecords = new List<FedexRecord>();

            return Error;
        }

        private bool CheckHeader(String data)
        {


            var da = data.Split(',').Select(x => x.Replace("\"", "").Replace(" ","")).ToList();
            for (int i = 0; i < refList.Count; i++)
            {
                if (refList[i] != da[i])
                {

                    ReasonError.AppendLine("Header not matching on " +refList[i]);
                    Error = true;
                    FormatError = true;
                    return false;

                }
            }
            return true;
        }
        public bool Next(string line)
        {



           var record = new FedexRecord(line, ReasonError, zTranslation);
            if (record.Error)
            {
                Error = true;
            }
            if (record.TranslationError)
            {
                TranslationError = true;
                Error = true;
            }
            if (!Error)
            {
                bool add = true;
                if (record.GTXTranslate != null)
                {
                    add = (record.GTXTranslate.KeyType == "GEBYR" || record.GTXTranslate.KeyType == "FRAGT");
                    if (record.GTXTranslate.KeyType == "GEBYR")
                    {
                        var rec = FedexRecords.FirstOrDefault(x => x.Tracking_Number == record.Tracking_Number && x.Invoice_Number == record.Invoice_Number && x.GTXTranslate.KeyType == "FRAGT");
                        var lst = record.Services;
                        if (rec != null)
                        {
                            add = false;
                            lst = rec.Services;
                            foreach (var se in record.Services)
                            {
                                lst.Add(se);
                            }


                        }
                        //lst.Add(new Service
                        //{
                        //    GTXCode = record.GTXTranslate.GTXName,
                        //    Price = record.Total_amount_incl_VAT

                        //});

                    }
                }


                if (add)
                {
                    FedexRecords.Add(record);
                }

            }

            return true;

        }

        public void MakeXmlAndWeightfile(string filename)
        {
            decimal oil = 0;
            FedexRecord privrec = null;
            var context = new DbMainStandard();
          
            var sb = new StringBuilder();
            var wfList = new List<WeightFileRecord>();
            foreach (var record in FedexRecords.OrderBy(x=>x.Invoice_Number).ToList())
            {
                if (privrec == null || privrec.Invoice_Number != record.Invoice_Number)
                {
                    context.InvoiceShipment.RemoveRange(context.InvoiceShipment.Where(x => x.Invoice == record.Invoice_Number && x.InvoiceDate == record.Invoice_Date));
                }
                if (privrec != null && privrec.Invoice_Number != record.Invoice_Number)
                {
                   
                    WriteXmlFile(privrec, oil, sb.ToString());
                    sb = new StringBuilder();
                    oil = 0;
                   

                }

                var sb2 = new StringBuilder();
                sb2.AppendLine(zDhlXml.FillServicesXml(record.GTXTranslate.GTXName, record.Freight_Amt + record.Vol_Disc));
              
                foreach (var service in record.Services)
                {
                    if (service.GTXCode == "FOILE")
                    {
                        oil = oil + service.Price;

                    }
                    
                    sb2.AppendLine(zDhlXml.FillServicesXml(service.GTXCode, service.Price));

                }
                
               
                sb.AppendLine(zDhlXml.FillShipmentXml(record.Tracking_Number, record.Ref_1, record.Ship_Date,
                    record.GTXTranslate.GTXName, record.Pcs, record.Bill_Wt, record.Freight_Amt+record.Vol_Disc,
                    record.Postal, record.Cntry1, record.Postal2, record.Cntry2, sb2.ToString()));

                wfList.Add(record.Convert());
                privrec = record;
               
                var listInvShip = new List<InvoiceShipmentHolder>();


                if (record.GTXTranslate.KeyType == "FRAGT")
                {

                    listInvShip.Add(record.StdConvert());
                }

                

                var _service = new InvoiceUploadSoapClient("InvoiceUploadSoap");
                var res = _service.ShipmentUpload(listInvShip.ToArray());
            }
            if (privrec != null)
            {
                WriteXmlFile(privrec, oil, sb.ToString());
            }
           

            WeightFileObj.CreateFile(Config.FedexRootFileDir, wfList, filename);
            

        }



        private void WriteXmlFile(FedexRecord rec, decimal oil, string xmlrec)
        {

            var xml = zDhlXml.FillFacturaXml(rec.Invoice_Number, rec.Invoice_Date, rec.Invoice_Date.AddDays(30), rec.Bill_To_Account, rec.Total_Non_Tax_Amt, oil, rec.Total_Tax_Amt, xmlrec);


            using (StreamWriter xmlout =
                new StreamWriter(Config.FedexRootFileDir + "\\Xml\\X" + rec.Invoice_Number + "_" + DateTime.Now.ToString("yyyyMMddmms") + ".xml", false))
            {
                xmlout.Write(xml);
            }



        }





    }
}
