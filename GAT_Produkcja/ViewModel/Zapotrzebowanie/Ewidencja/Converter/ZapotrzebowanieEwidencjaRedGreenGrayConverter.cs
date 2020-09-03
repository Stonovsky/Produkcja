using GAT_Produkcja.db.Enums;
using GAT_Produkcja.db.Repositories.UnitOfWork;
using GAT_Produkcja.Helpers.Theme;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace GAT_Produkcja.UI.ViewModel.Zapotrzebowanie.Ewidencja.Converter
{
    class ZapotrzebowanieEwidencjaRedGreenGrayConverter : MarkupExtension, IValueConverter
    {
        private static ZapotrzebowanieEwidencjaRedGreenGrayConverter _converter = null; //prywatna wlasciwosc odwolujaca sie do biezacej klasy
        private ThemeChangerHelper themeHelper;

        public ZapotrzebowanieEwidencjaRedGreenGrayConverter()
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider) // w tej wlasciwosci podajemy powyzsze jako wartosc
        {
            if (_converter == null) _converter = new ZapotrzebowanieEwidencjaRedGreenGrayConverter();
            return _converter;
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                SolidColorBrush brush;
                StatusZapotrzebowaniaEnum status = (StatusZapotrzebowaniaEnum)value;
                if (status==StatusZapotrzebowaniaEnum.Akceptacja)
                {
                    brush = new SolidColorBrush(Colors.Green);
                }
                else if (status==StatusZapotrzebowaniaEnum.BrakAkceptacji)
                {
                    brush = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    var acctualTheme = GetAcctualThemeName();
                    if (acctualTheme.ToLower().Contains("dark"))
                    {
                        brush = new SolidColorBrush(Colors.White);
                    }
                    else
                    {
                        brush = new SolidColorBrush(Colors.Black);
                    }
                }

                return brush;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string GetAcctualThemeName()
        {
            var app = (App)Application.Current;
            var dictionaries = app.GetThemeDictionariesByName("MaterialDesignThemes");
            return dictionaries.First().Source.OriginalString;
        }

    }
}
