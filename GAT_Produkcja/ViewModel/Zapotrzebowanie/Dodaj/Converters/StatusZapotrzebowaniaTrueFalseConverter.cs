using GAT_Produkcja.UI.Utilities.Dostep;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GAT_Produkcja.ViewModel.Zapotrzebowanie.Dodaj.Converters
{
    class StatusZapotrzebowaniaTrueFalseConverter : MarkupExtension, IValueConverter
    {
            private static StatusZapotrzebowaniaTrueFalseConverter _converter = null; //prywatna wlasciwosc odwolujaca sie do biezacej klasy

            public override object ProvideValue(IServiceProvider serviceProvider) // w tej wlasciwosci podajemy powyzsze jako wartosc
            {
                if (_converter == null)
                {
                    _converter = new StatusZapotrzebowaniaTrueFalseConverter();
                }

                return _converter;
            }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (UzytkownikZalogowany.Uzytkownik == null)
                {
                return true;
                }

                if (UzytkownikZalogowany.Uzytkownik.ID_Dostep == 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

    }
}
