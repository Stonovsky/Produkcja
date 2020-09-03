namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_DatyFaktycze_tblProdukcjaZlcecenieProdukcyjne : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "DataRozpoczeciaFakt", c => c.DateTime());
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "DataZakonczeniaFakt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "DataZakonczeniaFakt");
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "DataRozpoczeciaFakt");
        }
    }
}
