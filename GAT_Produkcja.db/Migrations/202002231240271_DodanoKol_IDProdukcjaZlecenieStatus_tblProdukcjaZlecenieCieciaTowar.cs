namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_IDProdukcjaZlecenieStatus_tblProdukcjaZlecenieCieciaTowar : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenieCieciaTowar", "IDProdukcjaZlecenieStatus", c => c.Int());
            CreateIndex("Produkcja.tblProdukcjaZlecenieCieciaTowar", "IDProdukcjaZlecenieStatus");
            AddForeignKey("Produkcja.tblProdukcjaZlecenieCieciaTowar", "IDProdukcjaZlecenieStatus", "Produkcja.tblProdukcjaZlecenieStatus", "IDProdukcjaZlecenieStatus");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlecenieCieciaTowar", "IDProdukcjaZlecenieStatus", "Produkcja.tblProdukcjaZlecenieStatus");
            DropIndex("Produkcja.tblProdukcjaZlecenieCieciaTowar", new[] { "IDProdukcjaZlecenieStatus" });
            DropColumn("Produkcja.tblProdukcjaZlecenieCieciaTowar", "IDProdukcjaZlecenieStatus");
        }
    }
}
