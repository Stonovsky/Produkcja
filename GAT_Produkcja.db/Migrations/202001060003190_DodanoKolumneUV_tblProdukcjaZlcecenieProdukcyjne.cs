namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumneUV_tblProdukcjaZlcecenieProdukcyjne : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "CzyUV", c => c.Boolean(nullable: false));
            AlterColumn("Produkcja.tblProdukcjaGniazdoWloknina", "StronaRolkiWyjsciowej", c => c.String(nullable: false));
            AlterColumn("Produkcja.tblProdukcjaGniazdoWloknina", "KodKreskowy", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Produkcja.tblProdukcjaGniazdoWloknina", "KodKreskowy", c => c.String());
            AlterColumn("Produkcja.tblProdukcjaGniazdoWloknina", "StronaRolkiWyjsciowej", c => c.String());
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "CzyUV");
        }
    }
}
