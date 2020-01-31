using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Text;
using UploadDHL.DataConnections;
using UploadDHL.DataUploadWeb;

namespace UploadDHL
{
    class DHLHandler: VendorHandler
    {
       
        
        public Translation zTranslation = new Translation(Config.TranslationFileDHL,Config.AccountsGTX);
     
      
        
        public bool TranslationError { get; set; }
        public bool FormatError { get; set; }
        public string InvoiceName { get; set; }


        public StringBuilder zReasonError = new StringBuilder();
        private static string refhead = "\"Line Type\",\"Billing Source\",\"Original Invoice Number\",\"Invoice Number\",\"Station Code\",\"Invoice Identifier\",\"Invoice Type\",\"Invoice Date\",\"Payment Terms\",\"Due Date\",\"Parent Account\",\"Billing Account\",\"Billing Account Name\",\"Billing Account Name(Additional)\",\"Billing Address 1\",\"Billing Address 2\",\"Billing Address 3\",\"Billing Postcode\",\"Billing City\",\"Billing State/Province\",\"Billing Country Code\",\"Billing Contact\",\"VAT Number\",\"Shipment Number\",\"Shipment Date\",\"Country Specific Label\",\"Country Specific Value\",\"Shipment Reference 1\",\"Shipment Reference 2\",\"Shipment Reference 3\",\"Product\",\"Product Name\",\"Pieces\",\"Origin\",\"Orig Name\",\"Orig Country Code\",\"Orig Country Name\",\"Senders Name\",\"Senders Address 1\",\"Senders Address 2\",\"Senders Address 3\",\"Senders Postcode\",\"Senders City\",\"Senders State/Province\",\"Senders Country\",\"Senders Contact\",\"Destination\",\"Dest Name\",\"Dest Country Code\",\"Dest Country Name\",\"Receivers Name\",\"Receivers Address 1\",\"Receivers Address 2\",\"Receivers Address 3\",\"Receivers Postcode\",\"Receivers City\",\"Receivers State/Province\",\"Receivers Country\",\"Receivers Contact\",\"Proof of Delivery/Name\",\"Description of Contents\",\"Event Description\",\"Dimensions\",\"Cust Scale Weight(A)\",\"DHL Scale Weight(B)\",\"Cust Vol Weight(V)\",\"DHL Vol Weight(W)\",\"Weight Flag\",\"Weight(kg)\",\"Currency\",\"Total amount(excl.VAT)\",\"Total amount(incl.VAT)\",\"Tax Code\",\"Total Tax\",\"Tax Adjustment\",\"Invoice Fee\",\"Weight Charge\",\"Weight Tax(VAT)\",\"Other Charges 1\",\"Other Charges 1 Amount\",\"Other Charges 2\",\"Other Charges 2 Amount\",\"Discount 1\",\"Discount 1 Amount\",\"Discount 2\",\"Discount 2 Amount\",\"Discount 3\",\"Discount 3 Amount\",\"Total Extra Charges(XC)\",\"Total Extra Charges Tax\",\"XC1 Code\",\"XC1 Name\",\"XC1 Charge\",\"XC1 Tax Code\",\"XC1 Tax\",\"XC1 Discount\",\"XC1 Total\",\"XC2 Code\",\"XC2 Name\",\"XC2 Charge\",\"XC2 Tax Code\",\"XC2 Tax\",\"XC2 Discount\",\"XC2 Total\",\"XC3 Code\",\"XC3 Name\",\"XC3 Charge\",\"XC3 Tax Code\",\"XC3 Tax\",\"XC3 Discount\",\"XC3 Total\",\"XC4 Code\",\"XC4 Name\",\"XC4 Charge\",\"XC4 Tax Code\",\"XC4 Tax\",\"XC4 Discount\",\"XC4 Total\",\"XC5 Code\",\"XC5 Name\",\"XC5 Charge\",\"XC5 Tax Code\",\"XC5 Tax\",\"XC5 Discount\",\"XC5 Total\",\"XC6 Code\",\"XC6 Name\",\"XC6 Charge\",\"XC6 Tax Code\",\"XC6 Tax\",\"XC6 Discount\",\"XC6 Total\",\"XC7 Code\",\"XC7 Name\",\"XC7 Charge\",\"XC7 Tax Code\",\"XC7 Tax\",\"XC7 Discount\",\"XC7 Total\",\"XC8 Code\",\"XC8 Name\",\"XC8 Charge\",\"XC8 Tax Code\",\"XC8 Tax\",\"XC8 Discount\",\"XC8 Total\",\"XC9 Code\",\"XC9 Name\",\"XC9 Charge\",\"XC9 Tax Code\",\"XC9 Tax\",\"XC9 Discount\",\"XC9 Total\"";
        

       

        public DHLHandler()
        {
            Error = zTranslation.Error;
            RootDir = Config.DHLRootFileDir;
            CarrierName = "DHL";
            Translation = zTranslation;

        }


        

        public bool CheckHeader(String data)
        {

            
           
            LineNumber++;
            var d = data.Replace("\"", "").Replace(" ", ""); ;
            var f = refhead.Replace("\"", "").Replace(" ", ""); ;


            return MatchHeader(d, f, ";");
          
        }
        public void Next(string[] line)
        {

            LineNumber++;
            var iLine = AddInvoiceLine(string.Join("|",line), 1, E_INI);
            var dhlRecord = new DHLRecord(line, zTranslation,LineNumber, InvoiceName);

            iLine.Status = dhlRecord.RecordStatus;
            iLine.Reason = string.Join("; ", dhlRecord.ErrorHelper.ToArray());
            if (!RecordOK(dhlRecord, iLine))
            {
                return;
            }
           

            Records.AddRange(dhlRecord.XmlRecords);

          






        }
        



    }
}
