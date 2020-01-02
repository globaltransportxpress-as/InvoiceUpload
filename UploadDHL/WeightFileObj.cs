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
      
        public static void CreateFile(string path , List<XMLRecord> list, string factura)
        {

            

            var filename = path + "\\Weight\\W_" + factura + "_" +
                           DateTime.Now.ToString("yyyyMMddmm") + ".csv";
            


            CreatePartFile(filename,list);
           
          
           




        }




        private static void CreatePartFile(string filename, List<XMLRecord> list)
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
                       
                      

                          
                           
                           
                            outputFile.WriteLine(rec.ToStringRec());
                       



                    }




                }

            }
            
         

        }


        public static string ServicePart(WeightFileRecord rec)
        {
            var s = rec.Services.Select(x => x.GTXCode + ":" + x.Price.ToString(CultureInfo.GetCultureInfo("da-DK"))).ToArray();
            return string.Join("|", s);
        }



    }

}
