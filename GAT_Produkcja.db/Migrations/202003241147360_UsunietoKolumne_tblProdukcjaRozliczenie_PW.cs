namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsunietoKolumne_tblProdukcjaRozliczenie_PW : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDPracownikGAT", "dbo.tblPracownikGAT");
            DropIndex("Produkcja.tblProdukcjaRozliczenie_PW", new[] { "IDPracownikGAT" });
            DropColumn("Produkcja.tblProdukcjaRozliczenie_PW", "IDPracownikGAT");
        }
        
        public override void Down()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_PW", "IDPracownikGAT", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaRozliczenie_PW", "IDPracownikGAT");
            AddForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDPracownikGAT", "dbo.tblPracownikGAT", "ID_PracownikGAT", cascadeDelete: true);
        }
    }
}
