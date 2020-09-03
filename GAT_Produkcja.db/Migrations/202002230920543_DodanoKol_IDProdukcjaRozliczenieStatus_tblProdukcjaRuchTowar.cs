namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_IDProdukcjaRozliczenieStatus_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRozliczenieStatus", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRozliczenieStatus");
            AddForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRozliczenieStatus", "Produkcja.tblProdukcjaRozliczenieStatus", "IDProdukcjaRozliczenieStatus", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRozliczenieStatus", "Produkcja.tblProdukcjaRozliczenieStatus");
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDProdukcjaRozliczenieStatus" });
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRozliczenieStatus");
        }
    }
}
