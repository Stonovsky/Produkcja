﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GAT_PRodukcja.dbProdukcjaPS
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ProdukcjaPSDbContext : DbContext
    {
        public ProdukcjaPSDbContext()
            : base("name=ProdukcjaPSDbContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Artykuły> Artykuły { get; set; }
        public virtual DbSet<Dostawca> Dostawca { get; set; }
        public virtual DbSet<Dyspozycje> Dyspozycje { get; set; }
        public virtual DbSet<Kalander> Kalander { get; set; }
        public virtual DbSet<Konfekcja> Konfekcja { get; set; }
        public virtual DbSet<Magazyn> Magazyn { get; set; }
        public virtual DbSet<MagazynWG> MagazynWG { get; set; }
        public virtual DbSet<Normy_zużycia> Normy_zużycia { get; set; }
        public virtual DbSet<Obsada> Obsada { get; set; }
        public virtual DbSet<Produkcja> Produkcja { get; set; }
        public virtual DbSet<Surowiec> Surowiec { get; set; }
        public virtual DbSet<Szarpak> Szarpak { get; set; }
        public virtual DbSet<Szerokość> Szerokość { get; set; }
    }
}
