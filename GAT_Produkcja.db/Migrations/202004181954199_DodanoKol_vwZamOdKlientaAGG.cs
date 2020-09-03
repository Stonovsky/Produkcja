namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_vwZamOdKlientaAGG : DbMigration
    {
        public override void Up()
        {
            //AddColumn("AGG.vwZamOdKlientaAGG", "Jm", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("AGG.vwZamOdKlientaAGG", "Jm");
        }
    }
}
