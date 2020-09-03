namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieViews_GTX2_MagazynRuch_FinanseIZobowiazania : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "GTEX2.vwFinanseNalZobGTX2",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            DataPowstania = c.DateTime(),
            //            TerminPlatnosci = c.DateTime(),
            //            DniSpoznienia = c.Int(),
            //            NrDok = c.String(),
            //            Kontrahent = c.String(),
            //            NIP = c.String(),
            //            Naleznosc = c.Decimal(precision: 18, scale: 2),
            //            Zobowiazanie = c.Decimal(precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "GTEX2.vwMagazynRuchGTX2",
            //    c => new
            //        {
            //            IdMagazynRuch = c.Int(nullable: false, identity: true),
            //            IdTowar = c.Int(nullable: false),
            //            TowarSymbol = c.String(),
            //            TowarNazwa = c.String(),
            //            IdMagazyn = c.Int(nullable: false),
            //            MagazynSymbol = c.String(),
            //            MagazynNazwa = c.String(),
            //            Data = c.DateTime(nullable: false),
            //            Ilosc = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Pozostalo = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Jm = c.String(),
            //            Cena = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Wartosc = c.Decimal(nullable: false, precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.IdMagazynRuch);
            
        }
        
        public override void Down()
        {
            //DropTable("GTEX2.vwMagazynRuchGTX2");
            //DropTable("GTEX2.vwFinanseNalZobGTX2");
        }
    }
}
