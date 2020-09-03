namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRozliczenie_PW1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_PW", "Cena", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRozliczenie_PW", "Cena");
        }
    }
}
