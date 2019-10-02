using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using UploadDHL.DataConnections;
using UploadDHL.DataUploadWeb;

namespace UploadDHL
{
    class PalletReportRecord
    {

        public bool FormatError;
        public DateTime Indlev_dato { get; set; }
        
        public String Afsenders_kundenummer { get; set; }
        public String Afsenders_land { get; set; }
        public String Afsender { get; set; }
        public String Afsenders_postnummer { get; set; }
        public String Forsendelsenummer { get; set; }
        public String Afsenders_reference { get; set; }
        public String Sidste_status { get; set; }
        public String Antal_kolli_sidste_status { get; set; }
        public String Status_tid { get; set; }
        public String Afsenders_id_på_modtager { get; set; }
        public String Retur_forsendelsesnummer { get; set; }
        public String Modtagers_kundenummer { get; set; }
        public String Modtager { get; set; }
        public String Modtagers_landekode { get; set; }
        public String Modtagers_postnummer { get; set; }
        public String Modtagers_Sted { get; set; }
        public String PostNord_afdelingsnavn { get; set; }
        public String Servicepartner_navn { get; set; }
        public String Servicepartner_postnummer { get; set; }
        public String Servicepartner_stednavn { get; set; }
        public String Fragtbetaler { get; set; }
        public String Produkt { get; set; }
        public String Varestrøm { get; set; }
        public String Transport_enhed { get; set; }
        public int Ant_kolli_totalt { get; set; }
        public int Fragtvægt { get; set; }
        public int Vægt_kg { get; set; }
        public String Volumen { get; set; }
        public String FragtbetalerCode { get; set; }
        public String Beregnet_levert { get; set; }
        public String Fakturanummer { get; set; }
        public Decimal Energitillæg { get; set; }
       
        public Decimal Fragt { get; set; }
        public Decimal Miljøtillæg { get; set; }
        public Decimal Total_til_betaling { get; set; }
        public Decimal Gebyr_modtager_betaler { get; set; }
        public Decimal Konverteringsgebyr { get; set; }
        public Decimal Genudbringning { get; set; }


        public PalletReportRecord(string[] data)
        {
            Indlev_dato = SafeDate(data[0]);
            Afsenders_kundenummer = data[1];
            Afsenders_land = data[2];
            Afsender = data[3];
            Afsenders_postnummer = data[4];
            Forsendelsenummer = data[5];
            Afsenders_reference = data[6];
            Sidste_status = data[7];
            Antal_kolli_sidste_status = data[8];
            Status_tid = data[9];
            Afsenders_id_på_modtager = data[10];
            Retur_forsendelsesnummer = data[11];
            Modtagers_kundenummer = data[12];
            Modtager = data[13];
            Modtagers_landekode = data[14];
            Modtagers_postnummer = data[15];
            Modtagers_Sted = data[16];
            PostNord_afdelingsnavn = data[17];
            Servicepartner_navn = data[18];
            Servicepartner_postnummer = data[19];
            Servicepartner_stednavn = data[20];
            Fragtbetaler = data[21];
            Produkt = data[22];
            Varestrøm = data[23];
            Transport_enhed = data[24];
            Ant_kolli_totalt = SafeInt(data[25]);
            Fragtvægt =SafeInt( data[26]);
            Vægt_kg = SafeInt(data[27]);
            Volumen = data[28];
            FragtbetalerCode = data[29];
            Beregnet_levert = data[30];
            Fakturanummer = data[31];
            Energitillæg = SafeDecimal(data[32]);
            Fragt = SafeDecimal(data[33]);
            Miljøtillæg = SafeDecimal(data[34]);
            Total_til_betaling = SafeDecimal(data[35]);
            Gebyr_modtager_betaler = SafeDecimal(data[36]);
            Konverteringsgebyr = SafeDecimal(data[37]);
            Genudbringning = SafeDecimal(data[38]);

        }


        public PDKPalletReport Convert()
        {
           
                var pobj = new PDKPalletReport
                {
                    Afsender = Afsender,
                    Afsenders_id_på_modtager = Afsenders_id_på_modtager,
                    Afsenders_kundenummer = Afsenders_kundenummer,
                    Afsenders_land = Afsenders_land,
                    Afsenders_postnummer = Afsenders_postnummer,
                    Afsenders_reference = Afsenders_reference,
                    Antal_kolli_sidste_status = Antal_kolli_sidste_status,
                    Ant_kolli_totalt = Ant_kolli_totalt,
                    Beregnet_levert = Beregnet_levert,
                    Energitillæg = Energitillæg,
                    Fakturanummer = Fakturanummer,
                    Forsendelsenummer = Forsendelsenummer,
                    Fragt = Fragt,
                    Fragtbetaler = Fragtbetaler,
                    FragtbetalerCode = FragtbetalerCode,
                    Fragtvægt = Fragtvægt,
                    Gebyr_modtager_betaler = Gebyr_modtager_betaler,
                    Genudbringning = Genudbringning,
                    Indlev_dato = Indlev_dato,
                    Konverteringsgebyr = Konverteringsgebyr,
                    Miljøtillæg = Miljøtillæg,
                    Modtager = Modtager,
                    Modtagers_kundenummer = Modtagers_kundenummer,
                    Modtagers_landekode = Modtagers_landekode,
                    Modtagers_postnummer = Modtagers_postnummer,
                    Modtagers_Sted = Modtagers_Sted,
                    PostNord_afdelingsnavn = PostNord_afdelingsnavn,
                    Produkt = Produkt,
                    Retur_forsendelsesnummer = Retur_forsendelsesnummer,
                    Servicepartner_navn = Servicepartner_navn,
                    Servicepartner_postnummer = Servicepartner_postnummer,
                    Servicepartner_stednavn = Servicepartner_stednavn,
                    Sidste_status = Sidste_status,
                    Status_tid = Status_tid,
                    Total_til_betaling = Total_til_betaling,
                    Transport_enhed = Transport_enhed,
                    Varestrøm = Varestrøm,
                    Volumen = Volumen,
                    Vægt_kg = Vægt_kg

                };

                return pobj;
             

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
