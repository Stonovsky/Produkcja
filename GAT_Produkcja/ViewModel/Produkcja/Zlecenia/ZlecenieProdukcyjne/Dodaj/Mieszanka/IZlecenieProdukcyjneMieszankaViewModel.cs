using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Mieszanka
{
    public interface IZlecenieProdukcyjneMieszankaViewModel
    {
        bool IsChanged { get; set; }
        bool IsValid { get; set; }

        Task DeleteAsync(int idZleceniaProdukcyjnego);
        void IsChanged_False();
        Task LoadAsync(int? idZleceniaProdukcyjnego);
        Task SaveAsync(int? idZleceniaProdukcyjnego);
    }
}