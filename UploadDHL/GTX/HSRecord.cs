using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

using UploadDHL.DataUploadWeb;

namespace UploadDHL
{
    class HSrecord:DataRecord
    {
        // "Fakturanr.;Linjenr.;Dato;Varenr.;Beskrivelse;Land;Pakkenr.;Vægt;Antal;Salgspris;Beløb;Beløb inkl. moms;Reference;Modtagernavn;Kundenr.;Kundenavn;Kundenavn2;Modtagerpostnr.;Modtagerby;Modtageradresse";
        private string[] zCSVdata;
      
        public bool FormatError { get; set; }
        public DateTime ShipDate { get; set; }
        public string  InvoiceNumber{ get; set; }
        public decimal Price { get; set; }
 
        public string PickupName { get; set; }
        public string PickupCoutry { get; set; }
        public string PickupAddress
        { get; set; }
        public string PickupCity
        { get; set; }
        public string PickupZip
        { get; set; }
        

        public HSrecord(string[] fields, Translation translationhandler, int lineno)
        {

            RecordStatus = VendorHandler.E_INI;
            zCSVdata = fields;
            InvLineNumber = lineno;
            GTXTranslate = translationhandler.DoTranslate("FRAGT", "FRAGT");

            RecordStatus = GTXTranslate.KeyType;


            XmlRecord = MakeXmlRecord();


        }
       

       

        


        private XMLRecord MakeXmlRecord()
        {
            return new XMLRecord
            {
               
                Awb = this.Awb,
                InvoiceNumber = InvoiceNumber,
                InvoiceDate = DateTime.Now,

                Price = Price,
                Vat = -1,


                Services = this.Services,
                CarrierCode = "HS",
                CarrierService = GTXTranslate.Key,
                GTXName = this.GTXTranslate.GTXName,
                KeyType = this.GTXTranslate.KeyType,
                Product = GTXTranslate.GTXProduct,
                Transport = (byte)GTXTranslate.GTXTransp,
                Shipdate = ShipDate,

                CompanyName = PickupName,
                Address1 = PickupAddress,
                Address2 = "",
                City = PickupCity,
                State = "",
                Zip = PickupZip,
                Country_Iata = "DK",
                NumberofCollies = (byte)1,

                Reference = "HS data",
                BilledWeight = 1,
                Total_Weight = 1M,
                Vol_Weight = 1M,

                Description = "",
                Costprice = Price,



          

    };
        }





       
        private string SafeZip(string zip)
        {
            if (string.IsNullOrWhiteSpace(zip))
            {
                return "___";

            }
            return zip;
        }

        private string SafeAddr(string addr,int ix)
        {
            if (addr.Length > 50)
            {
                var d = addr.Split('\n');
                if (ix == 0)
                {
                    if (d[0].Length > 50)
                    {
                        return d[0].Substring(0, 49);
                    }
                    return d[0];
                }
                else
                {
                    if (d.Length > 1)
                    {
                        if (d[1].Length > 50)
                        {
                            return d[1].Substring(0, 49);
                        }
                        return d[1];
                    }
                    
                }
            }

            return addr;
        }



       
        
        private DateTime SafeGTXDate(string data, string field)
        {

            try
            {
                
                if (data.Contains("-"))
                {
                 return DateTime.Parse(data);
                }
                else
                {
                    double d = double.Parse(data);



                    return DateTime.FromOADate(d);
                }

           

                //zReasonError.AppendLine("DateTimeFormat error line " + zCurrentLine);
            }
            catch (Exception)
            {
                RecordStatus = VendorHandler.E_DATE;
                ErrorHelper.Add(VendorHandler.E_DATE + "->" + field);

            }
            return new DateTime();

        }
       
    }
}
