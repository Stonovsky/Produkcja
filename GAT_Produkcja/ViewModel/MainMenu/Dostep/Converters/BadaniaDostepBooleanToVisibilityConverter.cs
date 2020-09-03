using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.UI.Utilities.Dostep;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GAT_Produkcja.UI.ViewModel.MainMenu.Dostep.Converters
{
    public class BadaniaDostepBooleanToVisibilityConverter : MarkupExtension, IValueConverter
    {

        private static BadaniaDostepBooleanToVisibilityConverter _converter = null; //prywatna wlasciwosc odwolujaca sie do biezacej klasy

        public override object ProvideValue(IServiceProvider serviceProvider) // w tej wlasciwosci podajemy powyzsze jako wartosc
        {
            if (_converter == null)
            {
                _converter = new BadaniaDostepBooleanToVisibilityConverter();
            }

            return _converter;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            tblPracownikGAT uzytkownik = UzytkownikZalogowany.Uzytkownik;

            if (uzytkownik != null)
            {
                if (uzytkownik.ID_Dostep == (int)TypDostepuEnum.Pelny||
                    uzytkownik.ID_Dostep == (int)TypDostepuEnum.Dyrektor||
                    uzytkownik.ID_Dostep == (int)TypDostepuEnum.Kierownik ||
                    uzytkownik.ID_Dostep == (int)TypDostepuEnum.KontrolaJakosci)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
