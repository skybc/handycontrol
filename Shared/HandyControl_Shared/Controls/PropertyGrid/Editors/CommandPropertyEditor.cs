using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HandyControl.Controls
{
    /// <summary>
    /// 为 ICommand 类型提供的属性编辑器：在属性面板中显示一个按钮，按钮的 Command绑定到目标属性。
    /// </summary>
    public class CommandPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem)
        {
            var button = new Button
            {
                Content = propertyItem.CommandContent ?? propertyItem.DisplayName,
                Padding = new Thickness(5, 0, 5, 0),
                MinWidth = 20,
                MinHeight = 20,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                IsEnabled = !propertyItem.IsReadOnly
            };

            // 如果没有显式内容，需要保证按钮至少显示属性名
            if (button.Content == null)
            {
                button.Content = propertyItem.PropertyName ?? "Command";
            }

            return button;
        }

        // 将编辑元素的 Command 属性与目标对象的 ICommand 属性绑定
        public override DependencyProperty GetDependencyProperty() => Button.CommandProperty;

        // 命令通常使用单向绑定（从源到目标）
        public override BindingMode GetBindingMode(PropertyItem propertyItem) => BindingMode.OneWay;
    }
}
