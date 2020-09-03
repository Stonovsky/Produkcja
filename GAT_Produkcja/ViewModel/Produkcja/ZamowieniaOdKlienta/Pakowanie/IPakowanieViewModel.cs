using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.CommandWpf;
using GAT_Produkcja.db;

namespace GAT_Produkcja.ViewModel.Produkcja.ZamowieniaOdKlienta.Pakowanie
{
    public interface IPakowanieViewModel
    {
        bool IsValid { get; }
        ObservableCollection<tblZamowienieHandlowePakowanie> ListaPakowanie { get; set; }
        IEnumerable<tblZamowienieHandlowePakowanieRodzaj> ListaRodzajowPakowania { get; set; }
        RelayCommand UsunCommand { get; set; }
        tblZamowienieHandlowePakowanie WybranePakowanie { get; set; }
        tblZamowienieHandlowePakowanieRodzaj WybranyRodzajPakowania { get; set; }
        RelayCommand ZaladujPodczasUruchomieniaCommand { get; set; }
    }
}