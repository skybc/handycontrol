using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HandyControl.Tools;

namespace HandyControl.Controls;

public class SwitchPropertyEditor : PropertyEditorBase
{
    public override FrameworkElement CreateElement(PropertyItem propertyItem)
    {
        //new ToggleButton
        //{
        //    Style = ResourceHelper.GetResourceInternal<Style>("ToggleButtonSwitch"),
        //    HorizontalAlignment = HorizontalAlignment.Left,
        //    IsEnabled = !propertyItem.IsReadOnly
        //};
        return new CheckBox()
        {
            //Style = ResourceHelper.GetResourceInternal<Style>("CheckBoxSwitch"),
            HorizontalAlignment = HorizontalAlignment.Left,
            IsEnabled = !propertyItem.IsReadOnly,
            MinHeight = 24,
            VerticalAlignment = VerticalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center
        };
    }

    public override DependencyProperty GetDependencyProperty() => ToggleButton.IsCheckedProperty;
}
