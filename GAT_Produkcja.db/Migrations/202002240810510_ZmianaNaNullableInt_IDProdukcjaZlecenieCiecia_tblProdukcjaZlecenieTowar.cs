namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNaNullableInt_IDProdukcjaZlecenieCiecia_tblProdukcjaZlecenieTowar : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaZlecenieCiecia", "Produkcja.tblProdukcjaZlecenieCiecia");
            DropIndex("Produkcja.tblProdukcjaZlecenieTowar", new[] { "IDProdukcjaZlecenieCiecia" });
            AlterColumn("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaZlecenieCiecia", c => c.Int());
            CreateIndex("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaZlecenieCiecia");
            AddForeignKey("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaZlecenieCiecia", "Produkcja.tblProdukcjaZlecenieCiecia", "IDProdukcjaZlecenieCiecia");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaZlecenieCiecia", "Produkcja.tblProdukcjaZlecenieCiecia");
            DropIndex("Produkcja.tblProdukcjaZlecenieTowar", new[] { "IDProdukcjaZlecenieCiecia" });
            AlterColumn("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaZlecenieCiecia", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaZlecenieCiecia");
            AddForeignKey("Produkcja.tblProdukcjaZlecenieTowar", "IDProdukcjaZlecenieCiecia", "Produkcja.tblProdukcjaZlecenieCiecia", "IDProdukcjaZlecenieCiecia", cascadeDelete: true);
        }
    }
}
