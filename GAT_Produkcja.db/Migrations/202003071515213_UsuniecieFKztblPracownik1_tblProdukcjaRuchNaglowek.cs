namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuniecieFKztblPracownik1_tblProdukcjaRuchNaglowek : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchNaglowek", "IDPracownikGAT1", "dbo.tblPracownikGAT");
            DropIndex("Produkcja.tblProdukcjaRuchNaglowek", new[] { "IDPracownikGAT1" });
            AlterColumn("Produkcja.tblProdukcjaRuchNaglowek", "IDPracownikGAT1", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("Produkcja.tblProdukcjaRuchNaglowek", "IDPracownikGAT1", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaRuchNaglowek", "IDPracownikGAT1");
            AddForeignKey("Produkcja.tblProdukcjaRuchNaglowek", "IDPracownikGAT1", "dbo.tblPracownikGAT", "ID_PracownikGAT", cascadeDelete: true);
        }
    }
}
