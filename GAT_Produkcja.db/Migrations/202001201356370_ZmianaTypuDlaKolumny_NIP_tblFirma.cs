namespace GAT_Produkcja.db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaTypuDlaKolumny_NIP_tblFirma : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblFirma", "NIP", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblFirma", "NIP", c => c.Short());
        }
    }
}
