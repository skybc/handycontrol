using System.Windows;

namespace HandyControl.Controls;

public class NumberPropertyEditor : PropertyEditorBase
{
    public NumberPropertyEditor()
    {

    }

    public NumberPropertyEditor(double minimum, double maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    public double Minimum { get; set; }

    public double Maximum { get; set; }
    public int DecimalPlaces { get; set; } = -1;

    public override FrameworkElement CreateElement(PropertyItem propertyItem)
    {
        var numeric = new NumericUpDown
        {
            IsReadOnly = propertyItem.IsReadOnly,
            Minimum = Minimum,
            Maximum = Maximum
        };

        if (DecimalPlaces >= 0)
        {
            numeric.DecimalPlaces = DecimalPlaces;
        }

        return numeric;
    }

    public override DependencyProperty GetDependencyProperty() => NumericUpDown.ValueProperty;
}
