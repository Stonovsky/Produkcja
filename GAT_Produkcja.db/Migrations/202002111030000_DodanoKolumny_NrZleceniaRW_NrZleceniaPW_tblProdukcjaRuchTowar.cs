namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumny_NrZleceniaRW_NrZleceniaPW_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "NrZleceniaRW", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "NrZleceniaPW", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "NrZleceniaPW");
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "NrZleceniaRW");
        }
    }
}
