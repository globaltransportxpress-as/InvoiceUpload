using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel.Channels;
using System.Text;
using Newtonsoft.Json.Linq;

namespace UploadDHL.Properties.DataSources
{
    public class PriceObject
    {
        public string OK { get; set; }
        public DateTime Date { get; set; }
        public String Payer { get; set; }

        public String Customer { get; set; }

        public String BillCustomer { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public String Awb { get; set; }
        public String CarrierAwb { get; set; }
        public string Account { get; set; }
        public string DataType { get; set; }



        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }

        public decimal CostEstimated { get; set; }
        public string TypeSalesId { get; set; }
        public string TypeCostId { get; set; }
        public int Items { get; set; }
        public string BillType { get; set; }
        public string ForwType { get; set; }

        public string Note { get; set; }
        public string Link { get; set; }
        public string Factura { get; set; }
        public string Log { get; set; }
        public string Carrier { get; set; }
        public int ForwId { get; set; }

        public PriceObject()
        {
        }
        public PriceObject(string data)
        {
            var rec = data.Replace("\"", "").Split(';');
            OK = rec[0];
            Date = SafeDate(rec[1]);
            Payer = rec[2];
            Customer = rec[3];
            BillCustomer = rec[4];
            Address = rec[5];
            Zip = rec[6];
            City = rec[7];
            Awb = rec[8];
            CarrierAwb = rec[9];
            Account = rec[10];
            DataType = rec[11];



            Price = SafeDecimal(rec[12]);
            CostPrice = SafeDecimal(rec[13]);

            CostEstimated = SafeDecimal(rec[14]);
            TypeSalesId = rec[15];
            TypeCostId = rec[16];
            Items = int.Parse(rec[17]);
            BillType = rec[18];
            ForwType = rec[19];

            Note = rec[20];
            Link = rec[21];
            Factura = rec[22];
            Log = rec[23];
            Carrier = rec[24];
            ForwId = int.Parse(rec[25]);



        }
        private decimal SafeDecimal(string s)
        {
            decimal d = 0M;
            if (decimal.TryParse(s, out d))
            {
                return d;
            }
            return d;
        }
        private DateTime SafeDate(string d)
        {

            DateTime dd;
            if (DateTime.TryParse(d, out dd))
            {
                return dd;
            }

            return DateTime.Now;

        }
        public PriceObject(PriceObject res)
        {
            
            
            Awb = "C"+ res.Awb;
            Payer = res.Payer;
            Customer = res.Customer;
            BillCustomer = res.BillCustomer;
            Account = res.Account;
            Address = res.Address;
            City = res.City;
            Zip = res.Zip;
            DataType = res.DataType;
            CarrierAwb = "";
            TypeSalesId = res.TypeSalesId;
            TypeCostId = res.TypeCostId;
            BillType = "Copy";
            Date = res.Date;
            CostEstimated = res.CostEstimated;
            Factura = res.Factura;
            Items = res.Items;
            CostPrice = 0;
            ForwType = "Copy";
            Link = res.Link;
            Log = res.Log;
            OK = "";
            Note = "Copy";
            Price = res.Price;
            Carrier = res.Carrier;
            ForwId = res.ForwId;



        }
        public JObject ToJSonLine()
        {
            var o = new JObject();

            o["type"] = DataType;
            o["id"] = TypeSalesId;
            o["amount"] = Items;
            o["sales_price"] = Price;
            o["planned_cost_price"] = CostEstimated;

            return o;




        }

        public JObject ToJSonHead()
        {
            var ad = Address.Split('|');

            var p = new JObject();

            p["company_name"] = Customer;
            p["attention"] = "";
            p["address_1"] = Address;
            p["address_2"] = "";
            p["post_code"] = Zip;
            p["city"] = City;
            p["country_code"] = "DK";




            var o = new JObject();

            o["account_id"] = Account;
            o["ship_date"] = Date.ToString("yyyy-MM-dd");
            o["awb"] = Awb;
            o["pickup_address"] = p;

            o["planned_cost_price"] = CostEstimated;
            o["order_lines"] = new JArray();
            return o;



        }


    }


}
