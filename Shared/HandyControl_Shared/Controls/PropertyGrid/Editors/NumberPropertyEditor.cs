using System.Windows;

namespace HandyControl.Controls;

/// <summary>
/// 数字属性编辑器，用于在属性网格中编辑数字类型的属性
/// </summary>
public class NumberPropertyEditor : PropertyEditorBase
{
    /// <summary>
    /// 初始化 <see cref="NumberPropertyEditor"/> 类的新实例
    /// </summary>
    public NumberPropertyEditor()
    {

    }

    /// <summary>
    /// 使用指定的最小值和最大值初始化 <see cref="NumberPropertyEditor"/> 类的新实例
    /// </summary>
    /// <param name="minimum">允许的最小值</param>
    /// <param name="maximum">允许的最大值</param>
    public NumberPropertyEditor(double minimum, double maximum)
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
    public double Maximum { get; set; }

    /// <summary>
    /// 获取或设置小数位数。默认值为 -1，表示使用默认小数位数
    /// </summary>
    public int DecimalPlaces { get; set; } = -1;

    /// <summary>
    /// 创建用于编辑属性值的 UI 元素
    /// </summary>
    /// <param name="propertyItem">要编辑的属性项</param>
    /// <returns>用于编辑数字值的 <see cref="NumericUpDown"/> 控件</returns>
    public override FrameworkElement CreateElement(PropertyItem propertyItem)
    {
        var numeric = new NumericUpDown
        {
            IsReadOnly = propertyItem.IsReadOnly,
            Minimum = Minimum,
            Maximum = Maximum
        };

        if (DecimalPlaces >= 0)
        {
            numeric.DecimalPlaces = DecimalPlaces;
        }

        return numeric;
    }

    /// <summary>
    /// 获取用于数据绑定的依赖属性
    /// </summary>
    /// <returns><see cref="NumericUpDown.ValueProperty"/> 依赖属性</returns>
    public override DependencyProperty GetDependencyProperty() => NumericUpDown.ValueProperty;
}
