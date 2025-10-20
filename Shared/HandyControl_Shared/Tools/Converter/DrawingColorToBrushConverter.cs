using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using DrawingColor = System.Drawing.Color;
using MediaColor = System.Windows.Media.Color;

namespace HandyControl.Tools.Converter;

/// <summary>
/// System.Drawing.Color 与 SolidColorBrush 之间的转换器
/// </summary>
public class DrawingColorToBrushConverter : IValueConverter
{
    /// <summary>
    /// 将 System.Drawing.Color 转换为 SolidColorBrush
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DrawingColor drawingColor)
        {
            // 将 System.Drawing.Color 转换为 System.Windows.Media.Color
            var mediaColor = MediaColor.FromArgb(
                drawingColor.A,
                drawingColor.R,
                drawingColor.G,
                drawingColor.B);

            return new SolidColorBrush(mediaColor);
        }

        // 默认返回白色画刷
        return Brushes.White;
    }

    /// <summary>
    /// 将 SolidColorBrush 转换回 System.Drawing.Color
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush brush)
        {
            var mediaColor = brush.Color;
            
            // 将 System.Windows.Media.Color 转换为 System.Drawing.Color
            return DrawingColor.FromArgb(
                mediaColor.A,
                mediaColor.R,
                mediaColor.G,
                mediaColor.B);
        }

        // 默认返回白色
        return DrawingColor.White;
    }
}
