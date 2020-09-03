namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsunietoKol_tblProdukcjaZlcecenieProdukcyjne : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowar", "Towar.tblTowar");
            DropForeignKey("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowarGeowlokninaParametryGramatura", "Towar.tblTowarGeowlokninaParametryGramatura");
            DropForeignKey("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowarGeowlokninaParametrySurowiec", "Towar.tblTowarGeowlokninaParametrySurowiec");
            //DropForeignKey("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowarGeowlokninaParametryRodzaj", "Towar.tblTowarGeowlokninaParametryRodzaj");
            DropIndex("Produkcja.tblProdukcjaZlcecenieProdukcyjne", new[] { "IDTowar" });
            DropIndex("Produkcja.tblProdukcjaZlcecenieProdukcyjne", new[] { "IDTowarGeowlokninaParametrySurowiec" });
            DropIndex("Produkcja.tblProdukcjaZlcecenieProdukcyjne", new[] { "IDTowarGeowlokninaParametryGramatura" });
            //DropIndex("Produkcja.tblProdukcjaZlcecenieProdukcyjne", new[] { "IDTowarGeowlokninaParametryRodzaj" });
            //RenameColumn(table: "Produkcja.tblProdukcjaZlcecenieProdukcyjne", name: "IDTowarGeowlokninaParametryRodzaj", newName: "tblTowarGeowlokninaParametryRodzaj_IDTowarGeowlokninaParametryRodzaj");
            //RenameIndex(table: "Produkcja.tblProdukcjaZlcecenieProdukcyjne", name: "IX_IDTowarGeowlokninaParametryRodzaj", newName: "IX_tblTowarGeowlokninaParametryRodzaj_IDTowarGeowlokninaParametryRodzaj");
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowar");
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowarGeowlokninaParametrySurowiec");
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowarGeowlokninaParametryGramatura");
            //DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowarGeowlokninaParametryRodzaj");
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IloscKg");
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IloscM2");
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "SzerokoscRolki");
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "DlugoscNawoju");
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "CzyUV");
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "CzyZakonczone");
        }
        
        public override void Down()
        {
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "CzyZakonczone", c => c.Boolean(nullable: false));
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "CzyUV", c => c.Boolean(nullable: false));
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "DlugoscNawoju", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "SzerokoscRolki", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IloscM2", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IloscKg", c => c.Decimal(nullable: false, precision: 19, scale: 4));
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowarGeowlokninaParametryGramatura", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowarGeowlokninaParametrySurowiec", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowar", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowarGeowlokninaParametryGramatura");
            CreateIndex("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowarGeowlokninaParametrySurowiec");
            CreateIndex("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowar");
            AddForeignKey("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowarGeowlokninaParametrySurowiec", "Towar.tblTowarGeowlokninaParametrySurowiec", "IDTowarGeowlokninaParametrySurowiec", cascadeDelete: true);
            AddForeignKey("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowarGeowlokninaParametryGramatura", "Towar.tblTowarGeowlokninaParametryGramatura", "IDTowarGeowlokninaParametryGramatura", cascadeDelete: true);
            AddForeignKey("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDTowar", "Towar.tblTowar", "IDTowar", cascadeDelete: true);
        }
    }
}
