using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GAT_Produkcja.db;

namespace GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny
{
    public interface IBadaniaGeowlokninRepository
    {
        Task DodajBadaniaDoBazyAsync(List<tblWynikiBadanGeowloknin> listaBadan);
        Task<ObservableCollection<tblWynikiBadanGeowloknin>> PobierzListeBadan();
        Task<tblWynikiBadanGeowloknin> PobierzWynikiBadaniaZBazyAsync(string sciezkaPliku);
        Task UaktualnijWynikiBadanWBazieAsync();
    }
}