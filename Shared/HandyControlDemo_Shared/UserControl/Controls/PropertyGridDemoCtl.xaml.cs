using System.Collections.ObjectModel;
using System.Windows;
using HandyControlDemo.Data;

namespace HandyControlDemo.UserControl;

public partial class PropertyGridDemoCtl
{
    public PropertyGridDemoCtl()
    {
        InitializeComponent();

        DemoModel = new PropertyGridDemoModel
        {
            String = "TestString",
            Enum = Gender.Female,
            Boolean = true,
            Integer = 98,
            VerticalAlignment = VerticalAlignment.Stretch,
            MediaColor = System.Windows.Media.Colors.Red,
            DrawingColor = System.Drawing.Color.Blue,
            Persons = new ObservableCollection<PersonItem>
            {
                new PersonItem { Name = "张三", Age = 30, Type = PersonType.Manager, IsActive = true, Salary = 8000.50 },
                new PersonItem { Name = "李四", Age = 25, Type = PersonType.Employee, IsActive = true, Salary = 5000.00 },
                new PersonItem { Name = "王五", Age = 35, Type = PersonType.Director, IsActive = false, Salary = 12000.75 }
            },
            Tags = new ObservableCollection<string>
            {
                "标签1",
                "标签2",
                "标签3"
            },
            Products = new ObservableCollection<ProductItem>
            {
                new ProductItem { Name = "笔记本电脑", Category = "电子产品", Price = 5999.99, Stock = 50, IsAvailable = true, Status = ProductStatus.Normal },
                new ProductItem { Name = "机械键盘", Category = "外设", Price = 399.00, Stock = 0, IsAvailable = false, Status = ProductStatus.OutOfStock },
                new ProductItem { Name = "显示器", Category = "电子产品", Price = 1999.00, Stock = 30, IsAvailable = true, Status = ProductStatus.Normal },
                new ProductItem { Name = "鼠标垫", Category = "配件", Price = 59.90, Stock = 100, IsAvailable = true, Status = ProductStatus.PreOrder }
            }
        };
    }

    public static readonly DependencyProperty DemoModelProperty = DependencyProperty.Register(
        nameof(DemoModel), typeof(PropertyGridDemoModel), typeof(PropertyGridDemoCtl), new PropertyMetadata(default(PropertyGridDemoModel)));

    public PropertyGridDemoModel DemoModel
    {
        get => (PropertyGridDemoModel) GetValue(DemoModelProperty);
        set => SetValue(DemoModelProperty, value);
    }
}
