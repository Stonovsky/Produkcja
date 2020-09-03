using GAT_Produkcja.db.Enums;
using GAT_Produkcja.UI.Converters;
using GAT_Produkcja.UI.Utilities.Dostep;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GAT_Produkcja.UI.Converters
{
    public class DostepSzefVisibilityConverter : BaseValueConverter<DostepSzefVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (UzytkownikZalogowany.Uzytkownik == null)
            {
                return Visibility.Collapsed;
            }

            if (UzytkownikZalogowany.Uzytkownik.ID_Dostep == (int)TypDostepuEnum.Pelny)
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
