namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRuchTowar1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaGniazdoProdukcyjne", c => c.Int());
            CreateIndex("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaGniazdoProdukcyjne");
            AddForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaGniazdoProdukcyjne", "Produkcja.tblProdukcjaGniazdoProdukcyjne", "IDProdukcjaGniazdoProdukcyjne");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaGniazdoProdukcyjne", "Produkcja.tblProdukcjaGniazdoProdukcyjne");
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDProdukcjaGniazdoProdukcyjne" });
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaGniazdoProdukcyjne");
        }
    }
}
