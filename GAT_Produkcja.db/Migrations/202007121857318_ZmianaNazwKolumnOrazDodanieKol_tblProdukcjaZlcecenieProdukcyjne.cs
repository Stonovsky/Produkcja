namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwKolumnOrazDodanieKol_tblProdukcjaZlcecenieProdukcyjne : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "Produkcja.tblProdukcjaZlcecenieProdukcyjne", name: "NrZleceniaProdukcyjnego", newName: "NrZlecenia");
            RenameColumn(table: "Produkcja.tblProdukcjaZlcecenieProdukcyjne", name: "KodZleceniaProdukcyjnego", newName: "KodKreskowy");
            RenameColumn(table: "Produkcja.tblProdukcjaZlcecenieProdukcyjne", name: "NazwaZleceniaProdukcyjnego", newName: "NazwaZlecenia");
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDKontrahent", c => c.Int());
            AddColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "RodzajPakowania", c => c.String());
            CreateIndex("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDKontrahent");
            AddForeignKey("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDKontrahent", "dbo.tblKontrahent", "ID_Kontrahent");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDKontrahent", "dbo.tblKontrahent");
            DropIndex("Produkcja.tblProdukcjaZlcecenieProdukcyjne", new[] { "IDKontrahent" });
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "RodzajPakowania");
            DropColumn("Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDKontrahent");
            RenameColumn(table: "Produkcja.tblProdukcjaZlcecenieProdukcyjne", name: "NazwaZlecenia", newName: "NazwaZleceniaProdukcyjnego");
            RenameColumn(table: "Produkcja.tblProdukcjaZlcecenieProdukcyjne", name: "KodKreskowy", newName: "KodZleceniaProdukcyjnego");
            RenameColumn(table: "Produkcja.tblProdukcjaZlcecenieProdukcyjne", name: "NrZlecenia", newName: "NrZleceniaProdukcyjnego");
        }
    }
}
