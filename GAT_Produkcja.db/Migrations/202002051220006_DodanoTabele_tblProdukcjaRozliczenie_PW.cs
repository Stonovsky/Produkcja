namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoTabele_tblProdukcjaRozliczenie_PW : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Produkcja.tblProdukcjaRozliczenie_PW",
                c => new
                    {
                        IDProdukcjaRozliczenie_PW = c.Int(nullable: false, identity: true),
                        IdZlecenia = c.Int(nullable: false),
                        NrZlecenia = c.String(maxLength: 20),
                        NrWz = c.String(maxLength: 20),
                        NrRolki = c.String(maxLength: 20),
                        IDSurowiec = c.Int(nullable: false),
                        NazwaTowaru = c.String(maxLength: 30),
                        Waga_kg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WagaOdpadu_kg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ilosc_m2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CenaProduktuBezNarzutow_kg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CenaProduktuBezNarzutow_m2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CenaSprzedazyGtex_m2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Wartosc = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IDPracownikGAT = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDProdukcjaRozliczenie_PW)
                .ForeignKey("dbo.tblPracownikGAT", t => t.IDPracownikGAT, cascadeDelete: true)
                .ForeignKey("Towar.tblTowar", t => t.IDSurowiec, cascadeDelete: true)
                .Index(t => t.IDSurowiec)
                .Index(t => t.IDPracownikGAT);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDSurowiec", "Towar.tblTowar");
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDPracownikGAT", "dbo.tblPracownikGAT");
            DropIndex("Produkcja.tblProdukcjaRozliczenie_PW", new[] { "IDPracownikGAT" });
            DropIndex("Produkcja.tblProdukcjaRozliczenie_PW", new[] { "IDSurowiec" });
            DropTable("Produkcja.tblProdukcjaRozliczenie_PW");
        }
    }
}
