using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

using UploadDHL.DataUploadWeb;

namespace UploadDHL
{
    class GTXHandler

    {


        private string zFactura;
        private string zCustomerNumber;
        private DateTime zFacturaDate;

        private List<string> awbs = new List<string>();
        public string Error { get; set; }
        public int DropLines { get; set; }

        public string Factura
        {
            get { return zFactura; }
        }

        public DateTime FacturaDate
        {
            get { return zFacturaDate; }
        }
        public Dictionary<string, int> Dic;
        public List<GTXrecord> Records = new List<GTXrecord>();
        private Translation zTranslation = new Translation(Config.TranslationFileGtx);
        
        private static string zfixhead =
                "PURCHORDERFORMNUM,Pieces,Weight,LNO,Amount,RECEIPTDATECONFIRMED,PICKUPNAME,PICKUPCOUNTRY,DELIVERYNAME,DELIVERYCOUNTRYREGIONID,CUSTOMERREF,ORGNUMBER,INVOICEACCOUNT,NAME,VARENUMMER,VARENAVN,ACCOUNTNUM,ProductGroup,SALESID,INTERCOMPANYORIGINALSALESID,INVOICEID,INVOICEDATE,LINENUM,OilAmount,TXT,TAXWRITECODE,DIMENSION,DIMENSION2_,DIMENSION3_,PU_ADRESS,PU_CITY,PU_ZIP,DL_ADRESS,DL_CITY,DL_ZIP,YEAR";


        public GTXHandler()
        {
            Error = zTranslation.Error;
           
            


       }

        public bool Header(string head)
        {
            var dd = zfixhead.Replace(".", "").Replace(" ", "").Split(',');
            var b = head.Split(';');
            if (b.Length < dd.Length)
            {
                return false;
            }
            for (int i = 0; i < dd.Length; i++)
            {
                if (b[i] != dd[i])
                {
                    return false;
                }            }
            return true;
        }
        public GTXrecord SetData(string[] da)


        {   var gtxRecord = new GTXrecord(da, zTranslation);

            var chkYear = DateTime.Now.Year - 2;
            if (gtxRecord.INVOICEDATE.Year < chkYear || gtxRecord.RECEIPTDATECONFIRMED.Year < chkYear)
            {
                Error = "Date problem";
                return null;
            }
            if (gtxRecord.PURCHORDERFORMNUM == "")
            {
                DropLines = DropLines + 1;
                return null;
            }


            if (gtxRecord.GTXTranslate==null)
           {
                

             Error = "Missing translation";
               return null;
         }
         

          
            if (gtxRecord.GTXTranslate.KeyType != "GEBYR" && gtxRecord.GTXTranslate.KeyType != "FRAGT" && gtxRecord.GTXTranslate.KeyType != "XML")
            {
              DropLines = DropLines + 1;
            
               return null;
           }
           



            if (gtxRecord.GTXTranslate.KeyType == "GEBYR")
            {

                var sv = new Service
                {
                    OrigalName = gtxRecord.GTXTranslate.Key,
                    GTXCode = gtxRecord.GTXTranslate.GTXName,
                    Price = gtxRecord.Amount


                };
                var record = Records.FirstOrDefault(x => x.PURCHORDERFORMNUM == gtxRecord.PURCHORDERFORMNUM && x.INVOICEID==gtxRecord.INVOICEID && 
                                                         x.GTXTranslate.KeyType == "FRAGT");
                if (record != null)
                {
                    gtxRecord = record;
                }
                else
                {
                    Records.Add(gtxRecord);


                }


                gtxRecord.Services.Add(sv);


            }
            else
            {
                if (gtxRecord.Weight == 0)
                {

                    Error = "Weight zerro";
                   
                    return null;

                }


                Records.Add(gtxRecord);
            }



            
            

             
            return gtxRecord;


        }

       
           

          public void MakeXML2()
        {
            var dhlXml = new DHLXML();
            GTXrecord privrec = null;
            var shipments = new StringBuilder();
            foreach (var gtxRecord in Records.OrderBy(x=>x.INVOICEID))
            {



                
                if (privrec != null && privrec.INVOICEID != gtxRecord.INVOICEID)
                {

                    WriteXMLToFile(dhlXml, privrec,shipments.ToString());
                    shipments = new StringBuilder();
                }



                var services = new StringBuilder();
                var total = gtxRecord.Services.Sum(x => x.Price) + gtxRecord.Amount;
               
                int count = 1;
                if (gtxRecord.GTXTranslate.KeyType=="FRAGT")
                {

                    services.Append(dhlXml.FillServicesXml(gtxRecord.GTXTranslate.GTXName, gtxRecord.Amount));
                       
                }
                foreach (var service in gtxRecord.Services)
                {
                    count++;
                    services.Append(dhlXml.FillServicesXml( service.GTXCode, service.Price));
                }
               
                shipments.Append(dhlXml.FillShipmentXml(gtxRecord.PURCHORDERFORMNUM, "",gtxRecord.RECEIPTDATECONFIRMED, gtxRecord.GTXTranslate.GTXName, 1,
                    gtxRecord.Weight, gtxRecord.Amount,gtxRecord.PU_ZIP, gtxRecord.PICKUPCOUNTRY, gtxRecord.DL_ZIP,
                    gtxRecord.DELIVERYCOUNTRYREGIONID, services.ToString()));

                privrec = gtxRecord;
            }

            if (!string.IsNullOrEmpty(shipments.ToString()))
            {
                WriteXMLToFile(dhlXml, privrec, shipments.ToString());
            }




        }



        private void WriteXMLToFile(DHLXML dhlXml, GTXrecord privrec, string shipments)
        {
            var invoiceShipmentLoad = new InvoiceShipmentLoad();
            var sumfragt = Records.Where(x => x.INVOICEID == privrec.INVOICEID).Sum(x => x.Amount);
                var tillæg = Records.Where(x => x.INVOICEID == privrec.INVOICEID).Sum(x => x.Services.Sum(y => y.Price));
                var oil = Records.Where(x => x.INVOICEID == privrec.INVOICEID).Sum(x => x.OilAmount);
                var tax = 0;


                var xml = dhlXml.FillFacturaXml(privrec.INVOICEID, privrec.INVOICEDATE, privrec.INVOICEDATE.AddDays(30), privrec.INVOICEACCOUNT, sumfragt, oil, 0, shipments);


                using (StreamWriter xmlout =
                    new StreamWriter(Config.GTXRootFileDir + "\\Xml\\X" + privrec.INVOICEID + "_" + DateTime.Now.ToString("yyyyMMddmms") + ".xml", false))
                {
                    xmlout.Write(xml);
                }

         




            foreach (var record in Records.Where(x => x.INVOICEID == privrec.INVOICEID && x.GTXTranslate.KeyType == "FRAGT"))
            {


                invoiceShipmentLoad.AddShipment(record.StdConvert());

            }


            if (invoiceShipmentLoad.Run() != "OK")
            {
                Error = "Failed web service missing shipment";
            }




           
        }
      
    }



}

