using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.UI.Utilities.Dostep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GAT_Produkcja.Helpers.Theme
{

    public class ThemeChangerHelper : IThemeChangerHelper
    {
        private readonly IUnitOfWork unitOfWork;
        private Uri uri;
        private App app;

        public ThemeChangerHelper(IUnitOfWork unitOfWork)
        {
            app = (App)Application.Current;
            this.unitOfWork = unitOfWork;
        }

        public void ChangeTheme(ThemeColorEnum color)
        {
            switch (color)
            {
                case ThemeColorEnum.Dark:
                    uri = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml");
                    Change();
                    break;
                case ThemeColorEnum.Light:
                    uri = new Uri(@"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml");
                    Change();
                    break;
                default:
                    break;
            }
        }

        public string GetAcctualThemeName()
        {
            var dictionaries = app.GetThemeDictionariesByName("MaterialDesignThemes");
            return dictionaries.First().Source.OriginalString;
        }
        private void Change()
        {
            var dictionaries = app.GetThemeDictionariesByName("MaterialDesignThemes");
            app.ChangeTheme(dictionaries.First(), uri);
        }

        public async Task AddToDataBase(int? idPracownika, ThemeColorEnum themeColorEnum)
        {
            if (idPracownika == null ||
                idPracownika == 0)
                return;

            var pracownik = await unitOfWork.tblPracownikGAT.GetByIdAsync(idPracownika.Value);

            if (pracownik.MotywKoloru != (int)themeColorEnum)
            {
                pracownik.MotywKoloru = (int)themeColorEnum;

                await unitOfWork.SaveAsync();
            }
        }

        public async Task SetThemeFromDB()
        {
            var pracownik = await unitOfWork.tblPracownikGAT.GetByIdAsync(UzytkownikZalogowany.Uzytkownik.ID_PracownikGAT);

            ChangeTheme((ThemeColorEnum)pracownik.MotywKoloru);
        }
    }
}
