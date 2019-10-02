using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UploadDHL.DataConnections;

namespace UploadDHL
{
    class PdkHandler : VendorHandler

    {


        private string zFactura;
        private string zCustomerNumber;
        private DateTime zFacturaDate;




        public string Factura
        {
            get { return zFactura; }
        }

        public DateTime FacturaDate
        {
            get { return zFacturaDate; }
        }

        public Dictionary<string, int> Dic;
      

        private Translation zTranslation = new Translation(Config.TranslationFilePDK);

        private static string zfixhead =
                "Stregkode;Dato;Ordre;Ordrepos.;Materiale;Frankering;Momsbelagt;Grundpris;Ialt(excl.moms);Frapostnr;Tilpostnr;Fra-land;Til-land;Vægt;Volumenvægt;Faktureretvægt;Længde;Bredde;Højde;Navn1;Adresse;"
            ;

        public PdkHandler()
        {
            Error = zTranslation.Error;
            RootDir = Config.PDKRootFileDir;
            CarrierName = "PDK";

        }


       


        public void SetData(string[] da)


        {
            LineNumber++;
                
            var data = string.Join("|", da);
            var iLine = AddInvoiceLine(data, 1, E_INI);

            if (da[0] == "Kundenummer:")
            {


                zCustomerNumber = da[2];
                iLine.Status = HEAD;

                return ;

            }
            if (da[0] == "Fakturanummer:")
            {


                zFactura = da[2];
                iLine.Status = HEAD;
                return ;

            }
            if (da[0] == "Fakturadato:")
            {
                DateTime dd;
                if (DateTime.TryParse(da[2], out dd))
                {
                    zFacturaDate = dd;
                    iLine.Status = HEAD;
                }
                else
                {
                    iLine.Status = E_DATE;
                }

                return ;

            }


            if (da[0] == "Stregkode")
            {


                Dic = MakeHeader(da, iLine);
                iLine.Status = HEAD;
                return ;

            }

            if (string.IsNullOrEmpty(zCustomerNumber) || zFacturaDate.Year < 2000 || string.IsNullOrEmpty(zFactura) ||
                Dic == null || string.IsNullOrEmpty(da[0]) || string.IsNullOrEmpty(da[1]))
            {

                iLine.Status = DROP;
                return ;
            }


            var pdkRec = new PDKrecord(da, Dic, zTranslation, zfixhead, LineNumber);
            pdkRec.Factura = zFactura;
            pdkRec.FacturaDate = zFacturaDate;
            pdkRec.CustomerNumber = zCustomerNumber;
            iLine.Status = pdkRec.RecordStatus;
            iLine.Reason = string.Join("; ", pdkRec.ErrorHelper);

            pdkRec.XmlRecord = pdkRec.MakeXmlRecord();
            if (!RecordOK(pdkRec, iLine))
            {
                return;
            }
            if (pdkRec.XmlRecord.KeyType == FRAGT)
            {

                Records.Add(pdkRec.XmlRecord);
                return;

            }
            AddServiceToShipment(Records, pdkRec.XmlRecord);
           

        }


      
       
        





      

        private Dictionary<string, int> MakeHeader(string[] hd, InvoiceLine iLine)
        {

            var ok = true;

            var dic = new Dictionary<string, int>();
            var i = 0;
            foreach (var d in hd)
            {

                var key = d.Replace(" ", "");
                if (key != "")
                {
                    if (zfixhead.Contains(key + ";"))
                    {

                        dic.Add(d.Replace(" ", ""), i);


                    }
                    else
                    {
                        if (!zTranslation.TranDictionary.ContainsKey(key))
                        {
                            zTranslation.AddMissing(key, "GEBYR");
                            iLine.Status = E_TRANS;
                        }
                        else
                        {
                            dic.Add(zTranslation.TranDictionary[key].Key, i);
                        }



                    }
                }

                i++;



            }
            if (ok)
            {
                return dic;
            }

            return null;


        }



    }

}

