using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HandyControl.Controls;

/// <summary>
/// 文本输入编辑器。
/// 支持基本的文本编辑功能，并可选择在右侧添加命令按钮。
/// </summary>
/// <remarks>
/// 当属性带有 <see cref="PropertyAttribute.CommandProperty"/> 特性时，
/// 编辑器会在文本框右侧显示一个按钮，用于执行关联的命令。
/// </remarks>
public class PlainTextPropertyEditor : PropertyEditorBase
{
    /// <summary>
    /// 创建文本输入编辑器的用户界面元素。
    /// </summary>
    /// <param name="propertyItem">属性项，包含属性的配置和元数据。</param>
    /// <returns>
    /// 返回文本框（如果没有命令）或包含文本框和按钮的Grid容器（如果有命令）。
    /// </returns>
    public override FrameworkElement CreateElement(PropertyItem propertyItem)
    {
        var textBox = new System.Windows.Controls.TextBox
        {
            IsReadOnly = propertyItem.IsReadOnly
        };

        // 如果没有命令属性，直接返回文本框
        if (string.IsNullOrWhiteSpace(propertyItem.Property?.CommandProperty))
        {
            return textBox;
        }

        // 创建 Grid 容器：左侧文本框，右侧按钮
        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        // 左侧：文本框
        Grid.SetColumn(textBox, 0);
        grid.Children.Add(textBox);

        // 右侧：按钮
        var button = new Button
        {
            Content = propertyItem.CommandContent ?? "...",
            Padding = new Thickness(5, 0, 5, 0),
            MinWidth = 20,
            MinHeight = 20,
            Margin = new Thickness(4, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right,
            IsEnabled = !propertyItem.IsReadOnly
        };

        // 绑定按钮的 Command 到指定的命令属性
        button.SetBinding(Button.CommandProperty, new Binding(propertyItem.Property.CommandProperty)
        {
            Source = propertyItem.Value
        });

        Grid.SetColumn(button, 1);
        grid.Children.Add(button);

        return grid;
    }

    /// <summary>
    /// 获取编辑器绑定的依赖属性。
    /// </summary>
    /// <returns>返回TextBox.TextProperty，用于数据绑定。</returns>
    public override DependencyProperty GetDependencyProperty() => System.Windows.Controls.TextBox.TextProperty;

    /// <summary>
    /// 为编辑器元素创建数据绑定。
    /// </summary>
    /// <param name="propertyItem">属性项，包含绑定的源对象和属性名。</param>
    /// <param name="element">要绑定的界面元素。</param>
    /// <remarks>
    /// 此方法处理两种情况：
    /// 1. 元素是 TextBox：直接绑定 Text 属性
    /// 2. 元素是 Grid：获取第一个子元素（TextBox）进行绑定
    /// </remarks>
    public override void CreateBinding(PropertyItem propertyItem, DependencyObject element)
    {
        TextBox textBox = null;

        // 如果是 Grid，获取其中的 TextBox；否则直接使用该元素
        if (element is Grid grid && grid.Children.Count > 0 && grid.Children[0] is TextBox)
        {
            textBox = (TextBox)grid.Children[0];
        }
        else if (element is TextBox tb)
        {
            textBox = tb;
        }

        if (textBox != null)
        {
            BindingOperations.SetBinding(textBox, GetDependencyProperty(),
                new Binding(propertyItem.PropertyName)
                {
                    Source = propertyItem.Value,
                    Mode = GetBindingMode(propertyItem),
                    UpdateSourceTrigger = GetUpdateSourceTrigger(propertyItem)
                });
        }
    }
}
