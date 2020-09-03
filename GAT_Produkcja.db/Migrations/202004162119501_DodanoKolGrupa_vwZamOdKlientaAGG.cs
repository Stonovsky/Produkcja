namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolGrupa_vwZamOdKlientaAGG : DbMigration
    {
        public override void Up()
        {
            //AddColumn("AGG.vwZamOdKlientaAGG", "Grupa", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("AGG.vwZamOdKlientaAGG", "Grupa");
        }
    }
}
