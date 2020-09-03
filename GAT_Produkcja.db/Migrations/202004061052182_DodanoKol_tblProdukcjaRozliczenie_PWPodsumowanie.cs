namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRozliczenie_PWPodsumowanie : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", "Szerokosc_m", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", "Dlugosc_m", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", "Dlugosc_m");
            DropColumn("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", "Szerokosc_m");
        }
    }
}
