using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nu.gtx.DbMain.Standard.PM;

namespace UploadDHL
{
    class PDKrecord
    {

        public String AWB  { get; set; }
        public String BillingAccount { get; set; }
        public DateTime Date { get; set; }
        public DateTime FacturaDate { get; set; }
        public string CustomerNumber { get; set; }
        public String Factura { get; set; }
        public String Order { get; set; }
        public String OrderLine { get; set; }
        public String FromZip { get; set; }
        public String ToZip { get; set; }

        public decimal Price { get; set; }
        public decimal PriceVat { get; set; }
        public bool Vat { get; set; }

        public String Material { get; set; }
        public String GTXMatrial { get; set; }
        public int GTXProduct { get; set; }
        public int GTXTransport { get; set; }

        public String Frankering { get; set; }
        public String Moms { get; set; }
        public decimal Weight { get; set; }
        public decimal VolWeight { get; set; }
        public decimal BillWeight { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }
        public String SenderCountry { get; set; }
        public String ReceiverCountry { get; set; }


        public List<Service> Services { get; set; }

        public string[] HeaderInfo { get; set; }
        public TranslationRecord GTXTranslate { get; set; }

        public WeightFileRecord Convert()
        {
            var wf = new WeightFileRecord();
            wf.Services = Services;
            wf.AWB = AWB;
            wf.BillWeight = BillWeight;
            wf.CreditorAccount = Factura;
            wf.SalesProduct = GTXProduct;
            wf.TransportProduct = GTXTransport;
            wf.Price = Price;
            return wf;


        }
        public InvoiceShipment StdConvert()
        {

            
               
                if (GTXTranslate.KeyType == "FRAGT")
                {

                    var wf = new InvoiceShipment
                    {

                        Status = 1,
                        Invoice = Factura,
                        InvoiceDate = FacturaDate,
                        VendorAccount = BillingAccount,

                        AWB = AWB,
                        Product = GTXTranslate.GTXProduct,
                        Transport = (byte)GTXTranslate.GTXTransp,
                        Shipdate = Date,
                        CustomerNumber = BillingAccount,
                        CompanyName = Name,
                        Address1 = "UnKnown",
                        Address2 = "UnKnown",
                        City = "Only Zip",
                        State ="",
                        Zip = FromZip,
                        Country_Iata = SenderCountry,
                        Reciever_CompanyName = Name,
                        Reciever_Address1 = "UnKnown",
                        Reciever_Address2 = "UnKnown",
                        Reciever_City = "Only Zip",
                        Reciever_State = "",
                        Reciever_Zip = ToZip,
                        Reciever_Country = ReceiverCountry,
                        Reciever_Country_Iata = ReceiverCountry,
                        Reciever_Phone = "00",
                        Reciever_Fax = "00",
                        Reciever_Email = "upload@gtx.nu",
                        Reciever_Reference = "",
                        NumberofCollies = (byte)1,
                        Reference = "",
                        Total_Weight = Weight,
                        Length = null,
                        Width = null,
                        Height = null,
                        Vol_Weight = VolWeight,
                        BilledWeight = BillWeight,
                        Customevalue = null,
                        PackValue = null,
                        PackValuta = null,
                        Description = "",
                        Costprice = Price,
                        Saleprice = null,
                        Oli = null




                    };

                    return wf;
                }
           

            return null;

        }




    }
}
