using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieProdukcyjne.Dodaj.Towar
{
    public interface IZlecenieProdukcyjneTowarViewModel
    {
        Task DeleteAsync(int idZlecenieProdukcyjne);
        void IsChanged_False();
        Task LoadAsync(int? idZlecenieProdukcyjne);
        Task SaveAsync(int? idZlecenieProdukcyjne);
    }
}