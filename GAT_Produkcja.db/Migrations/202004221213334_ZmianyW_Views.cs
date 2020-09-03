namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianyW_Views : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("AGG.vwFinanseNalZobAGG", "DataPowstania", c => c.DateTime());
            //AlterColumn("AGG.vwFinanseNalZobAGG", "TerminPlatnosci", c => c.DateTime());
            //AlterColumn("AGG.vwFinanseNalZobAGG", "DniSpoznienia", c => c.Int());
            //AlterColumn("GTEX.vwFinanseNalZobGTX", "DataPowstania", c => c.DateTime());
            //AlterColumn("GTEX.vwFinanseNalZobGTX", "TerminPlatnosci", c => c.DateTime());
            //AlterColumn("GTEX.vwFinanseNalZobGTX", "DniSpoznienia", c => c.Int());
            //DropColumn("AGG.vwFinanseNalZobAGG", "Firma");
            //DropColumn("GTEX.vwFinanseNalZobGTX", "Firma");
        }
        
        public override void Down()
        {
            //AddColumn("GTEX.vwFinanseNalZobGTX", "Firma", c => c.String());
            //AddColumn("AGG.vwFinanseNalZobAGG", "Firma", c => c.String());
            //AlterColumn("GTEX.vwFinanseNalZobGTX", "DniSpoznienia", c => c.Int(nullable: false));
            //AlterColumn("GTEX.vwFinanseNalZobGTX", "TerminPlatnosci", c => c.DateTime(nullable: false));
            //AlterColumn("GTEX.vwFinanseNalZobGTX", "DataPowstania", c => c.DateTime(nullable: false));
            //AlterColumn("AGG.vwFinanseNalZobAGG", "DniSpoznienia", c => c.Int(nullable: false));
            //AlterColumn("AGG.vwFinanseNalZobAGG", "TerminPlatnosci", c => c.DateTime(nullable: false));
            //AlterColumn("AGG.vwFinanseNalZobAGG", "DataPowstania", c => c.DateTime(nullable: false));
        }
    }
}
