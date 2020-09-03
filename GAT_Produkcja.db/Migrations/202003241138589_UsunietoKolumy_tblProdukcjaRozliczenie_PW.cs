namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsunietoKolumy_tblProdukcjaRozliczenie_PW : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDProdukcjaRozliczenie_RW", "Produkcja.tblProdukcjaRozliczenie_RW");
            DropIndex("Produkcja.tblProdukcjaRozliczenie_PW", new[] { "IDProdukcjaRozliczenie_RW" });
            DropColumn("Produkcja.tblProdukcjaRozliczenie_PW", "IDProdukcjaRozliczenie_RW");
        }
        
        public override void Down()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_PW", "IDProdukcjaRozliczenie_RW", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaRozliczenie_PW", "IDProdukcjaRozliczenie_RW");
            AddForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDProdukcjaRozliczenie_RW", "Produkcja.tblProdukcjaRozliczenie_RW", "IDProdukcjaRozliczenie_RW", cascadeDelete: true);
        }
    }
}
