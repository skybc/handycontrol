using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace HandyControl.Controls;

/// <summary>
/// 
/// </summary>
public class KV
{
    public string Key { get; set; }
    public object Value { get; set; }
}

public class EnumPropertyEditor : PropertyEditorBase
{
    private PropertyAttribute property;

    public EnumPropertyEditor(PropertyAttribute property)
    {
        this.property = property;
    }

    public override FrameworkElement CreateElement(PropertyItem propertyItem)
    {
        var cb = new System.Windows.Controls.ComboBox
        {
            IsEnabled = !propertyItem.IsReadOnly,

        };

        if (property != null)
        {
            // 是否有指定枚举列表来源
            if (!string.IsNullOrWhiteSpace(property.ComboBoxItemsSourceProperty))
            {
                cb.SetBinding(System.Windows.Controls.ComboBox.ItemsSourceProperty, new System.Windows.Data.Binding(property.ComboBoxItemsSourceProperty)
                {
                    Source = propertyItem.Value,
                    Mode = System.Windows.Data.BindingMode.OneWay
                });
                if (!string.IsNullOrWhiteSpace(property.DisplayMemberPathProperty))
                {
                    cb.DisplayMemberPath = property.DisplayMemberPathProperty;
                }
                if (!string.IsNullOrWhiteSpace(property.SelectedValuePathProperty))
                {
                    cb.SelectedValuePath = property.SelectedValuePathProperty;
                }
                return cb;
            }
        }

        var enums = Enum.GetValues(propertyItem.PropertyType);

        List<KV> list = new List<KV>();
        foreach (var item in enums)
        {
            string descrption = "";
            // 获取discription属性
            FieldInfo info = item.GetType().GetField(item.ToString());
            var attributes = info.GetCustomAttribute(typeof(DescriptionAttribute), true) as DescriptionAttribute;

            if (attributes != null)
            {
                descrption = attributes.Description;
            }
            else
            {
                descrption = item.ToString();
            }
            list.Add(new KV { Key = descrption, Value = item });
        }
        cb.DisplayMemberPath = "Key";
        cb.SelectedValuePath = "Value";
        cb.ItemsSource = list;
        return cb;
    }


    public override DependencyProperty GetDependencyProperty()
    {
        if (property != null
            && !string.IsNullOrWhiteSpace(property.ComboBoxItemsSourceProperty)
            && string.IsNullOrWhiteSpace(property.SelectedValuePathProperty))
        {
            return Selector.SelectedItemProperty;
        }
        return Selector.SelectedValueProperty;
    }
}
