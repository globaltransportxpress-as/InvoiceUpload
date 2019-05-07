using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using nu.gtx.DbMain.Standard.PM;

namespace UploadDHL
{
    class GTXrecord
    {
        // "Fakturanr.;Linjenr.;Dato;Varenr.;Beskrivelse;Land;Pakkenr.;Vægt;Antal;Salgspris;Beløb;Beløb inkl. moms;Reference;Modtagernavn;Kundenr.;Kundenavn;Kundenavn2;Modtagerpostnr.;Modtagerby;Modtageradresse";
        private string[] zCSVdata;
        public bool Error { get; set; }
        public bool TranslationError { get; set; }
        public bool FormatError { get; set; }
        public StringBuilder zReasonError;

        public string PURCHORDERFORMNUM
        {
            get { return zCSVdata[0]; }
        }
        public int Pieces
        {
            get { return SafeInt(zCSVdata[1]); }
        }
        public decimal Weight
        {
            get { return SafeDecimal(zCSVdata[2]); }
        }
        public string LNO
        {
            get { return zCSVdata[3]; }
        }
        public decimal Amount
        {
            get { return SafeDecimal( zCSVdata[4]); }
        }
        public DateTime RECEIPTDATECONFIRMED
        {
            get { return SafeDate(zCSVdata[5]); }
        }
        public string PICKUPNAME
        {
            get { return zCSVdata[6]; }
        }
        public string PICKUPCOUNTRY
        {
            get { return zCSVdata[7]; }
        }
        public string DELIVERYNAME
        {
            get { return zCSVdata[8]; }
        }
        public string DELIVERYCOUNTRYREGIONID
        {
            get { return zCSVdata[9]; }
        }
        public string CUSTOMERREF
        {
            get { return zCSVdata[10]; }
        }
        public string ORGNUMBER
        {
            get { return zCSVdata[11]; }
        }
        public string INVOICEACCOUNT
        {
            get { return zCSVdata[12]; }
        }
        public string NAME
        {
            get { return zCSVdata[13]; }
        }
        public string VARENUMMER
        {
            get { return zCSVdata[14]; }
        }
        public string VARENAVN
        {
            get { return zCSVdata[15]; }
        }
        public string ACCOUNTNUM
        {
            get { return zCSVdata[16]; }
        }
        public string ProductGroup
        {
            get { return zCSVdata[17]; }
        }
        public string SALESID
        {
            get { return zCSVdata[18]; }
        }
        public string INTERCOMPANYORIGINALSALESID
        {
            get { return zCSVdata[19]; }
        }
        public string INVOICEID
        {
            get { return zCSVdata[20]; }
        }
        public DateTime INVOICEDATE
        {
            get { return SafeDate(zCSVdata[21]); }
        }
        public string LINENUM
        {
            get { return zCSVdata[22]; }
        }
        public decimal OilAmount
        {
            get { return SafeDecimal(zCSVdata[23]); }
        }
        public string TXT
        {
            get { return zCSVdata[24]; }
        }
        public string TAXWRITECODE
        {
            get { return zCSVdata[25]; }
        }
        public string DIMENSION
        {
            get { return zCSVdata[26]; }
        }
        public string DIMENSION2_
        {
            get { return zCSVdata[27]; }
        }
        public string DIMENSION3_
        {
            get { return zCSVdata[28]; }
        }
        public string PU_ADRESS
        {
            get { return zCSVdata[29].Replace(SafeZip(PU_CITY),"").Replace(SafeZip(PU_ZIP),""); }
        }
        public string PU_CITY
        {
            get { return zCSVdata[30]; }
        }
        public string PU_ZIP
        {
            get { return zCSVdata[31]; }
        }
        public string DL_ADRESS
        {
            get { return zCSVdata[32].Replace(SafeZip(DL_CITY),"").Replace(SafeZip(DL_ZIP), ""); }
        }
        public string DL_CITY
        {
            get { return zCSVdata[33]; }
        }
        public string DL_ZIP
        {
            get { return zCSVdata[34]; }
        }
        public string YEAR
        {
            get { return zCSVdata[35]; }
        }

        public GTXrecord(string[] fields, Translation translationhandler)
        {


            zCSVdata = fields;
            GTXTranslate = translationhandler.DoTranslate(DIMENSION + "_" + LNO + "_" + DIMENSION2_ + "_" + DIMENSION3_ + "_" + VARENUMMER, "FRAGT");

            if (GTXTranslate != null && !GTXTranslate.KeyType.StartsWith("X") )
            {
                var e = 2;
            }



        }
        public List<Service> Services = new List<Service>();

        public TranslationRecord GTXTranslate { get; set; }

        

        public WeightFileRecord Convert()
        {
            var awbn = PURCHORDERFORMNUM;
            if (GTXTranslate.KeyType == "GEBYR")
            {
                awbn = "#" + PURCHORDERFORMNUM;
            }
            else
            {
                awbn = PURCHORDERFORMNUM;
            }
            var wf = new WeightFileRecord();
            wf.Services = Services;
            wf.AWB = awbn;
            wf.BillWeight = Weight;
            wf.CreditorAccount = INVOICEID;
            wf.SalesProduct = GTXTranslate.GTXProduct;
            wf.TransportProduct = GTXTranslate.GTXTransp;
            wf.Price = Amount;
           
            return wf;
        }
        public InvoiceShipment StdConvert()
        {



            if (GTXTranslate.KeyType == "FRAGT")
            {

                var wf = new InvoiceShipment
                {

                    Status = 1,
                    Invoice = INVOICEID,
                    InvoiceDate = DateTime.Now,
                    VendorAccount = ACCOUNTNUM,

                    AWB = PURCHORDERFORMNUM,
                    Product = GTXTranslate.GTXProduct,
                    Transport = (byte)GTXTranslate.GTXTransp,
                    Shipdate = RECEIPTDATECONFIRMED,
                    CustomerNumber = INVOICEACCOUNT,
                    CompanyName = PICKUPNAME,
                    Address1 = SafeAddr(PU_ADRESS,0),
                    Address2 = SafeAddr(PU_ADRESS, 1),
                    City = PU_CITY,
                    State = "",
                    Zip = PU_ZIP,
                    Country_Iata = PICKUPCOUNTRY,
                    Reciever_CompanyName = DELIVERYNAME,
                    Reciever_Address1 = SafeAddr(DL_ADRESS,0),
                    Reciever_Address2 = SafeAddr(DL_ADRESS, 1),
                    Reciever_City = DL_CITY,
                    Reciever_State = "",
                    Reciever_Zip =DL_ZIP,
                    Reciever_Country = DELIVERYCOUNTRYREGIONID,
                    Reciever_Country_Iata = DELIVERYCOUNTRYREGIONID,
                    Reciever_Phone = "00",
                    Reciever_Fax = "00",
                    Reciever_Email = "upload@gtx.nu",
                    Reciever_Reference = CUSTOMERREF,
                    NumberofCollies = (byte)Pieces,
                    Reference = CUSTOMERREF,
                    Total_Weight = Weight,
                    Length = null,
                    Width = null,
                    Height = null,
                    Vol_Weight = Weight,
                    BilledWeight = Weight,
                    Customevalue = null,
                    PackValue = null,
                    PackValuta = null,
                    Description = "",
                    Costprice = Amount,
                    Saleprice = null,
                    Oli = OilAmount




                };

                return wf;
            }


            return null;

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
                FormatError = true;
               
            }
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
