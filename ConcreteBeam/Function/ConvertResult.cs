using System;
using System.Globalization;
using System.Windows.Data;

namespace CrossStruc.ConcreteBeam.Function
{
    public class ConvertResult : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double.TryParse((string)values[0], out double val0);
            double.TryParse((string)values[1], out double val1);
            double.TryParse((string)values[2], out double val2);
            double.TryParse((string)values[3], out double val3);
            double max_val = Math.Max(Math.Max(Math.Max(val0, val1), val2), val3);
            if (max_val < 0.95)
            {
                return 0;
            }
            else if ((max_val >= 0.95) && (max_val <= 1))
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
