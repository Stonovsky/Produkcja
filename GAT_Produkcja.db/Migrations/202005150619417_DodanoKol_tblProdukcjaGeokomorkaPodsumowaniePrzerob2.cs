namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaGeokomorkaPodsumowaniePrzerob2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaGeokomorkaPodsumowaniePrzerob", "IloscWyprodukowana_kg", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaGeokomorkaPodsumowaniePrzerob", "IloscWyprodukowana_kg");
        }
    }
}
