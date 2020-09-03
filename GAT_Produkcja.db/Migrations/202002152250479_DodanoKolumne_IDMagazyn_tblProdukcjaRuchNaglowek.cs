namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumne_IDMagazyn_tblProdukcjaRuchNaglowek : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchNaglowek", "IDMagazyn", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaRuchNaglowek", "IDMagazyn");
            AddForeignKey("Produkcja.tblProdukcjaRuchNaglowek", "IDMagazyn", "Magazyn.tblMagazyn", "IDMagazyn", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchNaglowek", "IDMagazyn", "Magazyn.tblMagazyn");
            DropIndex("Produkcja.tblProdukcjaRuchNaglowek", new[] { "IDMagazyn" });
            DropColumn("Produkcja.tblProdukcjaRuchNaglowek", "IDMagazyn");
        }
    }
}
