namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumne_KodKreskowy_tblProdukcjaZlecenieCiecia : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaZlecenieCiecia", "KodKreskowy", c => c.String(maxLength:20));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaZlecenieCiecia", "KodKreskowy");
        }
    }
}
