using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UploadDHL.DataUploadWeb;

namespace UploadDHL
{
    class InvoiceShipmentLoad
    {
        private List<InvoiceShipmentHolder> invList = new List<InvoiceShipmentHolder>();
        private InvoiceUploadSoapClient _service = new InvoiceUploadSoapClient("InvoiceUploadSoap");
        public List<string> ErrorList { get; set; }

        public void AddShipment(InvoiceShipmentHolder inShip)
        {

            if (inShip != null)
            {
                invList.Add(inShip);
            }

        }





        public string Run()
        {
            ErrorList = new List<string>();
          
            var lst = invList.Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index / 500)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

            foreach (var l in lst)
            {


              ErrorList.Add(_service.ShipmentUpload(l.ToArray()));




            }


            if (ErrorList.All(x => x.Contains("OK")))
            {
                return "OK";
            }

            return "Invoice shipment error ";






        }



    }
}
