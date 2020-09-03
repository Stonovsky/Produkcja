namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Zmiany_vwZestSprzedazyAGG : DbMigration
    {
        public override void Up()
        {
            //DropPrimaryKey("AGG.vwZestSprzedazyAGG");
            //AlterColumn("AGG.vwZestSprzedazyAGG", "Id", c => c.Long(nullable: false, identity: true));
            //AlterColumn("AGG.vwZestSprzedazyAGG", "IdFirma", c => c.String());
            //AddPrimaryKey("AGG.vwZestSprzedazyAGG", "Id");
        }
        
        public override void Down()
        {
            //DropPrimaryKey("AGG.vwZestSprzedazyAGG");
            //AlterColumn("AGG.vwZestSprzedazyAGG", "IdFirma", c => c.Int(nullable: false));
            //AlterColumn("AGG.vwZestSprzedazyAGG", "Id", c => c.Int(nullable: false, identity: true));
            //AddPrimaryKey("AGG.vwZestSprzedazyAGG", "Id");
        }
    }
}
