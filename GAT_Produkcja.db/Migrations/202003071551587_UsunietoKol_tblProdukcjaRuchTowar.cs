namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsunietoKol_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "NrZleceniaPW");
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "NrZleceniaProdukcjnego");
        }
        
        public override void Down()
        {
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "NrZleceniaProdukcjnego", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "NrZleceniaPW", c => c.Int(nullable: false));
        }
    }
}
