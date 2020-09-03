namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoZmianaNazwyKol_tblProdukcjaZlecenieTowar : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenieTowar", name: "IloscRolek", newName: "Ilosc_szt");
        }
        
        public override void Down()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenieTowar", name: "Ilosc_szt", newName: "IloscRolek");
        }
    }
}
