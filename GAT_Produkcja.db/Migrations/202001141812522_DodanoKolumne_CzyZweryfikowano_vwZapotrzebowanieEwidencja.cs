namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumne_CzyZweryfikowano_vwZapotrzebowanieEwidencja : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaGniazdoWloknina", "CzyParametryZgodneZTolerancjami", c => c.Boolean(nullable: false));
            AddColumn("Produkcja.tblProdukcjaGniazdoWloknina", "UwagiDoParametrow", c => c.String());
            AlterColumn("dbo.tblZapotrzebowaniePozycje", "IDJm", c => c.Int());
            AlterColumn("dbo.tblZapotrzebowaniePozycje", "Nazwa", c => c.String(maxLength: 255));
            AlterColumn("dbo.tblZapotrzebowaniePozycje", "Ilosc", c => c.Double());
            AlterColumn("dbo.tblZapotrzebowaniePozycje", "Cena", c => c.Decimal(storeType: "money"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblZapotrzebowaniePozycje", "Cena", c => c.Decimal(nullable: false, storeType: "money"));
            AlterColumn("dbo.tblZapotrzebowaniePozycje", "Ilosc", c => c.Double(nullable: false));
            AlterColumn("dbo.tblZapotrzebowaniePozycje", "Nazwa", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.tblZapotrzebowaniePozycje", "IDJm", c => c.Int(nullable: false));
            DropColumn("Produkcja.tblProdukcjaGniazdoWloknina", "UwagiDoParametrow");
            DropColumn("Produkcja.tblProdukcjaGniazdoWloknina", "CzyParametryZgodneZTolerancjami");
        }
    }
}
