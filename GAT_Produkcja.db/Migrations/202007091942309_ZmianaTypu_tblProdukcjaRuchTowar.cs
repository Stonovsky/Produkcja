namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaTypu_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Produkcja.tblProdukcjaRuchTowar", "NrPalety", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Produkcja.tblProdukcjaRuchTowar", "NrPalety", c => c.String());
        }
    }
}
