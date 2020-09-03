namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwyKol_IdPracownikGAT_tblProdukcjaZlecenie : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenie", name: "IDPracownikGAT", newName: "IDZlecajacy");
            RenameIndex(table: "Produkcja.tblProdukcjaZlecenie", name: "IX_IDPracownikGAT", newName: "IX_IDZlecajacy");
        }
        
        public override void Down()
        {
            RenameIndex(table: "Produkcja.tblProdukcjaZlecenie", name: "IX_IDZlecajacy", newName: "IX_IDPracownikGAT");
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenie", name: "IDZlecajacy", newName: "IDPracownikGAT");
        }
    }
}
