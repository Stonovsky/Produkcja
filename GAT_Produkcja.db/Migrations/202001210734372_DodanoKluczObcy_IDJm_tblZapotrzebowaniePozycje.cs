namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKluczObcy_IDJm_tblZapotrzebowaniePozycje : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.tblZapotrzebowaniePozycje", "IDJm");
            AddForeignKey("dbo.tblZapotrzebowaniePozycje", "IDJm", "dbo.tblJm", "IDJm");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblZapotrzebowaniePozycje", "IDJm", "dbo.tblJm");
            DropIndex("dbo.tblZapotrzebowaniePozycje", new[] { "IDJm" });
        }
    }
}
