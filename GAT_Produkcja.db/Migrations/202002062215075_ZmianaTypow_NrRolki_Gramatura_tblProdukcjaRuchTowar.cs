namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaTypow_NrRolki_Gramatura_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Produkcja.tblProdukcjaRuchTowar", "Gramatura", c => c.Int(nullable: false));
            AlterColumn("Produkcja.tblProdukcjaRuchTowar", "NrRolki", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Produkcja.tblProdukcjaRuchTowar", "NrRolki", c => c.String());
            AlterColumn("Produkcja.tblProdukcjaRuchTowar", "Gramatura", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
