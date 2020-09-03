namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoTabele_tblProdukcjaZlecenieProdukcyjne_Mieszanka : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka",
                c => new
                    {
                        IDZlecenieProdukcyjneMieszanka = c.Int(nullable: false, identity: true),
                        IDProdukcjaZlecenieProdukcyjne = c.Int(),
                        IDTowar = c.Int(nullable: false),
                        ZawartoscProcentowa = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IloscKg = c.Decimal(precision: 18, scale: 2),
                        IloscSumarycznaKg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IDJm = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDZlecenieProdukcyjneMieszanka)
                .ForeignKey("dbo.tblJm", t => t.IDJm, cascadeDelete: true)
                .ForeignKey("Produkcja.tblProdukcjaZlcecenieProdukcyjne", t => t.IDProdukcjaZlecenieProdukcyjne)
                .ForeignKey("Towar.tblTowar", t => t.IDTowar, cascadeDelete: true)
                .Index(t => t.IDProdukcjaZlecenieProdukcyjne)
                .Index(t => t.IDTowar)
                .Index(t => t.IDJm);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IDTowar", "Towar.tblTowar");
            DropForeignKey("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IDProdukcjaZlecenieProdukcyjne", "Produkcja.tblProdukcjaZlcecenieProdukcyjne");
            DropForeignKey("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", "IDJm", "dbo.tblJm");
            DropIndex("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", new[] { "IDJm" });
            DropIndex("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", new[] { "IDTowar" });
            DropIndex("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka", new[] { "IDProdukcjaZlecenieProdukcyjne" });
            DropTable("Produkcja.tblProdukcjaZlecenieProdukcyjne_Mieszanka");
        }
    }
}
