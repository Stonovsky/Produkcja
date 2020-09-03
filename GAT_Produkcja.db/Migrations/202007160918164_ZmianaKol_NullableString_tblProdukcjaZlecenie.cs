namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaKol_NullableString_tblProdukcjaZlecenie : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Produkcja.tblProdukcjaZlecenie", "NazwaZlecenia", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("Produkcja.tblProdukcjaZlecenie", "NazwaZlecenia", c => c.String(nullable: false));
        }
    }
}
