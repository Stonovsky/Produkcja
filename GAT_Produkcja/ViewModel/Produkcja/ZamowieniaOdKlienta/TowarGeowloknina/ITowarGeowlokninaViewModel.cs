using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;
using System.Collections.ObjectModel;

namespace GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.TowarGeowloknina
{
    public interface ITowarGeowlokninaViewModel
    {
        bool IsValid { get; }
        RelayCommand PoEdycjiKomorkiCommand { get; set; }
        ObservableCollection<tblZamowienieHandloweTowarGeowloknina> ListaPozycjiGeowloknin { get; set; }

    }
}