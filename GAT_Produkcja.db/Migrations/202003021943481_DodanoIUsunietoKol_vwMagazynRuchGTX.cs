namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoIUsunietoKol_vwMagazynRuchGTX : DbMigration
    {
        public override void Up()
        {
            //Z uwagi na to iz jest to VIEW wszelkie manipulacje sa robione bezposrednio na bazie

            //AddColumn("GTEX.vwMagazynRuchGTX", "MagazynSymbol", c => c.String());
            //DropColumn("GTEX.vwMagazynRuchGTX", "Termin");
        }
        
        public override void Down()
        {
            //AddColumn("GTEX.vwMagazynRuchGTX", "Termin", c => c.DateTime(nullable: false));
            //DropColumn("GTEX.vwMagazynRuchGTX", "MagazynSymbol");
        }
    }
}
