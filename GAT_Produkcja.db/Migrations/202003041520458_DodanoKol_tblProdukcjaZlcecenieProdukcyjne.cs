namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaZlcecenieProdukcyjne : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "WartoscMieszanki_zl", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "WartoscMieszanki_zl_kg", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "UdzialSurowcowWMieszance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "UdzialSurowcowWMieszance");
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "WartoscMieszanki_zl_kg");
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "WartoscMieszanki_zl");
        }
    }
}
