namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRozliczenie_PW5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_PW", "NazwaRolkiBazowej", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRozliczenie_PW", "NazwaRolkiBazowej");
        }
    }
}
