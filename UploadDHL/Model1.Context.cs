﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DFEEntities : DbContext
    {
        public DFEEntities()
            : base("name=DFEEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<PDKPalletReport> PDKPalletReport { get; set; }
        public virtual DbSet<PDKPalletrecord> PDKPalletrecord { get; set; }
        public virtual DbSet<PDKPalletCollirecord> PDKPalletCollirecord { get; set; }
    }
}
