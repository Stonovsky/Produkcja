namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumne_CzyZweryfikowano_tblZapotrzebowanie : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblZapotrzebowanie", "CzyZweryfikowano", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblZapotrzebowanie", "CzyZweryfikowano");
        }
    }
}
