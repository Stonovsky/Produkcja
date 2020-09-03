using GAT_Produkcja.db;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.RW
{
    public interface IRozliczenieSQL_RW_Helper
    {
        Task DodajIlosciKgIWartoscDoRW(ObservableCollection<tblProdukcjaRozliczenie_RW> listaRWSurowca, ObservableCollection<tblProdukcjaRozliczenie_PW> listaPwWyrobuGotowego);
        Task<decimal> GenerujCeneMieszanki(int idZlecenieProdukcyjne);
        Task<IEnumerable<tblProdukcjaRozliczenie_RW>> GenerujRozliczenieRWAsync(tblProdukcjaRozliczenie_PW wybranaPozycjaKonfekcjiDoRozliczenia);
        Task LoadAsync();
        Task<decimal> PobierzWageOdpaduDlaZlecenia(int idZlecenieProdukcyjne);
        RwPodsumowanieModel PodsumujRW(IEnumerable<tblProdukcjaRozliczenie_RW> listaRW);
        Task ZaksiegujOdpadJakoRozliczony();
        Task<tblProdukcjaRozliczenie_PW> GenerujOdpadDlaPW(int idZlecenie);
        decimal GenerujCeneMieszanki(IEnumerable<tblProdukcjaRozliczenie_RW> listaRW);
    }
}