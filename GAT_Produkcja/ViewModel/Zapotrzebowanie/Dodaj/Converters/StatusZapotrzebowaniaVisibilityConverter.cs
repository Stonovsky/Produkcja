using GAT_Produkcja.UI.Utilities.Dostep;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GAT_Produkcja.ViewModel.Zapotrzebowanie.Dodaj.Converters
{
    class StatusZapotrzebowaniaVisibilityConverter : MarkupExtension, IValueConverter
    {
            private static StatusZapotrzebowaniaVisibilityConverter _converter = null; //prywatna wlasciwosc odwolujaca sie do biezacej klasy

            public override object ProvideValue(IServiceProvider serviceProvider) // w tej wlasciwosci podajemy powyzsze jako wartosc
            {
                if (_converter == null)
                {
                    _converter = new StatusZapotrzebowaniaVisibilityConverter();
                }

                return _converter;
            }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (UzytkownikZalogowany.Uzytkownik == null)
                {
                    return Visibility.Collapsed;
                }

                if (UzytkownikZalogowany.Uzytkownik.ID_Dostep == 1)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

    }
}
