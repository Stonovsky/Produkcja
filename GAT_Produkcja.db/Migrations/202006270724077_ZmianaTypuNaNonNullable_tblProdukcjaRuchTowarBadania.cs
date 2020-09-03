namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaTypuNaNonNullable_tblProdukcjaRuchTowarBadania : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Produkcja.tblProdukcjaRuchTowarBadania", "Gramatura_1", c => c.Int(nullable: false));
            AlterColumn("Produkcja.tblProdukcjaRuchTowarBadania", "Gramatura_2", c => c.Int(nullable: false));
            AlterColumn("Produkcja.tblProdukcjaRuchTowarBadania", "Gramatura_3", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Produkcja.tblProdukcjaRuchTowarBadania", "Gramatura_3", c => c.Int());
            AlterColumn("Produkcja.tblProdukcjaRuchTowarBadania", "Gramatura_2", c => c.Int());
            AlterColumn("Produkcja.tblProdukcjaRuchTowarBadania", "Gramatura_1", c => c.Int());
        }
    }
}
