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
        public string Linjenr
        {
            get { return zCSVdata[1]; }
        }
        public DateTime Dato
        {
            get { return SafeDate(zCSVdata[2], "Dato"); }
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
            get { return SafeDecimal(zCSVdata[7], "Vægt"); }

        }

        public int Antal
        {
            get { return SafeInt(zCSVdata[8]); }
        }

        public decimal Salgspris
        {
            get { return SafeDecimal(zCSVdata[9], "Salgspris"); }
        }

        private decimal zPrice;
        public decimal Beløb
        {
            get { return SafeDecimal(zCSVdata[10], "Beløb"); }
        }
        public decimal Beløbinklmoms
        {
            get { return SafeDecimal(zCSVdata[11], "Beløbinklmoms"); }
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


        private decimal zWeight;
        public GLSrecord(string[] fields, Translation th, int lineno)
        {

          
            TranslationHandler = th;
            zCSVdata = fields;
            InvLineNumber = lineno;
            GTXTranslate = TranslationHandler.DoTranslate(VareNo, VendorHandler.FRAGT);
            RecordStatus = GTXTranslate.KeyType;
            zWeight = Vægt;
            zPrice = Beløb;
            Awb = MakeCheckNumber(Pakkenr);
            

            
            if (Land == "")
            {
                zCountry = "DK";
            }
            else
            {
                var gtxtrans = TranslationHandler.DoTranslate(Land, "COUNTRY");
                if (gtxtrans.KeyType == VendorHandler.E_TRANS)
                {
                    GTXTranslate.KeyType = VendorHandler.E_COUNTRY;
                }
                else
                {
                    zCountry = gtxtrans.GTXName;


                }

            }

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
                InvoiceDate = this.Dato,
                Due_Date = this.Dato.AddDays(30),
                Price = this.Beløb,
                Vat = this.Beløbinklmoms - this.Beløb,

               
                Services = this.Services,
                VendorAccount = Kundenr,
                CarrierService = GTXTranslate.Key,
                GTXName = this.GTXTranslate.GTXName,
                KeyType = this.GTXTranslate.KeyType,
                Product = GTXTranslate.GTXProduct,
                Transport = (byte)GTXTranslate.GTXTransp,
                Shipdate = Dato,

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
                Length = 0,
                Width = 0,
                Height = 0,
                Vol_Weight = Vægt,
                BilledWeight = Vægt,
                Customevalue = 0,
                PackValue = 0,
                PackValuta = "",
                Description = "",
                Costprice = Beløb,





            };
        }


        public string MakeCheckNumber(string number)
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

            double TotalSum = (Even + (3 * Uneven) + addciffer);


            double Roundup = 10 * System.Convert.ToInt64(((TotalSum + 5) / 10));


            var Check = Roundup - TotalSum;
            if (Math.Abs(Check - 10) < 0.1)
                Check = 0;

            return number + Check.ToString(CultureInfo.InvariantCulture);
        }







    }
}
