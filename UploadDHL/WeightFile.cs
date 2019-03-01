using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace UploadDHL
{
    class WeightFile
    {

        public static void CreateFile(string path , List<WeightFileRecord> list, string factura)
        {

            var filename = path + "\\Weight\\W_" + factura + "_" +
                           DateTime.Now.ToString("yyyyMMddmm") + ".csv";
            var specialfile = path + "\\Weight\\W#_" + factura + "_" +
                           DateTime.Now.ToString("yyyyMMddmm") + ".csv";


            CreatePartFile(filename, list.Where(x => !x.AWB.StartsWith("#")).ToList());
            CreatePartFile(specialfile, list.Where(x => x.AWB.StartsWith("#")).ToList());
          
           




        }




        private static void CreatePartFile(string filename, List<WeightFileRecord> list)
        {
            if (list.Count > 0)
            {
                using (StreamWriter outputFile =
                    new StreamWriter(filename))
                {

                    foreach (var rec in list)
                    {
                        if (rec.BillWeight > 0 && rec.Price >= 0|| rec.AWB.StartsWith("#"))
                        {
                            var s = string.Format("{0};{1};{2};{3};{4};{5};{6}", rec.AWB,
                                rec.BillWeight.ToString(CultureInfo.GetCultureInfo("da-DK")), rec.SalesProduct, rec.TransportProduct, rec.CreditorAccount,
                                ServicePart(rec), rec.Price);
                            outputFile.WriteLine(s);
                        }



                    }




                }

            }



        }


        private static string ServicePart(WeightFileRecord rec)
        {
            var s = rec.Services.Select(x => x.GTXCode + ":" + x.Price.ToString(CultureInfo.GetCultureInfo("da-DK"))).ToArray();
            return string.Join("|", s);
        }



    }

}
