namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumnyZGramatura_tblProdukcjaGniazdoWloknina : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaGniazdoWloknina", "IDGramatura", c => c.Int(nullable: false));
            AddColumn("Produkcja.tblProdukcjaGniazdoWloknina", "Gramatura", c => c.Int(nullable: false));
            CreateIndex("Produkcja.tblProdukcjaGniazdoWloknina", "IDGramatura");
            AddForeignKey("Produkcja.tblProdukcjaGniazdoWloknina", "IDGramatura", "Towar.tblTowarGeowlokninaParametryGramatura", "IDTowarGeowlokninaParametryGramatura", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaGniazdoWloknina", "IDGramatura", "Towar.tblTowarGeowlokninaParametryGramatura");
            DropIndex("Produkcja.tblProdukcjaGniazdoWloknina", new[] { "IDGramatura" });
            DropColumn("Produkcja.tblProdukcjaGniazdoWloknina", "Gramatura");
            DropColumn("Produkcja.tblProdukcjaGniazdoWloknina", "IDGramatura");
        }
    }
}
