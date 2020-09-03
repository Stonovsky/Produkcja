namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NowaTabela_tblProdukcjaRozliczenie_Dane : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Produkcja.tblProdukcjaRozliczenie_Dane",
                c => new
                    {
                        IDProdukcjaRozliczenie_Dane = c.Int(nullable: false, identity: true),
                        IDTowar = c.Int(nullable: false),
                        IDTowarAccess = c.Int(nullable: false),
                        IDTowarSubiekt = c.Int(nullable: false),
                        TowarNazwa = c.String(maxLength: 30),
                        DataDodania = c.DateTime(nullable: false),
                        Szerokosc = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Dlugosc = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ilosc_m2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IloscRolek = c.Int(nullable: false),
                        DataDoRozliczenia = c.DateTime(nullable: false),
                        IDPracownikGAT = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDProdukcjaRozliczenie_Dane)
                .ForeignKey("dbo.tblPracownikGAT", t => t.IDPracownikGAT, cascadeDelete: true)
                .ForeignKey("Towar.tblTowar", t => t.IDTowar, cascadeDelete: true)
                .Index(t => t.IDTowar)
                .Index(t => t.IDPracownikGAT);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_Dane", "IDTowar", "Towar.tblTowar");
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_Dane", "IDPracownikGAT", "dbo.tblPracownikGAT");
            DropIndex("Produkcja.tblProdukcjaRozliczenie_Dane", new[] { "IDPracownikGAT" });
            DropIndex("Produkcja.tblProdukcjaRozliczenie_Dane", new[] { "IDTowar" });
            DropTable("Produkcja.tblProdukcjaRozliczenie_Dane");
        }
    }
}
