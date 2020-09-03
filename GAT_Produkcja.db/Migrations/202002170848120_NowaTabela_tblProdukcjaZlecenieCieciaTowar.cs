namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NowaTabela_tblProdukcjaZlecenieCieciaTowar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Produkcja.tblProdukcjaZlecenieCieciaTowar",
                c => new
                    {
                        IDProdukcjaZlecenieCieciaTowar = c.Int(nullable: false, identity: true),
                        IDProdukcjaZlecenieCiecia = c.Int(nullable: false),
                        IDTowar = c.Int(nullable: false),
                        IDTowarGeowlokninaParametryGramatura = c.Int(nullable: false),
                        IDTowarGeowlokninaParametrySurowiec = c.Int(nullable: false),
                        CzyUv = c.Boolean(nullable: false),
                        CzyKalandrowana = c.Boolean(nullable: false),
                        Szerokosc_m = c.Decimal(nullable: false, precision: 8, scale: 2),
                        Dlugosc_m = c.Decimal(nullable: false, precision: 8, scale: 2),
                        IloscRolek = c.Int(nullable: false),
                        Ilosc_m2 = c.Decimal(nullable: false, precision: 12, scale: 2),
                        RodzajPakowania = c.String(),
                        Uwagi = c.String(),
                    })
                .PrimaryKey(t => t.IDProdukcjaZlecenieCieciaTowar)
                .ForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", t => t.IDProdukcjaZlecenieCiecia, cascadeDelete: false)
                .ForeignKey("Towar.tblTowar", t => t.IDTowar, cascadeDelete: false)
                .ForeignKey("Towar.tblTowarGeowlokninaParametryGramatura", t => t.IDTowarGeowlokninaParametryGramatura, cascadeDelete: false)
                .ForeignKey("Towar.tblTowarGeowlokninaParametrySurowiec", t => t.IDTowarGeowlokninaParametrySurowiec, cascadeDelete: false)
                .Index(t => t.IDProdukcjaZlecenieCiecia)
                .Index(t => t.IDTowar)
                .Index(t => t.IDTowarGeowlokninaParametryGramatura)
                .Index(t => t.IDTowarGeowlokninaParametrySurowiec);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlecenieCieciaTowar", "IDTowarGeowlokninaParametrySurowiec", "Towar.tblTowarGeowlokninaParametrySurowiec");
            DropForeignKey("Produkcja.tblProdukcjaZlecenieCieciaTowar", "IDTowarGeowlokninaParametryGramatura", "Towar.tblTowarGeowlokninaParametryGramatura");
            DropForeignKey("Produkcja.tblProdukcjaZlecenieCieciaTowar", "IDTowar", "Towar.tblTowar");
            DropForeignKey("Produkcja.tblProdukcjaZlecenieCieciaTowar", "IDProdukcjaZlecenieCiecia", "Produkcja.tblProdukcjaZlecenieCiecia");
            DropIndex("Produkcja.tblProdukcjaZlecenieCieciaTowar", new[] { "IDTowarGeowlokninaParametrySurowiec" });
            DropIndex("Produkcja.tblProdukcjaZlecenieCieciaTowar", new[] { "IDTowarGeowlokninaParametryGramatura" });
            DropIndex("Produkcja.tblProdukcjaZlecenieCieciaTowar", new[] { "IDTowar" });
            DropIndex("Produkcja.tblProdukcjaZlecenieCieciaTowar", new[] { "IDProdukcjaZlecenieCiecia" });
            DropTable("Produkcja.tblProdukcjaZlecenieCieciaTowar");
        }
    }
}
