namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_SymbolTowaruSubiekt_tblProdukcjaRozliczenie_PW : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_PW", name: "NazwaTowaru", newName: "NazwaTowaruSubiekt");
            AddColumn("Produkcja.tblProdukcjaRozliczenie_PW", "SymbolTowaruSubiekt", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRozliczenie_PW", "SymbolTowaruSubiekt");
            RenameColumn(table: "Produkcja.tblProdukcjaRozliczenie_PW", name: "NazwaTowaruSubiekt", newName: "NazwaTowaru");
        }
    }
}
