namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoColumne_Uwagi_tblProdukcjaGniazdoWlokninaBadania : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "Uwagi", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "Uwagi");
        }
    }
}
