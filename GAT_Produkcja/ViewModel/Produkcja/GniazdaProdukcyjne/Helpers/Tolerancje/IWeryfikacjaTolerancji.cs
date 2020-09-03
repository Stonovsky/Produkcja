using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers.WeryfikacjaTolerancji;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers.Tolerancje
{
    public interface IWeryfikacjaTolerancji
    {
        Task<WeryfikacjaTolerancjiResult> CzyParametrZgodny(int idTowar, GeowlokninaParametryEnum parametr, int rzeczywistaWartoscParametru);
    }
}