using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace UploadDHL
{
    public partial class DataRecord
    {
        public String RecordStatus { get; set; }
        public int InvLineNumber { get; set; }
    
        public List<string> ErrorHelper = new List<string>();
        public Translation TranslationHandler { get; set; }
    
        public XMLRecord XmlRecord { get; set; }
        public TranslationRecord GTXTranslate { get; set; }



        public String Awb { get; set; }


        public List<Service> Services = new List<Service>();

        public DataRecord()
        {
            RecordStatus = VendorHandler.E_INI;
        }

        public DateTime SafeDate(string data, string field)
        {

            if (data != "")
            {
                DateTime dd;
                if (DateTime.TryParse(data, out dd))
                {
                    return dd;
                }
                if (DateTime.TryParse(data.Substring(0, 4) + "-" + data.Substring(4, 2) + "-" +
                                      data.Substring(6, 2), out dd))
                {
                    return dd;
                }

                RecordStatus = VendorHandler.E_DATE;
                ErrorHelper.Add(VendorHandler.E_DATE + "->" + field);
            }
            
            return new DateTime();

        }
        public string SafeString(string data)
        {

            if (data == "" || data == "0")
            {
                return "";
            }


            return data;
        }
        public int SafeInt(string no)
        {
            int o = 0;
            if (int.TryParse(no, out o))
            {
                return o;
            }
            return o;
        }
        public int SafeInt(string no, int def)
        {
            int o = def;
            if (int.TryParse(no, out o))
            {
                return o;
            }
            return o;
        }
        public decimal SafeDecimal(string data, string field )
        {
            decimal dec;

            if (data == "")
            {
                return 0;
            }
            if (data.Contains(","))
            {
                if (decimal.TryParse(data, NumberStyles.Any, CultureInfo.CurrentCulture, out dec))
                {
                    return dec;
                }
            }
            else
            {
                if (decimal.TryParse(data, NumberStyles.Any, CultureInfo.InvariantCulture, out dec))
                {
                    return dec;
                }
            }
           

            ErrorHelper.Add(VendorHandler.E_DECIMAL+"->"+field);

            RecordStatus = VendorHandler.E_DECIMAL;

            return 0;
        }

        public string ReplaceList(string ss, string replace)
        {

            for (var i = 0; i < replace.Length; i++)
            {
                ss = ss.Replace(replace[i].ToString(), "");
            }
            return ss;

        }

    }
}
