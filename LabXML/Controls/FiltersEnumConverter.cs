using System;
using System.Globalization;
using System.Windows.Data;

namespace LabXML.Controls;

public class FiltersEnumConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value?.Equals((Filters) parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.Equals(true) ? parameter : Binding.DoNothing;
    }
}