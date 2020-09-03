namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaKol_DopuszczenieNull_tblProdukcjaZlecenieProdukcyjne_Mieszanka : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IDTowar", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IDTowar", c => c.Int(nullable: false));
        }
    }
}
