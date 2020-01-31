using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UploadDHL.GTX
{
    public class AccountMatch
    {
        public int ForwId { get; set; }

        public string CompanyAddress { get; set; }
        public string Account { get; set; }
        public string Payer { get; set; }


        public AccountMatch(string data)
        {
            var d =data.Split('|');

            ForwId = int.Parse(d[0]);
            CompanyAddress = d[1];
            Account = d[2];
            Payer = d[3];


        }
    }
   
}
