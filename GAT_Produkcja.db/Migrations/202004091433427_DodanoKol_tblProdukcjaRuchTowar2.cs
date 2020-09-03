namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKol_tblProdukcjaRuchTowar2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieProdukcyjne", c => c.Int());
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "TowarNazwaMsAccess", c => c.String());
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "TowarNazwaSubiekt", c => c.String());
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "TowarSymbolSubiekt", c => c.String());
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "SurowiecSkrot", c => c.String());
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "ZlecenieNazwa", c => c.String());
            CreateIndex("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieProdukcyjne");
            AddForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieProdukcyjne", "Produkcja.tblProdukcjaZlcecenieProdukcyjne", "IDProdukcjaZlecenieProdukcyjne");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieProdukcyjne", "Produkcja.tblProdukcjaZlcecenieProdukcyjne");
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDProdukcjaZlecenieProdukcyjne" });
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "ZlecenieNazwa");
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "SurowiecSkrot");
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "TowarSymbolSubiekt");
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "TowarNazwaSubiekt");
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "TowarNazwaMsAccess");
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "IDProdukcjaZlecenieProdukcyjne");
        }
    }
}
