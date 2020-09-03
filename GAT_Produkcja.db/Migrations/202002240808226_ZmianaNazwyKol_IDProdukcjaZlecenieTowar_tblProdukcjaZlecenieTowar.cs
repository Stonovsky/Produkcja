namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwyKol_IDProdukcjaZlecenieTowar_tblProdukcjaZlecenieTowar : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenieTowar", name: "IDProdukcjaZlecenieCieciaTowar", newName: "IDProdukcjaZlecenieTowar");
        }
        
        public override void Down()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenieTowar", name: "IDProdukcjaZlecenieTowar", newName: "IDProdukcjaZlecenieCieciaTowar");
        }
    }
}
