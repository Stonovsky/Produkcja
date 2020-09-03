namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwyTabeli_tblProdukcjaZlecenieCieciaTowar_tblProdukcjaZlecenieTowar : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Produkcja.tblProdukcjaZlecenieCieciaTowar", newName: "tblProdukcjaZlecenieTowar");
        }
        
        public override void Down()
        {
            RenameTable(name: "Produkcja.tblProdukcjaZlecenieTowar", newName: "tblProdukcjaZlecenieCieciaTowar");
        }
    }
}
