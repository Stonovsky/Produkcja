namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaZlcecenieProdukcyjne1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDProdukcjaRozliczenieStatus", c => c.Int());
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "DataRozliczenia", c => c.DateTime());
            CreateIndex("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDProdukcjaRozliczenieStatus");
            AddForeignKey("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDProdukcjaRozliczenieStatus", "Produkcja.tblProdukcjaRozliczenieStatus", "IDProdukcjaRozliczenieStatus");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDProdukcjaRozliczenieStatus", "Produkcja.tblProdukcjaRozliczenieStatus");
            DropIndex("Produkcja.tblProdukcjaZlcecenieProdukcyjne", new[] { "IDProdukcjaRozliczenieStatus" });
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "DataRozliczenia");
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDProdukcjaRozliczenieStatus");
        }
    }
}
