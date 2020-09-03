namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KorektaNazwyTabeli_tblProdukcjaZlcecenieProdukcyjne_na_tblProdukcjaZlecenie : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Produkcja.tblProdukcjaZlcecenieProdukcyjne", newName: "tblProdukcjaZlecenie");
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenie", name: "IDProdukcjaZlecenieProdukcyjne", newName: "IDProdukcjaZlecenie");
        }
        
        public override void Down()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenie", name: "IDProdukcjaZlecenie", newName: "IDProdukcjaZlecenieProdukcyjne");
            RenameTable(name: "Produkcja.tblProdukcjaZlecenie", newName: "tblProdukcjaZlcecenieProdukcyjne");
        }
    }
}
