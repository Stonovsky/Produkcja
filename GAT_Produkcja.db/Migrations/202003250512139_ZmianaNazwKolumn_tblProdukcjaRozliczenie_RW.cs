namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwKolumn_tblProdukcjaRozliczenie_RW : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_RW", name: "NazwaSurowcaSubiekt", newName: "NazwaTowaruSubiekt");
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_RW", name: "SymbolSurowcaSubiekt", newName: "SymbolTowaruSubiekt");
        }
        
        public override void Down()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_RW", name: "SymbolTowaruSubiekt", newName: "SymbolSurowcaSubiekt");
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_RW", name: "NazwaTowaruSubiekt", newName: "NazwaSurowcaSubiekt");
        }
    }
}
