using Dapper.FluentMap.Dommel.Mapping;
using GAT_Produkcja.dbMsAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Mappers
{
    public class NormyZuzyciaMapper : DommelEntityMap<NormyZuzycia>
    {
        public NormyZuzyciaMapper()
        {
            Map(a => a.Id).ToColumn("Id");
            Map(a => a.ZlecenieID).ToColumn("[Nr zlecenia]");
            Map(a => a.Artykul).ToColumn("[Artykuł]");
            Map(a => a.Dostawca).ToColumn("Dostawca");
            Map(a => a.Surowiec).ToColumn("Surowiec");
            Map(a => a.Ilosc).ToColumn("[Ilość]");
            ToTable("Normy zużycia");
        }
    }
}
