namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_IDProdukcjaZlecenieStatus_tblProdukcjaZlcecenieProdukcyjne : DbMigration
    {
        public override void Up()
        {
            // wykonano na bazie danych z uwagi na OrphanedRecords (rekordy sieroty)
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDProdukcjaZlecenieStatus", "Produkcja.tblProdukcjaZlecenieStatus");
            DropIndex("Produkcja.tblProdukcjaZlcecenieProdukcyjne", new[] { "IDProdukcjaZlecenieStatus" });
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDProdukcjaZlecenieStatus");
        }
    }
}
