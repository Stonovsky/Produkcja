namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblTowarGeowlokninaParametry2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Towar.tblTowarGeowlokninaParametry", "WodoprzepuszczalnoscWPlaszczyznie_20kPa", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Towar.tblTowarGeowlokninaParametry", "WodoprzepuszczalnoscWPlaszczyznie_20kPa_Minimum", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Towar.tblTowarGeowlokninaParametry", "WodoprzepuszczalnoscWPlaszczyznie_100kPa", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Towar.tblTowarGeowlokninaParametry", "WodoprzepuszczalnoscWPlaszczyznie_100kPa_Minimum", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Towar.tblTowarGeowlokninaParametry", "WodoprzepuszczalnoscWPlaszczyznie_200kPa", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Towar.tblTowarGeowlokninaParametry", "WodoprzepuszczalnoscWPlaszczyznie_200kPa_Minimum", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Towar.tblTowarGeowlokninaParametry", "OdpornoscNaWarunkiAtmosferyczne", c => c.String());
            AddColumn("Towar.tblTowarGeowlokninaParametry", "OdpornoscNaUtlenianie", c => c.Int(nullable: false));
            AddColumn("Towar.tblTowarGeowlokninaParametry", "CzyUV", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Towar.tblTowarGeowlokninaParametry", "CzyUV");
            DropColumn("Towar.tblTowarGeowlokninaParametry", "OdpornoscNaUtlenianie");
            DropColumn("Towar.tblTowarGeowlokninaParametry", "OdpornoscNaWarunkiAtmosferyczne");
            DropColumn("Towar.tblTowarGeowlokninaParametry", "WodoprzepuszczalnoscWPlaszczyznie_200kPa_Minimum");
            DropColumn("Towar.tblTowarGeowlokninaParametry", "WodoprzepuszczalnoscWPlaszczyznie_200kPa");
            DropColumn("Towar.tblTowarGeowlokninaParametry", "WodoprzepuszczalnoscWPlaszczyznie_100kPa_Minimum");
            DropColumn("Towar.tblTowarGeowlokninaParametry", "WodoprzepuszczalnoscWPlaszczyznie_100kPa");
            DropColumn("Towar.tblTowarGeowlokninaParametry", "WodoprzepuszczalnoscWPlaszczyznie_20kPa_Minimum");
            DropColumn("Towar.tblTowarGeowlokninaParametry", "WodoprzepuszczalnoscWPlaszczyznie_20kPa");
        }
    }
}
