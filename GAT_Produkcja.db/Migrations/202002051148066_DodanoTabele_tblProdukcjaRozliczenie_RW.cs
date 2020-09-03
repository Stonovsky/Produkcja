namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoTabele_tblProdukcjaRozliczenie_RW : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Produkcja.tblProdukcjaRozliczenie_RW",
                c => new
                    {
                        IDProdukcjaRozliczenie_RW = c.Int(nullable: false, identity: true),
                        NrZlecenia = c.String(maxLength: 20),
                        IDSurowiec = c.Int(nullable: false),
                        IDSurowcaComarch = c.Int(nullable: false),
                        IDSurowiecMsAccess = c.Int(nullable: false),
                        NazwaSurowca = c.String(maxLength: 30),
                        Ilosc_kg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CenaJednostkowa = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Wartosc = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataDodania = c.DateTime(nullable: false),
                        IDPracownikGAT = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDProdukcjaRozliczenie_RW)
                .ForeignKey("dbo.tblPracownikGAT", t => t.IDPracownikGAT, cascadeDelete: true)
                .Index(t => t.IDPracownikGAT);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_RW", "IDPracownikGAT", "dbo.tblPracownikGAT");
            DropIndex("Produkcja.tblProdukcjaRozliczenie_RW", new[] { "IDPracownikGAT" });
            DropTable("Produkcja.tblProdukcjaRozliczenie_RW");
        }
    }
}
