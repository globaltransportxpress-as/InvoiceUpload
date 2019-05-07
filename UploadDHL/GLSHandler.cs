using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

namespace UploadDHL
{
    class GLSHandler

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
        public List<GLSrecord> Records = new List<GLSrecord>();
        private Translation zTranslation = new Translation(Config.TranslationFileGLS);
        
        private static string zfixhead =
                "Fakturanr.;Linjenr.;Dato;Varenr.;Beskrivelse;Land;Pakkenr.;Vægt;Antal;Salgspris;Beløb;Beløb inkl. moms;Reference;Modtagernavn;Kundenr.;Kundenavn;Kundenavn2;Modtagerpostnr.;Modtagerby;Modtageradresse";
            
        public GLSHandler()
        {
            Error = zTranslation.Error;
           
            


       }

        public bool Header(string head)
        {
            return (head == zfixhead.Replace(".","").Replace(" ",""));
           
        }
        public GLSrecord SetData(string[] da)


        {   var glsRecord = new GLSrecord(da, zTranslation);
           
         

         if(glsRecord.GTXTranslate==null)
           {
                

             Error = "Missing translation";
               return null;
         }
         

          
            if (glsRecord.GTXTranslate.KeyType != "GEBYR" && glsRecord.GTXTranslate.KeyType != "FRAGT" )
          {
              DropLines = DropLines + 1;
            
               return null;
           }
            if (glsRecord.Awb=="")
            {
                DropLines = DropLines + 1;
                return null;
            }




            var overw = glsRecord.GTXTranslate.Key.Contains("003099") ||
                        glsRecord.GTXTranslate.Key.Contains("053099");
           


            if (glsRecord.GTXTranslate.KeyType == "GEBYR" ||overw)
              {
                var record = Records.FirstOrDefault(x => x.Awb == glsRecord.Awb && x.GTXTranslate.KeyType=="FRAGT");
                  if (record == null)
                  {


                      record = glsRecord;
                  }
                  else
                  {
                      if (overw)
                      {
                        record.AddWeight(glsRecord.Antal);
                        record.AddPrice(glsRecord.Beløb);
                    }
                  }
                  if (!overw) { 
                    var sv = new Service
                  {
                      OrigalName = glsRecord.GTXTranslate.Key,
                      GTXCode = glsRecord.GTXTranslate.GTXName,
                      Price = glsRecord.Beløb


                  };

                  record.Services.Add(sv);
                }

            }



            if (string.IsNullOrEmpty(zFactura))
            {
                zFactura = glsRecord.Fakturanr;
                zFacturaDate = glsRecord.Dato;
            }
            

             Records.Add(glsRecord);
            return glsRecord;


        }

       
           

          public void MakeXML2()
        {
            var dhlXml = new DHLXML();

            var shipments = new StringBuilder();
            foreach (var glsRecord in Records)
            {
                var services = new StringBuilder();
                var total = glsRecord.Services.Sum(x => x.Price) + glsRecord.Beløb;
               
                int count = 1;
                if (glsRecord.GTXTranslate.KeyType=="FRAGT")
                {

                    services.Append(dhlXml.FillServicesXml(glsRecord.GTXTranslate.GTXName, glsRecord.Beløb));
                       
                }
                foreach (var service in glsRecord.Services)
                {
                    count++;
                    services.Append(dhlXml.FillServicesXml( service.GTXCode, service.Price));
                }
               
                shipments.Append(dhlXml.FillShipmentXml(glsRecord.Awb, "",glsRecord.Dato, glsRecord.GTXTranslate.GTXName, 1,
                    glsRecord.Vægt, glsRecord.Beløb,"0000", "DK", glsRecord.Modtagerpostnr,
                    glsRecord.Country, services.ToString()));
             

            }

            var sumfragt = Records.Sum(x => x.Beløb);
            var tillæg = Records.Sum(x => x.Services.Sum(y=>y.Price));
            var tax = Records.Sum(x => x.Beløbinklmoms)- sumfragt;


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

