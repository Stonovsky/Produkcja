namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRozliczenie_Naglowek : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_Naglowek", "IDZlecenie", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaRozliczenie_Naglowek", "NrZlecenia", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRozliczenie_Naglowek", "NrZlecenia");
            DropColumn("Produkcja.tblProdukcjaRozliczenie_Naglowek", "IDZlecenie");
        }
    }
}
