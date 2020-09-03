namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsunietoKolumne_tblProdukcjaRozliczenie_RW : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_RW", "IDPracownikGAT", "dbo.tblPracownikGAT");
            DropIndex("Produkcja.tblProdukcjaRozliczenie_RW", new[] { "IDPracownikGAT" });
            DropColumn("Produkcja.tblProdukcjaRozliczenie_RW", "IDPracownikGAT");
        }
        
        public override void Down()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_RW", "IDPracownikGAT", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaRozliczenie_RW", "IDPracownikGAT");
            AddForeignKey("Produkcja.tblProdukcjaRozliczenie_RW", "IDPracownikGAT", "dbo.tblPracownikGAT", "ID_PracownikGAT", cascadeDelete: true);
        }
    }
}
