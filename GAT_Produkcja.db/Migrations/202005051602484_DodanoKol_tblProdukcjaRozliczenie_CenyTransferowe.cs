namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRozliczenie_CenyTransferowe : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_CenyTransferowe", "DataDodania", c => c.DateTime());
            AddColumn("Produkcja.tblProdukcjaRozliczenie_CenyTransferowe", "CzyAktualna", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRozliczenie_CenyTransferowe", "CzyAktualna");
            DropColumn("Produkcja.tblProdukcjaRozliczenie_CenyTransferowe", "DataDodania");
        }
    }
}
