namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaDlugosciStringa_tblProdukcjaRozliczenie_Naglowek : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Produkcja.tblProdukcjaRozliczenie_Naglowek", "NrZlecenia", c => c.String(maxLength: 5));
        }
        
        public override void Down()
        {
            AlterColumn("Produkcja.tblProdukcjaRozliczenie_Naglowek", "NrZlecenia", c => c.String());
        }
    }
}
