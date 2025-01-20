using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using HandyControl.Controls;

namespace HandyControlDemo.Data;

public class PropertyGridDemoModel
{
    [Category("Category1")]
    [PropertyOrder(6)]
    public string String { get; set; }

    [Category("Category2")]
    [PropertyOrder(5)]
    public int Integer { get; set; }

    [Category("Category2")]
    [PropertyOrder(4)]
    public bool Boolean { get; set; }

    [Category("Category1")]
    [PropertyOrder(3)]
    public Gender Enum { get; set; }

    [PropertyOrder(2)]
    public HorizontalAlignment HorizontalAlignment { get; set; }

    [PropertyOrder(1)]
    public VerticalAlignment VerticalAlignment { get; set; }

    [PropertyOrder(0)]
    public ImageSource ImageSource { get; set; }
}

public enum Gender
{
    Male,
    Female
}
