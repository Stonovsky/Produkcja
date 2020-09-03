using GAT_Produkcja.db;

namespace GAT_Produkcja.ViewModel.Produkcja.Badania.Geowlokniny.PobieranieBadanZPliku
{
    public interface IBadaniaGeowlokninWynikiZPliku
    {
        tblWynikiBadanGeowloknin PobierzWynikiBadanZPlikuExcel(string sciezkaPliku, tblWynikiBadanGeowloknin badanieZBazy = null);
    }
}