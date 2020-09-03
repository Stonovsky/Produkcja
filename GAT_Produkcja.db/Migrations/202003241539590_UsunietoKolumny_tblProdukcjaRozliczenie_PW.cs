namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsunietoKolumny_tblProdukcjaRozliczenie_PW : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDSurowiec", "Towar.tblTowar");
            DropIndex("Produkcja.tblProdukcjaRozliczenie_PW", new[] { "IDSurowiec" });
            DropColumn("Produkcja.tblProdukcjaRozliczenie_PW", "IDSurowiec");
        }
        
        public override void Down()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_PW", "IDSurowiec", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaRozliczenie_PW", "IDSurowiec");
            AddForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDSurowiec", "Towar.tblTowar", "IDTowar", cascadeDelete: true);
        }
    }
}
