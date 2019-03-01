using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using nu.gtx.DbMain.Standard.PM;

namespace UploadDHL
{
    class GLSrecord
    {
        // "Fakturanr.;Linjenr.;Dato;Varenr.;Beskrivelse;Land;Pakkenr.;Vægt;Antal;Salgspris;Beløb;Beløb inkl. moms;Reference;Modtagernavn;Kundenr.;Kundenavn;Kundenavn2;Modtagerpostnr.;Modtagerby;Modtageradresse";
        private string[] zCSVdata;
        public bool Error { get; set; }
        public bool TranslationError { get; set; }
        public bool FormatError { get; set; }
        public StringBuilder zReasonError;
        public string Fakturanr
        {
            get { return zCSVdata[0]; }
        }
        public string Linjenr
        {
            get { return zCSVdata[1]; }
        }
        public DateTime Dato
        {
            get { return SafeDate(zCSVdata[2]); }
        }

        public string VareNo
        {
            get { return zCSVdata[2]; }
        }

        public string Beskrivelse
        {
            get { return zCSVdata[3]; }
        }

        public string Land
        {
            get { return zCSVdata[4]; }
        }

        public string Awb
        {
            get { return zCSVdata[5]; }
        }

        public decimal Weight
        {
            get { return SafeDecimal(zCSVdata[6]); }
        }

        public int NoPrs
        {
            get { return SafeInt(zCSVdata[7]); }
        }

        public decimal SalesPrice
        {
            get { return SafeDecimal(zCSVdata[8]); }
        }
        public decimal Price
        {
            get { return SafeDecimal(zCSVdata[9]); }
        }
        public decimal PriceVat
        {
            get { return SafeDecimal(zCSVdata[10]); }
        }
        public string Reference
        {
            get { return zCSVdata[11]; }
        }
       
        public string Recievername
        {
            get { return zCSVdata[12]; }
        }
        public string CustomerNumber
        {
            get { return zCSVdata[13]; }
        }
        public string CustomerName
        {
            get { return zCSVdata[14]; }
        }

        public string CustomerName2
        {
            get { return zCSVdata[15]; }
        }
        public string RecieverZip
        {
            get { return zCSVdata[13]; }
        }
        public string Reci
        {
            get { return zCSVdata[14]; }
        }

        public string CustomerNamXX
        {
            get { return zCSVdata[15]; }
        }


        //public TranslationRecord GTXTranslate { get; set; }

        //public WeightFileRecord Convert()
        //{
        //    var wf = new WeightFileRecord();
        //    wf.Services = Services;
        //    wf.AWB = AWB;
        //    wf.BillWeight = BillWeight;
        //    wf.CreditorAccount = Factura;
        //    wf.SalesProduct = GTXProduct;
        //    wf.TransportProduct = GTXTransport;
        //    wf.Price = Price;
        //    return wf;


        //}
        //public InvoiceShipment StdConvert()
        //{



        //    if (GTXTranslate.KeyType == "FRAGT")
        //    {

        //        var wf = new InvoiceShipment
        //        {

        //            Status = 1,
        //            Invoice = Factura,
        //            InvoiceDate = FacturaDate,
        //            VendorAccount = BillingAccount,

        //            AWB = AWB,
        //            Product = GTXTranslate.GTXProduct,
        //            Transport = (byte)GTXTranslate.GTXTransp,
        //            Shipdate = Date,
        //            CustomerNumber = BillingAccount,
        //            CompanyName = Name,
        //            Address1 = "UnKnown",
        //            Address2 = "UnKnown",
        //            City = "Only Zip",
        //            State = "",
        //            Zip = FromZip,
        //            Country_Iata = SenderCountry,
        //            Reciever_CompanyName = Name,
        //            Reciever_Address1 = "UnKnown",
        //            Reciever_Address2 = "UnKnown",
        //            Reciever_City = "Only Zip",
        //            Reciever_State = "",
        //            Reciever_Zip = ToZip,
        //            Reciever_Country = ReceiverCountry,
        //            Reciever_Country_Iata = ReceiverCountry,
        //            Reciever_Phone = "00",
        //            Reciever_Fax = "00",
        //            Reciever_Email = "upload@gtx.nu",
        //            Reciever_Reference = "",
        //            NumberofCollies = (byte)1,
        //            Reference = "",
        //            Total_Weight = Weight,
        //            Length = null,
        //            Width = null,
        //            Height = null,
        //            Vol_Weight = VolWeight,
        //            BilledWeight = BillWeight,
        //            Customevalue = null,
        //            PackValue = null,
        //            PackValuta = null,
        //            Description = "",
        //            Costprice = Price,
        //            Saleprice = null,
        //            Oli = null




        //        };

        //        return wf;
        //    }


        //    return null;

        //}





        private int SafeInt(string no)
        {
            int o = 0;
            if (int.TryParse(no, out o))
            {
                return o;
            }
            return o;
        }
        private string SafeString(string data)
        {

            if (data == "" || data == "0")
            {
                return "";
            }


            return data;
        }
        private DateTime SafeDate(string data)
        {


            DateTime dd;
            if (DateTime.TryParse(data.Substring(0, 4) + "-" + data.Substring(4, 2) + "-" +
                                  data.Substring(6, 2), out dd))
            {
                return dd;
            }
            //zReasonError.AppendLine("DateTimeFormat error line " + zCurrentLine);
            FormatError = true;
            return new DateTime();

        }
        private decimal SafeDecimal(string data)
        {
            decimal dec;
            if (data == "")
            {
                return 0;
            }

            if (decimal.TryParse(data, NumberStyles.Any, CultureInfo.InvariantCulture, out dec))
            {
                return dec;
            }


            //zReasonError.AppendLine("DecimalFormat error line " + zCurrentLine);
            FormatError = true;
            return 0;
        }
    }
}
