using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HandyControl.Tools.Converter;

/// <summary>
/// System.Windows.Media.Color 与 SolidColorBrush 之间的转换器
/// </summary>
public class MediaColorToBrushConverter : IValueConverter
{
    /// <summary>
    /// 将 Color 转换为 SolidColorBrush
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Color color)
        {
            return new SolidColorBrush(color);
        }

        // 默认返回白色画刷
        return Brushes.White;
    }

    /// <summary>
    /// 将 SolidColorBrush 转换回 Color
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush brush)
        {
            return brush.Color;
        }

        // 默认返回白色
        return Colors.White;
    }
}
