using GAT_Produkcja.db;
using GAT_Produkcja.dbMsAccess.Models;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.RW
{
    public interface IRozliczenieMsAcces_RW_Helper
    {
        Task DodajIlosciKgIWartoscDoRW(ObservableCollection<tblProdukcjaRozliczenie_RW> listaRWSurowca, 
                                       ObservableCollection<tblProdukcjaRozliczenie_PW> listaPwWyrobuGotowego);
        Task<IEnumerable<tblProdukcjaRozliczenie_RW>> GenerujRozliczenieRWAsync(tblProdukcjaRozliczenie_PW wybranaPozycjaKonfekcjiDoRozliczenia);
        Task LoadAsync();
        int PobierzIdSurowcaZNazwyMsAccess(string nazwaSurowca);
        RwPodsumowanieModel PodsumujRW(IEnumerable<tblProdukcjaRozliczenie_RW> listaRW);
        Task<decimal> GenerujCeneMieszanki(IEnumerable<NormyZuzycia> mieszanka);
        Task<decimal> GenerujCeneMieszanki(int idZlecenia);
    }
}