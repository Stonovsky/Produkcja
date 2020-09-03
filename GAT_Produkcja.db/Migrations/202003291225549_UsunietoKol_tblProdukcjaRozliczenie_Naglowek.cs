namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsunietoKol_tblProdukcjaRozliczenie_Naglowek : DbMigration
    {
        public override void Up()
        {
            DropColumn("Produkcja.tblProdukcjaRozliczenie_Naglowek", "RozliczoneTowary");
        }
        
        public override void Down()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_Naglowek", "RozliczoneTowary", c => c.String());
        }
    }
}
