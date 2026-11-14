using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace HandyControl.Controls;

/// <summary>
/// 枚举属性编辑器（用于 PropertyGrid 中显示和编辑枚举类型属性）
/// </summary>
public class KV
{
    /// <summary>
    /// 显示文本（Key）
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// 对应的枚举值（Value）
    /// </summary>
    public object Value { get; set; }
}

public class EnumPropertyEditor : PropertyEditorBase
{
    // 保存传入的 PropertyAttribute，用于自定义 ComboBox 的行为
    private PropertyAttribute property;

    /// <summary>
    /// 构造函数，接收可能包含自定义显示配置的 PropertyAttribute
    /// </summary>
    /// <param name="property">属性特性配置</param>
    public EnumPropertyEditor(PropertyAttribute property)
    {
        this.property = property;
    }

    /// <summary>
    /// 创建用于编辑枚举属性的 UI 元素（ComboBox）
    /// </summary>
    /// <param name="propertyItem">要编辑的属性项</param>
    /// <returns>用于绑定到属性的 FrameworkElement（ComboBox）</returns>
    public override FrameworkElement CreateElement(PropertyItem propertyItem)
    {
        // 创建 ComboBox 并根据是否可读写设置可用性
        var cb = new System.Windows.Controls.ComboBox
        {
            IsEnabled = !propertyItem.IsReadOnly,

        };

        if (property != null)
        {
            // 如果 PropertyAttribute 指定了 ComboBoxItemsSourceProperty，使用该源作为 ItemsSource
            if (!string.IsNullOrWhiteSpace(property.ComboBoxItemsSourceProperty))
            {
                cb.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(property.ComboBoxItemsSourceProperty)
                {
                    Source = propertyItem.Value,
                    Mode = BindingMode.OneWay
                });
                // 如果指定了显示成员路径，设置 DisplayMemberPath
                if (!string.IsNullOrWhiteSpace(property.DisplayMemberPathProperty))
                {
                    cb.DisplayMemberPath = property.DisplayMemberPathProperty;
                }
                // 如果指定了选中值路径，设置 SelectedValuePath
                if (!string.IsNullOrWhiteSpace(property.SelectedValuePathProperty))
                {
                    cb.SelectedValuePath = property.SelectedValuePathProperty;
                }
                else
                {
                    // 自定义数据源为 Enum[] 或 List<Enum> 时，保留原始枚举值作为选中项，
                    // 仅在 UI 中转换为 DescriptionAttribute 指定的文本。
                    cb.ItemTemplate = CreateEnumDescriptionTemplate();
                }
                return cb;
            }
        }

        // 如果没有自定义的 ItemsSource，则将枚举类型的所有值转换为显示列表
        var enums = Enum.GetValues(propertyItem.PropertyType);

        List<KV> list = new List<KV>();
        foreach (var item in enums)
        {
            string descrption = GetEnumDescription(item);
            // 将显示文本和枚举值封装成 KV 对象添加到列表
            list.Add(new KV { Key = descrption, Value = item });
        }
        // 设置 ComboBox 的显示和选中值路径，并绑定 ItemsSource
        cb.DisplayMemberPath = "Key";
        cb.SelectedValuePath = "Value";
        cb.ItemsSource = list;
        return cb;
    }

    internal static DataTemplate CreateEnumDescriptionTemplate()
    {
        var textBlock = new FrameworkElementFactory(typeof(TextBlock));
        textBlock.SetBinding(TextBlock.TextProperty, new Binding
        {
            Converter = EnumDescriptionConverter.Instance
        });

        return new DataTemplate
        {
            VisualTree = textBlock
        };
    }

    private static string GetEnumDescription(object value)
    {
        if (value is not Enum enumValue)
        {
            return value?.ToString() ?? string.Empty;
        }

        FieldInfo info = enumValue.GetType().GetField(enumValue.ToString());
        var attribute = info?.GetCustomAttribute<DescriptionAttribute>(true);
        return attribute?.Description ?? enumValue.ToString();
    }

    private sealed class EnumDescriptionConverter : IValueConverter
    {
        public static readonly EnumDescriptionConverter Instance = new EnumDescriptionConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => GetEnumDescription(value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }


    /// <summary>
    /// 获取用于绑定的依赖属性：当使用自定义 ItemsSource 且未指定 SelectedValuePath 时，绑定 SelectedItem；否则绑定 SelectedValue
    /// </summary>
    /// <returns>要绑定的 DependencyProperty</returns>
    public override DependencyProperty GetDependencyProperty()
    {
        if (property != null
            && !string.IsNullOrWhiteSpace(property.ComboBoxItemsSourceProperty)
            && string.IsNullOrWhiteSpace(property.SelectedValuePathProperty))
        {
            // 未指定 SelectedValuePath 时，使用 SelectedItem
            return Selector.SelectedItemProperty;
        }
        // 默认使用 SelectedValue（与 SelectedValuePath 对应）
        return Selector.SelectedValueProperty;
    }
}
