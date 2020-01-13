using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UploadDHL.Properties.DataSources
{
    public class PriceObject
    {
        public string OK { get; set; }
        public DateTime Date { get; set; }
        public String Customer { get; set; }
        public string Address { get; set; }
        public String Awb { get; set; }
        public int Account { get; set; }
        public string BillType { get; set; }
        public string ForwType { get; set; }

       
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }

        public decimal CostEstimated { get; set; }

        public int Items { get; set; }

        public string Note { get; set; }
        public string Link { get; set; }
        public string Factura { get; set; }
        public string Log { get; set; }
       
       



    }
}
