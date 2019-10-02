using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using UploadDHL.DataUploadWeb;


namespace UploadDHL
{
    class DHLRecord:DataRecord
    {

        private StringBuilder zXmlOut;
       
        private DHLXML zDhlXml;
        private int zCount;
        private ErrorHandler zErrorhandler ;
        public bool Error { get; set; }
        public bool TranslationError { get; set; }
        public bool FormatError { get; set; }
       




       

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
            get { return SafeDate(zCSVdata[7], "Invoice_Date"); }
        }

        public DateTime Payment_Terms
        {
            get { return SafeDate(zCSVdata[8], "Payment_Terms"); }
        }

        public DateTime Due_Date
        {
            get { return SafeDate(zCSVdata[9], "Due_Date"); }
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
            get { return SafeDate(zCSVdata[24], "Shipment_Date"); }
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
            get { return SafeDecimal(zCSVdata[63], "Cust_Scale_Weight_A"); }
        }

        public decimal DHL_Scale_Weight_B
        {
            get { return SafeDecimal(zCSVdata[64], "DHL_Scale_Weight_B"); }
        }

        public decimal Cust_Vol_Weight_V
        {
            get { return SafeDecimal(zCSVdata[65], "Cust_Vol_Weight_V"); }
        }

        public decimal DHL_Vol_Weight_W
        {
            get { return SafeDecimal(zCSVdata[66], "DHL_Vol_Weight_W"); }
        }

        public string Weight_Flag
        {
            get { return zCSVdata[67]; }
        }

        public decimal Weight_kg
        {
            get { return SafeDecimal(zCSVdata[68], "Weight_kg"); }
        }

        public string Currency
        {
            get { return zCSVdata[69]; }
        }

        public decimal Total_amount_excl_VAT
        {
            get { return SafeDecimal(zCSVdata[70], "Total_amount_excl_VAT"); }
        }

        public decimal Total_amount_incl_VAT
        {
            get { return SafeDecimal(zCSVdata[71], "Total_amount_incl_VAT"); }
        }

        public string Tax_Code
        {
            get { return zCSVdata[72]; }
        }

        public decimal Total_Tax
        {
            get { return SafeDecimal(zCSVdata[73], "Total_Tax"); }
        }

        public decimal Tax_Adjustment
        {
            get { return SafeDecimal(zCSVdata[74], "Tax_Adjustment"); }
        }

        public string Invoice_Fee
        {
            get { return zCSVdata[75]; }
        }

        public decimal Weight_Charge
        {
            get { return SafeDecimal(zCSVdata[76], "Weight_Charge"); }
        }

        public decimal Weight_Tax_VAT
        {
            get { return SafeDecimal(zCSVdata[77], "Weight_Tax_VAT"); }
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
            get { return SafeDecimal(zCSVdata[92], "XC1_Charge"); }
        }

        public string XC1_Tax_Code
        {
            get { return zCSVdata[93]; }
        }

        public decimal XC1_Tax
        {
            get { return SafeDecimal(zCSVdata[94], "XC1_Tax"); }
        }

        public decimal XC1_Discount
        {
            get { return SafeDecimal(zCSVdata[95], "XC1_Discount"); }
        }

        public decimal XC1_Total
        {
            get { return SafeDecimal(zCSVdata[96], "XC1_Total"); }
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
            get { return SafeDecimal(zCSVdata[99], "XC2_Charge"); }
        }

        public string XC2_Tax_Code
        {
            get { return zCSVdata[100]; }
        }

        public decimal XC2_Tax
        {
            get { return SafeDecimal(zCSVdata[101], "XC2_Tax"); }
        }

        public decimal XC2_Discount
        {
            get { return SafeDecimal(zCSVdata[102], "XC2_Discount"); }
        }

        public decimal XC2_Total
        {
            get { return SafeDecimal(zCSVdata[103], "XC2_Total"); }
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
            get { return SafeDecimal(zCSVdata[106], "XC3_Charge"); }
        }

        public string XC3_Tax_Code
        {
            get { return zCSVdata[107]; }
        }

        public decimal XC3_Tax
        {
            get { return SafeDecimal(zCSVdata[108], "XC3_Tax"); }
        }

        public decimal XC3_Discount
        {
            get { return SafeDecimal(zCSVdata[109], "XC3_Discount"); }
        }

        public decimal XC3_Total
        {
            get { return SafeDecimal(zCSVdata[110], "XC3_Total"); }
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
            get { return SafeDecimal(zCSVdata[113], "XC4_Charge"); }
        }

        public string XC4_Tax_Code
        {
            get { return zCSVdata[114]; }
        }

        public decimal XC4_Tax
        {
            get { return SafeDecimal(zCSVdata[115], "XC4_Tax"); }
        }

        public decimal XC4_Discount
        {
            get { return SafeDecimal(zCSVdata[116], "XC4_Discount"); }
        }

        public decimal XC4_Total
        {
            get { return SafeDecimal(zCSVdata[117], "XC4_Total"); }
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
            get { return SafeDecimal(zCSVdata[120], "XC5_Charge"); }
        }

        public string XC5_Tax_Code
        {
            get { return zCSVdata[121]; }
        }

        public decimal XC5_Tax
        {
            get { return SafeDecimal(zCSVdata[122], "XC5_Tax"); }
        }

        public decimal XC5_Discount
        {
            get { return SafeDecimal(zCSVdata[123], "XC5_Discount"); }
        }

        public decimal XC5_Total
        {
            get { return SafeDecimal(zCSVdata[124], "XC5_Total"); }
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
            get { return SafeDecimal(zCSVdata[127], "XC6_Charge"); }
        }

        public string XC6_Tax_Code
        {
            get { return zCSVdata[128]; }
        }

        public decimal XC6_Tax
        {
            get { return SafeDecimal(zCSVdata[129], "XC6_Tax"); }
        }

        public decimal XC6_Discount
        {
            get { return SafeDecimal(zCSVdata[130], "XC6_Discount"); }
        }

        public decimal XC6_Total
        {
            get { return SafeDecimal(zCSVdata[131], "XC6_Total"); }
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
            get { return SafeDecimal(zCSVdata[134], "XC7_Charge"); }
        }

        public string XC7_Tax_Code
        {
            get { return zCSVdata[135]; }
        }

        public decimal XC7_Tax
        {
            get { return SafeDecimal(zCSVdata[136], "XC7_Tax"); }
        }

        public decimal XC7_Discount
        {
            get { return SafeDecimal(zCSVdata[137], "XC7_Discount"); }
        }

        public decimal XC7_Total
        {
            get { return SafeDecimal(zCSVdata[138], "XC7_Total"); }
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
            get { return SafeDecimal(zCSVdata[141], "XC8_Charge"); }
        }

        public string XC8_Tax_Code
        {
            get { return zCSVdata[142]; }
        }

        public decimal XC8_Tax
        {
            get { return SafeDecimal(zCSVdata[143], "XC8_Tax"); }
        }

        public decimal XC8_Discount
        {
            get { return SafeDecimal(zCSVdata[144],"XC8_Discount"); }
        }

        public decimal XC8_Total
        {
            get { return SafeDecimal(zCSVdata[145], "XC8_Total"); }
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
            get { return SafeDecimal(zCSVdata[148], "XC9_Charge"); }
        }

        public string XC9_Tax_Code
        {
            get { return zCSVdata[149]; }
        }

        public decimal XC9_Tax
        {
            get { return SafeDecimal(zCSVdata[150], "XC9_Tax"); }
        }

        public decimal XC9_Discount
        {
            get { return SafeDecimal(zCSVdata[151], "XC9_Discount"); }
        }

        public decimal XC9_Total
        {
            get { return SafeDecimal(zCSVdata[152], "XC9_Total"); }
        }

       

        private decimal zTotalPrice;
        private decimal zTotalWeight;
        private decimal zTotalTax;
        private decimal zTotalFee;
    



      
       
        
        public XMLRecord MakeXmlRecord()
        {
            return new XMLRecord
            {
                Awb = this.Awb,
                InvoiceNumber = this.Invoice_Number,
                InvoiceLineNo = InvLineNumber,
                Price = this.Weight_Charge,
                Vat = this.Total_amount_incl_VAT-this.Total_amount_excl_VAT,
               
                GTXName = this.GTXTranslate.GTXName,
                KeyType = this.GTXTranslate.KeyType,
            
                Services = this.Services,
                InvoiceDate = Invoice_Date,
                Due_Date = Due_Date,
                VendorAccount = Billing_Account,
                CarrierService = GTXTranslate.Key,
                
                Product = GTXTranslate.GTXProduct,
                Transport = (byte)GTXTranslate.GTXTransp,
                Shipdate = Shipment_Date,
      
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
                Reciever_Phone = "00",
                Reciever_Fax = "00",
                Reciever_Email = "upload@gtx.nu",
                Reciever_Reference = Shipment_Reference_1,
                NumberofCollies = (byte)Pieces,
                Reference = Shipment_Reference_2,
                Total_Weight = Weight_kg,
                Length = 0,
                Width = 0,
                Height = 0,
                Vol_Weight = DHL_Vol_Weight_W,
                BilledWeight = Weight_kg,
                Customevalue = 0,
                PackValue = 0,
                PackValuta = "",
                Description = Description_of_Contents,
                Costprice = Total_amount_excl_VAT,
              


            };
        }


       

        public string XMLSafe(string data)
        {

            return data.Replace("&", "").Replace("<", "").Replace(">", "");


        }
        public DHLRecord(string data,  Translation translation, int lineno)
        {

            InvLineNumber = lineno;
         
            zCSVdata = data.Replace("\",\"", "|").Split('|');
            zCSVdata = zCSVdata.Select(x => x.Trim('"')).ToArray();
            TranslationHandler = translation;
            Awb = Shipment_Number;

            if (Line_Type != "I")
            {

                var key = TranslateAccount(Billing_Account) + Product + "_" +
                          Direction(Senders_Country, Receivers_Country);
                GTXTranslate = TranslationHandler.DoTranslate(key, VendorHandler.FRAGT);
                RecordStatus = GTXTranslate.KeyType;


                MakeAddtionelAll();


            }
            else
            {

                
                GTXTranslate = new TranslationRecord
                {
                    GTXName = VendorHandler.SUMHED,
                    Key = "HEAD"

                };

                RecordStatus = VendorHandler.HEAD;

                
               

            }
          
            XmlRecord = MakeXmlRecord();
            if (XmlRecord == null)
            {
                RecordStatus = VendorHandler.E_ERROR;
                ErrorHelper.Add("Conversion->GTX record");
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

       

        private string TranslateAccount(string name)
        {

            if (TranslationHandler.TranDictionary.ContainsKey(name))

            {
                return TranslationHandler.TranDictionary[name].GTXName;
            }

            return "";
        }


       

        private void MakeAddtionel(string code, string name, decimal charge, string taxCode, decimal tax, decimal discount, decimal total)
        {

            if (code != "" && !GTXTranslate.KeyType.StartsWith("E_"))
            {
                var trans = TranslationHandler.DoTranslate(code, VendorHandler.GEBYR);
                if (trans.KeyType == VendorHandler.GEBYR)
                {
                    var s = new Service
                    {
                        GTXCode = trans.GTXName,
                        Price = charge,
                        Tax = tax,
                        InvoiceLineNumber = InvLineNumber


                    };
                    Services.Add(s);



                }
                else
                {
                    if (trans.KeyType.StartsWith("E_"))
                    {
                        GTXTranslate.KeyType = trans.KeyType;
                        
                    }
                    
                }


            }


        }

      


    }

}
