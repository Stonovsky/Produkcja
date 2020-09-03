using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace GAT_Produkcja.UI.Converters
{
    public class PercentToIntConverter : BaseValueConverter<PercentToIntConverter> 
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal percent = 0;
            bool decimalCanBeParsed = decimal.TryParse(value.ToString(), out percent);

            if (decimalCanBeParsed)
                return percent * 100;

            return 0;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
