using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace UploadDHL
{
    class PdkHandler

    {


        private string zFactura;
        private string zCustomerNumber;
        private DateTime zFacturaDate;

        private List<string> awbs = new List<string>();
        public string Error { get; set; }
        public string ErrorWeight { get; set; }
        public int DropLines { get; set; }

        public string Factura
        {
            get { return zFactura; }
        }

        public DateTime FacturaDate
        {
            get { return zFacturaDate; }
        }
        public Dictionary<string, int> Dic;
        public List<PDKrecord> Records = new List<PDKrecord>();
        private Translation zTranslation = new Translation(Config.TranslationFilePDK);

        private static string zfixhead =
                "Stregkode;Dato;Ordre;Ordrepos.;Materiale;Frankering;Momsbelagt;Grundpris;Ialt(excl.moms);Frapostnr;Tilpostnr;Fra-land;Til-land;Vægt;Volumenvægt;Faktureretvægt;Længde;Bredde;Højde;Navn1;Adresse;"
            ;
        public PdkHandler()
        {
            Error = zTranslation.Error;
            ErrorWeight = "";
        }
        public PDKrecord SetData(string[] da)


        {


            if (da[0] == "Kundenummer:")
            {


                zCustomerNumber = da[2];
                return null;

            }
            if (da[0] == "Fakturanummer:")
            {


                zFactura = da[2];
                return null;

            }
            if (da[0] == "Fakturadato:")
            {


                zFacturaDate = DateTime.Parse(da[2]);
                return null;

            }


            if (da[0] == "Stregkode")
            {


                Dic = MakeHeader(da);

                return null;

            }

            if (string.IsNullOrEmpty(zCustomerNumber) || zFacturaDate.Year < 2000 || string.IsNullOrEmpty(zFactura) ||
                Dic == null || string.IsNullOrEmpty(da[0]) || string.IsNullOrEmpty(da[1]))
            {

                
                return null;
            }





            var pdkRec = new PDKrecord();
            pdkRec.Factura = zFactura;
            pdkRec.FacturaDate = zFacturaDate;
            pdkRec.CustomerNumber = zCustomerNumber;
            pdkRec.Services = new List<Service>();
            pdkRec.AWB = CheckDigits(da[Dic["Stregkode"]]);
            pdkRec.Price = SafeDecimal(da[Dic["Grundpris"]]);
            pdkRec.Material = ReplaceList(da[Dic["Materiale"]].Trim().ToUpper(), " 12345678908()-.,");
            if (!zTranslation.TranDictionary.ContainsKey(pdkRec.Material))
            {

                zTranslation.AddMissing(pdkRec.Material, "FRAGT");
                Error = "Missing translation";
                return null;
            }

             pdkRec.GTXTranslate = zTranslation.TranDictionary[pdkRec.Material];
            if (pdkRec.GTXTranslate.KeyType != "GEBYR" && pdkRec.GTXTranslate.KeyType != "FRAGT")
            {
                DropLines = DropLines + 1;
                return null;
            }


            pdkRec.Date = DateTime.Parse(da[Dic["Dato"]]);
            pdkRec.Order = da[Dic["Ordre"]];
            pdkRec.OrderLine = da[Dic["Ordrepos."]];

            pdkRec.Frankering = da[Dic["Frankering"]];
            pdkRec.Vat = SafeLookUp(da, "Momsbelagt", "").Equals("X");
            pdkRec.Price = SafeDecimal(da[Dic["Grundpris"]]);
            pdkRec.PriceVat = SafeDecimal(da[Dic["Ialt(excl.moms)"]]);
            pdkRec.FromZip = da[Dic["Frapostnr"]];
            pdkRec.ToZip = da[Dic["Tilpostnr"]];
            pdkRec.SenderCountry = da[Dic["Fra-land"]];
            pdkRec.ReceiverCountry = da[Dic["Til-land"]];
            pdkRec.Weight = SafeDecimal(da[Dic["Vægt"]]);

           


            pdkRec.VolWeight = SafeDecimal(da[Dic["Volumenvægt"]]);
            pdkRec.BillWeight = SafeDecimal(da[Dic["Faktureretvægt"]]);
            if (pdkRec.BillWeight == 0)
            {


                ErrorWeight = ErrorWeight + "," + pdkRec.AWB;
                return null;

            }
            pdkRec.Length = SafeDecimal(da[Dic["Længde"]]);
            pdkRec.Width = SafeDecimal(da[Dic["Bredde"]]);
            pdkRec.Height = SafeDecimal(da[Dic["Højde"]]);
            pdkRec.Name = SafeLookUp(da, "Navn1", "No name");
            pdkRec.Address = SafeLookUp(da, "Adresse", "No address");

            foreach (var k in Dic.Keys)
            {

                if (!zfixhead.Contains(k + ";") && SafeDecimal(da[Dic[k]]) > 0)
                {
                    var trans = zTranslation.TranDictionary[k];
                    if (trans.KeyType == "GEBYR")
                    {
                        var sv = new Service();
                        sv.OrigalName = k;
                        sv.GTXCode = trans.GTXName;
                        sv.Price = SafeDecimal(da[Dic[k]]);

                        pdkRec.Services.Add(sv);
                    }



                }
            }



            if (pdkRec.GTXTranslate.KeyType == "GEBYR")
            {

                var sv = new Service();
                sv.OrigalName = pdkRec.GTXTranslate.Key;
                sv.GTXCode = pdkRec.GTXTranslate.GTXName;
                sv.Price = pdkRec.Price;
               

                var record = Records.FirstOrDefault(x => x.AWB == pdkRec.AWB);
                if (record == null)
                {

                    pdkRec.Services.Add(sv);
                    Records.Add(pdkRec);
                }
                else
                {
                    record.Services.Add(sv);
                }




            }
            else
            {
                Records.Add(pdkRec);
            }
          


           

           
            return pdkRec;


        }


        private static string CheckDigits(string awb)
        {
            if (awb.StartsWith("050050"))
            {

                int CheckD = 36;
                int Mod = 36;


                char[] isoval2ascii =
                {
                    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                    'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                    'U', 'V', 'W', 'X', 'Y', 'Z', '*'
                };


                char[] awbarray = awb.ToCharArray(0, awb.Length);

                foreach (char c in awbarray)
                {
                    int charvalue = Array.IndexOf(isoval2ascii, c);
                    if (charvalue == -1)
                        return "-1";

                    CheckD += charvalue;
                    if (CheckD > Mod)
                        CheckD = CheckD - Mod;
                    CheckD = CheckD * 2;

                    if (CheckD > Mod)
                        CheckD = CheckD - (Mod + 1);

                }

                CheckD = (Mod + 1) - CheckD;
                if (CheckD == Mod)
                    CheckD = 0;

                return awb + isoval2ascii[CheckD].ToString();
            }
            return awb;

        }

        private string SafeLookUp(string[] da, string code, string def)
        {
            if (Dic.ContainsKey(code))
            {
                return da[Dic[code]];

            }

            return def;

        }
        private decimal SafeDecimal(string data)
        {
            decimal dec;
            if (data == "")
            {
                return 0;
            }

            if (decimal.TryParse(data, NumberStyles.Any, CultureInfo.GetCultureInfo("da-DK"), out dec))
            {
                return dec;
            }

            return 0;
        }

        private string ReplaceList(string ss, string replace)
        {

            for (var i = 0; i < replace.Length; i++)
            {
                ss = ss.Replace(replace[i].ToString(), "");
            }
            return ss;

        }

        private Dictionary<string, int> MakeHeader(string[] hd)
        {

            var ok = true;
            DropLines = 0;

            var dic = new Dictionary<string, int>();
            var i = 0;
            foreach (var d in hd)
            {

                var key = d.Replace(" ", "");
                if (key != "")
                {
                    if (zfixhead.Contains(key + ";"))
                    {

                        dic.Add(d.Replace(" ", ""), i);


                    }
                    else
                    {
                        if (!zTranslation.TranDictionary.ContainsKey(key))
                        {
                            zTranslation.AddMissing(key, "GEBYR");
                            Error = "Missing translation";
                            ok = false;
                        }
                        else
                        {
                            dic.Add(zTranslation.TranDictionary[key].Key, i);
                        }



                    }
                }

                i++;



            }
            if (ok)
            {
                return dic;
            }

            return null;


        }

        public void MakeXML2()
        {
            var dhlXml = new DHLXML();

            var shipments = new StringBuilder();
            foreach (var pdkRecord in Records)
            {
                var services = new StringBuilder();
                var total = pdkRecord.Services.Sum(x => x.Price) + pdkRecord.PriceVat;

                int count = 1;
                if (pdkRecord.GTXTranslate.KeyType=="FRAGT")
                {

                    services.Append(dhlXml.FillServicesXml(pdkRecord.GTXTranslate.GTXName, pdkRecord.Price));

                }
                foreach (var service in pdkRecord.Services)
                {
                    count++;
                    services.Append(dhlXml.FillServicesXml(service.GTXCode, service.Price));
                }
                
                shipments.Append(dhlXml.FillShipmentXml(pdkRecord.AWB, "", pdkRecord.Date, pdkRecord.GTXTranslate.GTXName, 1,
                    pdkRecord.BillWeight, pdkRecord.Price, pdkRecord.FromZip, pdkRecord.SenderCountry, pdkRecord.ToZip,
                    pdkRecord.ReceiverCountry, services.ToString()));


            }

            var sumfragt = Records.Sum(x => x.Price);
            var tillæg = Records.Sum(x => x.Services.Sum(y => y.Price));
            var tax = Records.Sum(x => x.PriceVat) - sumfragt;


            var xml = dhlXml.FillFacturaXml(Factura, zFacturaDate, zFacturaDate.AddDays(30), zCustomerNumber, sumfragt,
                tillæg, tax, shipments.ToString());




            using (StreamWriter xmlout =
                new StreamWriter(Config.PDKRootFileDir + "\\Xml\\X" + Factura + "_" + DateTime.Now.ToString("yyyyMMddmm") + ".xml", false))
            {
                xmlout.Write(xml);
            }



        }

    }



}

