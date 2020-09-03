namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblKonfiguracjaUrzadzen1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Konfiguracja.tblKonfiguracjaUrzadzen", "DrukarkaNazwa", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Konfiguracja.tblKonfiguracjaUrzadzen", "DrukarkaNazwa");
        }
    }
}
