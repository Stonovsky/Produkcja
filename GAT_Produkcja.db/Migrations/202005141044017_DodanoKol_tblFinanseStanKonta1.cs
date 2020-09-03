namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblFinanseStanKonta1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Finanse.tblFinanseStanKonta", "Firma", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("Finanse.tblFinanseStanKonta", "Firma");
        }
    }
}
