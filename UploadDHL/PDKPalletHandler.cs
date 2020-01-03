using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace UploadDHL
{
    class PdkPalletHandler

    {


        private string zFactura;
        private string zCustomerNumber;
        private DateTime zFacturaDate;

       
        public string Error { get; set; }
        public string ErrorWeight { get; set; }
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
        public List<Palletrecord> Records = new List<Palletrecord>();
        public ErrorHandler ErrorHandler { get; set; }
       

        private Translation zTranslation = new Translation(Config.TranslationFilePDK);

       
        public PdkPalletHandler()
        {
            Error = zTranslation.Error;
            ErrorWeight = "";
        }


        public Palletrecord SetData(string[] da)


        {





            try
            {
                var pdkRec = new Palletrecord(da);



                if (!zTranslation.TranDictionary.ContainsKey(pdkRec.Material))
                {

                    zTranslation.AddMissing(pdkRec.Material, "FRAGT");
                    Error = "Missing translation";
                    return null;
                }

                pdkRec.GTXTranslate = zTranslation.TranDictionary[pdkRec.Material];
                if (pdkRec.GTXTranslate.KeyType != "GEBYR" && pdkRec.GTXTranslate.KeyType != "FRAGT")
                {
                    DropLines = DropLines + 1;
                    return null;
                }





                Records.Add(pdkRec);


                return pdkRec;
            }
            catch (Exception ex)
            {
                return null;
            }


        }


        

        private string SafeLookUp(string[] da, string code, string def)
        {
            if (Dic.ContainsKey(code))
            {
                return da[Dic[code]];

            }

            return def;

        }


        private DateTime DateConvert(string[] da, string fieldname)
        {
            DateTime dd;
            if (DateTime.TryParse(SafeLookUp(da,fieldname,""), out dd))
            {
                return dd;
            }

            Error = "Convert error date ";
            ErrorHandler.Add("Date error", string.Format("Field name : {0} Data : {1}", fieldname, string.Join(";", da)), "DateConvert");
            return DateTime.Now;

        }



       
       
        

        public void MakeXML2()
        {
            var dhlXml = new DHLXML();

            var shipments = new StringBuilder();
            foreach (var pdkRecord in Records)
            {






                var services = new StringBuilder();
                var total = pdkRecord.Services.Sum(x => x.Price);

                int count = 1;
                if (pdkRecord.GTXTranslate.KeyType=="FRAGT")
                {

                    services.Append(dhlXml.FillServicesXml(pdkRecord.GTXTranslate.GTXName, pdkRecord.Price));

                }
                foreach (var service in pdkRecord.Services)
                {
                    count++;
                    services.Append(dhlXml.FillServicesXml(service.GTXCode, service.Price));
                }
                
                shipments.Append(dhlXml.FillShipmentXml(pdkRecord.AWB, "", pdkRecord.Date, pdkRecord.GTXTranslate.GTXName, 1,
                    pdkRecord.BillWeight, pdkRecord.Price, pdkRecord.FromZip, pdkRecord.SenderCountry, pdkRecord.ToZip,
                    pdkRecord.ReceiverCountry, services.ToString()));


            }

            var sumfragt = Records.Sum(x => x.Price);
            var tillæg = Records.Sum(x => x.Services.Sum(y => y.Price));
            var tax = 0;


            var xml = dhlXml.FillFacturaXml(Factura, zFacturaDate, zFacturaDate.AddDays(30), zCustomerNumber, sumfragt,
                tillæg, tax, shipments.ToString());




            using (StreamWriter xmlout =
                new StreamWriter(Config.PDKRootFileDir + "\\Xml\\X" + Factura + "_" + DateTime.Now.ToString("yyyyMMddmm") + ".xml", false))
            {
                xmlout.Write(xml);
            }



        }

    }



}

