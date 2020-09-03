using GAT_Produkcja.db;
using Org.BouncyCastle.Crypto.Macs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Magazyn.Ewidencje.Adapters
{
    public class MagazynRuchGTXGroupAdapter : vwMagazynRuchGTX
    {
        private readonly IEnumerable<vwMagazynRuchGTX> listaRuchuMagazynowGTX;

        #region CTOR
        public MagazynRuchGTXGroupAdapter(IEnumerable<vwMagazynRuchGTX> listaRuchuMagazynowGTX)
        {
            this.listaRuchuMagazynowGTX = listaRuchuMagazynowGTX;
        }
        #endregion

        /// <summary>
        /// Grupuje <see cref="vwMagazynRuchGTX" po idTowar/>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<vwMagazynRuchGTX> PobierzRuchZgrupowanyPoIdTowar()
        {
            return listaRuchuMagazynowGTX.GroupBy(g => new { g.IdTowar })
                                         .Select(se => new vwMagazynRuchGTX
                                         {
                                             IdTowar = se.First().IdTowar,
                                             Data = se.First().Data,
                                             Firma = se.First().Firma,
                                             IdMagazyn = se.First().IdMagazyn,
                                             IdMagazynRuch = se.First().IdMagazynRuch,
                                             MagazynNazwa = se.First().MagazynNazwa,
                                             MagazynSymbol = se.First().MagazynSymbol,
                                             TowarNazwa = se.First().TowarNazwa,
                                             TowarSymbol = se.First().TowarSymbol,
                                             Ilosc = se.Sum(s => s.Ilosc),
                                             Pozostalo = se.Sum(s => s.Pozostalo),
                                             Jm = se.First().Jm,
                                             Wartosc = se.Sum(s => s.Wartosc),
                                             Cena = se.Average(s => s.Cena),
                                         }).ToList();
        }
    }
}
