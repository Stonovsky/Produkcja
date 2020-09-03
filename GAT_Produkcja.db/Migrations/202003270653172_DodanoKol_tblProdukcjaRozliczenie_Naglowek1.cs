namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRozliczenie_Naglowek1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_Naglowek", "RozliczoneTowary", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRozliczenie_Naglowek", "RozliczoneTowary");
        }
    }
}
