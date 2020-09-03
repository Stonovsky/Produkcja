namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRuchNaglowek : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieTowar", c => c.Int());
            CreateIndex("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieTowar");
            AddForeignKey("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieTowar", "Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaZlecenieTowar");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieTowar", "Produkcja.tblProdukcjaZlecenieTowar");
            DropIndex("Produkcja.tblProdukcjaRuchNaglowek", new[] { "IDProdukcjaZlecenieTowar" });
            DropColumn("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieTowar");
        }
    }
}
