namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRozliczenie_PW : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_PW", "IDProdukcjaRozliczenie_RW", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaRozliczenie_RW", "IDSurowiecSubiekt", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaRozliczenie_RW", "IDNormaZuzyciaMsAccess", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaRozliczenie_RW", "NazwaSurowcaMsAccess", c => c.String(maxLength: 30));
            AddColumn("Produkcja.tblProdukcjaRozliczenie_RW", "NazwaSurowcaSubiekt", c => c.String());
            AddColumn("Produkcja.tblProdukcjaRozliczenie_RW", "Udzial", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("Produkcja.tblProdukcjaRozliczenie_PW", "IDProdukcjaRozliczenie_RW");
            AddForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDProdukcjaRozliczenie_RW", "Produkcja.tblProdukcjaRozliczenie_RW", "IDProdukcjaRozliczenie_RW", cascadeDelete: false);
            DropColumn("Produkcja.tblProdukcjaRozliczenie_RW", "NazwaSurowca");
        }
        
        public override void Down()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_RW", "NazwaSurowca", c => c.String(maxLength: 30));
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDProdukcjaRozliczenie_RW", "Produkcja.tblProdukcjaRozliczenie_RW");
            DropIndex("Produkcja.tblProdukcjaRozliczenie_PW", new[] { "IDProdukcjaRozliczenie_RW" });
            DropColumn("Produkcja.tblProdukcjaRozliczenie_RW", "Udzial");
            DropColumn("Produkcja.tblProdukcjaRozliczenie_RW", "NazwaSurowcaSubiekt");
            DropColumn("Produkcja.tblProdukcjaRozliczenie_RW", "NazwaSurowcaMsAccess");
            DropColumn("Produkcja.tblProdukcjaRozliczenie_RW", "IDNormaZuzyciaMsAccess");
            DropColumn("Produkcja.tblProdukcjaRozliczenie_RW", "IDSurowiecSubiekt");
            DropColumn("Produkcja.tblProdukcjaRozliczenie_PW", "IDProdukcjaRozliczenie_RW");
        }
    }
}
