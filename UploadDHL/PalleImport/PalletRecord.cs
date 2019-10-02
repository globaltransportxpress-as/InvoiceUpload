using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UploadDHL.DataConnections;
using UploadDHL.DataUploadWeb;

namespace UploadDHL
{
    class PalletRecord
    {

        public string Header = "Sendingsnummer,Indleveringsdato,Fragtbetaler,Afsender,Afsenderlandekode,Afsenderpostnummer,Afsenderstednavn,Modtager,ModtagerLandekode,ModtagerPostnummer,ModtagerStednavn,Antalkolli,FRB-vægt,Afsendersref.,Partiref.,Modtagersref.,Produkt,Vægt,Volum(dm3),Ladmeter,Fragtgrundlag,Tillægsydelse,Valuta,Kurs,Normalpris,Tillægaftalepris,Energitillægaftalepris,Miljøtillægavt.pris,Kapacitetstillæg,Hentetillægaftalepris,Aftalepris,Totalaft.Pris,Afsenderidpåmodtager,Sendingsindhold,Godstype,Fritekst,Postnummer";
        public String Factura { get; set; }
        public bool GoData { get; set; }
        public bool HeaderOK { get; set; }
        public bool FormatError { get; set; }
        public String Sendingsnummer { get; set; }
        public DateTime Indleveringsdato { get; set; }
        public String Fragtbetaler { get; set; }
        public String Afsender { get; set; }
        public String Afsenderlandekode { get; set; }
        public String Afsenderpostnummer { get; set; }
        public String Afsenderstednavn { get; set; }
        public String Modtager { get; set; }
        public String ModtagerLandekode { get; set; }
        public String ModtagerPostnummer { get; set; }
        public String ModtagerStednavn { get; set; }
        public int Antalkolli { get; set; }
        public decimal FRBvægt { get; set; }
        public String Afsendersref { get; set; }
        public String Partiref { get; set; }
        public String Modtagersref { get; set; }
        public String Produkt { get; set; }
        public decimal Vægt { get; set; }
        public decimal Volum { get; set; }
        public decimal Ladmeter { get; set; }
        public String Fragtgrundlag { get; set; }
        public String Tillægsydelse { get; set; }
        public String Valuta { get; set; }
        public String Kurs { get; set; }
        public decimal Normalpris { get; set; }
        public decimal Tillægaftalepris { get; set; }
        public decimal Energitillægaftalepris { get; set; }
        public decimal Miljøtillægavtpris { get; set; }
        public decimal Kapacitetstillæg { get; set; }
        public decimal Hentetillægaftalepris { get; set; }
        public decimal Aftalepris { get; set; }
        public decimal TotalaftPris { get; set; }
        public String Afsenderidpåmodtager { get; set; }
        public String Sendingsindhold { get; set; }
        public String Godstype { get; set; }
        public String Fritekst { get; set; }
        public String Postnummer { get; set; }


        public PalletRecord(string[]data, string factura, bool headerOK)
        {
            GoData = false;
            HeaderOK = headerOK;
            Factura = factura;
            if (data[0].Contains("Fakturagrundlag for fakturanummer"))
            {
                Factura = data[0].Replace("Fakturagrundlag for fakturanummer", "");
                return;
            }

            if (data[0].Contains("Sendingsnummer"))
            {
                
                HeaderOK = true;
                return;

            }
            if (HeaderOK)
            {

                Sendingsnummer = data[0];
                Indleveringsdato = SafeDate(data[1]);
                Fragtbetaler = data[2];
                Afsender = data[3];
                Afsenderlandekode = data[4];
                Afsenderpostnummer = data[5];
                Afsenderstednavn = data[6];
                Modtager = data[7];
                ModtagerLandekode = data[8];
                ModtagerPostnummer = data[9];
                ModtagerStednavn = data[10];
                Antalkolli = SafeInt(data[11]);
                FRBvægt = SafeDecimal(data[12]);
                Afsendersref = data[13];
                Partiref = data[14];
                Modtagersref = data[15];
                Produkt = data[16];
                Vægt = SafeDecimal(data[17]);
                Volum = SafeDecimal(data[18]);
                Ladmeter = SafeDecimal(data[19]);
                Fragtgrundlag = data[20];
                Tillægsydelse = data[21];
                Valuta = data[22];
                Kurs = data[23];
                Normalpris = SafeDecimal(data[24]);
                Tillægaftalepris = SafeDecimal(data[25]);
                Energitillægaftalepris = SafeDecimal(data[26]);
                Miljøtillægavtpris = SafeDecimal(data[27]);
                Kapacitetstillæg = SafeDecimal(data[28]);
                Hentetillægaftalepris = SafeDecimal(data[29]);
                Aftalepris = SafeDecimal(data[30]);
                TotalaftPris = SafeDecimal(data[31]);
                Afsenderidpåmodtager = data[32];
                Sendingsindhold = data[33];
                Godstype = data[34];
                Fritekst = data[35];
                Postnummer = data[36];








            }





        }
        public PDKPalletrecord Convert()
        {


            return new PDKPalletrecord
            {
                Factura=Factura,
                Sendingsnummer = Sendingsnummer,
                Indleveringsdato = Indleveringsdato,
                Fragtbetaler = Fragtbetaler,
                Afsender = Afsender,
                Afsenderlandekode = Afsenderlandekode,
                Afsenderpostnummer = Afsenderpostnummer,
                Afsenderstednavn = Afsenderstednavn,
                Modtager = Modtager,
                ModtagerLandekode = ModtagerLandekode,
                ModtagerPostnummer = ModtagerPostnummer,
                ModtagerStednavn = ModtagerStednavn,
                Antalkolli = Antalkolli,
                FRBvægt = FRBvægt,
                Afsendersref = Afsendersref,
                Partiref = Partiref,
                Modtagersref = Modtagersref,
                Produkt = Produkt,
                Vægt = Vægt,
                Volum = Volum,
                Ladmeter = Ladmeter,
                Fragtgrundlag = Fragtgrundlag,
                Tillægsydelse = Tillægsydelse,
                Valuta = Valuta,
                Kurs = Kurs,
                Normalpris = Normalpris,
                Tillægaftalepris = Tillægaftalepris,
                Energitillægaftalepris = Energitillægaftalepris,
                Miljøtillægavtpris = Miljøtillægavtpris,
                Kapacitetstillæg = Kapacitetstillæg,
                Hentetillægaftalepris = Hentetillægaftalepris,
                Aftalepris = Aftalepris,
                TotalaftPris = TotalaftPris,
                Afsenderidpåmodtager = Afsenderidpåmodtager,
                Sendingsindhold = Sendingsindhold,
                Godstype = Godstype,
                Fritekst = Fritekst,
                Postnummer = Postnummer,

            };


        }



        private int SafeInt(string no)
        {
            int o = 0;
            if (int.TryParse(no, out o))
            {
                return o;
            }
            return o;
        }
        private DateTime SafeDate(string data)
        {


            DateTime dd;
            if (DateTime.TryParse(data
                                  , out dd))
            {
                return dd;
            }
            //zReasonError.AppendLine("DateTimeFormat error line " + zCurrentLine);
            FormatError = true;
            return new DateTime();

        }
        private decimal SafeDecimal(string data)
        {
            decimal dec;
            if (data == "")
            {
                return 0;
            }

            if (decimal.TryParse(data, NumberStyles.Any, CultureInfo.CurrentCulture, out dec))
            {
                return dec;
            }


            //zReasonError.AppendLine("DecimalFormat error line " + zCurrentLine);
            FormatError = true;
            return 0;
        }




    }
}
