namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaGeokomorkaPodsumowaniePrzerob1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaGeokomorkaPodsumowaniePrzerob", "IloscNawrot_kg", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Produkcja.tblProdukcjaGeokomorkaPodsumowaniePrzerob", "Ilosc_m2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaGeokomorkaPodsumowaniePrzerob", "Ilosc_m2");
            DropColumn("Produkcja.tblProdukcjaGeokomorkaPodsumowaniePrzerob", "IloscNawrot_kg");
        }
    }
}
