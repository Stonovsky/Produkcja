namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwyKol_tblProdukcjaRozliczenie_PW1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_PW", name: "Cena", newName: "CenaJednostkowa");
        }
        
        public override void Down()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_PW", name: "CenaJednostkowa", newName: "Cena");
        }
    }
}
