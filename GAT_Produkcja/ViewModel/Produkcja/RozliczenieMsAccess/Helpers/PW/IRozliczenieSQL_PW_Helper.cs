using GAT_Produkcja.db;
using GAT_Produkcja.db.EntitesInterfaces;
using GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Modele;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers.PW
{
    public interface IRozliczenieSQL_PW_Helper
    {
        tblProdukcjaRozliczenie_PW GenerujOdpadDlaPW(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW);
        Task<tblProdukcjaRozliczenie_PW> GenerujOdpadDlaPW(int idZlecenie);
        IEnumerable<tblProdukcjaRozliczenie_PW> GenerujRozliczeniePW(IEnumerable<IProdukcjaRuchTowar> listaPozycjiKonfekcji, decimal cenaMieszanki);
        string GenerujRozliczoneTowary(IEnumerable<tblProdukcjaRozliczenie_PW> listaPodsumowanPWTowar);
        Task LoadAsync();
        PwPodsumowanieModel PodsumujListe(IEnumerable<tblProdukcjaRozliczenie_PW> lista);
        PwPodsumowanieModel PodsumujPW(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW);
        IEnumerable<tblProdukcjaRozliczenie_PW> PodsumujPWPodzialTowar(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW);
    }
}