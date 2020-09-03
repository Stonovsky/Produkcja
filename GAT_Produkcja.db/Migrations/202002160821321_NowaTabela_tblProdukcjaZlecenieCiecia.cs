namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NowaTabela_tblProdukcjaZlecenieCiecia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Produkcja.tblProdukcjaZlecenieCiecia",
                c => new
                    {
                        IDProdukcjaZlecenieCiecia = c.Int(nullable: false, identity: true),
                        NrZleceniaCiecia = c.Int(nullable: false),
                        IDZlecajacy = c.Int(nullable: false),
                        IDWykonujacy = c.Int(nullable: false),
                        IDTowar = c.Int(nullable: false),
                        IDKontrahent = c.Int(nullable: false),
                        DataZlecenia = c.DateTime(nullable: false),
                        DataWykonania = c.DateTime(nullable: false),
                        IDTowarGeowlokninaParametryGramatura = c.Int(nullable: false),
                        IDTowarGeowlokninaParametrySurowiec = c.Int(nullable: false),
                        CzyUv = c.Boolean(nullable: false),
                        CzyKalandrowana = c.Boolean(nullable: false),
                        Szerokosc_m = c.Decimal(nullable: false, precision: 8, scale: 2),
                        Dlugosc_m = c.Decimal(nullable: false, precision: 8, scale: 2),
                        IloscRolek = c.Int(nullable: false),
                        Ilosc_m2 = c.Decimal(nullable: false, precision: 10, scale: 2),
                        RodzajPakowania = c.String(),
                        Uwagi = c.String(),
                    })
                .PrimaryKey(t => t.IDProdukcjaZlecenieCiecia)
                .ForeignKey("dbo.tblKontrahent", t => t.IDKontrahent, cascadeDelete: false)
                .ForeignKey("dbo.tblPracownikGAT", t => t.IDWykonujacy, cascadeDelete: false)
                .ForeignKey("dbo.tblPracownikGAT", t => t.IDZlecajacy, cascadeDelete: false)
                .ForeignKey("Towar.tblTowar", t => t.IDTowar, cascadeDelete: false)
                .ForeignKey("Towar.tblTowarGeowlokninaParametryGramatura", t => t.IDTowarGeowlokninaParametryGramatura, cascadeDelete: false)
                .ForeignKey("Towar.tblTowarGeowlokninaParametrySurowiec", t => t.IDTowarGeowlokninaParametrySurowiec, cascadeDelete: false)
                .Index(t => t.IDZlecajacy)
                .Index(t => t.IDWykonujacy)
                .Index(t => t.IDTowar)
                .Index(t => t.IDKontrahent)
                .Index(t => t.IDTowarGeowlokninaParametryGramatura)
                .Index(t => t.IDTowarGeowlokninaParametrySurowiec);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowarGeowlokninaParametrySurowiec", "Towar.tblTowarGeowlokninaParametrySurowiec");
            DropForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowarGeowlokninaParametryGramatura", "Towar.tblTowarGeowlokninaParametryGramatura");
            DropForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowar", "Towar.tblTowar");
            DropForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", "IDZlecajacy", "dbo.tblPracownikGAT");
            DropForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", "IDWykonujacy", "dbo.tblPracownikGAT");
            DropForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", "IDKontrahent", "dbo.tblKontrahent");
            DropIndex("Produkcja.tblProdukcjaZlecenieCiecia", new[] { "IDTowarGeowlokninaParametrySurowiec" });
            DropIndex("Produkcja.tblProdukcjaZlecenieCiecia", new[] { "IDTowarGeowlokninaParametryGramatura" });
            DropIndex("Produkcja.tblProdukcjaZlecenieCiecia", new[] { "IDKontrahent" });
            DropIndex("Produkcja.tblProdukcjaZlecenieCiecia", new[] { "IDTowar" });
            DropIndex("Produkcja.tblProdukcjaZlecenieCiecia", new[] { "IDWykonujacy" });
            DropIndex("Produkcja.tblProdukcjaZlecenieCiecia", new[] { "IDZlecajacy" });
            DropTable("Produkcja.tblProdukcjaZlecenieCiecia");
        }
    }
}
