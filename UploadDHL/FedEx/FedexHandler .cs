using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

using UploadDHL.DataUploadWeb;

namespace UploadDHL
{
    class FedexHandler:VendorHandler
    {
       
        
        private Translation zTranslation = new Translation(Config.TranslationFileFedex);
      
       
 
       
       
        private static string refhead = "Master EDI No,Invoice Number,Invoice Date,Type,Settle,Inv Charge,Trans Cnt,Bill-To Account,Cntry,Total VAT Amt,VAT Billed Curr,Total Tax Amt,Tax Billed Curr,Total Non-Tax Amt,Non-Tax Billed Curr,Consolidated Acct,Co Cd,Ground Tracking Number,Tracking Number,Rebill,Non-Dup,Ship Date,Svc,Pkg,Grd Svc,IPD Adr,Msg Cd 1,Ref 1,Ref 2,Ref 3,Store No,Cust PO No,Cust Dept No,Cust Inv No,RMA No,Device No,Device,Payor,Net Chrg,Curr,Chrg 1,Freight Amt,Chrg 2,Vol Disc,Chrg 3,Earned Disc,Chrg 4,Auto Disc,Chrg 5,Perf Price Disc,Chrg 6,Fuel Amt,Chrg 7,Resi Amt,Chrg 8,DAS Amt,Chrg 9,On-Call Amt,Chrg 10,D.V. Amt,Chrg 11,Sign Svc Amt,Chrg 12,Sat Amt,Chrg 13,Addn Hndlg Amt,Chrg 14,Adr Corr Amt,Chrg 15,GST Amt,Chrg 16,Duty Amt,Chrg 17,Adv Fee Amt,Chrg 18,Orig VAT Amt,Chrg 19,Misc 1 Amt,Chrg 20,Misc 2 Amt,Chrg 21,Misc 3 Amt,Exchg Rate,Exc Curr,Fuel Pct,EU Bd,Count,Call Tag,Dec Value,Customs Value,DV-Cus Curr,Entry Number,MTWT No,Scale,Pcs,Bill Wt,Orig Wt,Multi Wt,Wt Unit,Length,Width,Height,Dim Unit,Divisor,Misc 1,Misc 2,Misc 3,Shipper Name,Shipper Company,Shipper Dept,Shipper Address 1,Shipper Address 2,Shipper City,ST,Postal,Origin Zip,Cntry1,Region,Recipient Name,Recipient Company,Recipient Address 1,Recipient Address 2,Recipient City,ST2,Postal2,Cntry2,Handling,Delivery,Time,Final,Exceptn,Attempt Date,Attempt Time,Signature,Svc Area,COD Amt,COD Trkg No,PDue,PDue Inv,Svc Pct,Rev Threshold Amt,Orig Recip Adr 1,Orig Recip Adr 2,Original City,ST3,Postal3,VAT No,FedEx VAT No,Cross Ref No,Intl Ground Ship No,";
        private List<string> refList= refhead.Split(',').Select(x => x.Replace(" ", "")).ToList();

        private decimal zTotalPrice;
        private decimal zTotalWeight;
        private decimal zTotalTax;
        private decimal zTotalFee;

        public FedexHandler()
        {
            Error = zTranslation.Error;
            RootDir = Config.FedexRootFileDir;
            CarrierName = "FEDEX";

        }


        public List<string> MissingTranslation()
        {
            return zTranslation.AddList;
        }

        public bool CheckHeader(String data)
        {

            LineNumber++;
            var da = data.Replace("\"", "").Replace(" ","");
            return MatchHeader(da, refhead.Replace(" ", ""), ",");
          
        }
        public void Next(string line)
        {
            LineNumber++;
            var iLine = AddInvoiceLine(line.Replace(",","|"), 1, E_INI);

            var fedexRecord = new FedexRecord(line,  zTranslation, LineNumber);
            iLine.Status = fedexRecord.RecordStatus;
            iLine.Reason = string.Join("; ", fedexRecord.ErrorHelper.ToArray());





            if (!RecordOK(fedexRecord, iLine))
            {
                return;
            }
            RegisterIvoceLine(fedexRecord.XmlRecord, iLine);

            var rec = fedexRecord.XmlRecord;
            if (rec.KeyType == FRAGT)
            {

                Records.Add(rec);
                return;
            }
            AddServiceToShipment(Records, rec); 


        }

        








    }
}
