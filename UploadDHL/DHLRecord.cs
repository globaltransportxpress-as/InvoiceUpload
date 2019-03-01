using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using nu.gtx.DbMain.Standard.PM;


namespace UploadDHL
{
    class DHLRecord
    {

        private StringBuilder zXmlOut;
        private Translation zTranslation;
        private DHLXML zDhlXml;
        private int zCount;
        public bool Error { get; set; }
        public bool TranslationError { get; set; }
        public bool FormatError { get; set; }
        public StringBuilder zReasonError;




        public string InvoiceName { get; set; }

        private decimal zTaxCharge;
        private decimal zNonTaxCharge;


        private string[] zCSVdata;

        public string Line_Type
        {
            get { return zCSVdata[0]; }
        }

        public string Billing_Source
        {
            get { return zCSVdata[1]; }
        }

        public string Original_Invoice_Number
        {
            get { return zCSVdata[2]; }
        }

        public string Invoice_Number
        {
            get { return zCSVdata[3]; }
        }

        public string Station_Code
        {
            get { return zCSVdata[4]; }
        }

        public string Invoice_Identifier
        {
            get { return zCSVdata[5]; }
        }

        public string Invoice_Type
        {
            get { return zCSVdata[6]; }
        }

        public DateTime Invoice_Date
        {
            get { return SafeDate(zCSVdata[7]); }
        }

        public DateTime Payment_Terms
        {
            get { return SafeDate(zCSVdata[8]); }
        }

        public DateTime Due_Date
        {
            get { return SafeDate(zCSVdata[9]); }
        }

        public string Parent_Account
        {
            get { return zCSVdata[10]; }
        }

        public string Billing_Account
        {
            get { return zCSVdata[11]; }
        }

        public string Billing_Account_Name
        {
            get { return zCSVdata[12]; }
        }

        public string Billing_Account_Name_A
        {
            get { return zCSVdata[13]; }
        }

        public string Billing_Address_1
        {
            get { return zCSVdata[14]; }
        }

        public string Billing_Address_2
        {
            get { return zCSVdata[15]; }
        }

        public string Billing_Address_3
        {
            get { return zCSVdata[16]; }
        }

        public string Billing_Postcode
        {
            get { return zCSVdata[17]; }
        }

        public string Billing_City
        {
            get { return zCSVdata[18]; }
        }

        public string Billing_StateProvince
        {
            get { return zCSVdata[19]; }
        }

        public string Billing_Country_Code
        {
            get { return zCSVdata[20]; }
        }

        public string Billing_Contact
        {
            get { return zCSVdata[21]; }
        }

        public string VAT_Number
        {
            get { return zCSVdata[22]; }
        }

        public string Shipment_Number
        {
            get { return zCSVdata[23]; }
        }

        public DateTime Shipment_Date
        {
            get { return SafeDate(zCSVdata[24]); }
        }

        public string Country_Specific_Label
        {
            get { return zCSVdata[25]; }
        }

        public string Country_Specific_Value
        {
            get { return zCSVdata[26]; }
        }

        public string Shipment_Reference_1
        {
            get { return zCSVdata[27]; }
        }

        public string Shipment_Reference_2
        {
            get { return zCSVdata[28]; }
        }

        public string Shipment_Reference_3
        {
            get { return zCSVdata[29]; }
        }

        public string Product
        {
            get { return SafeString(zCSVdata[30]); }
        }

        public string Product_Name
        {
            get { return SafeString(zCSVdata[31]); }
        }

        public int Pieces
        {
            get { return SafeInt(zCSVdata[32],1); }
        }

        public string Origin
        {
            get { return zCSVdata[33]; }
        }

        public string Orig_Name
        {
            get { return zCSVdata[34]; }
        }

        public string Orig_Country_Code
        {
            get { return zCSVdata[35]; }
        }

        public string Orig_Country_Name
        {
            get { return zCSVdata[36]; }
        }

        public string Senders_Name
        {
            get { return zCSVdata[37]; }
        }

        public string Senders_Address_1
        {
            get { return zCSVdata[38]; }
        }

        public string Senders_Address_2
        {
            get { return zCSVdata[39]; }
        }

        public string Senders_Address_3
        {
            get { return zCSVdata[40]; }
        }

        public string Senders_Postcode
        {
            get { return zCSVdata[41]; }
        }

        public string Senders_City
        {
            get { return zCSVdata[42]; }
        }

        public string Senders_StateProvince
        {
            get { return zCSVdata[43]; }
        }

        public string Senders_Country
        {
            get { return zCSVdata[44]; }
        }

        public string Senders_Contact
        {
            get { return zCSVdata[45]; }
        }

        public string Destination
        {
            get { return XMLSafe(zCSVdata[46]); }
        }

        public string Dest_Name
        {
            get { return XMLSafe(zCSVdata[47]); }
        }

        public string Dest_Country_Code
        {
            get { return zCSVdata[48]; }
        }

        public string Dest_Country_Name
        {
            get { return zCSVdata[49]; }
        }

        public string Receivers_Name
        {
            get { return XMLSafe(zCSVdata[50]); }
        }

        public string Receivers_Address_1
        {
            get { return XMLSafe(zCSVdata[51]); }
        }

        public string Receivers_Address_2
        {
            get { return XMLSafe(zCSVdata[52]); }
        }

        public string Receivers_Address_3
        {
            get { return zCSVdata[53]; }
        }

        public string Receivers_Postcode
        {
            get { return zCSVdata[54]; }
        }

        public string Receivers_City
        {
            get { return XMLSafe(zCSVdata[55]); }
        }

        public string Receivers_StateProvince
        {
            get { return zCSVdata[56]; }
        }

        public string Receivers_Country
        {
            get { return zCSVdata[57]; }
        }

        public string Receivers_Contact
        {
            get { return XMLSafe(zCSVdata[58]); }
        }

        public string Proof_of_DeliveryName
        {
            get { return XMLSafe(zCSVdata[59]); }
        }

        public string Description_of_Contents
        {
            get { return XMLSafe(zCSVdata[60]); }
        }

        public string Event_Description
        {
            get { return zCSVdata[61]; }
        }

        public string Dimensions
        {
            get { return zCSVdata[62]; }
        }

        public decimal Cust_Scale_Weight_A
        {
            get { return SafeDecimal(zCSVdata[63]); }
        }

        public decimal DHL_Scale_Weight_B
        {
            get { return SafeDecimal(zCSVdata[64]); }
        }

        public decimal Cust_Vol_Weight_V
        {
            get { return SafeDecimal(zCSVdata[65]); }
        }

        public decimal DHL_Vol_Weight_W
        {
            get { return SafeDecimal(zCSVdata[66]); }
        }

        public string Weight_Flag
        {
            get { return zCSVdata[67]; }
        }

        public decimal Weight_kg
        {
            get { return SafeDecimal(zCSVdata[68]); }
        }

        public string Currency
        {
            get { return zCSVdata[69]; }
        }

        public decimal Total_amount_excl_VAT
        {
            get { return SafeDecimal(zCSVdata[70]); }
        }

        public decimal Total_amount_incl_VAT
        {
            get { return SafeDecimal(zCSVdata[71]); }
        }

        public string Tax_Code
        {
            get { return zCSVdata[72]; }
        }

        public decimal Total_Tax
        {
            get { return SafeDecimal(zCSVdata[73]); }
        }

        public decimal Tax_Adjustment
        {
            get { return SafeDecimal(zCSVdata[74]); }
        }

        public string Invoice_Fee
        {
            get { return zCSVdata[75]; }
        }

        public decimal Weight_Charge
        {
            get { return SafeDecimal(zCSVdata[76]); }
        }

        public decimal Weight_Tax_VAT
        {
            get { return SafeDecimal(zCSVdata[77]); }
        }

        public string Other_Charges_1
        {
            get { return zCSVdata[78]; }
        }

        public string Other_Charges_1_Amount
        {
            get { return zCSVdata[79]; }
        }

        public string Other_Charges_2
        {
            get { return zCSVdata[80]; }
        }

        public string Other_Charges_2_Amount
        {
            get { return zCSVdata[81]; }
        }

        public string Discount_1
        {
            get { return zCSVdata[82]; }
        }

        public string Discount_1_Amount
        {
            get { return zCSVdata[83]; }
        }

        public string Discount_2
        {
            get { return zCSVdata[84]; }
        }

        public string Discount_2_Amount
        {
            get { return zCSVdata[85]; }
        }

        public string Discount_3
        {
            get { return zCSVdata[86]; }
        }

        public string Discount_3_Amount
        {
            get { return zCSVdata[87]; }
        }

        public string Total_Extra_Charges_XC
        {
            get { return zCSVdata[88]; }
        }

        public string Total_Extra_Charges_Tax
        {
            get { return zCSVdata[89]; }
        }

        public string XC1_Code
        {
            get { return SafeString(zCSVdata[90]); }
        }

        public string XC1_Name
        {
            get { return SafeString(zCSVdata[91]); }
        }

        public decimal XC1_Charge
        {
            get { return SafeDecimal(zCSVdata[92]); }
        }

        public string XC1_Tax_Code
        {
            get { return zCSVdata[93]; }
        }

        public decimal XC1_Tax
        {
            get { return SafeDecimal(zCSVdata[94]); }
        }

        public decimal XC1_Discount
        {
            get { return SafeDecimal(zCSVdata[95]); }
        }

        public decimal XC1_Total
        {
            get { return SafeDecimal(zCSVdata[96]); }
        }

        public string XC2_Code
        {
            get { return SafeString(zCSVdata[97]); }
        }

        public string XC2_Name
        {
            get { return SafeString(zCSVdata[98]); }
        }

        public decimal XC2_Charge
        {
            get { return SafeDecimal(zCSVdata[99]); }
        }

        public string XC2_Tax_Code
        {
            get { return zCSVdata[100]; }
        }

        public decimal XC2_Tax
        {
            get { return SafeDecimal(zCSVdata[101]); }
        }

        public decimal XC2_Discount
        {
            get { return SafeDecimal(zCSVdata[102]); }
        }

        public decimal XC2_Total
        {
            get { return SafeDecimal(zCSVdata[103]); }
        }

        public string XC3_Code
        {
            get { return SafeString(zCSVdata[104]); }
        }

        public string XC3_Name
        {
            get { return SafeString(zCSVdata[105]); }
        }

        public decimal XC3_Charge
        {
            get { return SafeDecimal(zCSVdata[106]); }
        }

        public string XC3_Tax_Code
        {
            get { return zCSVdata[107]; }
        }

        public decimal XC3_Tax
        {
            get { return SafeDecimal(zCSVdata[108]); }
        }

        public decimal XC3_Discount
        {
            get { return SafeDecimal(zCSVdata[109]); }
        }

        public decimal XC3_Total
        {
            get { return SafeDecimal(zCSVdata[110]); }
        }

        public string XC4_Code
        {
            get { return SafeString(zCSVdata[111]); }
        }

        public string XC4_Name
        {
            get { return SafeString(zCSVdata[112]); }
        }

        public decimal XC4_Charge
        {
            get { return SafeDecimal(zCSVdata[113]); }
        }

        public string XC4_Tax_Code
        {
            get { return zCSVdata[114]; }
        }

        public decimal XC4_Tax
        {
            get { return SafeDecimal(zCSVdata[115]); }
        }

        public decimal XC4_Discount
        {
            get { return SafeDecimal(zCSVdata[116]); }
        }

        public decimal XC4_Total
        {
            get { return SafeDecimal(zCSVdata[117]); }
        }

        public string XC5_Code
        {
            get { return SafeString(zCSVdata[118]); }
        }

        public string XC5_Name
        {
            get { return SafeString(zCSVdata[119]); }
        }

        public decimal XC5_Charge
        {
            get { return SafeDecimal(zCSVdata[120]); }
        }

        public string XC5_Tax_Code
        {
            get { return zCSVdata[121]; }
        }

        public decimal XC5_Tax
        {
            get { return SafeDecimal(zCSVdata[122]); }
        }

        public decimal XC5_Discount
        {
            get { return SafeDecimal(zCSVdata[123]); }
        }

        public decimal XC5_Total
        {
            get { return SafeDecimal(zCSVdata[124]); }
        }

        public string XC6_Code
        {
            get { return SafeString(zCSVdata[125]); }
        }

        public string XC6_Name
        {
            get { return SafeString(zCSVdata[126]); }
        }

        public decimal XC6_Charge
        {
            get { return SafeDecimal(zCSVdata[127]); }
        }

        public string XC6_Tax_Code
        {
            get { return zCSVdata[128]; }
        }

        public decimal XC6_Tax
        {
            get { return SafeDecimal(zCSVdata[129]); }
        }

        public decimal XC6_Discount
        {
            get { return SafeDecimal(zCSVdata[130]); }
        }

        public decimal XC6_Total
        {
            get { return SafeDecimal(zCSVdata[131]); }
        }

        public string XC7_Code
        {
            get { return SafeString(zCSVdata[132]); }
        }

        public string XC7_Name
        {
            get { return SafeString(zCSVdata[133]); }
        }

        public decimal XC7_Charge
        {
            get { return SafeDecimal(zCSVdata[134]); }
        }

        public string XC7_Tax_Code
        {
            get { return zCSVdata[135]; }
        }

        public decimal XC7_Tax
        {
            get { return SafeDecimal(zCSVdata[136]); }
        }

        public decimal XC7_Discount
        {
            get { return SafeDecimal(zCSVdata[137]); }
        }

        public decimal XC7_Total
        {
            get { return SafeDecimal(zCSVdata[138]); }
        }

        public string XC8_Code
        {
            get { return SafeString(zCSVdata[139]); }
        }

        public string XC8_Name
        {
            get { return SafeString(zCSVdata[140]); }
        }

        public decimal XC8_Charge
        {
            get { return SafeDecimal(zCSVdata[141]); }
        }

        public string XC8_Tax_Code
        {
            get { return zCSVdata[142]; }
        }

        public decimal XC8_Tax
        {
            get { return SafeDecimal(zCSVdata[143]); }
        }

        public decimal XC8_Discount
        {
            get { return SafeDecimal(zCSVdata[144]); }
        }

        public decimal XC8_Total
        {
            get { return SafeDecimal(zCSVdata[145]); }
        }

        public string XC9_Code
        {
            get { return SafeString(zCSVdata[146]); }
        }

        public string XC9_Name
        {
            get { return SafeString(zCSVdata[147]); }
        }

        public decimal XC9_Charge
        {
            get { return SafeDecimal(zCSVdata[148]); }
        }

        public string XC9_Tax_Code
        {
            get { return zCSVdata[149]; }
        }

        public decimal XC9_Tax
        {
            get { return SafeDecimal(zCSVdata[150]); }
        }

        public decimal XC9_Discount
        {
            get { return SafeDecimal(zCSVdata[151]); }
        }

        public decimal XC9_Total
        {
            get { return SafeDecimal(zCSVdata[152]); }
        }

        public TranslationRecord GTXTranslate { get; set; }


        public List<Service> Services = new List<Service>();

        private decimal zTotalPrice;
        private decimal zTotalWeight;
        private decimal zTotalTax;
        private decimal zTotalFee;
        private string zCurrentLine;



        public WeightFileRecord Convert()
        {

            if (GTXTranslate != null)
            {
                var awb = Shipment_Number;
                if (GTXTranslate.KeyType == "GEBYR")
                {
                    awb = "#" + awb;
                }
                var wf = new WeightFileRecord
                {

                    AWB = awb,
                    BillWeight = Weight_kg,
                    Price = Weight_Charge - Weight_Tax_VAT,
                    CreditorAccount = Invoice_Number,
                    SalesProduct = GTXTranslate.GTXProduct,
                    TransportProduct = GTXTranslate.GTXTransp,
                    Services = Services



                };

                return wf;
            }

            return null;

        }

        public int SafeInt(string d, int def)
        {
            int a = def;
            if (int.TryParse(d, out a))
            {
                return a;
            }
            return def;
        }

        public InvoiceShipment StdConvert()
        {

            if (GTXTranslate != null)
            {
                var awb = Shipment_Number;
                if (GTXTranslate.KeyType == "FRAGT")
                {

                    var wf = new InvoiceShipment
                    {

                        Status = 1,
                        Invoice = Invoice_Number,
                        InvoiceDate = Invoice_Date,
                        VendorAccount = Billing_Account,

                        AWB = Shipment_Number,
                        Product = GTXTranslate.GTXProduct,
                        Transport = (byte)GTXTranslate.GTXTransp,
                        Shipdate = Shipment_Date,
                        CustomerNumber = Billing_Account,
                        CompanyName = Senders_Name,
                        Address1 = Senders_Address_1,
                        Address2 = Senders_Address_2,
                        City = Senders_City,
                        State = Senders_StateProvince,
                        Zip = Senders_Postcode,
                        Country_Iata = Senders_Country,
                        Reciever_CompanyName = Receivers_Name,
                        Reciever_Address1 = Receivers_Address_1,
                        Reciever_Address2 = Receivers_Address_2,
                        Reciever_City = Receivers_City,
                        Reciever_State = Receivers_StateProvince,
                        Reciever_Zip = Receivers_Postcode,
                        Reciever_Country = Receivers_Country,
                        Reciever_Country_Iata = Receivers_Country,
                        Reciever_Phone ="00" ,
                        Reciever_Fax = "00",
                        Reciever_Email = "upload@gtx.nu",
                        Reciever_Reference = Shipment_Reference_1,
                        NumberofCollies = (byte) Pieces,
                        Reference = Shipment_Reference_2,
                        Total_Weight = Weight_kg,
                        Length =null ,
                        Width = null,
                        Height = null,
                        Vol_Weight = DHL_Vol_Weight_W,
                        BilledWeight = Weight_kg,
                        Customevalue = null,
                        PackValue = null,
                        PackValuta = null,
                        Description = Description_of_Contents,
                        Costprice = Total_amount_excl_VAT,
                        Saleprice = null,
                        Oli = null




                    };

                    return wf;
                }
            }

            return null;

        }



        public List<string> ProductTranslate(string line)
        {

            var lst = new List<string>();
            zCSVdata = line.Replace("\",\"", "|").Split('|');
            zCSVdata = zCSVdata.Select(x => x.Trim('"')).ToArray();
            if (!string.IsNullOrEmpty(Product) && TranslateName(Product) == "")
            {
                lst.Add(Product + ";" + Product_Name);
            }
            if (!string.IsNullOrEmpty(XC1_Code) && TranslateName(XC1_Code) == "")
            {
                lst.Add(XC1_Code + ";" + XC1_Name);
            }
            if (!string.IsNullOrEmpty(XC2_Code) && TranslateName(XC2_Code) == "")
            {
                lst.Add(XC2_Code + ";" + XC2_Name);
            }
            if (!string.IsNullOrEmpty(XC3_Code) && TranslateName(XC3_Code) == "")
            {
                lst.Add(XC3_Code + ";" + XC3_Name);
            }
            if (!string.IsNullOrEmpty(XC4_Code) && TranslateName(XC4_Code) == "")
            {
                lst.Add(XC4_Code + ";" + XC4_Name);
            }
            if (!string.IsNullOrEmpty(XC5_Code) && TranslateName(XC5_Code) == "")
            {
                lst.Add(XC5_Code + ";" + XC5_Name);
            }
            if (!string.IsNullOrEmpty(XC6_Code) && TranslateName(XC6_Code) == "")
            {
                lst.Add(XC6_Code + ";" + XC6_Name);
            }
            if (!string.IsNullOrEmpty(XC7_Code) && TranslateName(XC7_Code) == "")
            {
                lst.Add(XC7_Code + ";" + XC7_Name);
            }
            if (!string.IsNullOrEmpty(XC8_Code) && TranslateName(XC8_Code) == "")
            {
                lst.Add(XC8_Code + ";" + XC8_Name);
            }
            if (!string.IsNullOrEmpty(XC9_Code) && TranslateName(XC9_Code) == "")
            {
                lst.Add(XC9_Code + ";" + XC9_Name);
            }
            return lst;


        }

        public string XMLSafe(string data)
        {

            return data.Replace("&", "").Replace("<", "").Replace(">", "");


        }
        public DHLRecord(string line, StringBuilder sb, Translation translation)
        {
            zCurrentLine = line;
            zReasonError = sb;
            zCSVdata = line.Replace("\",\"", "|").Split('|');
            zCSVdata = zCSVdata.Select(x => x.Trim('"')).ToArray();
            zTranslation = translation;


            if (Line_Type != "I")
            {

                var key = TranslateAccount(Billing_Account) + Product + "_" + Direction(Senders_Country, Receivers_Country);
                GTXTranslate = TranslateObj(key, "FRAGT");

                InvoiceName = Invoice_Number;
                MakeAddtionelAll();


            }



        }

        private string Direction(string fromCode, string toCode)
        {
            if (fromCode == "DK")
            {
                if (toCode == "DK")
                {
                    return "DOM";
                }
                return "EXP";

            }
            else
            {
                if (toCode == "DK")
                {
                    return "IMP";
                }
                return "TREE";
            }
        }


        private string MakeAddtionelAll()
        {
            if (Tax_Code == "A")
            {
                zTaxCharge = zTaxCharge + Total_amount_excl_VAT;
            }
            if (Tax_Code == "B")
            {
                zNonTaxCharge = zNonTaxCharge + Total_amount_excl_VAT;
            }
            var sb = new StringBuilder();

            MakeAddtionel(XC1_Code, XC1_Name, XC1_Charge, XC1_Tax_Code, XC1_Tax, XC1_Discount, XC1_Total);
            MakeAddtionel(XC2_Code, XC2_Name, XC2_Charge, XC2_Tax_Code, XC2_Tax, XC2_Discount, XC2_Total);
            MakeAddtionel(XC3_Code, XC3_Name, XC3_Charge, XC3_Tax_Code, XC3_Tax, XC3_Discount, XC3_Total);
            MakeAddtionel(XC4_Code, XC4_Name, XC4_Charge, XC4_Tax_Code, XC4_Tax, XC4_Discount, XC4_Total);
            MakeAddtionel(XC5_Code, XC5_Name, XC5_Charge, XC5_Tax_Code, XC5_Tax, XC5_Discount, XC5_Total);
            MakeAddtionel(XC6_Code, XC6_Name, XC6_Charge, XC6_Tax_Code, XC6_Tax, XC6_Discount, XC6_Total);
            MakeAddtionel(XC7_Code, XC7_Name, XC7_Charge, XC7_Tax_Code, XC7_Tax, XC7_Discount, XC7_Total);
            MakeAddtionel(XC8_Code, XC8_Name, XC8_Charge, XC8_Tax_Code, XC8_Tax, XC8_Discount, XC8_Total);
            MakeAddtionel(XC9_Code, XC9_Name, XC9_Charge, XC9_Tax_Code, XC9_Tax, XC9_Discount, XC9_Total);

            return sb.ToString();

        }

        private TranslationRecord TranslateObj(string name, string type)
        {

            if (zTranslation.TranDictionary.ContainsKey(name))
            {
                var trans = zTranslation.TranDictionary[name];
                if (trans.KeyType == "" || trans.GTXName == "")
                {
                    TranslationError = true;
                    return null;

                }
                return trans;
            }
            zTranslation.AddMissing(name, type);
            TranslationError = true;


            return null;
        }

        private string TranslateAccount(string name)
        {

            if (zTranslation.TranDictionary.ContainsKey(name))

            {
                return zTranslation.TranDictionary[name].GTXName;
            }

            return "";
        }

        private string TranslateName(string name)
        {

            if (zTranslation.TranDictionary.ContainsKey(name))
            {
                return zTranslation.TranDictionary[name].GTXName;
            }
            zTranslation.AddMissing(name, "FRAGT");
            TranslationError = true;
            return "";
        }


        public void LastLine(string xmlShipments)
        {




        }


        private void MakeAddtionel(string code, string name, decimal charge, string taxCode, decimal tax, decimal discount, decimal total)
        {

            if (code != "")
            {
                var trans = TranslateObj(code, "GEBYR");
                if (trans != null && trans.KeyType == "GEBYR")
                {
                    var s = new Service
                    {
                        GTXCode = trans.GTXName,
                        Price = charge


                    };
                    Services.Add(s);



                }




            }


        }

        private DateTime SafeDate(string data)
        {


            DateTime dd;
            if (DateTime.TryParse(data.Substring(0, 4) + "-" + data.Substring(4, 2) + "-" +
                                  data.Substring(6, 2), out dd))
            {
                return dd;
            }
            zReasonError.AppendLine("DateTimeFormat error line " + zCurrentLine);
            FormatError = true;
            return new DateTime();

        }
        private string SafeString(string data)
        {

            if (data == "" || data == "0")
            {
                return "";
            }


            return data;
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

            zReasonError.AppendLine("DecimalFormat error line " + zCurrentLine);
            FormatError = true;
            return 0;
        }



    }

}
