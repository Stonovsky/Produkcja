using GAT_Produkcja.db;
using System.Collections.Generic;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers
{
    public interface IRozliczenieSQLHelper
    {
        IEnumerable<tblProdukcjaRozliczenie_PW> GenerujRozliczeniePW(IEnumerable<tblProdukcjaRuchTowar> listaPozycjiKonfekcji, decimal cenaMieszanki);
        IEnumerable<tblProdukcjaRozliczenie_PW> PodsumujPWPodzialTowar(IEnumerable<tblProdukcjaRozliczenie_PW> listaPW);

    }
}