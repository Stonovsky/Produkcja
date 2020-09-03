namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblFinanseStanKonta2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Finanse.tblFinanseStanKonta", "DataStanu", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Finanse.tblFinanseStanKonta", "DataStanu");
        }
    }
}
