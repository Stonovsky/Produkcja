namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoAtrybut_IDProdukcjaRuchTowarWyjsciowy_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchTowarWyjsciowy", c => c.Int());
            CreateIndex("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchTowarWyjsciowy");
            AddForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchTowarWyjsciowy", "Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchTowar");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchTowarWyjsciowy", "Produkcja.tblProdukcjaRuchTowar");
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDProdukcjaRuchTowarWyjsciowy" });
            AlterColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchTowarWyjsciowy", c => c.Int(nullable: false));
        }
    }
}
