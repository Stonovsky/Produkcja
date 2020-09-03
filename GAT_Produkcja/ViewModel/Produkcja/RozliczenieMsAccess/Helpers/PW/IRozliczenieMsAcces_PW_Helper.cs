using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.PW
{
    public interface IRozliczenieMsAcces_PW_Helper
    {
        tblProdukcjaRozliczenie_PW GenerujOdpadDlaPW(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW);


        /// <summary>
        /// Metoda generujaca opdad na bazie rozchodowanych rolek RW i przypisanych do nich odpadow
        /// </summary>
        /// <param name="idZlecenie"></param>
        /// <returns><see cref="tblProdukcjaRozliczenie_PW"/></returns>
        Task<tblProdukcjaRozliczenie_PW> GenerujOdpadDlaPW(int idZlecenie);



        /// <summary>
        /// Zwaraca liste rozliczen PW <see cref="tblProdukcjaRozliczenie_PW"/> na potrzeby EwidencjiProdukcji i grupowania pozycji
        /// </summary>
        /// <param name="listaPozycjiGniazda">pozycje gniazda produkcyjnego implementujacego interface <see cref="IGniazdoProdukcyjne"/> z bazy MsAccess</param>
        /// <returns></returns>
        IEnumerable<tblProdukcjaRozliczenie_PW> GenerujRozliczeniePW(IEnumerable<IGniazdoProdukcyjne> listaPozycjiKonfekcji, decimal cenaMieszanki);

        /// <summary>
        /// Generuje rozliczenie PW
        /// </summary>
        /// <param name="listaPozycjiKonfekcji"> lista pozycji konfekcji do rozliczenia</param>
        /// <param name="cenaMieszanki"></param>
        /// <returns></returns>
        IEnumerable<tblProdukcjaRozliczenie_PW> GenerujRozliczeniePW(IEnumerable<IProdukcjaRuchTowar> listaPozycjiKonfekcji, decimal cenaMieszanki);


        string GenerujRozliczoneTowary(IEnumerable<tblProdukcjaRozliczenie_PW> listaPodsumowanPWTowar);

        /// <summary>
        /// Laduje asynchronicznie wszystie niezbedne dane dla klasy
        /// </summary>
        /// <returns></returns>
        Task LoadAsync();


        PwPodsumowanieModel PodsumujListe(IEnumerable<Konfekcja> lista);

        /// <summary>
        /// Podsumowuje przeslana liste
        /// </summary>
        /// <param name="lista">lista elementow konfekcji, typ <see cref="tblProdukcjaRozliczenie_PW"/></param>
        /// <returns></returns>
        PwPodsumowanieModel PodsumujListe(IEnumerable<tblProdukcjaRozliczenie_PW> lista);
        PwPodsumowanieModel PodsumujPW(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW);
        IEnumerable<tblProdukcjaRozliczenie_PW> PodsumujPWPodzialTowar(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW);
    }
}