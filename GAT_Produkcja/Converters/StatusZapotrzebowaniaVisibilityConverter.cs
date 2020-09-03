using GAT_Produkcja.UI.Utilities.Dostep;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GAT_Produkcja.UI.Converters
{
    class StatusZapotrzebowaniaVisibilityConverter : BaseValueConverter<StatusZapotrzebowaniaVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
