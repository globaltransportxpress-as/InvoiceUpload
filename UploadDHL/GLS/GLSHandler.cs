using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UploadDHL.DataConnections;
using Microsoft.Office.Interop.Excel;

namespace UploadDHL
{
    class GLSHandler:VendorHandler

    {


        private string zFactura;
        private string zCustomerNumber;

      
        private DateTime zFacturaDate;

        private List<string> awbs = new List<string>();
     
    
       
        public long InvoiceId { get; set; }
        private int zLineNo;
        public string Factura
        {
            get { return zFactura; }
        }

        public DateTime FacturaDate
        {
            get { return zFacturaDate; }
        }
        public Dictionary<string, int> Dic;
       
        
        private Translation zTranslation = new Translation(Config.TranslationFileGLS);
        
        private static string zfixhead =
                "Fakturanr.;Linjenr.;Dato;Varenr.;Beskrivelse;Land;Pakkenr.;Vægt;Antal;Salgspris;Beløb;Beløb inkl. moms;Reference;Modtagernavn;Kundenr.;Kundenavn;Kundenavn2;Modtagerpostnr.;Modtagerby;Modtageradresse";
            
        public GLSHandler()
        {
            Error = zTranslation.Error;
            RootDir = Config.GLSRootFileDir;
            CarrierName = "GLS";

        }


        public bool Header(string[] columnNames)
        {
           
            var d = string.Join(";", columnNames).Replace(" ", "").Replace("#", "");
            var f = zfixhead.Replace(".", "").Replace(" ", "");

            LineNumber++;
            return MatchHeader(d,f,";");
        }
        public void Next(string[] da)
        {
            LineNumber++;

            var iLine= AddInvoiceLine(string.Join("|", da), 1, E_INI);
           
            var glsRecord = new GLSrecord(da, zTranslation,LineNumber);
            iLine.Status = glsRecord.RecordStatus;
            iLine.Reason = string.Join("; ", glsRecord.ErrorHelper.ToArray());
            
            if (!RecordOK(glsRecord, iLine))
            {
                return ;
            }
            RegisterIvoceLine(glsRecord.XmlRecord, iLine);

            var rec = glsRecord.XmlRecord;
            if (rec.KeyType == FRAGT)
            {

                Records.Add(rec);
                return ;

            }
                       var overw = rec.CarrierService.Contains("003099") ||
                        rec.CarrierService.Contains("053099");
      
         


                if (overw)
                {   var record = Records.FirstOrDefault(x => x.Awb == rec.Awb && x.KeyType == FRAGT);
                    if (record != null)
                    {
                        iLine.TransKey = VendorHandler.OVERWEIGHT;
                        record.BilledWeight = record.BilledWeight+ glsRecord.Antal;
                        record.Price = record.Price + glsRecord.Beløb;
                    }
                    else
                    {
                        iLine.TransKey = VendorHandler.E_OW;

                    }
                    return ;

                }

            AddServiceToShipment(Records, rec);



        }

        
           

          public void MakeXML2(List<XMLRecord> xmlRecords)
        {
            var dhlXml = new DHLXML();

            var shipments = new StringBuilder();
            foreach (var glsRecord in xmlRecords)
            {

                var services = new StringBuilder();
                var total = glsRecord.Services.Sum(x => x.Price) + glsRecord.Price;
               
                int count = 1;
                if (glsRecord.KeyType==FRAGT)
                {

                    services.Append(dhlXml.FillServicesXml(glsRecord.GTXName, glsRecord.Price));
                       
                }
                foreach (var service in glsRecord.Services)
                {
                    count++;
                    services.Append(dhlXml.FillServicesXml( service.GTXCode, service.Price));
                }
               
                shipments.Append(dhlXml.FillShipmentXml(glsRecord.Awb, "",glsRecord.InvoiceDate, glsRecord.GTXName, 1,
                    glsRecord.BilledWeight, glsRecord.Price,glsRecord.Zip, glsRecord.Country_Iata, glsRecord.Reciever_Zip,
                    glsRecord.Reciever_Country_Iata, services.ToString()));
             

            }

            var sumfragt = xmlRecords.Sum(x => x.Price);
            var tillæg = xmlRecords.Sum(x => x.Services.Sum(y=>y.Price));
            var tax = xmlRecords.Sum(x => x.Vat)- sumfragt;


            var xml = dhlXml.FillFacturaXml(Factura, zFacturaDate, zFacturaDate.AddDays(30), zCustomerNumber, sumfragt,
                tillæg, tax, shipments.ToString());




            using (StreamWriter xmlout =
                new StreamWriter(Config.GLSRootFileDir + "\\Xml\\X" + Factura + "_" + DateTime.Now.ToString("yyyyMMddmm") + ".xml", false))
            {
                xmlout.Write(xml);
            }



        }
      
    }



}

