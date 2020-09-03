namespace GAT_Produkcja.db.Migrations
{
    using GAT_Produkcja.db.Enums;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GAT_ProdukcjaModel>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GAT_ProdukcjaModel context)
        {
            //context.tblTowar.AddOrUpdate(x => x.IDTowar, new tblTowar { });


            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
