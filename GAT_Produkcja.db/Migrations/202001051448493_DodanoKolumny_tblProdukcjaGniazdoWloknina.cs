namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumny_tblProdukcjaGniazdoWloknina : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaGniazdoWloknina", "DataDodania", c => c.DateTime(nullable: false));
            AddColumn("Produkcja.tblProdukcjaGniazdoWloknina", "IDPracownikGAT", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaGniazdoWloknina", "NrRolki", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaGniazdoWloknina", "StronaRolkiWyjsciowej", c => c.String());
            AddColumn("Produkcja.tblProdukcjaGniazdoWloknina", "KodKreskowy", c => c.String());
            CreateIndex("Produkcja.tblProdukcjaGniazdoWloknina", "IDPracownikGAT");
            AddForeignKey("Produkcja.tblProdukcjaGniazdoWloknina", "IDPracownikGAT", "dbo.tblPracownikGAT", "ID_PracownikGAT", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaGniazdoWloknina", "IDPracownikGAT", "dbo.tblPracownikGAT");
            DropIndex("Produkcja.tblProdukcjaGniazdoWloknina", new[] { "IDPracownikGAT" });
            DropColumn("Produkcja.tblProdukcjaGniazdoWloknina", "KodKreskowy");
            DropColumn("Produkcja.tblProdukcjaGniazdoWloknina", "StronaRolkiWyjsciowej");
            DropColumn("Produkcja.tblProdukcjaGniazdoWloknina", "NrRolki");
            DropColumn("Produkcja.tblProdukcjaGniazdoWloknina", "IDPracownikGAT");
            DropColumn("Produkcja.tblProdukcjaGniazdoWloknina", "DataDodania");
        }
    }
}
