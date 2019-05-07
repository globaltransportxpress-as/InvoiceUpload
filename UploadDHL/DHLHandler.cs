using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Text;
using nu.gtx.DbMain.Standard.PM;
using UploadDHL.DataUploadWeb;

namespace UploadDHL
{
    class DHLHandler
    {
       
        
        private Translation zTranslation = new Translation(Config.TranslationFileDHL);
        private DHLXML zDhlXml;
        private List<DHLRecord> DHLRecords;
        private int zCount;
        public bool Error { get; set; }
        public bool TranslationError { get; set; }
        public bool FormatError { get; set; }
        public StringBuilder ReasonError = new StringBuilder();
        private static string refhead = "\"Line Type\",\"Billing Source\",\"Original Invoice Number\",\"Invoice Number\",\"Station Code\",\"Invoice Identifier\",\"Invoice Type\",\"Invoice Date\",\"Payment Terms\",\"Due Date\",\"Parent Account\",\"Billing Account\",\"Billing Account Name\",\"Billing Account Name(Additional)\",\"Billing Address 1\",\"Billing Address 2\",\"Billing Address 3\",\"Billing Postcode\",\"Billing City\",\"Billing State/Province\",\"Billing Country Code\",\"Billing Contact\",\"VAT Number\",\"Shipment Number\",\"Shipment Date\",\"Country Specific Label\",\"Country Specific Value\",\"Shipment Reference 1\",\"Shipment Reference 2\",\"Shipment Reference 3\",\"Product\",\"Product Name\",\"Pieces\",\"Origin\",\"Orig Name\",\"Orig Country Code\",\"Orig Country Name\",\"Senders Name\",\"Senders Address 1\",\"Senders Address 2\",\"Senders Address 3\",\"Senders Postcode\",\"Senders City\",\"Senders State/Province\",\"Senders Country\",\"Senders Contact\",\"Destination\",\"Dest Name\",\"Dest Country Code\",\"Dest Country Name\",\"Receivers Name\",\"Receivers Address 1\",\"Receivers Address 2\",\"Receivers Address 3\",\"Receivers Postcode\",\"Receivers City\",\"Receivers State/Province\",\"Receivers Country\",\"Receivers Contact\",\"Proof of Delivery/Name\",\"Description of Contents\",\"Event Description\",\"Dimensions\",\"Cust Scale Weight(A)\",\"DHL Scale Weight(B)\",\"Cust Vol Weight(V)\",\"DHL Vol Weight(W)\",\"Weight Flag\",\"Weight(kg)\",\"Currency\",\"Total amount(excl.VAT)\",\"Total amount(incl.VAT)\",\"Tax Code\",\"Total Tax\",\"Tax Adjustment\",\"Invoice Fee\",\"Weight Charge\",\"Weight Tax(VAT)\",\"Other Charges 1\",\"Other Charges 1 Amount\",\"Other Charges 2\",\"Other Charges 2 Amount\",\"Discount 1\",\"Discount 1 Amount\",\"Discount 2\",\"Discount 2 Amount\",\"Discount 3\",\"Discount 3 Amount\",\"Total Extra Charges(XC)\",\"Total Extra Charges Tax\",\"XC1 Code\",\"XC1 Name\",\"XC1 Charge\",\"XC1 Tax Code\",\"XC1 Tax\",\"XC1 Discount\",\"XC1 Total\",\"XC2 Code\",\"XC2 Name\",\"XC2 Charge\",\"XC2 Tax Code\",\"XC2 Tax\",\"XC2 Discount\",\"XC2 Total\",\"XC3 Code\",\"XC3 Name\",\"XC3 Charge\",\"XC3 Tax Code\",\"XC3 Tax\",\"XC3 Discount\",\"XC3 Total\",\"XC4 Code\",\"XC4 Name\",\"XC4 Charge\",\"XC4 Tax Code\",\"XC4 Tax\",\"XC4 Discount\",\"XC4 Total\",\"XC5 Code\",\"XC5 Name\",\"XC5 Charge\",\"XC5 Tax Code\",\"XC5 Tax\",\"XC5 Discount\",\"XC5 Total\",\"XC6 Code\",\"XC6 Name\",\"XC6 Charge\",\"XC6 Tax Code\",\"XC6 Tax\",\"XC6 Discount\",\"XC6 Total\",\"XC7 Code\",\"XC7 Name\",\"XC7 Charge\",\"XC7 Tax Code\",\"XC7 Tax\",\"XC7 Discount\",\"XC7 Total\",\"XC8 Code\",\"XC8 Name\",\"XC8 Charge\",\"XC8 Tax Code\",\"XC8 Tax\",\"XC8 Discount\",\"XC8 Total\",\"XC9 Code\",\"XC9 Name\",\"XC9 Charge\",\"XC9 Tax Code\",\"XC9 Tax\",\"XC9 Discount\",\"XC9 Total\"";
        private List<string> refList= refhead.Split(',').Select(x => x.Replace("\"", "").Replace(" ", "")).ToList();

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
            DHLRecords = new List<DHLRecord>();

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



            var record = new DHLRecord(line, ReasonError, zTranslation);
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
                        var rec = DHLRecords.FirstOrDefault(x => x.Shipment_Number == record.Shipment_Number);
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
                    DHLRecords.Add(record);
                    return true;
                }
                
            }
            return false;



        }

        public void MakeXmlAndWeightfile()
        {
            decimal oil = 0;
            decimal sumTotal_amount_excl_VAT = 0;
            decimal sumTotal_Tax = 0;
            DHLRecord lastrecord = null;
            var sb = new StringBuilder();
            var wfList = new List<WeightFileRecord>();
            foreach (var record in DHLRecords.Where(x => x.Line_Type != "I").ToList())
            {
                var sb2 = new StringBuilder();
                sb2.AppendLine(zDhlXml.FillServicesXml(record.GTXTranslate.GTXName,
                    record.Weight_Charge ));
                foreach (var service in record.Services)
                {
                    if (service.GTXCode == "FOILE")
                    {
                        oil = oil + service.Price;

                    }



                    sb2.AppendLine(zDhlXml.FillServicesXml(service.GTXCode, service.Price));

                }
                int pic = 1;

                sb.AppendLine(zDhlXml.FillShipmentXml(record.Shipment_Number, record.Shipment_Reference_1,
                    record.Shipment_Date,
                    record.GTXTranslate.GTXName, pic, record.Weight_kg, record.Weight_Charge ,
                    record.Senders_Postcode, record.Senders_Country, record.Receivers_Postcode,
                    record.Receivers_Country, sb2.ToString()));
                sumTotal_amount_excl_VAT = sumTotal_amount_excl_VAT + record.Total_amount_excl_VAT;
                sumTotal_Tax = sumTotal_Tax + record.Total_Tax;
                lastrecord = record;
                wfList.Add(record.Convert());


            }
            var rec = DHLRecords.FirstOrDefault(x => x.Line_Type == "I");
            var xml = "";
            if (rec != null)
            {
                xml = zDhlXml.FillFacturaXml(rec.Invoice_Number, rec.Invoice_Date, rec.Due_Date, rec.Billing_Account,
                    rec.Total_amount_excl_VAT, oil, rec.Total_Tax, sb.ToString());
            }
            else
            {
                if (lastrecord != null)
                {
                    xml = zDhlXml.FillFacturaXml(lastrecord.Invoice_Number, lastrecord.Invoice_Date,
                        lastrecord.Due_Date, lastrecord.Billing_Account, sumTotal_amount_excl_VAT, oil, sumTotal_Tax,
                        sb.ToString());
                }


            }

            if (xml != "")
            {
                using (StreamWriter xmlout =
                    new StreamWriter(
                        Config.DHLRootFileDir + "\\Xml\\X" + rec.Invoice_Number + "_" +
                        DateTime.Now.ToString("yyyyMMddmms") + ".xml", false))
                {
                    xmlout.Write(xml);
                }
            }

            //if (1 != 1)
            if (lastrecord != null)
            {
                WeightFileObj.CreateFile(Config.DHLRootFileDir, wfList, lastrecord.Invoice_Number);
                var listInvShip = new List<InvoiceShipmentHolder>();

               
               
               
                foreach (var record in DHLRecords.Where(x => x.Line_Type != "I" && x.Total_amount_excl_VAT>0 && x.GTXTranslate.KeyType=="FRAGT").ToList() )
                {

                    
                    listInvShip.Add(record.StdConvert());
                }

                 var _service=   new InvoiceUploadSoapClient("InvoiceUploadSoap");
                 var res = _service.ShipmentUpload(listInvShip.ToArray());
                if (res != "OK")
                {
                    Error = true;
                }
            }

           


        }

    }
}
