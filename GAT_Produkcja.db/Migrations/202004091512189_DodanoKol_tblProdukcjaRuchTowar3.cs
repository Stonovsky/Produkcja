namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRuchTowar3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "KierunekPrzychodu", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "KierunekPrzychodu");
        }
    }
}
