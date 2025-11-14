using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace HandyControl.Controls;

/// <summary>
/// 滑块数字属性编辑器，用于在属性网格中使用滑块编辑数字类型的属性
/// </summary>
public class SliderNumberPropertyEditor : PropertyEditorBase
{
    /// <summary>
    /// 初始化 <see cref="SliderNumberPropertyEditor"/> 类的新实例
    /// </summary>
    public SliderNumberPropertyEditor()
    {
    }

    /// <summary>
    /// 使用指定的最小值和最大值初始化 <see cref="SliderNumberPropertyEditor"/> 类的新实例
    /// </summary>
    /// <param name="minimum">允许的最小值</param>
    /// <param name="maximum">允许的最大值</param>
    public SliderNumberPropertyEditor(double minimum, double maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    /// <summary>
    /// 获取或设置允许的最小值
    /// </summary>
    public double Minimum { get; set; }

    /// <summary>
    /// 获取或设置允许的最大值
    /// </summary>
    public double Maximum { get; set; } = 100;

    /// <summary>
    /// 获取或设置小数位数。默认值为 -1，表示使用默认小数位数
    /// 此属性用于自动工具提示的小数位数显示
    /// </summary>
    public int DecimalPlaces { get; set; } = -1;

    /// <summary>
    /// 获取或设置滑块的刻度频率。默认值为 1
    /// </summary>
    public double TickFrequency { get; set; } = 1;

    /// <summary>
    /// 获取或设置是否启用刻度对齐。默认值为 false
    /// </summary>
    public bool IsSnapToTickEnabled { get; set; }

    /// <summary>
    /// 获取或设置刻度标记的放置位置。默认值为 TickPlacement.BottomRight
    /// </summary>
    public TickPlacement TickPlacement { get; set; } = TickPlacement.BottomRight;

    /// <summary>
    /// 获取或设置滑块方向。默认值为 Orientation.Horizontal
    /// </summary>
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// 获取或设置小步长。默认值为 0.1
    /// </summary>
    public double SmallChange { get; set; } = 0.1;

    /// <summary>
    /// 获取或设置大步长。默认值为 1
    /// </summary>
    public double LargeChange { get; set; } = 1;

    /// <summary>
    /// 获取或设置自动工具提示的放置位置。默认值为 AutoToolTipPlacement.TopLeft
    /// </summary>
    public AutoToolTipPlacement AutoToolTipPlacement { get; set; } = AutoToolTipPlacement.TopLeft;

    /// <summary>
    /// 获取或设置自动工具提示的精度（小数位数）。默认值为 0
    /// </summary>
    public int AutoToolTipPrecision { get; set; }

    /// <summary>
    /// 创建用于编辑属性值的 UI 元素
    /// </summary>
    /// <param name="propertyItem">要编辑的属性项</param>
    /// <returns>用于编辑数字值的 <see cref="Slider"/> 控件</returns>
    public override FrameworkElement CreateElement(PropertyItem propertyItem)
    {
        var slider = new Slider
        {
            IsEnabled = !propertyItem.IsReadOnly,
            Minimum = Minimum,
            Maximum = Maximum,
            TickFrequency = TickFrequency,
            IsSnapToTickEnabled = IsSnapToTickEnabled,
            TickPlacement = TickPlacement,
            Orientation = Orientation,
            SmallChange = SmallChange,
            LargeChange = LargeChange,
            AutoToolTipPlacement = AutoToolTipPlacement,
            AutoToolTipPrecision = DecimalPlaces >= 0 ? DecimalPlaces : AutoToolTipPrecision
        };

        return slider;
    }

    /// <summary>
    /// 获取用于数据绑定的依赖属性
    /// </summary>
    /// <returns><see cref="RangeBase.ValueProperty"/> 依赖属性</returns>
    public override DependencyProperty GetDependencyProperty() => RangeBase.ValueProperty;
}
