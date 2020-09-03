namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumny_KosztMieszankiZaKg_KosztCalkowityMieszanki_tblProdukcjaZlecenieProdukcyjne_Mieszanka : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "KosztMieszankiZaKg", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "KosztCalkowityMieszanki", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "KosztCalkowityMieszanki");
            DropColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "KosztMieszankiZaKg");
        }
    }
}
