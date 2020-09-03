namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwyKol_tblProdukcjaZlecenieTowar : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenieTowar", name: "IDProdukcjaZlecenieProdukcyjne", newName: "IDProdukcjaZlecenie");
            RenameIndex(table: "Produkcja.tblProdukcjaZlecenieTowar", name: "IX_IDProdukcjaZlecenieProdukcyjne", newName: "IX_IDProdukcjaZlecenie");
        }
        
        public override void Down()
        {
            RenameIndex(table: "Produkcja.tblProdukcjaZlecenieTowar", name: "IX_IDProdukcjaZlecenie", newName: "IX_IDProdukcjaZlecenieProdukcyjne");
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenieTowar", name: "IDProdukcjaZlecenie", newName: "IDProdukcjaZlecenieProdukcyjne");
        }
    }
}
