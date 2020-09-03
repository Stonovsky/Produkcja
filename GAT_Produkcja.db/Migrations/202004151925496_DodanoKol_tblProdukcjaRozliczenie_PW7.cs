namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRozliczenie_PW7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_PW", "SymbolRolkiBazowej", c => c.String(maxLength: 20));
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "NazwaRolkiBazowej", c => c.String());
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "SymbolRolkiBazowej", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "SymbolRolkiBazowej");
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "NazwaRolkiBazowej");
            DropColumn("Produkcja.tblProdukcjaRozliczenie_PW", "SymbolRolkiBazowej");
        }
    }
}
