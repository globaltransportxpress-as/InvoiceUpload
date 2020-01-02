using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UploadDHL.DataUploadWeb;

namespace UploadDHL
{
    class Palletrecord
    {
       

        public String AWB  { get; set; }
        public String BillingAccount { get; set; }
        public DateTime Date { get; set; }
   
        public String Factura { get; set; }
        
        public String FromZip { get; set; }
        public String ToZip { get; set; }

        public decimal Price { get; set; }
        public decimal PriceOil { get; set; }
      

        public String Material { get; set; }
      
        public int GTXProduct { get; set; }
        public int GTXTransport { get; set; }



        
       public decimal BillWeight { get; set; }
       
        public String Name { get; set; }
     
        public String SenderCountry { get; set; }
        public String ReceiverName { get; set; }
        public String ReceiverCountry { get; set; }
        public String SenderCity { get; set; }
        public String ReceiverCity { get; set; }
        public String Ref { get; set; }
        public List<Service> Services { get; set; }

        public string[] HeaderInfo { get; set; }
        public TranslationRecord GTXTranslate { get; set; }



        public Palletrecord(string[] data)
        {

            Date = DateTime.Parse(data[0]);
            BillingAccount = data[1];
            AWB = data[2];
            BillWeight = decimal.Parse(data[4]);
            SenderCountry = data[5];
            ReceiverCountry = data[6];
            Material = data[7];
            Price = decimal.Parse(data[8]);
            PriceOil = decimal.Parse(data[9]);

            Ref = data[10];
            Name = data[11];
            SenderCity = data[12];
            FromZip = data[13];
            ReceiverName = data[14];
            ReceiverCity = data[15];
            ToZip = data[16];
            Factura = data[17];





        }
        public WeightFileRecord Convert()
        {


            var awbn = AWB;
            if (GTXTranslate.KeyType == "GEBYR")
            {
                awbn = "#" + AWB;
            }
            var wf = new WeightFileRecord();
            wf.Services = Services;
            wf.AWB = awbn;
            wf.BillWeight = BillWeight;
            wf.CreditorAccount = Factura;
            wf.SalesProduct = GTXTranslate.GTXProduct;
            wf.TransportProduct = GTXTranslate.GTXTransp;
            wf.Price = Price;

            return wf;


        }
        public InvoiceShipmentHolder StdConvert()
        {

            
               
                if (GTXTranslate.KeyType == "FRAGT")
                {

                    var wf = new InvoiceShipmentHolder
                    {

                        Status = 1,
                        Invoice = Factura,
                        InvoiceDate = DateTime.Now,
                        VendorAccount = BillingAccount,

                        AWB = AWB,
                        Product = GTXTranslate.GTXProduct,
                        Transport = (byte)GTXTranslate.GTXTransp,
                        Shipdate = Date,
                        CustomerNumber = BillingAccount,
                        CompanyName = Name,
                        Address1 = ".",
                        Address2 = ".",
                        City = SenderCity,
                        State ="",
                        Zip = FromZip,
                        Country_Iata = SenderCountry,
                        Reciever_CompanyName = Name,
                        Reciever_Address1 = ".",
                        Reciever_Address2 = ".",
                        Reciever_City = ReceiverCity,
                        Reciever_State = "",
                        Reciever_Zip = ToZip,
                        Reciever_Country = ReceiverCountry,
                        Reciever_Country_Iata = ReceiverCountry,
                        Reciever_Phone = "00000000",
                        Reciever_Fax = "00",
                        Reciever_Email = "upload@gtx.nu",
                        Reciever_Reference = "",
                        NumberofCollies = (byte)1,
                        Reference = "",
                        Total_Weight = BillWeight,
                        Length = null,
                        Width = null,
                        Height = null,
                        Vol_Weight = BillWeight,
                        BilledWeight = BillWeight,
                        Customevalue = null,
                        PackValue = null,
                        PackValuta = null,
                        Description = Ref,
                        Costprice = Price,
                        Saleprice = null,
                        Oli = PriceOil




                    };

                    return wf;
                }
           

            return null;

        }




    }
}
