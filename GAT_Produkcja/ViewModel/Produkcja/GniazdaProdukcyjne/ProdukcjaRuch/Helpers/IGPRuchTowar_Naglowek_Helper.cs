using GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.Helpers;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Helpers
{
    public interface IGPRuchTowar_Naglowek_Helper
    {
        IMagazynRuchNaglowekSaveHelper MagazynRuchNaglowekSaveHelper { get; }
        IMagazynRuchTowarSaveHelper MagazynRuchTowarSaveHelper { get; }
    }
}