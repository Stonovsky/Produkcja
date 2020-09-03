namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_IdMsAccess_tblProdukcjaZlecenieProdukcyjne_Mieszanka : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IDMsAccess", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IDMsAccess");
        }
    }
}
