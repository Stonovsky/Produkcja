namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsunietoKol_tblProdukcjaRuchTowarBadania : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "IDProdukcjaGniazdoWloknina", "Produkcja.tblProdukcjaGniazdoWloknina");
            DropIndex("Produkcja.tblProdukcjaGniazdoWlokninaBadania", new[] { "IDProdukcjaGniazdoWloknina" });
            DropColumn("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "IDProdukcjaGniazdoWloknina");
            //RenameColumn(table: "Produkcja.tblProdukcjaGniazdoWlokninaBadania", name: "IDProdukcjaGniazdoWloknina", newName: "tblProdukcjaGniazdoWloknina_IDProdukcjaGniazdoWloknina");
            //AlterColumn("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "tblProdukcjaGniazdoWloknina_IDProdukcjaGniazdoWloknina", c => c.Int());
            //CreateIndex("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "tblProdukcjaGniazdoWloknina_IDProdukcjaGniazdoWloknina");
            //AddForeignKey("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "tblProdukcjaGniazdoWloknina_IDProdukcjaGniazdoWloknina", "Produkcja.tblProdukcjaGniazdoWloknina", "IDProdukcjaGniazdoWloknina");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "tblProdukcjaGniazdoWloknina_IDProdukcjaGniazdoWloknina", "Produkcja.tblProdukcjaGniazdoWloknina");
            DropIndex("Produkcja.tblProdukcjaGniazdoWlokninaBadania", new[] { "tblProdukcjaGniazdoWloknina_IDProdukcjaGniazdoWloknina" });
            AlterColumn("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "tblProdukcjaGniazdoWloknina_IDProdukcjaGniazdoWloknina", c => c.Int(nullable: false));
            RenameColumn(table: "Produkcja.tblProdukcjaGniazdoWlokninaBadania", name: "tblProdukcjaGniazdoWloknina_IDProdukcjaGniazdoWloknina", newName: "IDProdukcjaGniazdoWloknina");
            CreateIndex("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "IDProdukcjaGniazdoWloknina");
            AddForeignKey("Produkcja.tblProdukcjaGniazdoWlokninaBadania", "IDProdukcjaGniazdoWloknina", "Produkcja.tblProdukcjaGniazdoWloknina", "IDProdukcjaGniazdoWloknina", cascadeDelete: true);
        }
    }
}
