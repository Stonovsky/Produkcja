namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsunietoKolumny_tblProdukcjaRozliczenie_RW : DbMigration
    {
        public override void Up()
        {
            DropColumn("Produkcja.tblProdukcjaRozliczenie_RW", "IDSurowiec");
            DropColumn("Produkcja.tblProdukcjaRozliczenie_RW", "IDSurowcaComarch");
        }
        
        public override void Down()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_RW", "IDSurowcaComarch", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaRozliczenie_RW", "IDSurowiec", c => c.Int(nullable: false));
        }
    }
}
