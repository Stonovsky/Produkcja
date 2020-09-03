namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumne_GramaturaSrednia_tblProdukcjaGniazdoWlokninaBadania : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "GramaturaSrednia", c => c.Decimal(nullable: false, precision: 8, scale: 2));
            DropColumn("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "Gramatura_4");
            DropColumn("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "Gramatura_5");
        }
        
        public override void Down()
        {
            AddColumn("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "Gramatura_5", c => c.Int());
            AddColumn("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "Gramatura_4", c => c.Int());
            DropColumn("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "GramaturaSrednia");
        }
    }
}
