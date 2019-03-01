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
    class FedexRecord
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

        public TranslationRecord GTXTranslate { get; set; }


        public List<Service> Services = new List<Service>();

        private decimal zTotalPrice;
        private decimal zTotalWeight;
        private decimal zTotalTax;
        private decimal zTotalFee;
        private string zCurrentLine;

        public string Master_EDI_No
        {
            get { return SafeString(zCSVdata[0]); }
        }
        public string Invoice_Number
        {
            get { return SafeString(zCSVdata[1]); }
        }
        public DateTime Invoice_Date
        {
            get { return SafeDate(zCSVdata[2]); }
        }
        public string Type
        {
            get { return SafeString(zCSVdata[3]); }
        }
        public string Settle
        {
            get { return SafeString(zCSVdata[4]); }
        }
        public decimal Inv_Charge
        {
            get { return SafeDecimal(zCSVdata[5]); }
        }
        public int Trans_Cnt
        {
            get { return SafeInt(zCSVdata[6]); }
        }
        public string Bill_To_Account
        {
            get
            {
                return SafeString(zCSVdata[7]);
            }
        }
        public string Cntry
        {
            get { return SafeString(zCSVdata[8]); }
        }
        public decimal Total_VAT_Amt
        {
            get { return SafeDecimal(zCSVdata[9]); }
        }
        public decimal VAT_Billed_Curr
        {
            get { return SafeDecimal(zCSVdata[10]); }
        }
        public decimal Total_Tax_Amt
        {
            get { return SafeDecimal(zCSVdata[11]); }
        }
        public string Tax_Billed_Curr
        {
            get { return SafeString(zCSVdata[12]); }
        }
        public decimal Total_Non_Tax_Amt
        {
            get { return SafeDecimal(zCSVdata[13]); }
        }
        public string Non_Tax_Billed_Curr
        {
            get { return SafeString(zCSVdata[14]); }
        }
        public string Consolidated_Acct
        {
            get { return SafeString(zCSVdata[15]); }
        }
        public string Co_Cd
        {
            get { return SafeString(zCSVdata[16]); }
        }
        public string Ground_Tracking_Number
        {
            get { return SafeString(zCSVdata[17]); }
        }
        public string Tracking_Number
        {
            get { return SafeString(zCSVdata[18]); }
        }
        public string Rebill
        {
            get { return SafeString(zCSVdata[19]); }
        }
        public string Non_Dup
        {
            get
            {
                return SafeString(zCSVdata[20]);
            }
        }
        public DateTime Ship_Date
        {
            get { return SafeDate(zCSVdata[21]); }
        }
        public string Svc
        {
            get { return SafeString(zCSVdata[22]); }
        }
        public int Pkg
        {
            get { return SafeInt(zCSVdata[23]); }
        }
        public string Grd_Svc
        {
            get { return SafeString(zCSVdata[24]); }
        }
        public string IPD_Adr
        {
            get { return SafeString(zCSVdata[25]); }
        }
        public string Msg_Cd_1
        {
            get { return SafeString(zCSVdata[26]); }
        }
        public string Ref_1
        {
            get { return SafeString(zCSVdata[27]); }
        }
        public string Ref_2
        {
            get { return SafeString(zCSVdata[28]); }
        }
        public string Ref_3
        {
            get { return SafeString(zCSVdata[29]); }
        }
        public string Store_No
        {
            get { return SafeString(zCSVdata[30]); }
        }
        public string Cust_PO_No
        {
            get { return SafeString(zCSVdata[31]); }
        }
        public string Cust_Dept_No
        {
            get { return SafeString(zCSVdata[32]); }
        }
        public string Cust_Inv_No
        {
            get { return SafeString(zCSVdata[33]); }
        }
        public string RMA_No
        {
            get { return SafeString(zCSVdata[34]); }
        }
        public string Device_No
        {
            get { return SafeString(zCSVdata[35]); }
        }
        public string Device
        {
            get { return SafeString(zCSVdata[36]); }
        }
        public string Payor
        {
            get { return SafeString(zCSVdata[37]); }
        }
        public decimal Net_Chrg
        {
            get { return SafeDecimal(zCSVdata[38]); }
        }
        public string Curr
        {
            get { return SafeString(zCSVdata[39]); }
        }
        public string Chrg_1
        {
            get { return SafeString(zCSVdata[40]); }
        }
        public decimal Freight_Amt
        {
            get { return SafeDecimal(zCSVdata[41]); }
        }
        public string Chrg_2
        {
            get { return SafeString(zCSVdata[42]); }
        }
        public decimal Vol_Disc
        {
            get { return SafeDecimal(zCSVdata[43]); }
        }
        public string Chrg_3
        {
            get { return SafeString(zCSVdata[44]); }
        }
        public decimal Earned_Disc
        {
            get { return SafeDecimal(zCSVdata[45]); }
        }
        public string Chrg_4
        {
            get { return SafeString(zCSVdata[46]); }
        }
        public decimal Auto_Disc
        {
            get { return SafeDecimal(zCSVdata[47]); }
        }
        public string Chrg_5
        {
            get { return SafeString(zCSVdata[48]); }
        }
        public decimal Perf_Price_Disc
        {
            get { return SafeDecimal(zCSVdata[49]); }
        }
        public string Chrg_6
        {
            get { return SafeString(zCSVdata[50]); }
        }
        public decimal Fuel_Amt
        {
            get { return SafeDecimal(zCSVdata[51]); }
        }
        public string Chrg_7
        {
            get { return SafeString(zCSVdata[52]); }
        }
        public decimal Resi_Amt
        {
            get { return SafeDecimal(zCSVdata[53]); }
        }
        public string Chrg_8
        {
            get { return SafeString(zCSVdata[54]); }
        }
        public decimal DAS_Amt
        {
            get { return SafeDecimal(zCSVdata[55]); }
        }
        public string Chrg_9
        {
            get { return SafeString(zCSVdata[56]); }
        }
        public decimal On_Call_Amt
        {
            get
            {
                return SafeDecimal(zCSVdata[57]);
            }
        }
        public string Chrg_10
        {
            get { return SafeString(zCSVdata[58]); }
        }
        public decimal DV_Amt
        {
            get { return SafeDecimal(zCSVdata[59]); }
        }
        public string Chrg_11
        {
            get { return SafeString(zCSVdata[60]); }
        }
        public decimal Sign_Svc_Amt
        {
            get { return SafeDecimal(zCSVdata[61]); }
        }
        public string Chrg_12
        {
            get { return SafeString(zCSVdata[62]); }
        }
        public decimal Sat_Amt
        {
            get { return SafeDecimal(zCSVdata[63]); }
        }
        public string Chrg_13
        {
            get { return SafeString(zCSVdata[64]); }
        }
        public decimal Addn_Hndlg_Amt
        {
            get { return SafeDecimal(zCSVdata[65]); }
        }
        public string Chrg_14
        {
            get { return SafeString(zCSVdata[66]); }
        }
        public decimal Adr_Corr_Amt
        {
            get { return SafeDecimal(zCSVdata[67]); }
        }
        public string Chrg_15
        {
            get { return SafeString(zCSVdata[68]); }
        }
        public decimal GST_Amt
        {
            get { return SafeDecimal(zCSVdata[69]); }
        }
        public string Chrg_16
        {
            get { return SafeString(zCSVdata[70]); }
        }
        public decimal Duty_Amt
        {
            get { return SafeDecimal(zCSVdata[71]); }
        }
        public string Chrg_17
        {
            get { return SafeString(zCSVdata[72]); }
        }
        public decimal Adv_Fee_Amt
        {
            get { return SafeDecimal(zCSVdata[73]); }
        }
        public string Chrg_18
        {
            get { return SafeString(zCSVdata[74]); }
        }
        public decimal Orig_VAT_Amt
        {
            get { return SafeDecimal(zCSVdata[75]); }
        }
        public string Chrg_19
        {
            get { return SafeString(zCSVdata[76]); }
        }
        public decimal Misc_1_Amt
        {
            get { return SafeDecimal(zCSVdata[77]); }
        }
        public string Chrg_20
        {
            get { return SafeString(zCSVdata[78]); }
        }
        public decimal Misc_2_Amt
        {
            get { return SafeDecimal(zCSVdata[79]); }
        }
        public string Chrg_21
        {
            get { return SafeString(zCSVdata[80]); }
        }
        public decimal Misc_3_Amt
        {
            get { return SafeDecimal(zCSVdata[81]); }
        }
        public string Exchg_Rate
        {
            get { return SafeString(zCSVdata[82]); }
        }
        public string Exc_Curr
        {
            get { return SafeString(zCSVdata[83]); }
        }
        public string Fuel_Pct
        {
            get { return SafeString(zCSVdata[84]); }
        }
        public string EU_Bd
        {
            get { return SafeString(zCSVdata[85]); }
        }
        public string Count
        {
            get { return SafeString(zCSVdata[86]); }
        }
        public string Call_Tag
        {
            get { return SafeString(zCSVdata[87]); }
        }
        public string Dec_Value
        {
            get { return SafeString(zCSVdata[88]); }
        }
        public string Customs_Value
        {
            get { return SafeString(zCSVdata[89]); }
        }
        public string DV_Cus_Curr
        {
            get
            {
                return SafeString(zCSVdata[90]);
            }
        }
        public string Entry_Number
        {
            get { return SafeString(zCSVdata[91]); }
        }
        public string MTWT_No
        {
            get { return SafeString(zCSVdata[92]); }
        }
        public string Scale
        {
            get { return SafeString(zCSVdata[93]); }
        }
        public int Pcs
        {
            get { return SafeInt(zCSVdata[94]); }
        }
        public decimal Bill_Wt
        {
            get { return SafeDecimal(zCSVdata[95]); }
        }
        public string Orig_Wt
        {
            get { return SafeString(zCSVdata[96]); }
        }
        public string Multi_Wt
        {
            get { return SafeString(zCSVdata[97]); }
        }
        public string Wt_Unit
        {
            get { return SafeString(zCSVdata[98]); }
        }
        public Decimal Length
        {
            get { return SafeDecimal(zCSVdata[99]); }
        }
        public Decimal Width
        {
            get { return SafeDecimal(zCSVdata[100]); }
        }
        public Decimal Height
        {
            get { return SafeDecimal(zCSVdata[101]); }
        }
        public string Dim_Unit
        {
            get { return SafeString(zCSVdata[102]); }
        }
        public string Divisor
        {
            get { return SafeString(zCSVdata[103]); }
        }
        public string Misc_1
        {
            get { return SafeString(zCSVdata[104]); }
        }
        public string Misc_2
        {
            get { return SafeString(zCSVdata[105]); }
        }
        public string Misc_3
        {
            get { return SafeString(zCSVdata[106]); }
        }
        public string Shipper_Name
        {
            get { return SafeString(zCSVdata[107]); }
        }
        public string Shipper_Company
        {
            get { return SafeString(zCSVdata[108]); }
        }
        public string Shipper_Dept
        {
            get { return SafeString(zCSVdata[109]); }
        }
        public string Shipper_Address_1
        {
            get { return SafeString(zCSVdata[110]); }
        }
        public string Shipper_Address_2
        {
            get { return SafeString(zCSVdata[111]); }
        }
        public string Shipper_City
        {
            get { return SafeString(zCSVdata[112]); }
        }
        public string ST
        {
            get { return SafeString(zCSVdata[113]); }
        }
        public string Postal
        {
            get { return SafeString(zCSVdata[114]); }
        }
        public string Origin_Zip
        {
            get { return SafeString(zCSVdata[115]); }
        }
        public string Cntry1
        {
            get { return SafeString(zCSVdata[116]); }
        }
        public string Region
        {
            get { return SafeString(zCSVdata[117]); }
        }
        public string Recipient_Name
        {
            get { return SafeString(zCSVdata[118]); }
        }
        public string Recipient_Company
        {
            get { return SafeString(zCSVdata[119]); }
        }
        public string Recipient_Address_1
        {
            get { return SafeString(zCSVdata[120]); }
        }
        public string Recipient_Address_2
        {
            get { return SafeString(zCSVdata[121]); }
        }
        public string Recipient_City
        {
            get { return SafeString(zCSVdata[122]); }
        }
        public string ST2
        {
            get { return SafeString(zCSVdata[123]); }
        }
        public string Postal2
        {
            get { return SafeString(zCSVdata[124]); }
        }
        public string Cntry2
        {
            get { return SafeString(zCSVdata[125]); }
        }
        public string Handling
        {
            get { return SafeString(zCSVdata[126]); }
        }
        public string Delivery
        {
            get { return SafeString(zCSVdata[127]); }
        }
        public string Time
        {
            get { return SafeString(zCSVdata[128]); }
        }
        public string Final
        {
            get { return SafeString(zCSVdata[129]); }
        }
        public string Exceptn
        {
            get { return SafeString(zCSVdata[130]); }
        }
        public string Attempt_Date
        {
            get { return SafeString(zCSVdata[131]); }
        }
        public string Attempt_Time
        {
            get { return SafeString(zCSVdata[132]); }
        }
        public string Signature
        {
            get { return SafeString(zCSVdata[133]); }
        }
        public string Svc_Area
        {
            get { return SafeString(zCSVdata[134]); }
        }
        public string COD_Amt
        {
            get { return SafeString(zCSVdata[135]); }
        }
        public string COD_Trkg_No
        {
            get { return SafeString(zCSVdata[136]); }
        }
        public string PDue
        {
            get { return SafeString(zCSVdata[137]); }
        }
        public string PDue_Inv
        {
            get { return SafeString(zCSVdata[138]); }
        }
        public string Svc_Pct
        {
            get { return SafeString(zCSVdata[139]); }
        }
        public string Rev_Threshold_Amt
        {
            get { return SafeString(zCSVdata[140]); }
        }
        public string Orig_Recip_Adr_1
        {
            get { return SafeString(zCSVdata[141]); }
        }
        public string Orig_Recip_Adr_2
        {
            get { return SafeString(zCSVdata[142]); }
        }
        public string Original_City
        {
            get { return SafeString(zCSVdata[143]); }
        }
        public string ST3
        {
            get { return SafeString(zCSVdata[144]); }
        }
        public string Postal3
        {
            get { return SafeString(zCSVdata[145]); }
        }
        public string VAT_No
        {
            get { return SafeString(zCSVdata[146]); }
        }
        public string FedEx_VAT_No
        {
            get { return SafeString(zCSVdata[147]); }
        }
        public string Cross_Ref_No
        {
            get { return SafeString(zCSVdata[148]); }
        }
        public string Intl_Ground_Ship_No
        {
            get { return SafeString(zCSVdata[149]); }
        }



        public WeightFileRecord Convert()
        {

            if (GTXTranslate != null)
            {
                var awb = Tracking_Number;
                if (GTXTranslate.KeyType == "GEBYR")
                {
                    awb = "#" + awb;
                }
                var wf = new WeightFileRecord
                {

                    AWB = awb,
                    BillWeight = Bill_Wt,
                    Price = Freight_Amt,
                    CreditorAccount = Invoice_Number,
                    SalesProduct = GTXTranslate.GTXProduct,
                    TransportProduct = GTXTranslate.GTXTransp,
                    Services = Services



                };

                return wf;
            }

            return null;

        }

        public InvoiceShipment StdConvert()
        {

            if (GTXTranslate != null)
            {
                var awb = Tracking_Number;
                if (GTXTranslate.KeyType == "FRAGT")
                {

                    var wf = new InvoiceShipment
                    {

                        Status = 1,
                        Invoice = Invoice_Number,
                        InvoiceDate = Invoice_Date,
                        VendorAccount = Bill_To_Account,

                        AWB = awb,
                        Product = GTXTranslate.GTXProduct,
                        Transport = (byte)GTXTranslate.GTXTransp,
                        Shipdate = Ship_Date,
                        CustomerNumber = Bill_To_Account,
                        CompanyName = Shipper_Name,
                        Address1 = Shipper_Address_1,
                        Address2 = Shipper_Address_2,
                        City = Shipper_City,
                        State = "",
                        Zip = Postal,
                        Country_Iata = Cntry1,
                        Reciever_CompanyName = Recipient_Company,
                        Reciever_Address1 = Recipient_Address_1,
                        Reciever_Address2 = Recipient_Address_2,
                        Reciever_City = Recipient_City,
                        Reciever_State = "",
                        Reciever_Zip = Postal2,
                        Reciever_Country = Cntry2,
                        Reciever_Country_Iata = Cntry2,
                        Reciever_Phone = "00",
                        Reciever_Fax = "00",
                        Reciever_Email = "upload@gtx.nu",
                        Reciever_Reference = Ref_1,
                        NumberofCollies = (byte)Pcs,
                        Reference = Ref_2,
                        Total_Weight = Bill_Wt,
                        Length = Length,
                        Width = Width,
                        Height = Height,
                        Vol_Weight = Bill_Wt,
                        BilledWeight = Bill_Wt,
                        Customevalue = null,
                        PackValue = null,
                        PackValuta = null,
                        Description = "- ",
                        Costprice = Total_Non_Tax_Amt,
                        Saleprice = null,
                        Oli = null




                    };

                    return wf;
                }
            }

            return null;

        }
        public string XMLSafe(string data)
        {

            return data.Replace("&", "").Replace("<", "").Replace(">", "");


        }
        public FedexRecord(string line, StringBuilder sb, Translation translation)
        {
            zCurrentLine = line;
            zReasonError = sb;
            zCSVdata = line.Split(',');
            zCSVdata = zCSVdata.Select(x => x.Trim('"')).ToArray();
            zTranslation = translation;
            



               var key = Svc + Pkg + "_" + Direction(Cntry1, Cntry2);
               GTXTranslate = TranslateObj(key, "FRAGT");

               InvoiceName = Invoice_Number;
               
               MakeAddtionelAll();


            



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


        private void MakeAddtionelAll()
        {
            
          

          

            MakeAddtionel(Chrg_2, Vol_Disc, "Vol_Disc");
            MakeAddtionel(Chrg_3, Earned_Disc, "Earned_Disc");
            MakeAddtionel(Chrg_4, Auto_Disc, "Auto_Disc");
            MakeAddtionel(Chrg_5, Perf_Price_Disc, "Perf_Price_Disc");
            MakeAddtionel(Chrg_6, Fuel_Amt, "Fuel_Amt");
            MakeAddtionel(Chrg_7, Resi_Amt, "Resi_Amt");
            MakeAddtionel(Chrg_8, DAS_Amt, "DAS_Amt");
            MakeAddtionel(Chrg_9, On_Call_Amt, "On-Call_Amt");
            MakeAddtionel(Chrg_10, DV_Amt, "D.V._Amt");
            MakeAddtionel(Chrg_11, Sign_Svc_Amt, "Sign_Svc_Amt");
            MakeAddtionel(Chrg_12, Sat_Amt, "Sat_Amt");
            MakeAddtionel(Chrg_13, Addn_Hndlg_Amt, "Addn_Hndlg_Amt");
            MakeAddtionel(Chrg_14, Adr_Corr_Amt, "Adr_Corr_Amt");
            MakeAddtionel(Chrg_15, GST_Amt, "GST_Amt");
            MakeAddtionel(Chrg_16, Duty_Amt, "Duty_Amt");
            MakeAddtionel(Chrg_17, Adv_Fee_Amt, "Adv_Fee_Amt");
            MakeAddtionel(Chrg_18, Orig_VAT_Amt, "Orig_VAT_Amt");
            MakeAddtionel(Chrg_19, Misc_1_Amt, "Misc_1_Amt");
            MakeAddtionel(Chrg_20, Misc_2_Amt, "Misc_2_Amt");
            MakeAddtionel(Chrg_21, Misc_3_Amt, "Misc_3_Amt");



        }

        private TranslationRecord TranslateObj(string name, string type)
        {

            if (zTranslation.TranDictionary.ContainsKey(name))
            {
                return zTranslation.TranDictionary[name];
            }
            zTranslation.AddMissing(name, type);
            TranslationError = true;


            return null;
        }

       


        public void LastLine(string xmlShipments)
        {




        }


        private void MakeAddtionel(string code, decimal price, string name)
        {

            if (code != "")
            {
                var trans = TranslateObj(code +"_"+name, "GEBYR");
                if (trans != null && trans.KeyType == "GEBYR")
                {
                    var s = new Service
                    {
                        GTXCode = trans.GTXName,
                        Price = price


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

        private int SafeInt(string no)
        {
            int o = 0;
           if( int.TryParse(no,out o))
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
