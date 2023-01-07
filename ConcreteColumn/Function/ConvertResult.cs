using System;
using System.Globalization;
using System.Windows.Data;

namespace CrossStruc.ConcreteColumn.Function
{
    public class ConvertResult : IMultiValueConverter // Range ratio design result
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double.TryParse((string)values[0], out double val0);
            double.TryParse((string)values[1], out double val1);
            double.TryParse((string)values[2], out double val2);
            double maxval = Math.Max(Math.Max(val0, val1), val2);
            if (maxval < 0.95)
            {
                return 0;
            }
            else if ((maxval >= 0.95) && (maxval <= 1))
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
