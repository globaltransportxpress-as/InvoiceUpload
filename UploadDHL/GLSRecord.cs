﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

using UploadDHL.DataUploadWeb;

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
            get { return zCSVdata[3]; }
        }

        public string Beskrivelse
        {
            get { return zCSVdata[4]; }
        }

        public string Land
        {
            get { return zCSVdata[5]; }
        }

        private string zCountry;
        public string Country
        {
            get { return zCountry; }
        }
        public string Pakkenr
        {
            get { return zCSVdata[6]; }
        }

        public decimal Vægt
        {
            get { return zWeight; }
            
        }

        public int Antal
        {
            get { return SafeInt(zCSVdata[8]); }
        }

        public decimal Salgspris
        {
            get { return SafeDecimal(zCSVdata[9]); }
        }

        private decimal zPrice;
        public decimal Beløb
        {
            get { return zPrice; }
        }
        public decimal Beløbinklmoms
        {
            get { return SafeDecimal(zCSVdata[11]); }
        }
        public string Reference
        {
            get { return zCSVdata[12]; }
        }
       
        public string Modtagernavn
        {
            get { return zCSVdata[13]; }
        }
        public string Kundenr
        {
            get { return zCSVdata[14]; }
        }
        public string Kundenavn
        {
            get { return zCSVdata[15]; }
        }

        public string Kundenavn2
        {
            get { return zCSVdata[16]; }
        }
        public string Modtagerpostnr
        {
            get { return zCSVdata[17]; }
        }
        public string Modtagerby
        {
            get { return zCSVdata[18]; }
        }

        public string Modtageradresse
        {
            get { return zCSVdata[19]; }
        }
        public string Awb
        {
            get { return MakeCheckNumber(Pakkenr); }
        }

        private decimal zWeight;
        public GLSrecord(string[] fields, Translation translationhandler)
        {

           
            zCSVdata = fields;
            GTXTranslate = translationhandler.DoTranslate(VareNo, "FRAGT");
            zWeight = SafeDecimal(zCSVdata[7]);
            zPrice = SafeDecimal(zCSVdata[10]);
            if (Land == "")
            {
                zCountry = "DK";
            }
            else
            {
                var gtxtrans = translationhandler.DoTranslate(Land, "COUNTRY");
                if (gtxtrans == null)
                {
                    GTXTranslate = null; // to indicate translation error
                }
                else
                {
                    zCountry = gtxtrans.GTXName;

                }

            }


        }
        public List<Service> Services = new List<Service>();

        public TranslationRecord GTXTranslate { get; set; }

        public void AddWeight(decimal w)
        {
            zWeight = zWeight + w;
        }
        public void AddPrice(decimal w)
        {
            zPrice = zPrice + w;
        }

        public WeightFileRecord Convert()
        {
            var awbn = Awb;
            if (GTXTranslate.KeyType == "GEBYR")
            {
                awbn = "#" + Awb;
            }
            var wf = new WeightFileRecord();
            wf.Services = Services;
            wf.AWB = awbn;
            wf.BillWeight = Vægt;
            wf.CreditorAccount = Fakturanr;
            wf.SalesProduct = GTXTranslate.GTXProduct;
            wf.TransportProduct = GTXTranslate.GTXTransp;
            wf.Price = Beløb;
            return wf;
        }
        public InvoiceShipmentHolder StdConvert()
        {



            if (GTXTranslate.KeyType == "FRAGT")
            {

                var wf = new InvoiceShipmentHolder()
                {

                    Status = 1,
                    Invoice = Fakturanr,
                    InvoiceDate = DateTime.Now,
                    VendorAccount = Kundenr,

                    AWB = Awb,
                    Product = GTXTranslate.GTXProduct,
                    Transport = (byte)GTXTranslate.GTXTransp,
                    Shipdate = Dato,
                    CustomerNumber = Kundenr,
                    CompanyName = Kundenavn,
                    Address1 = "UnKnown",
                    Address2 = "UnKnown",
                    City = "Only Zip",
                    State = "",
                    Zip = "0000",
                    Country_Iata = "DK",
                    Reciever_CompanyName = Modtagernavn,
                    Reciever_Address1 = Modtageradresse,
                    Reciever_Address2 = "UnKnown",
                    Reciever_City = Modtagerby,
                    Reciever_State = "",
                    Reciever_Zip = Modtagerpostnr,
                    Reciever_Country = Country,
                    Reciever_Country_Iata = Country,
                    Reciever_Phone = "00",
                    Reciever_Fax = "00",
                    Reciever_Email = "upload@gtx.nu",
                    Reciever_Reference = " ",
                    NumberofCollies = (byte)1,
                    Reference = "",
                    Total_Weight = Vægt,
                    Length = null,
                    Width = null,
                    Height = null,
                    Vol_Weight = Vægt,
                    BilledWeight = Vægt,
                    Customevalue = null,
                    PackValue = null,
                    PackValuta = null,
                    Description = "",
                    Costprice = Beløb,
                    Saleprice = null,
                    Oli = null




                };

                return wf;
            }


            return null;

        }

        public string MakeCheckNumber( string number)
        {
            //  AwbNumber awbnumber = db().GetAwb(product, 1);

            //string number =awbnumber.Prefix + awbnumber.Awb.ToString();
            if (number == "")
            {
                return number;
            }
            int addciffer = 1;
            if (VareNo.StartsWith("008"))
            {
                addciffer = 0;
            }

            int Even = 0;
            int Uneven = 0;
            char[] textnumber = new char[12];

            for (int count = 0; count <= 11; count++)
            {
                textnumber[count] = '0';
            }
            textnumber = number.ToCharArray();
            //Uneven
            for (int count = 0; count <= textnumber.Length - 1; count = count + 2)
            {
                Uneven += int.Parse(textnumber[count].ToString());
            }

            //Even
            for (int count = 1; count <= textnumber.Length - 1; count = count + 2)
            {
                Even += int.Parse(textnumber[count].ToString());
            }

            double TotalSum =( Even + (3 * Uneven) + addciffer);


            double Roundup = 10 * System.Convert.ToInt64(((TotalSum + 5) / 10));


            var Check = Roundup - TotalSum;
            if (Math.Abs(Check - 10) < 0.1)
                Check = 0;

            return number + Check.ToString(CultureInfo.InvariantCulture);
        }




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
            if (DateTime.TryParse(data
                                  , out dd))
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

            if (decimal.TryParse(data, NumberStyles.Any, CultureInfo.CurrentCulture, out dec))
            {
                return dec;
            }


            //zReasonError.AppendLine("DecimalFormat error line " + zCurrentLine);
            FormatError = true;
            return 0;
        }
    }
}
