namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanoKolumne_IDTowar_tblProdukcjaRuchTowar : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "IDTowarGeowlokninaParametryGramatura", "Towar.tblTowarGeowlokninaParametryGramatura");
            DropForeignKey("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "IDTowarGeowlokninaParametrySurowiec", "Towar.tblTowarGeowlokninaParametrySurowiec");
            DropIndex("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", new[] { "IDTowarGeowlokninaParametryGramatura" });
            DropIndex("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", new[] { "IDTowarGeowlokninaParametrySurowiec" });
            AddColumn("Produkcja.tblProdukcjaRuchTowar", "IDTowar", c => c.Int());
            AlterColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "IDTowarGeowlokninaParametryGramatura", c => c.Int());
            AlterColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "IDTowarGeowlokninaParametrySurowiec", c => c.Int());
            CreateIndex("Produkcja.tblProdukcjaRuchTowar", "IDTowar");
            CreateIndex("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "IDTowarGeowlokninaParametryGramatura");
            CreateIndex("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "IDTowarGeowlokninaParametrySurowiec");
            AddForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDTowar", "Towar.tblTowar", "IDTowar");
            AddForeignKey("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "IDTowarGeowlokninaParametryGramatura", "Towar.tblTowarGeowlokninaParametryGramatura", "IDTowarGeowlokninaParametryGramatura");
            AddForeignKey("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "IDTowarGeowlokninaParametrySurowiec", "Towar.tblTowarGeowlokninaParametrySurowiec", "IDTowarGeowlokninaParametrySurowiec");
        }
        
        public override void Down()
        {
            DropForeignKey("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "IDTowarGeowlokninaParametrySurowiec", "Towar.tblTowarGeowlokninaParametrySurowiec");
            DropForeignKey("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "IDTowarGeowlokninaParametryGramatura", "Towar.tblTowarGeowlokninaParametryGramatura");
            DropForeignKey("Produkcja.tblProdukcjaRuchTowar", "IDTowar", "Towar.tblTowar");
            DropIndex("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", new[] { "IDTowarGeowlokninaParametrySurowiec" });
            DropIndex("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", new[] { "IDTowarGeowlokninaParametryGramatura" });
            DropIndex("Produkcja.tblProdukcjaRuchTowar", new[] { "IDTowar" });
            AlterColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "IDTowarGeowlokninaParametrySurowiec", c => c.Int(nullable: false));
            AlterColumn("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "IDTowarGeowlokninaParametryGramatura", c => c.Int(nullable: false));
            DropColumn("Produkcja.tblProdukcjaRuchTowar", "IDTowar");
            CreateIndex("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "IDTowarGeowlokninaParametrySurowiec");
            CreateIndex("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "IDTowarGeowlokninaParametryGramatura");
            AddForeignKey("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "IDTowarGeowlokninaParametrySurowiec", "Towar.tblTowarGeowlokninaParametrySurowiec", "IDTowarGeowlokninaParametrySurowiec", cascadeDelete: true);
            AddForeignKey("Produkcja.tblProdukcjaZlecenieProdukcyjne_GniazdaProdParamTowar", "IDTowarGeowlokninaParametryGramatura", "Towar.tblTowarGeowlokninaParametryGramatura", "IDTowarGeowlokninaParametryGramatura", cascadeDelete: true);
        }
    }
}
