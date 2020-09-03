namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoViews_vwFinanseBankAGG_GTX_GTX2 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.vwFinanseBankAGG",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            IdFirma = c.Int(nullable: false),
            //            Firma = c.String(),
            //            Nazwa = c.String(),
            //            Numer = c.String(),
            //            Bank = c.String(),
            //            Waluta = c.String(),
            //            Opis = c.String(),
            //            CzyRachunekVAT = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.vwFinanseBankGTX",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            IdFirma = c.Int(nullable: false),
            //            Firma = c.String(),
            //            Nazwa = c.String(),
            //            Numer = c.String(),
            //            Bank = c.String(),
            //            Waluta = c.String(),
            //            Opis = c.String(),
            //            CzyRachunekVAT = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.vwFinanseBankGTX2",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            IdFirma = c.Int(nullable: false),
            //            Firma = c.String(),
            //            Nazwa = c.String(),
            //            Numer = c.String(),
            //            Bank = c.String(),
            //            Waluta = c.String(),
            //            Opis = c.String(),
            //            CzyRachunekVAT = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.vwFinanseBankGTX2");
            //DropTable("dbo.vwFinanseBankGTXes");
            //DropTable("dbo.vwFinanseBankAGGs");
        }
    }
}
