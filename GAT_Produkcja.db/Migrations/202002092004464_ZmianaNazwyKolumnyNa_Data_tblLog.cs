namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNazwyKolumnyNa_Data_tblLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblLog", "Data", c => c.DateTime());
            DropColumn("dbo.tblLog", "Czas");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblLog", "Czas", c => c.DateTime());
            DropColumn("dbo.tblLog", "Data");
        }
    }
}
