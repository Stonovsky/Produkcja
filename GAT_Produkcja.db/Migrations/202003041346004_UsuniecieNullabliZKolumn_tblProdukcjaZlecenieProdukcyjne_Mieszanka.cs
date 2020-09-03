namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuniecieNullabliZKolumn_tblProdukcjaZlecenieProdukcyjne_Mieszanka : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IloscKg", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IloscKg", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
