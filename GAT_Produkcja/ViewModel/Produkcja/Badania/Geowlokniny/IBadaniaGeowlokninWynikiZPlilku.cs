using GAT_Produkcja.db;

namespace GAT_Produkcja.ViewModel.Badania.Geowlokniny
{
    public interface IBadaniaGeowlokninWynikiZPliku
    {
        tblWynikiBadanGeowloknin PobierzWynikiBadanZPlikuExcel(string sciezkaPliku);
    }
}