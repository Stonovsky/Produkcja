namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmienionoNazweKolumny_NrZlecenia_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            RenameColumn("Produkcja.tblProdukcjaRuchTowar", "NrZleceniaRW", "NrZlecenia");
        }
        
        public override void Down()
        {
            RenameColumn("Produkcja.tblProdukcjaRuchTowar", "NrZlecenia", "NrZleceniaRW");
        }
    }
}
