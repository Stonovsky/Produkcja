namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_IdMsAccess_tblProdukcjaZlecenie : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenie", "IDMsAccess", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaZlecenie", "IDMsAccess");
        }
    }
}
