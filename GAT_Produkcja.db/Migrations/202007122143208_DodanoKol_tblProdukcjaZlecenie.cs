namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaZlecenie : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenie", "IDWykonujacy", c => c.Int());
            CreateIndex("Produkcja.tblProdukcjaZlecenie", "IDWykonujacy");
            AddForeignKey("Produkcja.tblProdukcjaZlecenie", "IDWykonujacy", "dbo.tblPracownikGAT", "ID_PracownikGAT");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlecenie", "IDWykonujacy", "dbo.tblPracownikGAT");
            DropIndex("Produkcja.tblProdukcjaZlecenie", new[] { "IDWykonujacy" });
            DropColumn("Produkcja.tblProdukcjaZlecenie", "IDWykonujacy");
        }
    }
}
