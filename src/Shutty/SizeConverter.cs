using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Shutty
{
    public class SizeConverter : MarkupExtension, IValueConverter
    {
        public string RelativeSize { get; set; }
        private static readonly CultureInfo ci = new CultureInfo("en-US");
        private double relSize => double.Parse(RelativeSize, ci);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double v = (double)value;
            return v * relSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
            //double v = (double)value;
            //return v / relSize;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
