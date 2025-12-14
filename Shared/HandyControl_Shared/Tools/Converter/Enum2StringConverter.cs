using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace HandyControl.Tools.Converter;

/// <summary>
/// Enum to string converter using EnumHelper
/// </summary>
public class Enum2StringConverter : IValueConverter
{
    /// <summary>
    /// Convert enum value to its description string
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return string.Empty;
        }

        try
        {
            var enumType = value.GetType();
            if (!enumType.IsEnum)
            {
                return value.ToString();
            }

            // Reference EnumHelper.GetDescription logic
            string result = value.ToString();
            FieldInfo info = enumType.GetField(value.ToString());
            if (info != null)
            {
                var attributes = info.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attributes != null && attributes.FirstOrDefault() != null)
                {
                    result = (attributes.First() as DescriptionAttribute)?.Description ?? result;
                }
            }

            return result;
        }
        catch
        {
            return value?.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Convert string back to enum value
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || targetType == null || !targetType.IsEnum)
        {
            return Binding.DoNothing;
        }

        try
        {
            string stringValue = value as string ?? value.ToString();

            if (string.IsNullOrEmpty(stringValue))
            {
                return Binding.DoNothing;
            }

            // Try to parse the string as an enum value first
            if (Enum.TryParse(targetType, stringValue, true, out var result))
            {
                return result;
            }

            // Reference EnumHelper.GetValueByDescription logic
            foreach (var field in targetType.GetFields())
            {
                if (field.Name == stringValue)
                {
                    return field.GetValue(null);
                }

                var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attributes != null && attributes.FirstOrDefault() != null)
                {
                    if (attributes.First().Description == stringValue)
                    {
                        return field.GetValue(null);
                    }
                }
            }

            return Binding.DoNothing;
        }
        catch
        {
            return Binding.DoNothing;
        }
    }
}
