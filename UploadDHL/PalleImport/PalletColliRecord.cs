using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using UploadDHL.DataUploadWeb;

namespace UploadDHL
{
    class PalletColliRecord
    {

        public string Header = "Sendingsnummer,Indleveringsdato,Fragtbetaler,Afsender,Afsenderlandekode,Afsenderpostnummer,Afsenderstednavn,Modtager,ModtagerLandekode,ModtagerPostnummer,ModtagerStednavn,Antalkolli,FRB-vægt,Afsendersref.,Partiref.,Modtagersref.,Produkt,Vægt,Volum(dm3),Ladmeter,Fragtgrundlag,Tillægsydelse,Valuta,Kurs,Normalpris,Tillægaftalepris,Energitillægaftalepris,Miljøtillægavt.pris,Kapacitetstillæg,Hentetillægaftalepris,Aftalepris,Totalaft.Pris,Afsenderidpåmodtager,Sendingsindhold,Godstype,Fritekst,Postnummer";
        public String Factura { get; set; }
        public bool GoData { get; set; }
        public bool HeaderOK { get; set; }
        public bool FormatError { get; set; }
        public String Sendingsnummer { get; set; }
        public String Kollinummer { get; set; }
        public DateTime Indleveringsdato { get; set; }
        public String Fragtbetaler { get; set; }
        public String Afsender { get; set; }
        public String Afsender_landekode { get; set; }
        public String Afsender_postnummer { get; set; }
        public String Afsender_stednavn { get; set; }
        public String Modtager { get; set; }
        public String Modtager_Landekode { get; set; }
        public String Modtager_Postnummer { get; set; }
        public String Modtager_Stednavn { get; set; }
        public String FRBvægt { get; set; }
    public String Afsenders_ref { get; set; }
    public String Partiref { get; set; }
    public String Modtagers_ref { get; set; }
    public String Produkt { get; set; }
    public decimal Vægt { get; set; }
    public decimal Volum_dm3 { get; set; }
    public String Palletype { get; set; }
    public int Længde { get; set; }
    public int Bredde { get; set; }
    public int Højde { get; set; }
    public decimal Total_aft_Pris { get; set; }
    public String Valuta { get; set; }



    public PalletColliRecord(string[]data, string factura, bool headerOK)
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
                Kollinummer = data[1];
                Indleveringsdato =SafeDate( data[2]);
                Fragtbetaler = data[3];
                Afsender = data[4];
                Afsender_landekode = data[5];
                Afsender_postnummer = data[6];
                Afsender_stednavn = data[7];
                Modtager = data[8];
                Modtager_Landekode = data[9];
                Modtager_Postnummer = data[10];
                Modtager_Stednavn = data[11];
                FRBvægt = data[12];
                Afsenders_ref = data[13];
                Partiref = data[14];
                Modtagers_ref = data[15];
                Produkt = data[16];
                Vægt = SafeDecimal(data[17]);
                Volum_dm3 = SafeDecimal(data[18]);
                Palletype = data[19];
                Længde = SafeInt(data[20]);
                Bredde = SafeInt( data[21]);
                Højde = SafeInt(data[22]);
                Total_aft_Pris = SafeDecimal(data[23]);
                Valuta = data[24];

            }





        }
        public PDKPalletCollirecord Convert()
        {


            return new PDKPalletCollirecord
            {
                Factura=Factura,
                Sendingsnummer = Sendingsnummer,
                Kollinummer = Kollinummer,
                Indleveringsdato = Indleveringsdato,
                Fragtbetaler = Fragtbetaler,
                Afsender = Afsender,
                Afsender_landekode = Afsender_landekode,
                Afsender_postnummer = Afsender_postnummer,
                Afsender_stednavn = Afsender_stednavn,
                Modtager = Modtager,
                Modtager_Landekode = Modtager_Landekode,
                Modtager_Postnummer = Modtager_Postnummer,
                Modtager_Stednavn = Modtager_Stednavn,
                FRBvægt = FRBvægt,
                Afsenders_ref = Afsenders_ref,
                Partiref = Partiref,
                Modtagers_ref = Modtagers_ref,
                Produkt = Produkt,
                Vægt = Vægt,
                Volum_dm3 = Volum_dm3,
                Palletype = Palletype,
                Længde = Længde,
                Bredde = Bredde,
                Højde = Højde,
                Total_aft_Pris = Total_aft_Pris,
                Valuta = Valuta,


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
