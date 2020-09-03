using System.Collections.Generic;
using GAT_Produkcja.db;

namespace GAT_Produkcja.ViewModel.Zapotrzebowanie.Pliki
{
    public interface IPlikiCRUD
    {
        List<tblPliki> PobierzListePlikowDoDodania(object obj);
        void UsunPlikZSerwera(IEnumerable<tblPliki> pliki);
        void KopiujPlikNaSerwer(tblPliki plik);
    }
}