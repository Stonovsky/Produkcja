namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class DodanoAtrybutTabel_vwFinanseBankAGG_GTX_GTX2 : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.vwFinanseBankAGGs", newName: "vwFinanseBankAGG");
            //RenameTable(name: "dbo.vwFinanseBankGTXes", newName: "vwFinanseBankGTX");
            //MoveTable(name: "dbo.vwFinanseBankAGG", newSchema: "Finanse");
            //MoveTable(name: "dbo.vwFinanseBankGTX", newSchema: "Finanse");
            //MoveTable(name: "dbo.vwFinanseBankGTX2", newSchema: "Finanse");
        }

        public override void Down()
        {
            //MoveTable(name: "Finanse.vwFinanseBankGTX2", newSchema: "dbo");
            //MoveTable(name: "Finanse.vwFinanseBankGTX", newSchema: "dbo");
            //MoveTable(name: "Finanse.vwFinanseBankAGG", newSchema: "dbo");
            //RenameTable(name: "dbo.vwFinanseBankGTX", newName: "vwFinanseBankGTXes");
            //RenameTable(name: "dbo.vwFinanseBankAGG", newName: "vwFinanseBankAGGs");
        }
    }
}
