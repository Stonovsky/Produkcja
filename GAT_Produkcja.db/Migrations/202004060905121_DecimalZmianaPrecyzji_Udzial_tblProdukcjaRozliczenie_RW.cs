namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DecimalZmianaPrecyzji_Udzial_tblProdukcjaRozliczenie_RW : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Produkcja.tblProdukcjaRozliczenie_RW", "Udzial", c => c.Decimal(nullable: false, precision: 18, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("Produkcja.tblProdukcjaRozliczenie_RW", "Udzial", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
