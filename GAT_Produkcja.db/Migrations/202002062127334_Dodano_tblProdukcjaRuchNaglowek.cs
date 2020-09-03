namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dodano_tblProdukcjaRuchNaglowek : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Produkcja.tblProdukcjaRuchNaglowek",
                c => new
                    {
                        IDProdukcjaRuchNaglowek = c.Int(nullable: false, identity: true),
                        IDProdukcjaGniazdoProdukcyjne = c.Int(nullable: false),
                        IDProdukcjaZlecenieProdukcyjne = c.Int(nullable: false),
                        IDPracownikGAT = c.Int(nullable: false),
                        IDPracownikGAT1 = c.Int(nullable: false),
                        DataDodania = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDProdukcjaRuchNaglowek)
                .ForeignKey("dbo.tblPracownikGAT", t => t.IDPracownikGAT, cascadeDelete: false)
                .ForeignKey("dbo.tblPracownikGAT", t => t.IDPracownikGAT1, cascadeDelete: false)
                .ForeignKey("Produkcja.tblProdukcjaGniazdoProdukcyjne", t => t.IDProdukcjaGniazdoProdukcyjne, cascadeDelete: false)
                .ForeignKey("Produkcja.tblProdukcjaZlcecenieProdukcyjne", t => t.IDProdukcjaZlecenieProdukcyjne, cascadeDelete: false)
                .Index(t => t.IDProdukcjaGniazdoProdukcyjne)
                .Index(t => t.IDProdukcjaZlecenieProdukcyjne)
                .Index(t => t.IDPracownikGAT)
                .Index(t => t.IDPracownikGAT1);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaZlecenieProdukcyjne", "Produkcja.tblProdukcjaZlcecenieProdukcyjne");
            DropForeignKey("Produkcja.tblProdukcjaRuchNaglowek", "IDProdukcjaGniazdoProdukcyjne", "Produkcja.tblProdukcjaGniazdoProdukcyjne");
            DropForeignKey("Produkcja.tblProdukcjaRuchNaglowek", "IDPracownikGAT1", "dbo.tblPracownikGAT");
            DropForeignKey("Produkcja.tblProdukcjaRuchNaglowek", "IDPracownikGAT", "dbo.tblPracownikGAT");
            DropIndex("Produkcja.tblProdukcjaRuchNaglowek", new[] { "IDPracownikGAT1" });
            DropIndex("Produkcja.tblProdukcjaRuchNaglowek", new[] { "IDPracownikGAT" });
            DropIndex("Produkcja.tblProdukcjaRuchNaglowek", new[] { "IDProdukcjaZlecenieProdukcyjne" });
            DropIndex("Produkcja.tblProdukcjaRuchNaglowek", new[] { "IDProdukcjaGniazdoProdukcyjne" });
            DropTable("Produkcja.tblProdukcjaRuchNaglowek");
        }
    }
}
