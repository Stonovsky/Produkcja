namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwKol_tblProdukcjaZlecenieProdukcyjne_Mieszanka2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "CenaWgUdzialu_kg", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "CenaWgUdzialu_kg");
        }
    }
}
