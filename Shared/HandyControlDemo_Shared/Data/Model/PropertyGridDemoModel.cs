using System.Collections.ObjectModel;
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

    private ObservableCollection<PersonItem> _persons; 
    [Category("集合编辑")]
    [DisplayName("人员列表")]
    [Property(Height = 200, AddCommandProperty = nameof(AddPersonCommand), DeleteCommandProperty = nameof(DeletePersonCommand), TitleVerticalAlignment = VerticalAlignment.Top ,TitleTop = 8  )]
    [PropertyOrder(200)]
    public ObservableCollection<PersonItem> Persons
    {
        get => _persons;
        set
        {
            _persons = value;
            OnPropertyChanged(nameof(Persons));
        }
    }

    private ObservableCollection<string> _tags;
    [Category("集合编辑")]
    [DisplayName("标签列表")]
    [Property(IsListBox = true, Height = 150, AddCommandProperty = nameof(AddTagCommand), DeleteCommandProperty = nameof(DeleteTagCommand), TitleVerticalAlignment = VerticalAlignment.Top, TitleTop = 8)]
    [PropertyOrder(201)]
    public ObservableCollection<string> Tags
    {
        get => _tags;
        set
        {
            _tags = value;
            OnPropertyChanged(nameof(Tags));
        }
    }

    public ICommand AddPersonCommand => new RelayCommand(() =>
    {
        if (Persons == null)
        {
            Persons = new ObservableCollection<PersonItem>();
        }
        Persons.Add(new PersonItem
        {
            Name = "新员工",
            Age = 25,
            Type = PersonType.Employee,
            IsActive = true,
            Salary = 5000
        });
    });

    public ICommand DeletePersonCommand => new RelayCommand(() =>
    {
        if (Persons != null && Persons.Count > 0)
        {
            Persons.RemoveAt(Persons.Count - 1);
        }
    });

    public ICommand AddTagCommand => new RelayCommand(() =>
    {
        if (Tags == null)
        {
            Tags = new ObservableCollection<string>();
        }
        Tags.Add($"标签{Tags.Count + 1}");
    });

    public ICommand DeleteTagCommand => new RelayCommand(() =>
    {
        if (Tags != null && Tags.Count > 0)
        {
            Tags.RemoveAt(Tags.Count - 1);
        }
    });

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

/// <summary>
/// 演示 DataGrid 编辑的示例类
/// </summary>
public class PersonItem : INotifyPropertyChanged
{
    private string _name;
    private int _age;
    private PersonType _type;
    private bool _isActive;
    private double _salary;

    [Property(DisplayName = "姓名")]
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

    [Property(DisplayName = "年龄")]
    [PropertyOrder(2)]
    [NumberRange(0, 150)]
    public int Age
    {
        get => _age;
        set
        {
            _age = value;
            OnPropertyChanged(nameof(Age));
        }
    }

    [Property(DisplayName = "类型")]
    [PropertyOrder(3)]
    public PersonType Type
    {
        get => _type;
        set
        {
            _type = value;
            OnPropertyChanged(nameof(Type));
        }
    }

    [Property(DisplayName = "激活")]
    [PropertyOrder(4)]
    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            OnPropertyChanged(nameof(IsActive));
        }
    }

    [Property(DisplayName = "薪资")]
    [PropertyOrder(5)]
    [NumberRange(0, 1000000, DecimalPlaces = 2)]
    public double Salary
    {
        get => _salary;
        set
        {
            _salary = value;
            OnPropertyChanged(nameof(Salary));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public enum PersonType
{
    [Description("员工")]
    Employee,
    [Description("经理")]
    Manager,
    [Description("主管")]
    Director
}
