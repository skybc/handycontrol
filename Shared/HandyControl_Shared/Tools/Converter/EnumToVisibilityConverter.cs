using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace HandyControl.Tools.Converter
{
    public class EnumToVisibilityConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter is null)
            {
                return  Visibility.Collapsed;
            }
            string parameterString = parameter.ToString();
            if (string.IsNullOrEmpty(parameterString))
            {
                return Visibility.Collapsed;
            }
            if (Enum.IsDefined(value.GetType(), value) == false)
            {
                return Visibility.Collapsed;
            }
            object parameterValue = Enum.Parse(value.GetType(), parameterString);
            return parameterValue.Equals(value)? Visibility.Visible: Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter is null)
            {
                return Binding.DoNothing;
            }
            string parameterString = parameter.ToString();
            if (string.IsNullOrEmpty(parameterString))
            {
                return Binding.DoNothing;
            }
            return Enum.Parse(targetType, parameterString);
        }
    }
}
