namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoTabele_tblProdukcjaGeokomorkaPodsumowaniePrzerob : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Produkcja.tblProdukcjaGeokomorkaPodsumowaniePrzerob",
                c => new
                    {
                        IdProdukcjaGeokomorkaPodsumowaniePrzerobrodukcjaGeokomorkaPodsumowanie = c.Int(nullable: false, identity: true),
                        Ilosc_kg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CenaJedn_kg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Wartosc = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataOd = c.DateTime(nullable: false),
                        DataDo = c.DateTime(nullable: false),
                        IdOperator = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdProdukcjaGeokomorkaPodsumowaniePrzerobrodukcjaGeokomorkaPodsumowanie)
                .ForeignKey("dbo.tblPracownikGAT", t => t.IdOperator, cascadeDelete: true)
                .Index(t => t.IdOperator);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaGeokomorkaPodsumowaniePrzerob", "IdOperator", "dbo.tblPracownikGAT");
            DropIndex("Produkcja.tblProdukcjaGeokomorkaPodsumowaniePrzerob", new[] { "IdOperator" });
            DropTable("Produkcja.tblProdukcjaGeokomorkaPodsumowaniePrzerob");
        }
    }
}
