using GAT_Produkcja.UI.Converters;
using GAT_Produkcja.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GAT_Produkcja.Converters
{
    public class ApplicationWindowConverter : BaseValueConverter<ApplicationWindowConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ApplicationWindowsEnum)value)
            {
                case ApplicationWindowsEnum.Login:
                    break;
                case ApplicationWindowsEnum.RozliczenieProdukcji:
                    break;
                default:
                    break;
            }
            switch ((ApplicationWindowsEnum)value)
            {
                
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
