namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_IDProdukcjaZlecenieCieciaTowar_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieCieciaTowar", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieCieciaTowar");
            AddForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieCieciaTowar", "Produkcja.tblProdukcjaZlecenieCieciaTowar", "IDProdukcjaZlecenieCieciaTowar", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieCieciaTowar", "Produkcja.tblProdukcjaZlecenieCieciaTowar");
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDProdukcjaZlecenieCieciaTowar" });
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieCieciaTowar");
        }
    }
}
