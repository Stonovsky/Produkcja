namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwyKol_tblProdukcjaZlcecenieProdukcyjne : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaZlcecenieProdukcyjne", name: "WartoscMieszanki_zl_kg", newName: "CenaMieszanki_zl");
        }
        
        public override void Down()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaZlcecenieProdukcyjne", name: "CenaMieszanki_zl", newName: "WartoscMieszanki_zl_kg");
        }
    }
}
