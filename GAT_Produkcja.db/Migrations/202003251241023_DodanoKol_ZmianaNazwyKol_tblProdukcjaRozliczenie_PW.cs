namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_ZmianaNazwyKol_tblProdukcjaRozliczenie_PW : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_PW", name: "Ilosc_m2", newName: "Ilosc");
            AddColumn("Produkcja.tblProdukcjaRozliczenie_PW", "IDJm", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaRozliczenie_PW", "IDJm");
            AddForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDJm", "dbo.tblJm", "IDJm", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDJm", "dbo.tblJm");
            DropIndex("Produkcja.tblProdukcjaRozliczenie_PW", new[] { "IDJm" });
            DropColumn("Produkcja.tblProdukcjaRozliczenie_PW", "IDJm");
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_PW", name: "Ilosc", newName: "Ilosc_m2");
        }
    }
}
