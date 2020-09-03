namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblKonfiguracjaUrzadzen : DbMigration
    {
        public override void Up()
        {
            AddColumn("Konfiguracja.tblKonfiguracjaUrzadzen", "IDOperator", c => c.Int());
            CreateIndex("Konfiguracja.tblKonfiguracjaUrzadzen", "IDOperator");
            AddForeignKey("Konfiguracja.tblKonfiguracjaUrzadzen", "IDOperator", "dbo.tblPracownikGAT", "ID_PracownikGAT");
        }
        
        public override void Down()
        {
            DropForeignKey("Konfiguracja.tblKonfiguracjaUrzadzen", "IDOperator", "dbo.tblPracownikGAT");
            DropIndex("Konfiguracja.tblKonfiguracjaUrzadzen", new[] { "IDOperator" });
            DropColumn("Konfiguracja.tblKonfiguracjaUrzadzen", "IDOperator");
        }
    }
}
