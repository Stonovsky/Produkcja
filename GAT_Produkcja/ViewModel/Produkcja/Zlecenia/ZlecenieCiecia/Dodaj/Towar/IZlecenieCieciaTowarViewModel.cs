using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.Zlecenia.ZlecenieCiecia.Dodaj.Towar
{
    public interface IZlecenieCieciaTowarViewModel
    {
        Task DeleteAsync(int idZlecenieCiecia);
        bool IsValid {get;}
        Task LoadAsync(int? idZlecenieCiecia);
        Task SaveAsync(int? idZlecenieCiecia);
    }
}