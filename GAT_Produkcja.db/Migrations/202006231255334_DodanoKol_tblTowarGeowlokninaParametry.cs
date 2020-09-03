namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblTowarGeowlokninaParametry : DbMigration
    {
        public override void Up()
        {
            AddColumn("Towar.tblTowarGeowlokninaParametry", "IDTowarGeowlokninaParametryGramatura", c => c.Int());
            CreateIndex("Towar.tblTowarGeowlokninaParametry", "IDTowarGeowlokninaParametryGramatura");
            AddForeignKey("Towar.tblTowarGeowlokninaParametry", "IDTowarGeowlokninaParametryGramatura", "Towar.tblTowarGeowlokninaParametryGramatura", "IDTowarGeowlokninaParametryGramatura");
        }
        
        public override void Down()
        {
            DropForeignKey("Towar.tblTowarGeowlokninaParametry", "IDTowarGeowlokninaParametryGramatura", "Towar.tblTowarGeowlokninaParametryGramatura");
            DropIndex("Towar.tblTowarGeowlokninaParametry", new[] { "IDTowarGeowlokninaParametryGramatura" });
            DropColumn("Towar.tblTowarGeowlokninaParametry", "IDTowarGeowlokninaParametryGramatura");
        }
    }
}
