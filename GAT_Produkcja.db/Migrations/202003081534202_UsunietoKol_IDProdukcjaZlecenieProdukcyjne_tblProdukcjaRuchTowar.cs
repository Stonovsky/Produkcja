namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsunietoKol_IDProdukcjaZlecenieProdukcyjne_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieProdukcyjne", "Produkcja.tblProdukcjaZlcecenieProdukcyjne");
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDProdukcjaZlecenieProdukcyjne" });
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieProdukcyjne");
        }
        
        public override void Down()
        {
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieProdukcyjne", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieProdukcyjne");
            AddForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieProdukcyjne", "Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDProdukcjaZlecenieProdukcyjne", cascadeDelete: true);
        }
    }
}
