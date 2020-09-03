namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwKol_tblProdukcjaZlecenieProdukcyjne_Mieszanka : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", name: "KosztMieszankiZaKg", newName: "Koszt_kg");
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", name: "KosztCalkowityMieszanki", newName: "Wartosc");
        }
        
        public override void Down()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", name: "Wartosc", newName: "KosztCalkowityMieszanki");
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", name: "Koszt_kg", newName: "KosztMieszankiZaKg");
        }
    }
}
