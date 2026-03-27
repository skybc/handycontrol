using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
namespace HandyControl.Controls;

public class CommboxPropertyEditor : PropertyEditorBase
{
    private PropertyAttribute property;

    public CommboxPropertyEditor(PropertyAttribute property)
    {
        this.property = property;
    }

    public override FrameworkElement CreateElement(PropertyItem propertyItem)
    {
        var cb = new System.Windows.Controls.ComboBox()
        {
            IsEnabled = !propertyItem.IsReadOnly,
        };

        var strinStrs = property.ComboBoxItemsSourceProperty.Split(";", StringSplitOptions.RemoveEmptyEntries);

        if (strinStrs.Length > 1)
        {
            cb.ItemsSource = strinStrs;
        }
        else
        {
            cb.SetBinding(System.Windows.Controls.ComboBox.ItemsSourceProperty, new Binding(property.ComboBoxItemsSourceProperty)
            {
                Source = propertyItem.Value,
                Mode = BindingMode.OneWay
            });
        }


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

    public override DependencyProperty GetDependencyProperty()
    {
        if (!string.IsNullOrWhiteSpace(property.SelectedValuePathProperty))
        {
            return System.Windows.Controls.ComboBox.SelectedValueProperty;
        }
        return System.Windows.Controls.ComboBox.SelectedItemProperty;
    }
    public override void CreateBinding(PropertyItem propertyItem, DependencyObject element)
    {

        var strinStrs = property.ComboBoxItemsSourceProperty.Split(";", StringSplitOptions.RemoveEmptyEntries);

        if (element is not System.Windows.Controls.ComboBox cb)
        {
            return;
        }
        if (strinStrs.Length > 1)
        {
            cb.ItemsSource = strinStrs;
        }
        else
        {
            cb.SetBinding(System.Windows.Controls.ComboBox.ItemsSourceProperty, new Binding(property.ComboBoxItemsSourceProperty)
            {
                Source = propertyItem.Value,
                Mode = BindingMode.OneWay
            });
        }
        base.CreateBinding(propertyItem, element);
    }


}
