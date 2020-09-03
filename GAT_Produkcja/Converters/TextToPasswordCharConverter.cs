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
    class TextToPasswordCharConverter : BaseValueConverter<TextToPasswordCharConverter> 
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new String('*', value?.ToString().Length ?? 0);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
