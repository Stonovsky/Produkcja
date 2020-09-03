namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsunietoKolumny_tblProdukcjaZlecenieCiecia : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowar", "Towar.tblTowar");
            DropForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowarGeowlokninaParametryGramatura", "Towar.tblTowarGeowlokninaParametryGramatura");
            DropForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowarGeowlokninaParametrySurowiec", "Towar.tblTowarGeowlokninaParametrySurowiec");
            DropIndex("Produkcja.tblProdukcjaZlecenieCiecia", new[] { "IDTowar" });
            DropIndex("Produkcja.tblProdukcjaZlecenieCiecia", new[] { "IDTowarGeowlokninaParametryGramatura" });
            DropIndex("Produkcja.tblProdukcjaZlecenieCiecia", new[] { "IDTowarGeowlokninaParametrySurowiec" });
            DropColumn("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowar");
            DropColumn("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowarGeowlokninaParametryGramatura");
            DropColumn("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowarGeowlokninaParametrySurowiec");
            DropColumn("Produkcja.tblProdukcjaZlecenieCiecia", "CzyUv");
            DropColumn("Produkcja.tblProdukcjaZlecenieCiecia", "CzyKalandrowana");
            DropColumn("Produkcja.tblProdukcjaZlecenieCiecia", "Szerokosc_m");
            DropColumn("Produkcja.tblProdukcjaZlecenieCiecia", "Dlugosc_m");
            DropColumn("Produkcja.tblProdukcjaZlecenieCiecia", "IloscRolek");
            DropColumn("Produkcja.tblProdukcjaZlecenieCiecia", "Ilosc_m2");
        }
        
        public override void Down()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenieCiecia", "Ilosc_m2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Produkcja.tblProdukcjaZlecenieCiecia", "IloscRolek", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaZlecenieCiecia", "Dlugosc_m", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Produkcja.tblProdukcjaZlecenieCiecia", "Szerokosc_m", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Produkcja.tblProdukcjaZlecenieCiecia", "CzyKalandrowana", c => c.Boolean(nullable: false));
            AddColumn("Produkcja.tblProdukcjaZlecenieCiecia", "CzyUv", c => c.Boolean(nullable: false));
            AddColumn("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowarGeowlokninaParametrySurowiec", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowarGeowlokninaParametryGramatura", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowar", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowarGeowlokninaParametrySurowiec");
            CreateIndex("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowarGeowlokninaParametryGramatura");
            CreateIndex("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowar");
            AddForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowarGeowlokninaParametrySurowiec", "Towar.tblTowarGeowlokninaParametrySurowiec", "IDTowarGeowlokninaParametrySurowiec", cascadeDelete: true);
            AddForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowarGeowlokninaParametryGramatura", "Towar.tblTowarGeowlokninaParametryGramatura", "IDTowarGeowlokninaParametryGramatura", cascadeDelete: true);
            AddForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", "IDTowar", "Towar.tblTowar", "IDTowar", cascadeDelete: true);
        }
    }
}
