namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NowaTabela_tblProdukcjaZlecenieStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Produkcja.tblProdukcjaZlecenieStatus",
                c => new
                    {
                        IDProdukcjaZlecenieStatus = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.IDProdukcjaZlecenieStatus);
            
        }
        
        public override void Down()
        {
            DropTable("Produkcja.tblProdukcjaZlecenieStatus");
        }
    }
}
