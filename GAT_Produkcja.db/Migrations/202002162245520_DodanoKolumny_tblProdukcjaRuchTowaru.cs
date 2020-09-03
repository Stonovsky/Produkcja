namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumny_tblProdukcjaRuchTowaru : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieProdukcyjne", "Produkcja.tblProdukcjaZlcecenieProdukcyjne");
            DropIndex("Produkcja.tblProdukcjaRuchNaglowek", new[] { "IDProdukcjaZlecenieProdukcyjne" });
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "Cena_kg", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "Cena_m2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "Wartosc", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieProdukcyjne", c => c.Int());
            CreateIndex("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieProdukcyjne");
            AddForeignKey("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieProdukcyjne", "Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDProdukcjaZlecenieProdukcyjne");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieProdukcyjne", "Produkcja.tblProdukcjaZlcecenieProdukcyjne");
            DropIndex("Produkcja.tblProdukcjaRuchNaglowek", new[] { "IDProdukcjaZlecenieProdukcyjne" });
            AlterColumn("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieProdukcyjne", c => c.Int(nullable: false));
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "Wartosc");
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "Cena_m2");
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "Cena_kg");
            CreateIndex("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieProdukcyjne");
            AddForeignKey("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieProdukcyjne", "Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDProdukcjaZlecenieProdukcyjne", cascadeDelete: true);
        }
    }
}
