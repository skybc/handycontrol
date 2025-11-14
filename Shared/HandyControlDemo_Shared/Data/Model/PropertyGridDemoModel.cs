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

    [Category("文件选择")]
    [DisplayName("配置文件")]
    [PropertyFile(Extension = ".txt|.json|.xml|.config")]
    [PropertyOrder(102)]
    public string ConfigFilePath { get; set; }

    [Category("文件选择")]
    [DisplayName("图片文件")]
    [PropertyFile(Extension = ".jpg|.png|.gif|.bmp")]
    [PropertyOrder(103)]
    public string ImageFilePath { get; set; }

    [Category("文件选择")]
    [DisplayName("任意文件")]
    [PropertyFile]
    [PropertyOrder(104)]
    public string AnyFilePath { get; set; }

    [Category("文件选择")]
    [DisplayName("输出目录")]
    [PropertyFolder(Description = "请选择输出目录")]
    [PropertyOrder(105)]
    public string OutputFolder { get; set; }

    [Category("文件选择")]
    [DisplayName("工作目录")]
    [PropertyFolder]
    [PropertyOrder(106)]
    public string WorkingDirectory { get; set; }

    [PropertyOrder(0)]
    public ImageSource ImageSource { get; set; }

    private ObservableCollection<PersonItem> _persons;
    [Category("集合编辑")]
    [DisplayName("人员列表")]
    [Property(Height = 200, AddCommandProperty = nameof(AddPersonCommand), DeleteCommandProperty = nameof(DeletePersonCommand), TitleVerticalAlignment = VerticalAlignment.Top, TitleTop = 8)]
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

    private ObservableCollection<ProductItem> _products;
    [Category("集合编辑")]
    [DisplayName("产品列表（内置编辑）")]
    [Property(Height = 200, TitleVerticalAlignment = VerticalAlignment.Top, TitleTop = 8)]
    [PropertyOrder(202)]
    public ObservableCollection<ProductItem> Products
    {
        get => _products;
        set
        {
            _products = value;
            OnPropertyChanged(nameof(Products));
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

/// <summary>
/// 演示内置 DataGrid 编辑功能的产品类（无需自定义命令）
/// </summary>
public class ProductItem : INotifyPropertyChanged
{
    private string _name;
    private string _category;
    private double _price;
    private int _stock;
    private bool _isAvailable;
    private ProductStatus _status;

    [Property(DisplayName = "产品名称")]
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

    [Property(DisplayName = "分类")]
    [PropertyOrder(2)]
    public string Category
    {
        get => _category;
        set
        {
            _category = value;
            OnPropertyChanged(nameof(Category));
        }
    }

    [Property(DisplayName = "价格")]
    [PropertyOrder(3)]
    [NumberRange(0, 99999.99, DecimalPlaces = 2)]
    public double Price
    {
        get => _price;
        set
        {
            _price = value;
            OnPropertyChanged(nameof(Price));
        }
    }

    [Property(DisplayName = "库存")]
    [PropertyOrder(4)]
    [NumberRange(0, 9999)]
    public int Stock
    {
        get => _stock;
        set
        {
            _stock = value;
            OnPropertyChanged(nameof(Stock));
        }
    }

    [Property(DisplayName = "可用")]
    [PropertyOrder(5)]
    public bool IsAvailable
    {
        get => _isAvailable;
        set
        {
            _isAvailable = value;
            OnPropertyChanged(nameof(IsAvailable));
        }
    }

    [Property(DisplayName = "状态")]
    [PropertyOrder(6)]
    public ProductStatus Status
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged(nameof(Status));
        }
    }
    [PropertyOrder(7)]
    [Property(DisplayName ="按钮属性")]
    public TextBox Txt { get; set; } = new TextBox();

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public enum ProductStatus
{
    [Description("正常")]
    Normal,
    [Description("缺货")]
    OutOfStock,
    [Description("停产")]
    Discontinued,
    [Description("预售")]
    PreOrder
}
