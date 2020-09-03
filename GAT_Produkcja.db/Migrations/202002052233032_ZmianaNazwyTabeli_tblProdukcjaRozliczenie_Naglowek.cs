namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwyTabeli_tblProdukcjaRozliczenie_Naglowek : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Produkcja.tblProdukcjaRozliczenie_Dane", newName: "tblProdukcjaRozliczenie_Naglowek");
        }
        
        public override void Down()
        {
            RenameTable(name: "Produkcja.tblProdukcjaRozliczenie_Naglowek", newName: "tblProdukcjaRozliczenie_Dane");
        }
    }
}
