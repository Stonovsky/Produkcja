namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoTabele_tblProdukcjaRozliczenie_PWPodsumowanie : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie",
                c => new
                    {
                        IDProdukcjaRozliczenie_PWPodsumowanie = c.Int(nullable: false, identity: true),
                        IDProdukcjaRozliczenie_Naglowek = c.Int(nullable: false),
                        IDZlecenie = c.Int(nullable: false),
                        NrZlecenia = c.String(maxLength: 20),
                        SymbolTowaruSubiekt = c.String(maxLength: 30),
                        NazwaTowaruSubiekt = c.String(maxLength: 100),
                        Ilosc = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Waga_kg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IDJm = c.Int(nullable: false),
                        CenaProduktuBezNarzutow_kg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CenaProduktuBezNarzutow_m2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CenaSprzedazyGtex_m2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CenaHurtowaAGG_m2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CenaJednostkowa = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Wartosc = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IDProdukcjaRozliczenie_PWPodsumowanie)
                .ForeignKey("dbo.tblJm", t => t.IDJm, cascadeDelete: true)
                .ForeignKey("Produkcja.tblProdukcjaRozliczenie_Naglowek", t => t.IDProdukcjaRozliczenie_Naglowek, cascadeDelete: true)
                .Index(t => t.IDProdukcjaRozliczenie_Naglowek)
                .Index(t => t.IDJm);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", "IDProdukcjaRozliczenie_Naglowek", "Produkcja.tblProdukcjaRozliczenie_Naglowek");
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", "IDJm", "dbo.tblJm");
            DropIndex("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", new[] { "IDJm" });
            DropIndex("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", new[] { "IDProdukcjaRozliczenie_Naglowek" });
            DropTable("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie");
        }
    }
}
