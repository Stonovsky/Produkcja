namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuniecieFK_IDTowar_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDTowar", "Towar.tblTowar");
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDTowar" });
        }
        
        public override void Down()
        {
            CreateIndex("Produkcja.tblProdukcjaRuchTowar", "IDTowar");
            AddForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDTowar", "Towar.tblTowar", "IDTowar");
        }
    }
}
