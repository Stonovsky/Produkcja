namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsunietoKol_tblProdukcjaZlecenieProdukcyjne_Mieszanka : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IloscMieszanki_kg", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "CenaMieszanki_kg", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IloscSumarycznaKg");
            DropColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "CenaWgUdzialu_kg");
            DropColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "KosztWgUdzialu_kg");
        }
        
        public override void Down()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "KosztWgUdzialu_kg", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "CenaWgUdzialu_kg", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IloscSumarycznaKg", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "CenaMieszanki_kg");
            DropColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IloscMieszanki_kg");
        }
    }
}
