namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumny_tblProdukcjaZlecenieTowar : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaZlecenieProdukcyjne", c => c.Int());
            AddColumn("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaGniazdoProdukcyjne", c => c.Int());
            AddColumn("Produkcja.tblProdukcjaZlecenieTowar", "Ilosc_kg", c => c.Decimal(precision: 18, scale: 2));
            CreateIndex("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaZlecenieProdukcyjne");
            CreateIndex("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaGniazdoProdukcyjne");
            AddForeignKey("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaGniazdoProdukcyjne", "Produkcja.tblProdukcjaGniazdoProdukcyjne", "IDProdukcjaGniazdoProdukcyjne");
            AddForeignKey("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaZlecenieProdukcyjne", "Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDProdukcjaZlecenieProdukcyjne");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaZlecenieProdukcyjne", "Produkcja.tblProdukcjaZlcecenieProdukcyjne");
            DropForeignKey("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaGniazdoProdukcyjne", "Produkcja.tblProdukcjaGniazdoProdukcyjne");
            DropIndex("Produkcja.tblProdukcjaZlecenieTowar", new[] { "IDProdukcjaGniazdoProdukcyjne" });
            DropIndex("Produkcja.tblProdukcjaZlecenieTowar", new[] { "IDProdukcjaZlecenieProdukcyjne" });
            DropColumn("Produkcja.tblProdukcjaZlecenieTowar", "Ilosc_kg");
            DropColumn("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaGniazdoProdukcyjne");
            DropColumn("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaZlecenieProdukcyjne");
        }
    }
}
