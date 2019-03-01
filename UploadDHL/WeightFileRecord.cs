using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UploadDHL
{
    class WeightFileRecord
    {


        public string AWB { get; set; }
        public decimal BillWeight { get; set; }
        public decimal Price { get; set; }

        public int SalesProduct { get; set; }

        public int TransportProduct { get; set; }

        public string CreditorAccount { get; set; }

        public List<Service> Services { get; set; }

      
    }
}
