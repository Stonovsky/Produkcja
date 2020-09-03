using Dapper.FluentMap.Dommel.Mapping;
using GAT_Produkcja.dbMsAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.dbMsAccess.Mappers
{
    public class ProdukcjaMapper: DommelEntityMap<Produkcja>
    {
        public ProdukcjaMapper()
        {
            Map(a => a.Id).ToColumn("Id");
            Map(a => a.ZlecenieID).ToColumn("Nr zlecenia");
            Map(a => a.NrSztuki).ToColumn("Nr sztuki");
            Map(a => a.Data).ToColumn("Data");
            Map(a => a.Godzina).ToColumn("Godz");
            Map(a => a.Artykul).ToColumn("Artykuł");
            Map(a => a.OperatorID).ToColumn("Operator");
            Map(a => a.PomocOperatoraID).ToColumn("[Pomoc operatora]");
            Map(a => a.Szerokosc).ToColumn("Szerokość");
            Map(a => a.Dlugosc).ToColumn("Długość");
            Map(a => a.Waga).ToColumn("Waga");
            Map(a => a.Gramatura1).ToColumn("[Gramatura 1]");
            Map(a => a.Gramatura2).ToColumn("[Gramatura 2]");
            Map(a => a.Gramatura3).ToColumn("[Gramatura 3]");
            Map(a => a.Postoj).ToColumn("[Postój]");
            Map(a => a.WagaOdpadu).ToColumn("[Waga odp]");
            Map(a => a.Odpad).ToColumn("[Odpad]");
            Map(a => a.CzyProduktKalandowany).ToColumn("[Kalandrowanie]");
            ToTable("Produkcja");
        }
    }
}
