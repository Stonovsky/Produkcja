using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.RuchTowar.PW
{
    public interface IGPRuchTowarPWViewModel
    {
        Task DeleteAsync(int idRuchNaglowek);
        void IsChanged_False();
        Task LoadAsync(int? idRuchNaglowek);
        Task SaveAsync(int? idRuchNaglowek);
        RelayCommand AddCommand { get; set; }

        bool IsValid { get; }
        bool IsChanged { get; }
    }
}