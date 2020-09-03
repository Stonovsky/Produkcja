namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRozliczenie_RW_tblProdukcjaRozliczenie_PW : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_PW", "IDProdukcjaRozliczenie_Naglowek", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaRozliczenie_RW", "IDProdukcjaRozliczenie_Naglowek", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaRozliczenie_PW", "IDProdukcjaRozliczenie_Naglowek");
            CreateIndex("Produkcja.tblProdukcjaRozliczenie_RW", "IDProdukcjaRozliczenie_Naglowek");
            AddForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDProdukcjaRozliczenie_Naglowek", "Produkcja.tblProdukcjaRozliczenie_Naglowek", "IDProdukcjaRozliczenie_Naglowek", cascadeDelete: false);
            AddForeignKey("Produkcja.tblProdukcjaRozliczenie_RW", "IDProdukcjaRozliczenie_Naglowek", "Produkcja.tblProdukcjaRozliczenie_Naglowek", "IDProdukcjaRozliczenie_Naglowek", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_RW", "IDProdukcjaRozliczenie_Naglowek", "Produkcja.tblProdukcjaRozliczenie_Naglowek");
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDProdukcjaRozliczenie_Naglowek", "Produkcja.tblProdukcjaRozliczenie_Naglowek");
            DropIndex("Produkcja.tblProdukcjaRozliczenie_RW", new[] { "IDProdukcjaRozliczenie_Naglowek" });
            DropIndex("Produkcja.tblProdukcjaRozliczenie_PW", new[] { "IDProdukcjaRozliczenie_Naglowek" });
            DropColumn("Produkcja.tblProdukcjaRozliczenie_RW", "IDProdukcjaRozliczenie_Naglowek");
            DropColumn("Produkcja.tblProdukcjaRozliczenie_PW", "IDProdukcjaRozliczenie_Naglowek");
        }
    }
}
