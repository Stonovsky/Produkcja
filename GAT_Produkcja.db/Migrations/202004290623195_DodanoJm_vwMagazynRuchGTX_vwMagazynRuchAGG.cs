namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoJm_vwMagazynRuchGTX_vwMagazynRuchAGG : DbMigration
    {
        public override void Up()
        {
            //AddColumn("AGG.vwMagazynRuchAGG", "Jm", c => c.String());
            //AddColumn("GTEX.vwMagazynRuchGTX", "Jm", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("GTEX.vwMagazynRuchGTX", "Jm");
            //DropColumn("AGG.vwMagazynRuchAGG", "Jm");
        }
    }
}
