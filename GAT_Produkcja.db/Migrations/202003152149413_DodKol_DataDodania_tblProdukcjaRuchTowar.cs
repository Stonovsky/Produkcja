namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodKol_DataDodania_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "DataDodania", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "DataDodania");
        }
    }
}
