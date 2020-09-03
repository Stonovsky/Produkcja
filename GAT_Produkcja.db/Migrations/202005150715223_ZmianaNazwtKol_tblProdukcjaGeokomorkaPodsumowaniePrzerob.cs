namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwtKol_tblProdukcjaGeokomorkaPodsumowaniePrzerob : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaGeokomorkaPodsumowaniePrzerob", name: "IdProdukcjaGeokomorkaPodsumowaniePrzerobrodukcjaGeokomorkaPodsumowanie", newName: "IdProdukcjaGeokomorkaPodsumowaniePrzerob");
        }
        
        public override void Down()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaGeokomorkaPodsumowaniePrzerob", name: "IdProdukcjaGeokomorkaPodsumowaniePrzerob", newName: "IdProdukcjaGeokomorkaPodsumowaniePrzerobrodukcjaGeokomorkaPodsumowanie");
        }
    }
}
