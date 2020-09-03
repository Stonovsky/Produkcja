namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaTypuIdNaNullable_tblProdukcjaRozliczenie_PW : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDJm", "dbo.tblJm");
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", "IDJm", "dbo.tblJm");
            DropIndex("Produkcja.tblProdukcjaRozliczenie_PW", new[] { "IDJm" });
            DropIndex("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", new[] { "IDJm" });
            AlterColumn("Produkcja.tblProdukcjaRozliczenie_PW", "IDJm", c => c.Int());
            AlterColumn("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", "IDJm", c => c.Int());
            CreateIndex("Produkcja.tblProdukcjaRozliczenie_PW", "IDJm");
            CreateIndex("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", "IDJm");
            AddForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDJm", "dbo.tblJm", "IDJm");
            AddForeignKey("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", "IDJm", "dbo.tblJm", "IDJm");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", "IDJm", "dbo.tblJm");
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDJm", "dbo.tblJm");
            DropIndex("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", new[] { "IDJm" });
            DropIndex("Produkcja.tblProdukcjaRozliczenie_PW", new[] { "IDJm" });
            AlterColumn("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", "IDJm", c => c.Int(nullable: false));
            AlterColumn("Produkcja.tblProdukcjaRozliczenie_PW", "IDJm", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", "IDJm");
            CreateIndex("Produkcja.tblProdukcjaRozliczenie_PW", "IDJm");
            AddForeignKey("Produkcja.tblProdukcjaRozliczenie_PWPodsumowanie", "IDJm", "dbo.tblJm", "IDJm", cascadeDelete: true);
            AddForeignKey("Produkcja.tblProdukcjaRozliczenie_PW", "IDJm", "dbo.tblJm", "IDJm", cascadeDelete: true);
        }
    }
}
