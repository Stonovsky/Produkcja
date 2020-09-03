namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_IDProdukcjaZlecenieStatus_tblProdukcjaZlecenieCiecia : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenieCiecia", "IDProdukcjaZlecenieStatus", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaZlecenieCiecia", "IDProdukcjaZlecenieStatus");
            AddForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", "IDProdukcjaZlecenieStatus", "Produkcja.tblProdukcjaZlecenieStatus", "IDProdukcjaZlecenieStatus", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlecenieCiecia", "IDProdukcjaZlecenieStatus", "Produkcja.tblProdukcjaZlecenieStatus");
            DropIndex("Produkcja.tblProdukcjaZlecenieCiecia", new[] { "IDProdukcjaZlecenieStatus" });
            DropColumn("Produkcja.tblProdukcjaZlecenieCiecia", "IDProdukcjaZlecenieStatus");
        }
    }
}
