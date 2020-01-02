using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;
using UploadDHL.DataUploadWeb;


namespace UploadDHL
{
    public class XMLRecord
    {
        public string Awb { get; set; }
        public DateTime Shipdate { get; set; }
        public string CompanyName { get; set; }

        /// <remarks/>
        public string Address1 { get; set; }

        public string City { get; set; }

       
        public string Zip { get; set; }


        public string KeyType { get; set; }
        public string GTXName { get; set; }

        public decimal Price { get; set; }
        public decimal Vat { get; set; }
        public List<Service> Services { get; set; }


        public string VendorAccount { get; set; }


        /// <remarks/>
        public int Product { get; set; }

        /// <remarks/>
        public byte Transport { get; set; }
        public GetForwarderId.ForwObj ForwarderObj{ get; set; }

       
        public string InvoiceNumber { get; set; }

        public int InvoiceLineNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime Due_Date { get; set; }

       



        public string CarrierCode { get; set; }

        public string CarrierService { get; set; }
       

        /// <remarks/>
      

        /// <remarks/>
        public string CustomerNumber { get; set; }

        /// <remarks/>
        
        /// <remarks/>
        public string Address2 { get; set; }

        /// <remarks/>
        public string State { get; set; }

        /// <remarks/>
        public string Country_Iata { get; set; }

        /// <remarks/>
        public string Reciever_CompanyName { get; set; }

        /// <remarks/>
        public string Reciever_Address1 { get; set; }

        /// <remarks/>
        public string Reciever_Address2 { get; set; }

        /// <remarks/>
        public string Reciever_City { get; set; }

        /// <remarks/>
        public string Reciever_State { get; set; }

        /// <remarks/>
        public string Reciever_Zip { get; set; }

        /// <remarks/>
        public string Reciever_Country { get; set; }

        /// <remarks/>
        public string Reciever_Country_Iata { get; set; }

        /// <remarks/>
        public string Reciever_Phone { get; set; }

        /// <remarks/>
        public string Reciever_Fax { get; set; }

        /// <remarks/>
        public string Reciever_Email { get; set; }

        /// <remarks/>
        public string Reciever_Reference { get; set; }

        /// <remarks/>
        public byte NumberofCollies { get; set; }

        /// <remarks/>
        public string Reference { get; set; }

        /// <remarks/>
        public decimal Total_Weight { get; set; }

        /// <remarks/>
        public decimal Length { get; set; }

        /// <remarks/>
        public decimal Width { get; set; }

        /// <remarks/>
        public decimal Height { get; set; }

        /// <remarks/>
        public decimal Vol_Weight { get; set; }

        /// <remarks/>
        public decimal BilledWeight { get; set; }

        /// <remarks/>
        public decimal Customevalue { get; set; }

        /// <remarks/>
        public decimal PackValue { get; set; }

        /// <remarks/>
        public string PackValuta { get; set; }

        /// <remarks/>
        public string Description { get; set; }

        /// <remarks/>
        public decimal Costprice { get; set; }

        /// <remarks/>
        public decimal Saleprice { get; set; }

        /// <remarks/>
        public decimal Oil()
        {
          return   Services.Where(x => x.GTXCode == "FOILE").Sum(x => x.Price);
        }
        public decimal Total_Price()
        {
            return Vat + Services.Sum(x => x.Price);
        }
        public decimal Total_Vat()
        {
            return Price + Services.Sum(x => x.Tax);
        }

        public WeightFileRecord Convert()
        {


            var awbn = Awb;
            if (KeyType == "GEBYR")
            {
                awbn = "#" + Awb;
            }
            var wf = new WeightFileRecord();
            wf.Services = Services;
            wf.AWB = awbn;
            wf.BillWeight = BilledWeight;
            wf.CreditorAccount = InvoiceNumber;
            wf.SalesProduct = Product;
            wf.TransportProduct =Transport;
            wf.Price = Price;
            return wf;


        }

        public int ForwarderId()
        {
            if (ForwarderObj != null)
            {
                return ForwarderObj.Id;
            }
            return 0;
        }

        public string ToStringRec() {
            
                var sep = ";";
           
                return CarrierCode +sep+ForwarderId() + sep + Awb + sep + InvoiceNumber + sep + InvoiceDate + sep + Price + sep + Vat +
                       sep + CarrierService + sep + GTXName + sep + KeyType + sep + Product + sep + Transport + sep +
                       Shipdate + sep + CompanyName + sep + Address1 + sep + City + sep + Zip + sep + Reference + sep +
                       Description + sep + Costprice;
            } 

        public InvoiceShipmentHolder StdConvert()
        {



            if (KeyType == "FRAGT")
            {
                var sp = ServicePart();
                var wf = new InvoiceShipmentHolder()
                {

                    Status = 1,
                    Invoice = InvoiceNumber,
                    InvoiceDate = DateTime.Now,
                    VendorAccount = VendorAccount,
                    AWB = Awb,
                    Product = Product,
                    Transport = Transport,
                    Shipdate = Shipdate,
                    CustomerNumber = VendorAccount,
                    CompanyName = CompanyName,
                    Address1 = Address1,
                    Address2 = Address2,
                    City =City,
                    State = State,
                    Zip = Zip,
                    Country_Iata =Country_Iata,
                    Reciever_CompanyName = Reciever_CompanyName,
                    Reciever_Address1 = Reciever_Address1,
                    Reciever_Address2 = Reciever_Address2,
                    Reciever_City = Reciever_City,
                    Reciever_State = Reciever_State,
                    Reciever_Zip = Reciever_Zip,
                    Reciever_Country = Reciever_Country,
                    Reciever_Country_Iata = Reciever_Country_Iata,
                    Reciever_Phone = Reciever_Phone,
                    Reciever_Fax = Reciever_Fax,
                    Reciever_Email = Reciever_Email,
                    Reciever_Reference = Reciever_Reference,
                    NumberofCollies = NumberofCollies,
                    Reference = Reference,
                    Total_Weight = Total_Weight,
                    Length = Length,
                    Width = Width,
                    Height = Height,
                    Vol_Weight = Vol_Weight,
                    BilledWeight = BilledWeight,
                    Customevalue = Customevalue,
                    PackValue = PackValue,
                    PackValuta = PackValuta,
                    Description = Description,
                    Costprice = Costprice,
                    Saleprice = null,
                    Oli = null,
                    ServiceList = sp




                };

                return wf;
            }


            return null;

        }



        public  string ServicePart()
        {
            var s = Services.Select(x => x.GTXCode + ":" + x.Price.ToString(CultureInfo.GetCultureInfo("da-DK"))).ToArray();
            return string.Join("|", s);
        }


    }





}
