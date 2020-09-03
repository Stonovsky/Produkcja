namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaZlecenieTowar1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenieTowar", "CzyWielokrotnoscDlugosci", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaZlecenieTowar", "CzyWielokrotnoscDlugosci");
        }
    }
}
