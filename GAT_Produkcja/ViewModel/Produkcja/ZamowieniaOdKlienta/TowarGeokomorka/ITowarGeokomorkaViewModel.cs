using GAT_Produkcja.db;
using System.Collections.ObjectModel;

namespace GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.TowarGeokomorka
{
    public interface ITowarGeokomorkaViewModel
    {
        bool IsValid { get; set; }
        ObservableCollection<tblZamowienieHandloweTowarGeokomorka> ListaPozycjiGeokomorek { get; set; }

    }
}