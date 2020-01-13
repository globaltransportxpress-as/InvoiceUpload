using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using UploadDHL.DataUploadWeb;


namespace UploadDHL
{
    class DHLRecord:DataRecord
    {

        private StringBuilder zXmlOut;
       
       
        private int zCount;
    
        public bool Error { get; set; }
        public bool TranslationError { get; set; }
        public bool FormatError { get; set; }


       public List<XMLRecord> XmlRecords = new List<XMLRecord>();






        private decimal zTaxCharge;
        private decimal zNonTaxCharge;
        private string zInvoiceNumber;


        private string[] zCSVdata;

        public string CustomerName
        {
            get { return zCSVdata[0]; }
        }

        public string Type
        {
            get { return zCSVdata[1]; }
        }

        public string Reference
        {
            get { return zCSVdata[2]; }
        }

        public DateTime Date
        {
            get { return SafeDate(zCSVdata[3], "Date"); }
        }

        public decimal PickUpCharge
        {
            get { return SafeDecimal(zCSVdata[4] , "PickUpCharge"); }
        }

        public decimal FutileCharge
        {
            get { return SafeDecimal(zCSVdata[5], "FutileCharge"); }
        }

        public decimal WaitCharge
        {
            get { return SafeDecimal(zCSVdata[6], "WaitCharge"); }
        }

        public decimal OtherCharge
        {
            get { return SafeDecimal(zCSVdata[7], "OtherCharge"); }
        }

        public string Account
        {
            get { return zCSVdata[8]; }
        }

        
        public string Senders_Name
        {
            get { return zCSVdata[9]; }
        }

        public string Senders_Address
        {
            get { return zCSVdata[10]; }
        }



        public string Senders_Postcode
        {
            get { return zCSVdata[11]; }
        }

        public string Senders_City
        {
            get { return zCSVdata[12]; }
        }






        public List<XMLRecord> MakeXmlRecords()
        {
            try
            {
                var lst = new List<XMLRecord>();
                if (PickUpCharge + FutileCharge + OtherCharge + WaitCharge == 0)
                {
                    RecordStatus = VendorHandler.DROP;
                    return null;
                }
                if (PickUpCharge > 0  )
                {
                    lst.Add( MakeXmlRecord(PickUpCharge, Type, VendorHandler.FRAGT));
                }
                if (FutileCharge > 0 && NoErrror())
                {
                    lst.Add(MakeXmlRecord(FutileCharge, "FutileCharge", VendorHandler.FRAGT));
                }
                if (WaitCharge > 0 && NoErrror())
                {
                    lst.Add(MakeXmlRecord(WaitCharge, "WaitCharge", VendorHandler.GEBYR));
                }
                if (OtherCharge > 0 && NoErrror())
                {
                    lst.Add(MakeXmlRecord(OtherCharge, "OtherCharge", VendorHandler.GEBYR));
                }
                
                
                return lst;
            }
            catch (Exception ex)
            {
                Error = true;
                RecordStatus = VendorHandler.E_ERROR;
                ErrorHelper.Add(ex.Message);
                
            }
            return null;

        }


        private bool NoErrror()
        {
            if (RecordStatus != VendorHandler.E_INI && RecordStatus.StartsWith("E_"))
            {
                return false;
            }
            return true;
        }

        public XMLRecord MakeXmlRecord(decimal price, string ptype, string  vtype)
        {
            GTXTranslate = TranslationHandler.DoTranslate(ptype, vtype);
            RecordStatus = GTXTranslate.KeyType;
            
            return new XMLRecord
            {
                Awb = this.Awb,
                InvoiceNumber = zInvoiceNumber,
                InvoiceDate = DateTime.Now,

                Price = price,
                Vat = -1,


                Services = this.Services,
                CarrierCode = "DHL",
                CarrierService = GTXTranslate.Key,
                GTXName = this.GTXTranslate.GTXName,
                KeyType = this.GTXTranslate.KeyType,
                Product = GTXTranslate.GTXProduct,
                Transport = (byte)GTXTranslate.GTXTransp,
                Shipdate = Date,

                CompanyName = Senders_Name,
                Address1 = Senders_Address,
                Address2 = "",
                City = Senders_City,
                State = "",
                Zip = Senders_Postcode,
                Country_Iata = "DK",
                NumberofCollies = (byte)1,

                Reference = Reference,
                BilledWeight = 1,
                Total_Weight = 1M,
                Vol_Weight = 1M,

                Description = "",
                Costprice = price,



            };
        }



       

        
        public DHLRecord(string[] data,  Translation translation, int lineno, string invoiceNumber)
        {

            InvLineNumber = lineno;
            zInvoiceNumber = invoiceNumber;
            zCSVdata = data;
          

            TranslationHandler = translation;
            Awb = Reference;
            if (Awb == "")
            {
                Awb= invoiceNumber + "_" + lineno;

            }

            
          
            XmlRecords = MakeXmlRecords();
            if (XmlRecords!=null && XmlRecords.Count > 0)
            {
                XmlRecord = XmlRecords[0];
            }
           
           

        }

      

       
       

       
       

        
      


    }

}
