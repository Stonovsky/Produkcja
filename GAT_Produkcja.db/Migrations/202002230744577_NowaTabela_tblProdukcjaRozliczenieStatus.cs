namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NowaTabela_tblProdukcjaRozliczenieStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Produkcja.tblProdukcjaRozliczenieStatus",
                c => new
                    {
                        IDProdukcjaRozliczenieStatus = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.IDProdukcjaRozliczenieStatus);
            
        }
        
        public override void Down()
        {
            DropTable("Produkcja.tblProdukcjaRozliczenieStatus");
        }
    }
}
