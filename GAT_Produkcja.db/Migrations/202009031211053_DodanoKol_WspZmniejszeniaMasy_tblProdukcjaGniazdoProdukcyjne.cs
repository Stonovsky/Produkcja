namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_WspZmniejszeniaMasy_tblProdukcjaGniazdoProdukcyjne : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaGniazdoProdukcyjne", "WspZmniejszeniaMasy", c => c.Decimal(nullable: false, precision: 6, scale: 4));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaGniazdoProdukcyjne", "WspZmniejszeniaMasy");
        }
    }
}
