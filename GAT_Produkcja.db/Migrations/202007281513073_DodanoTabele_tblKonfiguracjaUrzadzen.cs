namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoTabele_tblKonfiguracjaUrzadzen : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Konfiguracja.tblKonfiguracjaUrzadzen",
                c => new
                    {
                        IDKonfiguracjaUrzadzen = c.Int(nullable: false, identity: true),
                        NazwaKomputera = c.String(),
                        DrukarkaIP = c.String(),
                        WagaComPort = c.String(),
                        DataDodania = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDKonfiguracjaUrzadzen);
            
        }
        
        public override void Down()
        {
            DropTable("Konfiguracja.tblKonfiguracjaUrzadzen");
        }
    }
}
