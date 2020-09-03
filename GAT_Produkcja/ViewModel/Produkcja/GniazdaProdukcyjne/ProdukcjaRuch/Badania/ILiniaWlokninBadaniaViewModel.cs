using GAT_Produkcja.db;
using System.Threading.Tasks;

namespace GAT_Produkcja.ViewModel.Produkcja.GniazdaProdukcyjne.ProdukcjaRuch.Badania
{
    public interface ILiniaWlokninBadaniaViewModel
    {
        tblProdukcjaGniazdoWlokninaBadania Badanie { get; set; }
        tblProdukcjaGniazdoWlokninaBadania BadanieOrg { get; set; }
        bool IsChanged { get; }
        bool IsValid { get; }

        Task DeleteAsync(int id);
        void IsChanged_False();
        Task LoadAsync(int? id);
        Task SaveAsync(int? idProdukcjaGniazdoWloknin);
    }
}