using System.Windows;

namespace HandyControl.Controls;

/// <summary>
/// 文本输入编辑器
/// </summary>
public class PlainTextPropertyEditor : PropertyEditorBase
{
    public override FrameworkElement CreateElement(PropertyItem propertyItem) {

        var textBox = new System.Windows.Controls.TextBox
        {
            IsReadOnly = propertyItem.IsReadOnly
        };

        return textBox;
    }

    public override DependencyProperty GetDependencyProperty() => System.Windows.Controls.TextBox.TextProperty;
}
