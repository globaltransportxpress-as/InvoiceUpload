﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace UploadDHL
{
    class PalletHandler

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
        public List<PalletReportRecord> Records = new List<PalletReportRecord>();
        public ErrorHandler ErrorHandler { get; set; }
       

        private Translation zTranslation = new Translation(Config.TranslationFilePDK);

       
        public PalletHandler()
        {
            Error = zTranslation.Error;
            ErrorWeight = "";
        }


        public PalletReportRecord SetData(string[] da)


        {


                      




            var pRec = new PalletReportRecord(da);
           

           
            
           






            return pRec;


        }


        

    }



}

