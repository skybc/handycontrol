using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows;
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
        var cb = new System.Windows.Controls.ComboBox
        {
            IsEnabled = !propertyItem.IsReadOnly,

        };


        cb.SetBinding(ComboBox.ItemsSourceProperty, new Binding(property.ComboBoxItemsSourceProperty)
        {
            Source = propertyItem.Value,
            Mode = BindingMode.OneWay
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

    public override DependencyProperty GetDependencyProperty()
    {
        if(string.IsNullOrWhiteSpace(property.SelectedValuePathProperty))
        {
            return Selector.SelectedItemProperty;
        }
        return Selector.SelectedItemProperty;
    }

}
