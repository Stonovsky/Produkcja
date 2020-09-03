namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_SymbolSurowcaSubiekt_tblProdukcjaRozliczenie_RW : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_RW", "SymbolSurowcaSubiekt", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRozliczenie_RW", "SymbolSurowcaSubiekt");
        }
    }
}
