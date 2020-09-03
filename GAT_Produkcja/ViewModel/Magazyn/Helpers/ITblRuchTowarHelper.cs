using GAT_Produkcja.db;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Magazyn.Helpers
{
    public interface ITblRuchTowarHelper
    {
        tblRuchTowar Towar { get; set; }

        Task DodajDoBazyDanych(tblRuchTowar towar, tblRuchStatus statusRuchu, tblRuchNaglowek naglowek);
    }
}