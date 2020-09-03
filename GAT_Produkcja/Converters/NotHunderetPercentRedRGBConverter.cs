using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GAT_Produkcja.UI.Converters
{
    public class NotHunderetPercentRedRGBConverter : BaseValueConverter<NotHunderetPercentRedRGBConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return value;

            SolidColorBrush brush;

            decimal percentage = (decimal)value;
            if (percentage < 1)
            {
                return new SolidColorBrush(Colors.OrangeRed);
            }
            return new SolidColorBrush(Colors.AliceBlue);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
