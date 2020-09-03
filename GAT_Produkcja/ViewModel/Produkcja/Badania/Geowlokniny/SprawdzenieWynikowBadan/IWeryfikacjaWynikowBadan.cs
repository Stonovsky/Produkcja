using System.Collections.Generic;
using System.Threading.Tasks;
using GAT_Produkcja.db;

namespace GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.WeryfikacjaWynikowBadan
{
    public interface IWeryfikacjaWynikowBadan
    {
        List<tblWynikiBadanGeowloknin> ListaBadan { get; set; }
        List<tblTowarGeowlokninaParametry> ListaParametrowWymaganych { get; set; }

        Task SprawdzCzyWynikiBadanWTolerancjach();
        Task SprawdzCzyWynikiBadanWTolerancjach(tblWynikiBadanGeowloknin badanie);

    }
}