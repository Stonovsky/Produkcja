namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_IDProdukcjaGniazdoProdukcyjne_tblProdukcjaZlecenie : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenie", "IDProdukcjaGniazdoProdukcyjne", c => c.Int());
            CreateIndex("Produkcja.tblProdukcjaZlecenie", "IDProdukcjaGniazdoProdukcyjne");
            AddForeignKey("Produkcja.tblProdukcjaZlecenie", "IDProdukcjaGniazdoProdukcyjne", "Produkcja.tblProdukcjaGniazdoProdukcyjne", "IDProdukcjaGniazdoProdukcyjne");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlecenie", "IDProdukcjaGniazdoProdukcyjne", "Produkcja.tblProdukcjaGniazdoProdukcyjne");
            DropIndex("Produkcja.tblProdukcjaZlecenie", new[] { "IDProdukcjaGniazdoProdukcyjne" });
            DropColumn("Produkcja.tblProdukcjaZlecenie", "IDProdukcjaGniazdoProdukcyjne");
        }
    }
}
