namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoView_vwZestSprzedazyAGG : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "AGG.vwZestSprzedazyAGG",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            IdFirma = c.Int(nullable: false),
            //            Firma = c.String(),
            //            DokId = c.Int(nullable: false),
            //            DataSprzedazy = c.DateTime(nullable: false),
            //            Rok = c.Int(nullable: false),
            //            NazwaKontrahenta = c.String(),
            //            AdresKontrahenta = c.String(),
            //            MiejscowoscKontrahenta = c.String(),
            //            Kodocztowy = c.String(),
            //            NIP = c.String(),
            //            Towar = c.String(),
            //            NrDokSprzedazy = c.String(),
            //            DokTyp = c.Int(nullable: false),
            //            TytulDok = c.String(),
            //            Podtytul = c.String(),
            //            Ilosc = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Jm = c.String(),
            //            CenaJedn = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Netto = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Brutto = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Koszt = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Zysk = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Marza = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Grupa = c.String(),
            //            Handlowiec = c.String(),
            //            Nroferty = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("AGG.vwZestSprzedazyAGG");
        }
    }
}
