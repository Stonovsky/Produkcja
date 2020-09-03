namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumne_IDPracownikOdpZaZakup_tblZapotrzebowanie : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblZapotrzebowanie", "IDPracownikOdpZaZakup", c => c.Int());
            CreateIndex("dbo.tblZapotrzebowanie", "IDPracownikOdpZaZakup");
            AddForeignKey("dbo.tblZapotrzebowanie", "IDPracownikOdpZaZakup", "dbo.tblPracownikGAT", "ID_PracownikGAT");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblZapotrzebowanie", "IDPracownikOdpZaZakup", "dbo.tblPracownikGAT");
            DropIndex("dbo.tblZapotrzebowanie", new[] { "IDPracownikOdpZaZakup" });
            DropColumn("dbo.tblZapotrzebowanie", "IDPracownikOdpZaZakup");
        }
    }
}
