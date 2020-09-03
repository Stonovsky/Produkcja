namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwKolumn_tblProdukcjaRozliczenie_PW : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_PW", name: "Waga_kg", newName: "Ilosc_kg");
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_PW", name: "WagaOdpadu_kg", newName: "Odpad_kg");
        }
        
        public override void Down()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_PW", name: "Odpad_kg", newName: "WagaOdpadu_kg");
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_PW", name: "Ilosc_kg", newName: "Waga_kg");
        }
    }
}
