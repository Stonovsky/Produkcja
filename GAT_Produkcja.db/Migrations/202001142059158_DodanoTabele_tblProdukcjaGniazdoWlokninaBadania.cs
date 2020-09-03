namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoTabele_tblProdukcjaGniazdoWlokninaBadania : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Produkcja.tblProdukcjaGniazdoWlokninaBadania",
                c => new
                    {
                        IDProdukcjaGniazdoWlokninaBadania = c.Int(nullable: false, identity: true),
                        IDProdukcjaGniazdoWloknina = c.Int(nullable: false),
                        Gramatura_1 = c.Int(),
                        Gramatura_2 = c.Int(),
                        Gramatura_3 = c.Int(),
                        Gramatura_4 = c.Int(),
                        Gramatura_5 = c.Int(),
                        CzySrenidaGramaturaWTolerancjach = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IDProdukcjaGniazdoWlokninaBadania)
                .ForeignKey("Produkcja.tblProdukcjaGniazdoWloknina", t => t.IDProdukcjaGniazdoWloknina, cascadeDelete: true)
                .Index(t => t.IDProdukcjaGniazdoWloknina);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "IDProdukcjaGniazdoWloknina", "Produkcja.tblProdukcjaGniazdoWloknina");
            DropIndex("Produkcja.tblProdukcjaGniazdoWlokninaBadania", new[] { "IDProdukcjaGniazdoWloknina" });
            DropTable("Produkcja.tblProdukcjaGniazdoWlokninaBadania");
        }
    }
}
