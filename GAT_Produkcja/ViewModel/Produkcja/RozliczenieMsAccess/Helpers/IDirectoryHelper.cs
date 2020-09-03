using GAT_Produkcja.db.EntitesInterfaces;
using System.Collections.Generic;

namespace GAT_Produkcja.ViewModel.Produkcja.RozliczenieMsAccess.Helpers
{
    public interface IDirectoryHelper
    {
        string GenerujSciezke(IEnumerable<IProdukcjaRozliczenie> listaRozliczenia);
    }
}