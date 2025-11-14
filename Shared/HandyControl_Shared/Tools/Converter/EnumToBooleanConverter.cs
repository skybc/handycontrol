using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace HandyControl.Tools.Converter
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter is null)
            {
                return false;
            }
            string parameterString = parameter.ToString();
            if (string.IsNullOrEmpty(parameterString))
            {
                return false;
            }
            if (Enum.IsDefined(value.GetType(), value) == false)
            {
                return false;
            }
            object parameterValue = Enum.Parse(value.GetType(), parameterString);
            return parameterValue.Equals(value);
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
