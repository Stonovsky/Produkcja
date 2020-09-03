namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaStringLenghtDlaTowarNazwa_tblProdukcjaRozliczenie_Naglowek : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Produkcja.tblProdukcjaRozliczenie_Naglowek", "TowarNazwa", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("Produkcja.tblProdukcjaRozliczenie_Naglowek", "TowarNazwa", c => c.String(maxLength: 30));
        }
    }
}
