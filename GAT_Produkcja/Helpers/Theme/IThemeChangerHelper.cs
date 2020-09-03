using System.Threading.Tasks;

namespace GAT_Produkcja.Helpers.Theme
{
    public interface IThemeChangerHelper
    {
        void ChangeTheme(ThemeColorEnum color);
        string GetAcctualThemeName();
        Task SetThemeFromDB();
        Task AddToDataBase(int? idPracownika, ThemeColorEnum themeColorEnum);


    }
}