namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dodano_tblProdukcjaRuchTowarBadania : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Produkcja.tblProdukcjaRuchTowarBadania",
                c => new
                    {
                        IDProdukcjaRuchTowarBadania = c.Int(nullable: false, identity: true),
                        IDProdukcjaRuchTowar = c.Int(nullable: false),
                        Gramatura_1 = c.Int(),
                        Gramatura_2 = c.Int(),
                        Gramatura_3 = c.Int(),
                        GramaturaSrednia = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CzySredniaGramaturaWTolerancjach = c.Boolean(nullable: false),
                        UwagiGramatura = c.String(),
                    })
                .PrimaryKey(t => t.IDProdukcjaRuchTowarBadania)
                .ForeignKey("Produkcja.tblProdukcjaRuchTowar", t => t.IDProdukcjaRuchTowar, cascadeDelete: true)
                .Index(t => t.IDProdukcjaRuchTowar);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchTowarBadania", "IDProdukcjaRuchTowar", "Produkcja.tblProdukcjaRuchTowar");
            DropIndex("Produkcja.tblProdukcjaRuchTowarBadania", new[] { "IDProdukcjaRuchTowar" });
            DropTable("Produkcja.tblProdukcjaRuchTowarBadania");
        }
    }
}
