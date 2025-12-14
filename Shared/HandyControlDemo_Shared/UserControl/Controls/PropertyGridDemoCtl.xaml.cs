using System.Collections.ObjectModel;
using System.Windows;
using HandyControlDemo.Data;

namespace HandyControlDemo.UserControl;

public partial class PropertyGridDemoCtl
{
    private object _originalModel;

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

        // 保存原始对象用于测试
        _originalModel = DemoModel;
    }

    public static readonly DependencyProperty DemoModelProperty = DependencyProperty.Register(
        nameof(DemoModel), typeof(object), typeof(PropertyGridDemoCtl), new PropertyMetadata(default(object)));

    public object DemoModel
    {
        get => (object) GetValue(DemoModelProperty);
        set => SetValue(DemoModelProperty, value);
    }

    /// <summary>
    /// 测试用例1：更新同类型对象数据
    /// 验证：应用缓存，不重建界面，只更新PropertyItem中的Value值
    /// </summary>
    private void OnUpdateSameType(object sender, RoutedEventArgs e)
    {
        // 创建同类型的新对象
        var newModel = new PropertyGridDemoModel
        {
            String = "Updated String - " + System.DateTime.Now.ToString("HH:mm:ss"),
            Enum = Gender.Male,
            Boolean = false,
            Integer = 4112,
            VerticalAlignment = VerticalAlignment.Center,
            MediaColor = System.Windows.Media.Colors.Green,
            DrawingColor = System.Drawing.Color.Yellow,
            Persons = new ObservableCollection<PersonItem>
            {
                new PersonItem { Name = "测试者", Age = 2118, Type = PersonType.Manager, IsActive = true, Salary = 9000.00 }
            },
            Tags = new ObservableCollection<string> { "新标签" },
            Products = new ObservableCollection<ProductItem>
            {
                new ProductItem { Name = "测试产品", Category = "测试", Price = 99.99, Stock = 10, IsAvailable = true, Status = ProductStatus.Normal }
            }
        };

        DemoModel = newModel;
        MessageBox.Show("已更新同类型对象！\n（缓存应该被使用，只更新数据不重建界面）", "缓存测试");
    }

    /// <summary>
    /// 测试用例2：切换到不同类型的对象
    /// 验证：缓存失效，重新构建界面
    /// </summary>
    private void OnSwitchDifferentType(object sender, RoutedEventArgs e)
    {
        // 创建一个不同的对象类型（简化模型）
        var simpleModel = new
        {
            Name = "Simple Model",
            Value = 100,
            Description = "这是一个不同的类型"
        };

        DemoModel = simpleModel;
        MessageBox.Show("已切换到不同类型对象！\n（缓存失效，应该重新构建界面）", "类型切换测试");
    }

    /// <summary>
    /// 恢复到原始的PropertyGridDemoModel类型对象
    /// </summary>
    private void OnRestoreDefault(object sender, RoutedEventArgs e)
    {
        DemoModel = _originalModel;
        MessageBox.Show("已恢复到原始对象！", "恢复测试");
    }
}
