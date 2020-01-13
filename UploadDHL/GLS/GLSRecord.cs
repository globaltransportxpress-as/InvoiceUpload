using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

using UploadDHL.DataUploadWeb;

namespace UploadDHL
{
    class GLSrecord : DataRecord
    {
        // "Fakturanr.;Linjenr.;Dato;Varenr.;Beskrivelse;Land;Pakkenr.;Vægt;Antal;Salgspris;Beløb;Beløb inkl. moms;Reference;Modtagernavn;Kundenr.;Kundenavn;Kundenavn2;Modtagerpostnr.;Modtagerby;Modtageradresse";
        private string[] zCSVdata;

      
       

        public string Fakturanr
        {
            get { return zCSVdata[0]; }
        }
        public DateTime FacturaDato
        {
            get { return SafeDate(zCSVdata[1], "FacturaDato"); }
        }
        public string Linjenr
        {
            get { return zCSVdata[2]; }
        }
        public DateTime Dato
        {
            get { return SafeDate(zCSVdata[3], "Dato"); }
        }

        public string VareNo
        {
            get { return zCSVdata[4]; }
        }
        
        public string Beskrivelse
        {
            get { return zCSVdata[5]; }
        }
        public int Antal
        {
            get { return SafeInt(zCSVdata[6]); }
        }

        public string ValutaCode
        {
            get { return zCSVdata[7]; }
        }

        public decimal Salgspris
        {
            get { return SafeDecimal(zCSVdata[8], "Salgspris"); }
        }

        private decimal zPrice;
        public decimal Beløb
        {
            get { return SafeDecimal(zCSVdata[9], "Beløb"); }
        }
        public decimal Beløbinklmoms
        {
            get { return SafeDecimal(zCSVdata[10], "Beløbinklmoms"); }
        }

        public string Country
        {
            get { return zCSVdata[11]; }
        }



        public string Pakkenr
        {
            get { return zCSVdata[12]; }
        }

       
        public string Reference
        {
            get { return zCSVdata[13]; }
        }
        public string Vægt
        {
            get { return zCSVdata[14]; }

        }

        public string Navn
        {
            get { return zCSVdata[15]; }
        }
        public string Adresse
        {
            get { return zCSVdata[16]; }
        }
       
        public string Postnr
        {
            get { return zCSVdata[17]; }
        }
        public string City
        {
            get { return zCSVdata[18]; }
        }
        public string Remarks
        {
            get { return zCSVdata[19]; }
        }



        private decimal zWeight;
        public GLSrecord(string[] fields, Translation th, int lineno)
        {

          
            TranslationHandler = th;
            zCSVdata = fields;
            InvLineNumber = lineno;
            GTXTranslate = TranslationHandler.DoTranslate(VareNo, VendorHandler.FRAGT);
            RecordStatus = GTXTranslate.KeyType;
            
            zPrice = Beløb;
            Awb = Pakkenr;
            

            
            XmlRecord = MakeXmlRecord();
            if (XmlRecord == null)
            {
                RecordStatus = VendorHandler.E_ERROR;
                ErrorHelper.Add("Conversion record ->gtxRecord ");
            }



        }


        


        private XMLRecord MakeXmlRecord()
        {
            return new XMLRecord
            {
                Awb = this.Awb,
                InvoiceNumber = this.Fakturanr,
                InvoiceDate = this.FacturaDato,
               
                Price = this.Beløb,
                Vat = this.Beløbinklmoms - this.Beløb,

               
                Services = this.Services,
                CarrierCode= "GLS",
                CarrierService = GTXTranslate.Key,
                GTXName = this.GTXTranslate.GTXName,
                KeyType = this.GTXTranslate.KeyType,
                Product = GTXTranslate.GTXProduct,
                Transport = (byte)GTXTranslate.GTXTransp,
                Shipdate = Dato,

                CompanyName = Navn,
                Address1 = Adresse,
                Address2 = "",
                City = City,
                State = "",
                Zip = Postnr,
                Country_Iata = "DK",
                NumberofCollies =(byte) Antal,
                
                Reference = Reference,
                BilledWeight = 1,
                Total_Weight = 1M,
                Vol_Weight = 1M,
                
                Description = Remarks,
                Costprice = Beløb,





            };
        }


        






    }
}
