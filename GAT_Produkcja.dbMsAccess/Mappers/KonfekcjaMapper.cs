using Dapper.FluentMap.Dommel.Mapping;
using GAT_Produkcja.dbMsAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Mappers
{
    public class KonfekcjaMapper : DommelEntityMap<Konfekcja>
    {
        public KonfekcjaMapper()
        {
            Map(a => a.Id).ToColumn("Id");
            Map(a => a.ZlecenieID).ToColumn("[Nr zlecenia]");
            Map(a => a.NrSztuki).ToColumn("[Nr sztuki]");
            Map(a => a.Artykul).ToColumn("[Artykuł]");
            Map(a => a.Data).ToColumn("Data");
            Map(a => a.Godzina).ToColumn("Godz");
            Map(a => a.Numer).ToColumn("[Numer]");
            Map(a => a.NumerMG).ToColumn("[NumerMG]");
            Map(a => a.OperatorID).ToColumn("Operator");
            Map(a => a.PomocOperatoraID).ToColumn("[Pomoc operatora]");
            Map(a => a.Szerokosc).ToColumn("Szerokość");
            Map(a => a.Dlugosc).ToColumn("Długość");
            Map(a => a.Waga).ToColumn("Waga");
            Map(a => a.Gatunek).ToColumn("[Gatunek]");
            Map(a => a.Odpad).ToColumn("[Odpad]");
            Map(a => a.WagaOdpadu).ToColumn("[Waga odp]");
            Map(a => a.DataWG).ToColumn("[DataWG]");
            Map(a => a.NrWz).ToColumn("[Nr WZ]");
            Map(a => a.IloscM2).ToColumn("[M2]");
            Map(a => a.CzyZaksiegowano).ToColumn("[CzyZaksiegowano]");
            Map(a => a.Przychody).ToColumn("[Przychody]");
            ToTable("Konfekcja");
        }
    }
}
