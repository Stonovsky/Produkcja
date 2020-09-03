using GAT_Produkcja.db.Enums;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Magazyn.Helpers
{
    public interface ITblRuchNaglowekHelper
    {
        Task<NrDokumentuRuchNaglowek> NrDokumentuGenerator(StatusRuchuTowarowEnum statusRuchuTowarowEnum);
    }
}