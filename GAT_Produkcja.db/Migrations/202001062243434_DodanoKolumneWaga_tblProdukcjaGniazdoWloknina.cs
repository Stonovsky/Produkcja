namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumneWaga_tblProdukcjaGniazdoWloknina : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaGniazdoWloknina", "Waga_Kg", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaGniazdoWloknina", "Waga_Kg");
        }
    }
}
