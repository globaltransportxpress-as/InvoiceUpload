//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UploadDHL
{
    using System;
    using System.Collections.Generic;
    
    public partial class PDKPalletReport
    {
        public int Id { get; set; }
        public System.DateTime Indlev_dato { get; set; }
        public string Afsenders_kundenummer { get; set; }
        public string Afsenders_land { get; set; }
        public string Afsender { get; set; }
        public string Afsenders_postnummer { get; set; }
        public string Forsendelsenummer { get; set; }
        public string Afsenders_reference { get; set; }
        public string Sidste_status { get; set; }
        public string Antal_kolli_sidste_status { get; set; }
        public string Status_tid { get; set; }
        public string Afsenders_id_på_modtager { get; set; }
        public string Retur_forsendelsesnummer { get; set; }
        public string Modtagers_kundenummer { get; set; }
        public string Modtager { get; set; }
        public string Modtagers_landekode { get; set; }
        public string Modtagers_postnummer { get; set; }
        public string Modtagers_Sted { get; set; }
        public string PostNord_afdelingsnavn { get; set; }
        public string Servicepartner_navn { get; set; }
        public string Servicepartner_postnummer { get; set; }
        public string Servicepartner_stednavn { get; set; }
        public string Fragtbetaler { get; set; }
        public string Produkt { get; set; }
        public string Varestrøm { get; set; }
        public string Transport_enhed { get; set; }
        public int Ant_kolli_totalt { get; set; }
        public int Fragtvægt { get; set; }
        public int Vægt_kg { get; set; }
        public string Volumen { get; set; }
        public string FragtbetalerCode { get; set; }
        public string Beregnet_levert { get; set; }
        public string Fakturanummer { get; set; }
        public decimal Energitillæg { get; set; }
        public decimal Fragt { get; set; }
        public decimal Miljøtillæg { get; set; }
        public decimal Total_til_betaling { get; set; }
        public decimal Gebyr_modtager_betaler { get; set; }
        public decimal Konverteringsgebyr { get; set; }
        public decimal Genudbringning { get; set; }
    }
}
