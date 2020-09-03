namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwyKol_IDProdukcjaRozliczenie_PWPodsumowanie : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", name: "Waga_kg", newName: "Ilosc_kg");
        }
        
        public override void Down()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", name: "Ilosc_kg", newName: "Waga_kg");
        }
    }
}
