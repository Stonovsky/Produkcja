namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_Zaawansowanie_tblProdukcjaZlecenieCiecia : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenieCiecia", "Zaawansowanie", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaZlecenieCiecia", "Zaawansowanie");
        }
    }
}
