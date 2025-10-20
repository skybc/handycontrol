using System;
using System.Windows;
using System.Windows.Data;
using HandyControl.Tools.Converter;

namespace HandyControl.Controls;

/// <summary>
/// 颜色属性编辑器，用于编辑 System.Windows.Media.Color 和 System.Drawing.Color 类型的属性
/// </summary>
public class ColorPropertyEditor : PropertyEditorBase
{
    /// <summary>
    /// 颜色类型枚举
    /// </summary>
    public enum ColorType
    {
        /// <summary>
        /// System.Windows.Media.Color
        /// </summary>
        MediaColor,

        /// <summary>
        /// System.Drawing.Color
        /// </summary>
        DrawingColor
    }

    private ColorType _colorType;

    /// <summary>
    /// 创建颜色选择控件元素
    /// </summary>
    /// <param name="propertyItem">属性项</param>
    /// <returns>ColorPopup 控件</returns>
    public override FrameworkElement CreateElement(PropertyItem propertyItem)
    {
        // 判断颜色类型
        _colorType = propertyItem.PropertyType == typeof(System.Drawing.Color)
            ? ColorType.DrawingColor
            : ColorType.MediaColor;

        var colorPopup = new ColorPopup
        {
            IsEnabled = !propertyItem.IsReadOnly,
            MinWidth = 100,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        // 设置圆角
        BorderElement.SetCornerRadius(colorPopup, new CornerRadius(4));

        return colorPopup;
    }

    /// <summary>
    /// 获取依赖属性（ColorPopup.SelectedBrushProperty）
    /// </summary>
    /// <returns>SelectedBrush 依赖属性</returns>
    public override DependencyProperty GetDependencyProperty() => ColorPopup.SelectedBrushProperty;

    /// <summary>
    /// 获取值转换器（根据颜色类型返回对应的转换器）
    /// </summary>
    /// <param name="propertyItem">属性项</param>
    /// <returns>颜色转换器实例</returns>
    protected override IValueConverter GetConverter(PropertyItem propertyItem)
    {
        return _colorType == ColorType.DrawingColor
            ? new DrawingColorToBrushConverter()
            : new MediaColorToBrushConverter();
    }
}
