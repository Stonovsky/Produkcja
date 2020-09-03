using GAT_Produkcja.db.Enums;
using GAT_Produkcja.UI.Utilities.Dostep;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GAT_Produkcja.UI.Converters
{
    public class DostepProdukcjaVisibilityConverter : BaseValueConverter<DostepProdukcjaVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (UzytkownikZalogowany.Uzytkownik == null)
            {
                return Visibility.Collapsed;
            }

            if (UzytkownikZalogowany.Uzytkownik.ID_Dostep == (int)TypDostepuEnum.Pelny
                || UzytkownikZalogowany.Uzytkownik.ID_Dostep==(int)TypDostepuEnum.Dyrektor
                || UzytkownikZalogowany.Uzytkownik.ID_Dostep==(int)TypDostepuEnum.Finanse
                || UzytkownikZalogowany.Uzytkownik.ID_Dostep==(int)TypDostepuEnum.KontrolaJakosci
                || UzytkownikZalogowany.Uzytkownik.ID_Dostep==(int)TypDostepuEnum.Kierownik
                || UzytkownikZalogowany.Uzytkownik.ID_Dostep==(int)TypDostepuEnum.Operator
                )   
                
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
