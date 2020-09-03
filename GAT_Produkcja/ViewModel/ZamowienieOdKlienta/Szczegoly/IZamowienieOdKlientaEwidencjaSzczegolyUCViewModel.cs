using GAT_Produkcja.db;
using GAT_Produkcja.Helpers.Interfaces;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.ZamowienieOdKlienta.Szczegoly
{
    public interface IZamowienieOdKlientaEwidencjaSzczegolyUCViewModel : IButtonActive
    {
        ObservableCollection<vwZamOdKlientaAGG> ListaZamowienOdKlientow { get; set; }

        Task LoadAsync(int? id);
        void SetState(string name);
    }
}