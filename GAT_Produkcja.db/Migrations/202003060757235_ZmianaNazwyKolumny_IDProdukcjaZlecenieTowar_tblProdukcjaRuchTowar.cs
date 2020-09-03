namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwyKolumny_IDProdukcjaZlecenieTowar_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRuchTowar", name: "IDProdukcjaZlecenieCieciaTowar", newName: "IDProdukcjaZlecenieTowar");
            RenameIndex(table: "Produkcja.tblProdukcjaRuchTowar", name: "IX_IDProdukcjaZlecenieCieciaTowar", newName: "IX_IDProdukcjaZlecenieTowar");
        }
        
        public override void Down()
        {
            RenameIndex(table: "Produkcja.tblProdukcjaRuchTowar", name: "IX_IDProdukcjaZlecenieTowar", newName: "IX_IDProdukcjaZlecenieCieciaTowar");
            RenameColumn(table: "Produkcja.tblProdukcjaRuchTowar", name: "IDProdukcjaZlecenieTowar", newName: "IDProdukcjaZlecenieCieciaTowar");
        }
    }
}
