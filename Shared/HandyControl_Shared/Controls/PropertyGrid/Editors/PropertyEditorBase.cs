using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HandyControl.Controls;

public abstract class PropertyEditorBase : DependencyObject
{
    public abstract FrameworkElement CreateElement(PropertyItem propertyItem);

    public virtual void CreateBinding(PropertyItem propertyItem, DependencyObject element)
    {
        IValueConverter converter = GetConverter(propertyItem); 
        BindingOperations.SetBinding(element, GetDependencyProperty(),
            new Binding($"{propertyItem.PropertyName}")
            {
                Source = propertyItem.Value,
                Mode = GetBindingMode(propertyItem),
                UpdateSourceTrigger = GetUpdateSourceTrigger(propertyItem),
                Converter = converter
            });
    }

    public abstract DependencyProperty GetDependencyProperty();
    ///
    public virtual BindingMode GetBindingMode(PropertyItem propertyItem) => propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;

    public virtual UpdateSourceTrigger GetUpdateSourceTrigger(PropertyItem propertyItem) => UpdateSourceTrigger.PropertyChanged;

    protected virtual IValueConverter GetConverter(PropertyItem propertyItem)
    {

        if (propertyItem.Property?.ConverterType != null)
        {
            return (IValueConverter)Activator.CreateInstance(propertyItem.Property.ConverterType);
        }
        return null;
    }

    public virtual void SetDeflautValue(FrameworkElementFactory factory, PropertyItem propertyItem)
    {

    }

    public virtual void CreateBinding(PropertyItem propertyItem, FrameworkElementFactory factory)
    {
        //throw new NotImplementedException();
    }
}
