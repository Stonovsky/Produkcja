namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaGeokomorkaPodsumowaniePrzerob : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaGeokomorkaPodsumowaniePrzerob", "DataDodania", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaGeokomorkaPodsumowaniePrzerob", "DataDodania");
        }
    }
}
