namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRuchTowar6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "NrPalety", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "NrPalety");
        }
    }
}
