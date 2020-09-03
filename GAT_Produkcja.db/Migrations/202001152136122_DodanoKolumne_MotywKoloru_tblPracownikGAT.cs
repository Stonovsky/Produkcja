namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumne_MotywKoloru_tblPracownikGAT : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblPracownikGAT", "MotywKoloru", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblPracownikGAT", "MotywKoloru");
        }
    }
}
