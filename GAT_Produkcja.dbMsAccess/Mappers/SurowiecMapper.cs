using Dapper.FluentMap.Dommel.Mapping;
using GAT_Produkcja.dbMsAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Mappers
{
    public class SurowiecMapper : DommelEntityMap<Surowiec>
    {
        public SurowiecMapper()
        {
            Map(a => a.Id).ToColumn("Id");
            Map(a => a.LP).ToColumn("Lp");
            Map(a => a.NazwaSurowca).ToColumn("[Nazwa surowca]");
            ToTable("Surowiec");

        }
    }
}
