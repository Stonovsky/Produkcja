namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblTowarGeowlokninaParametry1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Towar.tblTowarGeowlokninaParametry", "IDTowar", "Towar.tblTowar");
            DropIndex("Towar.tblTowarGeowlokninaParametry", new[] { "IDTowar" });
            AddColumn("Towar.tblTowarGeowlokninaParametry", "IDTowarGeowlokninaParametrySurowiec", c => c.Int());
            AlterColumn("Towar.tblTowarGeowlokninaParametry", "IDTowar", c => c.Int());
            CreateIndex("Towar.tblTowarGeowlokninaParametry", "IDTowar");
            CreateIndex("Towar.tblTowarGeowlokninaParametry", "IDTowarGeowlokninaParametrySurowiec");
            AddForeignKey("Towar.tblTowarGeowlokninaParametry", "IDTowarGeowlokninaParametrySurowiec", "Towar.tblTowarGeowlokninaParametrySurowiec", "IDTowarGeowlokninaParametrySurowiec");
            AddForeignKey("Towar.tblTowarGeowlokninaParametry", "IDTowar", "Towar.tblTowar", "IDTowar");
        }
        
        public override void Down()
        {
            DropForeignKey("Towar.tblTowarGeowlokninaParametry", "IDTowar", "Towar.tblTowar");
            DropForeignKey("Towar.tblTowarGeowlokninaParametry", "IDTowarGeowlokninaParametrySurowiec", "Towar.tblTowarGeowlokninaParametrySurowiec");
            DropIndex("Towar.tblTowarGeowlokninaParametry", new[] { "IDTowarGeowlokninaParametrySurowiec" });
            DropIndex("Towar.tblTowarGeowlokninaParametry", new[] { "IDTowar" });
            AlterColumn("Towar.tblTowarGeowlokninaParametry", "IDTowar", c => c.Int(nullable: false));
            DropColumn("Towar.tblTowarGeowlokninaParametry", "IDTowarGeowlokninaParametrySurowiec");
            CreateIndex("Towar.tblTowarGeowlokninaParametry", "IDTowar");
            AddForeignKey("Towar.tblTowarGeowlokninaParametry", "IDTowar", "Towar.tblTowar", "IDTowar", cascadeDelete: true);
        }
    }
}
