namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumneNrRolki_tblRuchTowar : DbMigration
    {
        public override void Up()
        {
            AddColumn("Magazyn.tblRuchTowar", "NrRolki", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Magazyn.tblRuchTowar", "NrRolki");
        }
    }
}
