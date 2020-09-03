namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NowaTabela_tblProdukcjaRuchTowarStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Produkcja.tblProdukcjaRuchTowarStatus",
                c => new
                    {
                        IDProdukcjaRuchTowarStatus = c.Int(nullable: false, identity: true),
                        Status = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.IDProdukcjaRuchTowarStatus);
            
        }
        
        public override void Down()
        {
            DropTable("Produkcja.tblProdukcjaRuchTowarStatus");
        }
    }
}
