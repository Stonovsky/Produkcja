namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumny_tblProdukcjaZlcecenieProdukcyjne : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "NrDokumentu", c => c.String(maxLength:20));
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "DataUtworzenia", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "DataUtworzenia");
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "NrDokumentu");
        }
    }
}
