using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UploadDHL
{
    public class Service
    {

        public string  OrigalName { get; set; }
        public string GTXCode { get; set; }
        public decimal Price { get; set; }

        public decimal Tax { get; set; }
        public bool HasOil { get; set; }

        public int InvoiceLineNumber { get; set; }

    }
   



}
