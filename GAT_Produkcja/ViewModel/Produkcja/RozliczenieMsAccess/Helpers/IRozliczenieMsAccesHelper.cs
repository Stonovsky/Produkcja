using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.EntitesInterfaces;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.Utilities.EppFile;
using GAT_Produkcja.Utilities.ExcelReport;
using GAT_Produkcja.Utilities.Wrappers;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers
{
    public interface IRozliczenieMsAccesHelper
    {
        IXlsService ExcelReportGenerator { get; }
        IEppFileGenerator EppFileGenerator { get; }
        IDirectoryHelper DirectoryHelper{ get; }

        /// <summary>
        /// Generuje cene mieszanki na podstawie IdZlecenia
        /// </summary>
        /// <param name="idZlecenia"></param>
        /// <returns></returns>
        Task<decimal> GenerujCeneMieszanki(int idZlecenia);

        /// <summary>
        /// Generuje cene mieszanki na podstawie list RW
        /// </summary>
        /// <param name="listaRW"><see cref="IEnumerable{T}"/></param>
        /// <returns><see cref="decimal"/></returns>
        decimal GenerujCeneMieszanki(IEnumerable<tblProdukcjaRozliczenie_RW> listaRW);

        /// <summary>
        /// Tworzy pozycje odpadu dla przekazanej listy PW. Sumuje odpad z listy przeslanej jako argument, tworzy nazwe i symbol towaru zgodny z Subiekt GT
        /// </summary>
        /// <param name="listaPW">lista PW </param>
        /// <returns></returns>
        tblProdukcjaRozliczenie_PW GenerujOdpadDlaPW(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW);


        /// <summary>
        /// Metoda generujaca opdad na bazie rozchodowanych rolek RW i przypisanych do nich odpadow
        /// </summary>
        /// <param name="idZlecenie"></param>
        /// <returns><see cref="tblProdukcjaRozliczenie_PW"/></returns>
        Task<tblProdukcjaRozliczenie_PW> GenerujOdpadDlaPW(int idZlecenie);


        /// <summary>
        /// Zwraca Liste rozliczen PW <see cref="tblProdukcjaRozliczenie_PW"/>, wykorzystywana do konwersji listy dostepnych pozycji do rozliczenia
        /// </summary>
        /// <param name="listaPozycjiKonfekcji">pozycje konfekcji z MsAccess</param>
        /// <param name="cenaKgMieszanki">cena mnieszanki za kg</param>
        /// <returns></returns>
        //IEnumerable<tblProdukcjaRozliczenie_PW> GenerujRozliczeniePW(IEnumerable<Konfekcja> listaPozycjiKonfekcji, decimal cenaKgMieszanki);

        /// <summary>
        /// Zwaraca liste rozliczen PW <see cref="tblProdukcjaRozliczenie_PW"/> na potrzeby EwidencjiProdukcji
        /// </summary>
        /// <param name="listaPozycjiGniazda">pozycje gniazda produkcyjnego implementujacego interface <see cref="IGniazdoProdukcyjne"/> z bazy MsAccess</param>
        /// <returns></returns>
        IEnumerable<tblProdukcjaRozliczenie_PW> GenerujRozliczeniePW(IEnumerable<IGniazdoProdukcyjne> listaPozycjiGniazda, decimal cenaKgMiesanki);


        IEnumerable<tblProdukcjaRozliczenie_PW> GenerujRozliczeniePW(IEnumerable<IProdukcjaRuchTowar> listaPozycjiKonfekcji, decimal cenaKgMieszanki);


        /// <summary>
        /// Tworzy rozliczenie RW dla pozycji mieszanki wraz z idTowaru z Subiekt oraz udzialami
        /// </summary>
        /// <param name="wybranaPozycjaKonfekcjiDoRozliczenia">Wybrana pozycja konfekcji dla ktorej prowadzimy rozliczenie</param>
        /// <returns></returns>
        Task<IEnumerable<tblProdukcjaRozliczenie_RW>> GenerujRozliczenieRWAsync(tblProdukcjaRozliczenie_PW wybranaPozycjaKonfekcjiDoRozliczenia);

        Task LoadAsync();

        /// <summary>
        /// Dodaje do listy RW ilosci zgodnie z udzialem. 
        /// Sumaryczna ilosc kg bierze z PW jako sume z wagi i sume z odpadu dla wszystkich elementow
        /// </summary>
        /// <param name="listaRWSurowca"></param>
        /// <param name="listaPwWyrobuGotowego"></param>
        Task DodajIlosciKgIWartoscDoRW(ObservableCollection<tblProdukcjaRozliczenie_RW> listaRWSurowca, ObservableCollection<tblProdukcjaRozliczenie_PW> listaPwWyrobuGotowego);

        /// <summary>
        /// Generuje podsumowanie RW
        /// </summary>
        /// <param name="listaRW"></param>
        /// <returns></returns>
        RwPodsumowanieModel PodsumujRW(IEnumerable<tblProdukcjaRozliczenie_RW> listaRW);

        /// <summary>
        /// Generuje podsumowanie PW
        /// </summary>
        /// <param name="listaPW"></param>
        /// <returns></returns>
        PwPodsumowanieModel PodsumujPW(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW);

        /// <summary>
        /// Podsumowuje rozliczenie PW z podziałem na towar na potrzeby exportu do SubiektGT
        /// </summary>
        /// <param name="listaPW"> lista encji <see cref="tblProdukcjaRozliczenie_PW"/></param>
        /// <returns></returns>
        IEnumerable<tblProdukcjaRozliczenie_PW> PodsumujPWPodzialTowar(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW);


        /// <summary>
        /// Podsumowuje przeslana liste konfekcji wg zadanych parametrow
        /// </summary>
        /// <param name="lista">Lista elementow konfekcji, typ <see cref="Konfekcja"/></param>
        /// <returns></returns>
        PwPodsumowanieModel PodsumujListe(IEnumerable<Konfekcja> lista);
        /// <summary>
        /// Podsumowuje przeslana liste konfekcji
        /// </summary>
        /// <param name="lista">lista elementow konfekcji, typ <see cref="tblProdukcjaRozliczenie_PW"/></param>
        /// <returns></returns>
        PwPodsumowanieModel PodsumujListe(IEnumerable<tblProdukcjaRozliczenie_PW> lista);

        /// <summary>
        /// Pobiera liste konfekcji dla danego zlecenia wraz z klauzulami WHERE zgodnie z zalozonymi kryteriami
        /// </summary>
        /// <param name="idZlecenia">Id zlecenia z MsAccess</param>
        /// <returns></returns>
        Task<IEnumerable<Konfekcja>> PobierzListeKonfekcjiDlaZlecenia(int idZlecenia);

        /// <summary>
        /// Pobiera konfekcje do rozliczenia z bazy MS Access
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IProdukcjaRuchTowar>> PobierzKonfekcjeDoRozliczenia();

        /// <summary>
        /// Metoda zwracajaca liste <see cref="tblProdukcjaRozliczenie_PW"/> wraz z obliczeniem wartosci
        /// </summary>
        /// <param name="listaPobranaZMsAccess">lista pobrana jako <see cref="IGniazdoProdukcyjne"/> bezposrednio z bazy MsAccess</param>
        /// <returns></returns>
        Task<IEnumerable<tblProdukcjaRozliczenie_PW>> GenerujZgrupowanaListePoNazwieZObliczeniemKosztow(IEnumerable<IGniazdoProdukcyjne> listaPobranaZMsAccess);

    }
}