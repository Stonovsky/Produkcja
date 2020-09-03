using Dapper.FluentMap.Dommel.Mapping;
using GAT_Produkcja.dbMsAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Mappers
{
    public class KalanderMapper : DommelEntityMap<Kalander>
    {
        public KalanderMapper()
        {
            Map(a => a.Id).ToColumn("Id");
            Map(a => a.ZlecenieID).ToColumn("[Nr zlecenia]");
            Map(a => a.NrSztuki).ToColumn("[Nr sztuki]");
            Map(a => a.SzerokoscIgiel).ToColumn("[Szer igł]");
            Map(a => a.WagaIgiel).ToColumn("[Waga igł]");
            Map(a => a.DlugoscIgiel).ToColumn("[Długość igł]");
            Map(a => a.Data).ToColumn("Data");
            Map(a => a.Godzina).ToColumn("Godz");
            Map(a => a.OperatorID).ToColumn("Operator");
            Map(a => a.PomocOperatoraID).ToColumn("[Pomoc operatora]");
            Map(a => a.Szerokosc).ToColumn("Szerokość");
            Map(a => a.Dlugosc).ToColumn("Długość");
            Map(a => a.Waga).ToColumn("Waga");
            Map(a => a.WagaKrajki).ToColumn("[Waga krajki]");
            Map(a => a.Odpad).ToColumn("[Odpad]");
            Map(a => a.WagaOdpadu).ToColumn("[Waga odp]");
            Map(a => a.KodOdpadu).ToColumn("[Kod odpadu]");
            Map(a => a.Konfekcja).ToColumn("[Konfekcja]");
            Map(a => a.IloscM2).ToColumn("[M2]");
            ToTable("Kalander");

        }
    }
}
