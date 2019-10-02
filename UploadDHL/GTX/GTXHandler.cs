using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

using UploadDHL.DataUploadWeb;

namespace UploadDHL
{
    class GTXHandler:VendorHandler

    {


        private string zFactura;
        private string zCustomerNumber;
        private DateTime zFacturaDate;

     
      

        public string Factura
        {
            get { return zFactura; }
        }

        public DateTime FacturaDate
        {
            get { return zFacturaDate; }
        }
        public Dictionary<string, int> Dic;
   
        private Translation zTranslation = new Translation(Config.TranslationFileGtx);
        
        private static string zfixhead =
                "PURCHORDERFORMNUM,Pieces,Weight,LNO,Amount,RECEIPTDATECONFIRMED,PICKUPNAME,PICKUPCOUNTRY,DELIVERYNAME,DELIVERYCOUNTRYREGIONID,CUSTOMERREF,ORGNUMBER,INVOICEACCOUNT,NAME,VARENUMMER,VARENAVN,ACCOUNTNUM,ProductGroup,SALESID,INTERCOMPANYORIGINALSALESID,INVOICEID,INVOICEDATE,LINENUM,OilAmount,TXT,TAXWRITECODE,DIMENSION,DIMENSION2_,DIMENSION3_,PU_ADRESS,PU_CITY,PU_ZIP,DL_ADRESS,DL_CITY,DL_ZIP,YEAR";


        public GTXHandler()
        {
            Error = zTranslation.Error;
            RootDir = Config.GTXRootFileDir;
            CarrierName = "GTX";




        }

        public bool Header(string head)
        {
            LineNumber++;
            return MatchHeader(head, zfixhead.Replace(".", "").Replace(" ", ""), ",");
        }
        public void SetData(string[] da)


        {
            LineNumber++;
            var iLine = AddInvoiceLine(string.Join("|", da), 1, E_INI);

            var gtxRecord = new GTXrecord(da, zTranslation,LineNumber);

            iLine.Status = gtxRecord.RecordStatus;
            iLine.Reason = string.Join("; ", gtxRecord.ErrorHelper.ToArray());

            if (!RecordOK(gtxRecord, iLine))
            {
                return;
            }
            RegisterIvoceLine(gtxRecord.XmlRecord, iLine);

            var rec = gtxRecord.XmlRecord;
            if (rec.KeyType == FRAGT )
            {

                Records.Add(rec);
                return;

            }


            AddServiceToShipment(Records, rec);
            
        }

       
           

        


      
    }



}

