using GAT_Produkcja.db.Enums;
using GAT_Produkcja.UI.Utilities.Dostep;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GAT_Produkcja.UI.Converters
{
    public class OdDostepuDyrektorTrueConverter : BaseValueConverter<OdDostepuDyrektorTrueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (UzytkownikZalogowany.Uzytkownik == null)
            {
                return false;
            }

            if (UzytkownikZalogowany.Uzytkownik.ID_Dostep == (int)TypDostepuEnum.Pelny
                || UzytkownikZalogowany.Uzytkownik.ID_Dostep==(int)TypDostepuEnum.Dyrektor)   
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
