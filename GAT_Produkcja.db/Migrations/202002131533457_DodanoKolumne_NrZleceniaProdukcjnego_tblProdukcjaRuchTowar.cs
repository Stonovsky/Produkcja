namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumne_NrZleceniaProdukcjnego_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "NrZleceniaProdukcjnego", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "NrZleceniaProdukcjnego");
        }
    }
}
