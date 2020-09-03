namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNaTypNullable_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchNaglowek", "Produkcja.tblProdukcjaRuchNaglowek");
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieTowar", "Produkcja.tblProdukcjaZlecenieTowar");
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDProdukcjaRuchNaglowek" });
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDProdukcjaZlecenieTowar" });
            AlterColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchNaglowek", c => c.Int());
            AlterColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieTowar", c => c.Int());
            CreateIndex("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchNaglowek");
            CreateIndex("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieTowar");
            AddForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchNaglowek", "Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaRuchNaglowek");
            AddForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieTowar", "Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaZlecenieTowar");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieTowar", "Produkcja.tblProdukcjaZlecenieTowar");
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchNaglowek", "Produkcja.tblProdukcjaRuchNaglowek");
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDProdukcjaZlecenieTowar" });
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDProdukcjaRuchNaglowek" });
            AlterColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieTowar", c => c.Int(nullable: false));
            AlterColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchNaglowek", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieTowar");
            CreateIndex("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchNaglowek");
            AddForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieTowar", "Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaZlecenieTowar", cascadeDelete: true);
            AddForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaRuchNaglowek", "Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaRuchNaglowek", cascadeDelete: true);
        }
    }
}
