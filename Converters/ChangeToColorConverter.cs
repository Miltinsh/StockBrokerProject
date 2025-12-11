using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace StockBrokerProject
{
    public class ChangeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal change)
            {
                if (change > 0)
                    return new SolidColorBrush(Color.FromRgb(0, 200, 83));
                else if (change < 0)
                    return new SolidColorBrush(Color.FromRgb(229, 57, 53));
                else
                    return new SolidColorBrush(Color.FromRgb(127, 140, 141));
            }
            return new SolidColorBrush(Color.FromRgb(127, 140, 141));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
