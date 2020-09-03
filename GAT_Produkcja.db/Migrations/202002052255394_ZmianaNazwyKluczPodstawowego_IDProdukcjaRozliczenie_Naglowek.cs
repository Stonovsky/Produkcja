namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwyKluczPodstawowego_IDProdukcjaRozliczenie_Naglowek : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_Naglowek", name: "IDProdukcjaRozliczenie_Dane", newName: "IDProdukcjaRozliczenie_Naglowek");
        }
        
        public override void Down()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_Naglowek", name: "IDProdukcjaRozliczenie_Naglowek", newName: "IDProdukcjaRozliczenie_Dane");
        }
    }
}
