using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using UploadDHL.DataUploadWeb;

namespace UploadDHL
{
    class WeightFileObj
    {
      
        public static void CreateFile(string path , List<WeightFileRecord> list, string factura)
        {

            

            var filename = path + "\\Weight\\W_" + factura + "_" +
                           DateTime.Now.ToString("yyyyMMddmm") + ".csv";
            var specialfile = path + "\\Weight\\W#_" + factura + "_" +
                           DateTime.Now.ToString("yyyyMMddmm") + ".csv";


            CreatePartFile(filename, list.Where(x => !x.AWB.StartsWith("#")).ToList(), factura);
            CreatePartFile(specialfile, list.Where(x => x.AWB.StartsWith("#")).ToList(),"#"+factura);
          
           




        }




        private static void CreatePartFile(string filename, List<WeightFileRecord> list, string factura)
        {
           
            var wlist = new List<string>();
            if (list.Count > 0)
            {
                using (StreamWriter outputFile =
                    new StreamWriter(filename))
                {
                    outputFile.WriteLine("awb;weight;salesp;trans;invoice;service;price");
                    foreach (var rec in list)
                    {
                       
                        if ((rec.AWB.StartsWith("#") || rec.BillWeight > 0) && rec.Price >= 0 )
                        {

                            var s = string.Format("{0};{1};{2};{3};{4};{5};{6}", rec.AWB,
                                rec.BillWeight.ToString(CultureInfo.GetCultureInfo("da-DK")), rec.SalesProduct, rec.TransportProduct, rec.CreditorAccount,
                                ServicePart(rec), rec.Price);

                          
                            wlist.Add(s);
                           
                            outputFile.WriteLine(s);
                        }



                    }




                }

            }
            if (wlist.Count>0)
            {
                var service = new InvoiceUploadSoapClient("InvoiceUploadSoap");
               // service.WeighFileUpload(wlist.ToArray(), factura);
            }
         

        }


        private static string ServicePart(WeightFileRecord rec)
        {
            var s = rec.Services.Select(x => x.GTXCode + ":" + x.Price.ToString(CultureInfo.GetCultureInfo("da-DK"))).ToArray();
            return string.Join("|", s);
        }



    }

}
