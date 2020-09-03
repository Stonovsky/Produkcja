namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmienionoDlugoscNazwyTowaru_tblProdukcjaRozliczenie_RW : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Produkcja.tblProdukcjaRozliczenie_PW", "NazwaTowaruSubiekt", c => c.String(maxLength: 100));
            AlterColumn("Produkcja.tblProdukcjaRozliczenie_PW", "SymbolTowaruSubiekt", c => c.String(maxLength: 30));
            AlterColumn("Produkcja.tblProdukcjaRozliczenie_RW", "NazwaSurowcaMsAccess", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("Produkcja.tblProdukcjaRozliczenie_RW", "NazwaSurowcaMsAccess", c => c.String(maxLength: 30));
            AlterColumn("Produkcja.tblProdukcjaRozliczenie_PW", "SymbolTowaruSubiekt", c => c.String());
            AlterColumn("Produkcja.tblProdukcjaRozliczenie_PW", "NazwaTowaruSubiekt", c => c.String(maxLength: 30));
        }
    }
}
