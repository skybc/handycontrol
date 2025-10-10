using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;

namespace HandyControlDemo.Data;

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
    [Property(VisibleProperty = nameof(IsShowEnum))]
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
    Male,
    Female
}
