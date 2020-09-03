namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblFinanseStanKonta : DbMigration
    {
        public override void Up()
        {
            AddColumn("Finanse.tblFinanseStanKonta", "Waluta", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("Finanse.tblFinanseStanKonta", "Waluta");
        }
    }
}
