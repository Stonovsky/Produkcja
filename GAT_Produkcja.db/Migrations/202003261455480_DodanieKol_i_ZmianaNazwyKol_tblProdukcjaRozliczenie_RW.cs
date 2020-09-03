namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieKol_i_ZmianaNazwyKol_tblProdukcjaRozliczenie_RW : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_RW", name: "Ilosc_kg", newName: "Ilosc");
            AddColumn("Produkcja.tblProdukcjaRozliczenie_RW", "IDJm", c => c.Int());
            CreateIndex("Produkcja.tblProdukcjaRozliczenie_RW", "IDJm");
            AddForeignKey("Produkcja.tblProdukcjaRozliczenie_RW", "IDJm", "dbo.tblJm", "IDJm");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_RW", "IDJm", "dbo.tblJm");
            DropIndex("Produkcja.tblProdukcjaRozliczenie_RW", new[] { "IDJm" });
            DropColumn("Produkcja.tblProdukcjaRozliczenie_RW", "IDJm");
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_RW", name: "Ilosc", newName: "Ilosc_kg");
        }
    }
}
