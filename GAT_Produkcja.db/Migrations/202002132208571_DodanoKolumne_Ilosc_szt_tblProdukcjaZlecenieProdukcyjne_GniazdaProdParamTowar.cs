namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumne_Ilosc_szt_tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "Ilosc_szt", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "Ilosc_szt");
        }
    }
}
