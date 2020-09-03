namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoTabele_tblFinanseStanKonta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Finanse.tblFinanseStanKonta",
                c => new
                    {
                        IDFinanseStanKonta = c.Int(nullable: false, identity: true),
                        IdFirma = c.Int(nullable: false),
                        IdBank = c.Int(nullable: false),
                        BankNazwa = c.String(),
                        NrKonta = c.String(),
                        Stan = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataDodania = c.DateTime(nullable: false),
                        IdOperator = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDFinanseStanKonta)
                .ForeignKey("dbo.tblFirma", t => t.IdFirma, cascadeDelete: true)
                .ForeignKey("dbo.tblPracownikGAT", t => t.IdOperator, cascadeDelete: true)
                .Index(t => t.IdFirma)
                .Index(t => t.IdOperator);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Finanse.tblFinanseStanKonta", "IdOperator", "dbo.tblPracownikGAT");
            DropForeignKey("Finanse.tblFinanseStanKonta", "IdFirma", "dbo.tblFirma");
            DropIndex("Finanse.tblFinanseStanKonta", new[] { "IdOperator" });
            DropIndex("Finanse.tblFinanseStanKonta", new[] { "IdFirma" });
            DropTable("Finanse.tblFinanseStanKonta");
        }
    }
}
