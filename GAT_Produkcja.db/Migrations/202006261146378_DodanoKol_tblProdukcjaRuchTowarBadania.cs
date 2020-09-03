namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRuchTowarBadania : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "IDProdukcjaRuchTowar", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "IDProdukcjaGniazdoProdukcyjne", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "IDProdukcjaRuchTowar");
            CreateIndex("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "IDProdukcjaGniazdoProdukcyjne");
            AddForeignKey("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "IDProdukcjaGniazdoProdukcyjne", "Produkcja.tblProdukcjaGniazdoProdukcyjne", "IDProdukcjaGniazdoProdukcyjne", cascadeDelete: false);
            AddForeignKey("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "IDProdukcjaRuchTowar", "Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchTowar", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "IDProdukcjaRuchTowar", "Produkcja.tblProdukcjaRuchTowar");
            DropForeignKey("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "IDProdukcjaGniazdoProdukcyjne", "Produkcja.tblProdukcjaGniazdoProdukcyjne");
            DropIndex("Produkcja.tblProdukcjaGniazdoWlokninaBadania", new[] { "IDProdukcjaGniazdoProdukcyjne" });
            DropIndex("Produkcja.tblProdukcjaGniazdoWlokninaBadania", new[] { "IDProdukcjaRuchTowar" });
            DropColumn("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "IDProdukcjaGniazdoProdukcyjne");
            DropColumn("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "IDProdukcjaRuchTowar");
        }
    }
}
