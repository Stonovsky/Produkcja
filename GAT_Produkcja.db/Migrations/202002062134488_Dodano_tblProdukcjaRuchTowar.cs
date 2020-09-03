namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dodano_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Produkcja.tblProdukcjaRuchTowar",
                c => new
                    {
                        IDProdukcjaRuchTowar = c.Int(nullable: false, identity: true),
                        IDProdukcjaRuchNaglowek = c.Int(nullable: false),
                        IDRuchStatus = c.Int(nullable: false),
                        IDGramatura = c.Int(nullable: false),
                        Gramatura = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Szerokosc_m = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Dlugosc_m = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Waga_kg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ilosc_m2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NrRolki = c.String(maxLength:20),
                        KodKreskowy = c.String(maxLength: 20),
                        CzyKalandrowana = c.Boolean(nullable: false),
                        CzyParametryZgodne = c.Boolean(nullable: false),
                        Uwagi = c.String(),
                        UwagiParametry = c.String(),
                        IDProdukcjaRuchTowarWyjsciowy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDProdukcjaRuchTowar)
                .ForeignKey("Produkcja.tblProdukcjaRuchNaglowek", t => t.IDProdukcjaRuchNaglowek, cascadeDelete: false)
                .ForeignKey("Magazyn.tblRuchStatus", t => t.IDRuchStatus, cascadeDelete: false)
                .ForeignKey("Towar.tblTowarGeowlokninaParametryGramatura", t => t.IDGramatura, cascadeDelete: false)
                .Index(t => t.IDProdukcjaRuchNaglowek)
                .Index(t => t.IDRuchStatus)
                .Index(t => t.IDGramatura)
                .Index(t => t.KodKreskowy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDGramatura", "Towar.tblTowarGeowlokninaParametryGramatura");
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDRuchStatus", "Magazyn.tblRuchStatus");
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchNaglowek", "Produkcja.tblProdukcjaRuchNaglowek");
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "KodKreskowy" });
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDGramatura" });
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDRuchStatus" });
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDProdukcjaRuchNaglowek" });
            DropTable("Produkcja.tblProdukcjaRuchTowar");
        }
    }
}
