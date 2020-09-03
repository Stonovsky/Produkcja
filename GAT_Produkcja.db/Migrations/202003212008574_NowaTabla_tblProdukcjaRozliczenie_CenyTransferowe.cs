namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NowaTabla_tblProdukcjaRozliczenie_CenyTransferowe : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Produkcja.tblProdukcjaRozliczenie_CenyTransferowe",
                c => new
                    {
                        IDProdukcjaRozliczenie_CenyTransferowe = c.Int(nullable: false, identity: true),
                        TowarNazwa = c.String(),
                        IDTowarGrupa = c.Int(nullable: false),
                        CenaTransferowa = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CenaHurtowa = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IDProdukcjaRozliczenie_CenyTransferowe)
                .ForeignKey("Towar.tblTowarGrupa", t => t.IDTowarGrupa, cascadeDelete: true)
                .Index(t => t.IDTowarGrupa);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRozliczenie_CenyTransferowe", "IDTowarGrupa", "Towar.tblTowarGrupa");
            DropIndex("Produkcja.tblProdukcjaRozliczenie_CenyTransferowe", new[] { "IDTowarGrupa" });
            DropTable("Produkcja.tblProdukcjaRozliczenie_CenyTransferowe");
        }
    }
}
