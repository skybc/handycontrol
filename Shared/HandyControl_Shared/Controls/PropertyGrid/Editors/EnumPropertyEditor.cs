using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace HandyControl.Controls;

public class EnumPropertyEditor : PropertyEditorBase
{
    public class KV
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }

    public override FrameworkElement CreateElement(PropertyItem propertyItem)
    {
        var cb = new System.Windows.Controls.ComboBox
        {
            IsEnabled = !propertyItem.IsReadOnly,

        };

        var enums = Enum.GetValues(propertyItem.PropertyType);
        List<KV> list = new List<KV>();
        foreach (var item in enums)
        {
            string descrption = "";
            // 获取discription属性
            var desattr = item.GetType().GetCustomAttribute<DescriptionAttribute>();
            if (desattr != null)
            {
                descrption = desattr.Description;
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


    public override DependencyProperty GetDependencyProperty() => Selector.SelectedValueProperty;
}
