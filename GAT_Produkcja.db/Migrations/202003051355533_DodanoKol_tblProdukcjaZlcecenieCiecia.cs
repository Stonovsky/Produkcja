namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaZlcecenieCiecia : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenieCiecia", "IDProdukcjaRozliczenieStatus", c => c.Int());
            AddColumn("Produkcja.tblProdukcjaZlecenieCiecia", "DataRozliczenia", c => c.DateTime());
            CreateIndex("Produkcja.tblProdukcjaZlecenieCiecia", "IDProdukcjaRozliczenieStatus");
            AddForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", "IDProdukcjaRozliczenieStatus", "Produkcja.tblProdukcjaRozliczenieStatus", "IDProdukcjaRozliczenieStatus");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", "IDProdukcjaRozliczenieStatus", "Produkcja.tblProdukcjaRozliczenieStatus");
            DropIndex("Produkcja.tblProdukcjaZlecenieCiecia", new[] { "IDProdukcjaRozliczenieStatus" });
            DropColumn("Produkcja.tblProdukcjaZlecenieCiecia", "DataRozliczenia");
            DropColumn("Produkcja.tblProdukcjaZlecenieCiecia", "IDProdukcjaRozliczenieStatus");
        }
    }
}
