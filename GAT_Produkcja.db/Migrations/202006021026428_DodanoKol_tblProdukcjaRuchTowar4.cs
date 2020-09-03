namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRuchTowar4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "IDMsAccess", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "IDMsAccess");
        }
    }
}
