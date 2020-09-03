namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsunietoKol_tblProdukcjaRozliczenie_Naglowek1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("Produkcja.tblProdukcjaRozliczenie_Naglowek", "IDTowarSubiekt");
        }
        
        public override void Down()
        {
            AddColumn("Produkcja.tblProdukcjaRozliczenie_Naglowek", "IDTowarSubiekt", c => c.Int(nullable: false));
        }
    }
}
