namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_IDProdukcjaRuchTowarStatus_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchTowarStatus", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchTowarStatus");
            AddForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchTowarStatus", "Produkcja.tblProdukcjaRuchTowarStatus", "IDProdukcjaRuchTowarStatus", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchTowarStatus", "Produkcja.tblProdukcjaRuchTowarStatus");
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDProdukcjaRuchTowarStatus" });
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchTowarStatus");
        }
    }
}
