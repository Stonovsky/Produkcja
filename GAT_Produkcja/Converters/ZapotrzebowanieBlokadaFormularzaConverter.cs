using GAT_Produkcja.db;
using GAT_Produkcja.db.Enums;
using GAT_Produkcja.UI.Utilities.Dostep;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GAT_Produkcja.UI.Converters
{
    public class ZapotrzebowanieBlokadaFormularzaConverter : BaseValueConverter<ZapotrzebowanieBlokadaFormularzaConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            tblZapotrzebowanieStatus status = (tblZapotrzebowanieStatus)value;

            if (status is null) return true;

            if (UzytkownikZalogowany.Uzytkownik is null) return true;

            if (status.IDZapotrzebowanieStatus == (int)StatusZapotrzebowaniaEnum.Oczekuje) return true;

            if (status.IDZapotrzebowanieStatus != (int)StatusZapotrzebowaniaEnum.Oczekuje)
                if (UzytkownikZalogowany.Uzytkownik.ID_Dostep == (int)TypDostepuEnum.Pelny
                    || UzytkownikZalogowany.Uzytkownik.ID_Dostep == (int)TypDostepuEnum.Dyrektor)
                    return true;

            return false;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
