using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
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


        public decimal FINVIR { get; set; }
      

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

        public string[] servicenames= "FINVIR|H79402|H79412|H79413|H79406|H79401|H79400|H79403|HTBD".Split('|');

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
            Services = new List<Service>();

            for (var i = 10; i < 19; i++)
            {
               var price = decimal.Parse(data[i]);
                if (price > 0)
                {
                    var ser = new Service();
                    ser.Price = price;
                    ser.GTXCode = servicenames[i - 10];
                     
                    Services.Add(ser);

                }


                

            }
          







        Ref = data[19];
            Name = data[20];
            SenderCity = data[21];
            FromZip = data[22];
            ReceiverName = data[23];
            ReceiverCity = data[24];
            ToZip = data[25];
            Factura = data[26];





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
                        Reciever_CompanyName = ReceiverName,
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
