namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "NrRolkiPelny", c => c.String());
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "NrRolkiBazowej", c => c.String());
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "KodKreskowyRolkiBazowej", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "KodKreskowyRolkiBazowej");
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "NrRolkiBazowej");
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "NrRolkiPelny");
        }
    }
}
