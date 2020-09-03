using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Magazyn.Ewidencje.SubiektGT.Ewidencja.Decorators
{
    public class GrupowanieTowarowDecorator
    {
        private readonly IEnumerable<IMagazynRuchSubiekt> towary;
        #region CTOR
        public GrupowanieTowarowDecorator(IEnumerable<IMagazynRuchSubiekt> towary)
        {
            this.towary = towary;
        }
        #endregion

        public IEnumerable<IMagazynRuchSubiekt> Grupuj()
        {
            return towary.GroupBy(g => g.TowarSymbol)
                        .Select(se => new vwMagazynRuchAGG
                        {
                            MagazynNazwa = se.First().MagazynNazwa,
                            TowarSymbol = se.First().TowarSymbol,
                            TowarNazwa = se.First().TowarNazwa,
                            Ilosc = se.Sum(s => s.Ilosc),
                            Pozostalo = se.Sum(s => s.Pozostalo),
                            Jm = se.First().Jm,
                            Cena = se.Average(s => s.Cena),
                            Wartosc = se.Sum(s => s.Wartosc)
                        });
        }
    }
}
