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
    
    public partial class PDKPalletCollirecord
    {
        public int Id { get; set; }
        public string Factura { get; set; }
        public string Sendingsnummer { get; set; }
        public string Kollinummer { get; set; }
        public System.DateTime Indleveringsdato { get; set; }
        public string Fragtbetaler { get; set; }
        public string Afsender { get; set; }
        public string Afsender_landekode { get; set; }
        public string Afsender_postnummer { get; set; }
        public string Afsender_stednavn { get; set; }
        public string Modtager { get; set; }
        public string Modtager_Landekode { get; set; }
        public string Modtager_Postnummer { get; set; }
        public string Modtager_Stednavn { get; set; }
        public string FRBvægt { get; set; }
        public string Afsenders_ref { get; set; }
        public string Partiref { get; set; }
        public string Modtagers_ref { get; set; }
        public string Produkt { get; set; }
        public Nullable<decimal> Vægt { get; set; }
        public Nullable<decimal> Volum_dm3 { get; set; }
        public string Palletype { get; set; }
        public Nullable<int> Længde { get; set; }
        public Nullable<int> Bredde { get; set; }
        public Nullable<int> Højde { get; set; }
        public Nullable<decimal> Total_aft_Pris { get; set; }
        public string Valuta { get; set; }
    }
}
