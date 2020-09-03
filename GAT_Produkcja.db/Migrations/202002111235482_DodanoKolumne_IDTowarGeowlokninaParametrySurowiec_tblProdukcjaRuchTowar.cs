namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumne_IDTowarGeowlokninaParametrySurowiec_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "IDTowarGeowlokninaParametrySurowiec", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaRuchTowar", "IDTowarGeowlokninaParametrySurowiec");
            AddForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDTowarGeowlokninaParametrySurowiec", "Towar.tblTowarGeowlokninaParametrySurowiec", "IDTowarGeowlokninaParametrySurowiec", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDTowarGeowlokninaParametrySurowiec", "Towar.tblTowarGeowlokninaParametrySurowiec");
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDTowarGeowlokninaParametrySurowiec" });
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "IDTowarGeowlokninaParametrySurowiec");
        }
    }
}
