using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using HandyControlDemo.Data;

namespace HandyControlDemo.UserControl;

public partial class PropertyGridDemoCtl
{
    private object _originalModel;

    public PropertyGridDemoCtl()
    {
        InitializeComponent();
        Loaded += (_, _) => ScheduleMeasurement();
        Unloaded += (_, _) => _measurementVersion++;

        DemoModel = new PropertyGridPerformanceModel();

        // 保存原始对象用于测试
        _originalModel = DemoModel;
    }

    public static readonly DependencyProperty DemoModelProperty = DependencyProperty.Register(
        nameof(DemoModel), typeof(object), typeof(PropertyGridDemoCtl), new PropertyMetadata(default(object), OnDemoModelChanged));

    public object DemoModel
    {
        get => (object) GetValue(DemoModelProperty);
        set => SetValue(DemoModelProperty, value);
    }

    private int _measurementVersion;

    private static void OnDemoModelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        var page = (PropertyGridDemoCtl)sender;
        if (page.IsLoaded) { page.ScheduleMeasurement(); }
    }

    private async void ScheduleMeasurement()
    {
        var version = ++_measurementVersion;
        var model = DemoModel;
        LegacyTiming.Text = "PropertyGrid：测量中…";
        FastTiming.Text = "FastPropertyGrid：测量中…";
        await MeasureAsync("PropertyGrid", () => LegacyGrid.SelectedObject = model,
            text => LegacyTiming.Text = text, version);
        if (version != _measurementVersion || !IsLoaded) { return; }
        await MeasureAsync("FastPropertyGrid", () => FastGrid.SelectedObject = model,
            text => FastTiming.Text = text, version);
    }

    private async Task MeasureAsync(string name, Action select, Action<string> report, int version)
    {
        await Dispatcher.InvokeAsync(() => { }, DispatcherPriority.ApplicationIdle);
        if (version != _measurementVersion || !IsLoaded) { return; }
        var watch = Stopwatch.StartNew();
        select();
        var loadMilliseconds = watch.Elapsed.TotalMilliseconds;
        var rendered = new TaskCompletionSource<bool>();
        EventHandler handler = null;
        RoutedEventHandler unloaded = null;
        handler = (_, _) =>
        {
            CompositionTarget.Rendering -= handler;
            Unloaded -= unloaded;
            rendered.TrySetResult(true);
        };
        unloaded = (_, _) =>
        {
            CompositionTarget.Rendering -= handler;
            Unloaded -= unloaded;
            rendered.TrySetResult(false);
        };
        CompositionTarget.Rendering += handler;
        Unloaded += unloaded;
        if (!await rendered.Task || version != _measurementVersion || !IsLoaded) { return; }
        var totalMilliseconds = watch.Elapsed.TotalMilliseconds;
        var renderMilliseconds = totalMilliseconds - loadMilliseconds;
        report($"{name}：加载 {loadMilliseconds:F2} ms ｜布局/渲染等待 {renderMilliseconds:F2} ms ｜首帧总计 {totalMilliseconds:F2} ms");
    }

    /// <summary>
    /// 测试用例1：更新同类型对象数据
    /// 验证：应用缓存，不重建界面，只更新PropertyItem中的Value值
    /// </summary>
    private void OnUpdateSameType(object sender, RoutedEventArgs e)
    {
        DemoModel = new PropertyGridPerformanceModel
        {
            Property0000 = "Updated " + DateTime.Now.ToString("HH:mm:ss.fff"),
            Property0001 = Environment.TickCount
        };
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
