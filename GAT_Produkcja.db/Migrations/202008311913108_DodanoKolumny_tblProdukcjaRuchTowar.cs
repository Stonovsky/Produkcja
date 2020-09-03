namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumny_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "IDZlecenieBazowe", c => c.Int());
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "NrZleceniaBazowego", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "NrZleceniaBazowego");
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "IDZlecenieBazowe");
        }
    }
}
