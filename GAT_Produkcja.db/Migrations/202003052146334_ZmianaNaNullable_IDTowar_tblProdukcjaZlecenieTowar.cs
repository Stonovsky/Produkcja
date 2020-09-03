namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNaNullable_IDTowar_tblProdukcjaZlecenieTowar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Produkcja.tblProdukcjaZlecenieTowar", "IDTowar", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("Produkcja.tblProdukcjaZlecenieTowar", "IDTowar", c => c.Int(nullable: false));
        }
    }
}
