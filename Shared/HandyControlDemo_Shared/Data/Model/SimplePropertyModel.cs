using System.ComponentModel;
using HandyControl.Controls;

namespace HandyControlDemo.Data;

/// <summary>
/// 简化的属性模型，用于测试PropertyGrid的缓存机制
/// 当PropertyGrid切换到此类型时，应该清除缓存并重建界面
/// </summary>
[TitleWidth(100, System.Windows.GridUnitType.Pixel)]
public class SimplePropertyModel : INotifyPropertyChanged
{
    private string _name;
    private int _value;
    private string _description;

    [Category("基本信息")]
    [DisplayName("名称")]
    [PropertyOrder(1)]
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    [Category("基本信息")]
    [DisplayName("数值")]
    [PropertyOrder(2)]
    [NumberRange(0, 1000)]
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            OnPropertyChanged(nameof(Value));
        }
    }

    [Category("基本信息")]
    [DisplayName("描述")]
    [PropertyOrder(3)]
    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
