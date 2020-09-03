namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoAtrybutTabel_vwFinanseBankAGG_GTX_GTX21 : DbMigration
    {
        public override void Up()
        {
            //MoveTable(name: "Finanse.vwFinanseBankAGG", newSchema: "AGG");
            //MoveTable(name: "Finanse.vwFinanseBankGTX", newSchema: "GTEX");
            //MoveTable(name: "Finanse.vwFinanseBankGTX2", newSchema: "GTEX2");
        }
        
        public override void Down()
        {
            //MoveTable(name: "GTEX2.vwFinanseBankGTX2", newSchema: "Finanse");
            //MoveTable(name: "GTEX.vwFinanseBankGTX", newSchema: "Finanse");
            //MoveTable(name: "AGG.vwFinanseBankAGG", newSchema: "Finanse");
        }
    }
}
