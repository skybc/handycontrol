using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using DrawingColor = System.Drawing.Color;
using MediaColor = System.Windows.Media.Color;

namespace HandyControlDemo.Data;

[TitleWidth(100, GridUnitType.Pixel)]
public class PropertyGridDemoModel : INotifyPropertyChanged
{
    private bool isShowEnum = false;

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

    [Property(IsIgnore = true)]
    public bool IsShowEnum
    {
        get => isShowEnum; set
        {
            isShowEnum = value;
            OnPropertyChanged(nameof(IsShowEnum));
        }
    }
    [PropertyOrder(2)]
    public HorizontalAlignment HorizontalAlignment { get; set; }

    [PropertyOrder(1)]
    public VerticalAlignment VerticalAlignment { get; set; }

    [Category("颜色")]
    [DisplayName("媒体颜色")]
    [PropertyOrder(100)]
    public MediaColor MediaColor { get; set; }

    [Category("颜色")]
    [DisplayName("绘图颜色")]
    [PropertyOrder(101)]
    public DrawingColor DrawingColor { get; set; }

    [Property(CommandContentName = "...", CommandProperty = nameof(SelectFile))]
    public string Path { get; set; }
    public ICommand SelectFile
    {
        get => new RelayCommand(() =>
    {
        var dialog = new Microsoft.Win32.OpenFileDialog();
        if (dialog.ShowDialog() == true)
        {
            Path = dialog.FileName;
            OnPropertyChanged(nameof(Path));
        }
    });
    }
    //[PropertyOrder(0)]
    //public ImageSource ImageSource { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public enum Gender
{
    [Description("男")]
    Male,
    [Description("女")]
    Female
}
