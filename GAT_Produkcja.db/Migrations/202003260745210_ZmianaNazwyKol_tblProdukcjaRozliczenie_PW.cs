namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwyKol_tblProdukcjaRozliczenie_PW : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_PW", name: "IdZlecenia", newName: "IDZlecenie");
        }
        
        public override void Down()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_PW", name: "IDZlecenie", newName: "IdZlecenia");
        }
    }
}
