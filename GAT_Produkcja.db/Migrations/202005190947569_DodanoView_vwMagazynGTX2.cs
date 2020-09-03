namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoView_vwMagazynGTX2 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "GTEX2.vwMagazynGTX2",
            //    c => new
            //        {
            //            IdMagazyn = c.Int(nullable: false, identity: true),
            //            Symbol = c.String(),
            //            Nazwa = c.String(),
            //            Status = c.Int(nullable: false),
            //            Opis = c.String(),
            //        })
            //    .PrimaryKey(t => t.IdMagazyn);
        }
        
        public override void Down()
        {
            //DropTable("GTEX2.vwMagazynGTX2");
        }
    }
}
