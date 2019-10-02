using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UploadDHL.DataConnections;
using UploadDHL.DataUploadWeb;

namespace UploadDHL
{
    class PDKrecord:DataRecord
    {

      
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

        private Dictionary<string, int> zDic;



        public string[] HeaderInfo { get; set; }

        public PDKrecord(string[] da, Dictionary<string,int> dic, Translation translation, string fixhead, int lineno)
        {
            zDic = dic;
            Material = ReplaceList(da[dic["Materiale"]].Trim().ToUpper(), " 12345678908()-.,");
            InvLineNumber = lineno;
            GTXTranslate = translation.DoTranslate(Material, VendorHandler.FRAGT);


            RecordStatus = GTXTranslate.KeyType;

            if (RecordStatus == VendorHandler.FRAGT || RecordStatus == VendorHandler.GEBYR)
            {
               
                Awb = CheckDigits(da[dic["Stregkode"]], GTXTranslate.GTXProduct);
                Price = SafeLookupDecimal(da, "Grundpris");




                Date = DateConvert(da, "Dato");
                Order = da[zDic["Ordre"]];
                OrderLine = da[zDic["Ordrepos."]];

                Frankering = da[zDic["Frankering"]];
                Vat = SafeLookUp(da, "Momsbelagt", "").Equals("X");

                PriceVat = SafeLookupDecimal(da, "Ialt(excl.moms)");
                FromZip = da[zDic["Frapostnr"]];
                ToZip = da[zDic["Tilpostnr"]];
                SenderCountry = da[zDic["Fra-land"]];
                ReceiverCountry = da[zDic["Til-land"]];
                Weight = SafeLookupDecimal(da, "Vægt");




                VolWeight = SafeLookupDecimal(da, "Volumenvægt");
                BillWeight = SafeLookupDecimal(da, "Faktureretvægt");
                
                Length = SafeLookupDecimal(da, "Længde");
                Width = SafeLookupDecimal(da, "Bredde");
                Height = SafeLookupDecimal(da, "Højde");
                Name = SafeLookUp(da, "Navn1", "No name");
                Address = SafeLookUp(da, "Adresse", "No address");

                foreach (var k in zDic.Keys.Where(x => !fixhead.Contains(x + ";")))
                {

                    if (SafeLookupDecimal(da, k) > 0)
                    {
                        var trans = translation.TranDictionary[k];
                        if (trans.KeyType == VendorHandler.GEBYR)
                        {
                            var sv = new Service
                            {
                                OrigalName = k,
                                GTXCode = trans.GTXName,
                                Price = SafeLookupDecimal(da, k),
                                InvoiceLineNumber = InvLineNumber


                        };
                        Services.Add(sv);
                        }



                    }
                }
            }
           
            

            
        }
       
       
        public XMLRecord MakeXmlRecord()
        {
            if (string.IsNullOrEmpty(ReceiverCountry))
            {
                ReceiverCountry = "DK";
            }
            return new XMLRecord
            {
                Awb = this.Awb,
                InvoiceNumber = this.Factura,
              
                Price = this.Price,
                
                Vat = this.PriceVat - this.Price,
                GTXName = this.GTXTranslate.GTXName,
                KeyType = this.GTXTranslate.KeyType,
              
                Services = this.Services,
                InvoiceDate = FacturaDate,
                Due_Date = FacturaDate.AddDays(30),
                VendorAccount = CustomerNumber,
                Product = GTXTranslate.GTXProduct,
                Transport = (byte)GTXTranslate.GTXTransp,
                Shipdate = Date,
         
                CompanyName = Name,
                Address1 = "UnKnown",
                Address2 = "UnKnown",
                City = "Only Zip",
                State = "",
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
                Length = 0,
                Width = 0,
                Height = 0,
                Vol_Weight = VolWeight,
                BilledWeight = BillWeight,
                Customevalue = 0,
                PackValue = 0,
                PackValuta = "",
                Description = "",
                Costprice = Price,
                
                
             


            };
        }


        private DateTime DateConvert(string[] da, string fieldname)
        {
            DateTime dd;
            if (DateTime.TryParse(SafeLookUp(da, fieldname, ""), out dd))
            {
                return dd;
            }

            RecordStatus = VendorHandler.E_DATE;
           ErrorHelper.Add("Field name :" + fieldname);
           return DateTime.Now;

        }

        private decimal SafeLookupDecimal(string[] da, string fieldname)
        {
            decimal dec;
            var lookup = SafeLookUp(da, fieldname, "err");
            if (lookup == "err")
            {

                RecordStatus = VendorHandler.E_DIC;
                ErrorHelper.Add("Field not exist"+ fieldname);
              
                return 0;

            }

            if (lookup == "")
            {
                return 0;
            }

            if (decimal.TryParse(lookup, NumberStyles.Any, CultureInfo.GetCultureInfo("da-DK"), out dec))
            {
                return dec;
            }
            ErrorHelper.Add("Decimal Error" + fieldname);
            RecordStatus = VendorHandler.E_DECIMAL;

            return 0;
        }
        private string SafeLookUp(string[] da, string code, string def)
        {
            if (zDic.ContainsKey(code))
            {
                return da[zDic[code]];

            }

            return def;

        }

        private static string CheckDigits(string awb, int prod)
        {
            if (awb.StartsWith("050050") || ((prod == 110 || prod == 76) && awb.StartsWith("050")))
            {

                int CheckD = 36;
                int Mod = 36;


                char[] isoval2ascii =
                {
                    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                    'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                    'U', 'V', 'W', 'X', 'Y', 'Z', '*'
                };


                char[] awbarray = awb.ToCharArray(0, awb.Length);

                foreach (char c in awbarray)
                {
                    int charvalue = Array.IndexOf(isoval2ascii, c);
                    if (charvalue == -1)
                        return "-1";

                    CheckD += charvalue;
                    if (CheckD > Mod)
                        CheckD = CheckD - Mod;
                    CheckD = CheckD * 2;

                    if (CheckD > Mod)
                        CheckD = CheckD - (Mod + 1);

                }

                CheckD = (Mod + 1) - CheckD;
                if (CheckD == Mod)
                    CheckD = 0;

                return awb + isoval2ascii[CheckD].ToString();
            }
            return awb;

        }


    }
}
