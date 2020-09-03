using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers
{
    public interface IKonfekcjaHelper
    {
        Task<IEnumerable<tblProdukcjaRuchTowar>> PobierzKonfekcjeDoRozliczenia();

        /// <summary>
        /// Pobiera liste pozycji konfekcji dla konkrentego zlecenia
        /// </summary>
        /// <param name="idZlecenia"></param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<tblProdukcjaRuchTowar>> PobierzListeKonfekcjiDlaZlecenia(int idZlecenia);
        
        /// <summary>
        /// Pobiera skladniki mieszanki (z dostepnego surowca) oraz generuje encje rozliczenia <see cref="tblProdukcjaRozliczenie_RW"/>
        /// </summary>
        /// <param name="wybranaPozycjaKonfekcjiDoRozliczenia"></param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<tblProdukcjaRozliczenie_RW>> GenerujRozliczenieRWAsync(tblProdukcjaRozliczenie_PW wybranaPozycjaKonfekcjiDoRozliczenia);

        /// <summary>
        /// Pobiera cene mieszanki dla zlecenia produkcyjnego
        /// </summary>
        /// <param name="idZlecenieProdukcyjne"></param>
        /// <returns>cene</returns>
        Task<decimal> PobierzCeneMieszankiDlaZleceniaProdukcji(int idZlecenieProdukcyjne);

    }
}