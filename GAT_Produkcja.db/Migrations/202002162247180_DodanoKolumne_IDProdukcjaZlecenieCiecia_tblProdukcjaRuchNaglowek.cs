namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumne_IDProdukcjaZlecenieCiecia_tblProdukcjaRuchNaglowek : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieCiecia", c => c.Int());
            CreateIndex("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieCiecia");
            AddForeignKey("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieCiecia", "Produkcja.tblProdukcjaZlecenieCiecia", "IDProdukcjaZlecenieCiecia");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieCiecia", "Produkcja.tblProdukcjaZlecenieCiecia");
            DropIndex("Produkcja.tblProdukcjaRuchNaglowek", new[] { "IDProdukcjaZlecenieCiecia" });
            DropColumn("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieCiecia");
        }
    }
}
