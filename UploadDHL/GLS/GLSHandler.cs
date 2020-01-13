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
       
        
        private Translation zTranslation = new Translation(Config.TranslationFilePickupGLS);
        
        private static string zfixhead =
                "Fakturanr;Fakturadato;Linjenr;Dato;Varenr;Beskrivelse;Antal;Valutakode;Salgspris;Beløb;Beløbinklmoms;Land;Pakkenr/bookingnr;Eksternreference;Vægt;Navn;Adresse;Postnr;By;Bemærkning";


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
            //RegisterIvoceLine(glsRecord.XmlRecord, iLine);

            var rec = glsRecord.XmlRecord;
          

                Records.Add(rec);
                return ;

            
                     

            //AddServiceToShipment(Records, rec);



        }

        
           

        
      
    }



}

