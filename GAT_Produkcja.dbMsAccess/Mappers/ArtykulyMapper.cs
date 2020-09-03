using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;
using GAT_Produkcja.dbMsAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GAT_Produkcja.dbMsAccess.Mappers
{
    public class ArtykulyMapper: DommelEntityMap<Artykuly>
    {
        public ArtykulyMapper()
        {
            Map(a => a.Id).ToColumn("Id");
            Map(a => a.NazwaArtykulu).ToColumn("Nazwa_art");
            Map(a => a.Opis).ToColumn("Opis");
            ToTable("Artykuły");
        }
    }
}
