namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwKolumn_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRuchTowar", name: "IDZlecenieBazowe", newName: "IDZleceniePodstawowe");
            RenameColumn(table: "Produkcja.tblProdukcjaRuchTowar", name: "NrZleceniaBazowego", newName: "NrZleceniaPodstawowego");
        }
        
        public override void Down()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRuchTowar", name: "NrZleceniaPodstawowego", newName: "NrZleceniaBazowego");
            RenameColumn(table: "Produkcja.tblProdukcjaRuchTowar", name: "IDZleceniePodstawowe", newName: "IDZlecenieBazowe");
        }
    }
}
