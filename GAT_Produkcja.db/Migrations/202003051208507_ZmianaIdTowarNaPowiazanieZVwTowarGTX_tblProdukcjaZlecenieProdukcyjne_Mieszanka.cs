namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaIdTowarNaPowiazanieZVwTowarGTX_tblProdukcjaZlecenieProdukcyjne_Mieszanka : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IDTowar", "Towar.tblTowar");
            //AddForeignKey("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IDTowar", "GTEX.vwTowarGTX", "IdTowar", cascadeDelete: false);
        }
        
        public override void Down()
        {
            //DropForeignKey("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IDTowar", "GTEX.vwTowarGTX");
            AddForeignKey("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IDTowar", "Towar.tblTowar", "IDTowar", cascadeDelete: false);
        }
    }
}
