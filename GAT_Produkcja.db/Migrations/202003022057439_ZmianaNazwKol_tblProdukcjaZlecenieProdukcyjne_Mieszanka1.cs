namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwKol_tblProdukcjaZlecenieProdukcyjne_Mieszanka1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "Cena_kg", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", name: "Koszt_kg", newName: "KosztWgUdzialu_kg");
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", name: "Wartosc", newName: "Wartosc_kg");

        }

        public override void Down()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", name: "KosztWgUdzialu_kg", newName: "Koszt_kg");
            RenameColumn(table: "Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", name: "Wartosc_kg", newName: "Wartosc");
            DropColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "Cena_kg");
        }
    }
}
