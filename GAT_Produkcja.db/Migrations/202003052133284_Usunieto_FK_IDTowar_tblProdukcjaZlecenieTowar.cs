namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Usunieto_FK_IDTowar_tblProdukcjaZlecenieTowar : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlecenieTowar", "IDTowar", "Towar.tblTowar");
            DropIndex("Produkcja.tblProdukcjaZlecenieTowar", new[] { "IDTowar" });
        }
        
        public override void Down()
        {
            CreateIndex("Produkcja.tblProdukcjaZlecenieTowar", "IDTowar");
            AddForeignKey("Produkcja.tblProdukcjaZlecenieTowar", "IDTowar", "Towar.tblTowar", "IDTowar", cascadeDelete: true);
        }
    }
}
