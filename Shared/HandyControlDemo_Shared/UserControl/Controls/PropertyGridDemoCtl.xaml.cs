using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using HandyControlDemo.Data;

namespace HandyControlDemo.UserControl;

public partial class PropertyGridDemoCtl
{
    private object _originalModel;
    private MultiSelectionModel[] _multiSelection;
    private CustomEditorModel[] _customModels;

    public PropertyGridDemoCtl()
    {
        InitializeComponent();
        FastGrid.EditCompleted += OnEditCompleted;
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
        await MeasureAsync("FastPropertyGrid", () => FastGrid.SelectedObjects = new[] { model },
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
        var simpleModel = new SimpleModel
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
    private void OnMultiSelection(object sender, RoutedEventArgs e)
    {
        _measurementVersion++;
        _multiSelection = new[]
        {
            new MultiSelectionModel { Name = "共同名称", Count = 1 },
            new MultiSelectionModel { Name = "共同名称", Count = 2 }
        };
        FastGrid.SelectedObjects = _multiSelection;
        FastTiming.Text = "多对象测试：名称相同，数量不同。修改属性后点击查看结果。";
    }

    private void OnShowMultiSelection(object sender, RoutedEventArgs e)
    {
        if (_multiSelection == null)
        {
            MessageBox.Show("请先点击多对象编辑测试。", "多对象测试");
            return;
        }
        FastGrid.Refresh();
        MessageBox.Show($"对象 1：名称 = {_multiSelection[0].Name}，数量 = {_multiSelection[0].Count}\n" +
                        $"对象 2：名称 = {_multiSelection[1].Name}，数量 = {_multiSelection[1].Count}", "多对象修改结果");
    }

    private void OnCustomEditorTest(object sender, RoutedEventArgs e)
    {
        _measurementVersion++;
        var model = new CustomEditorModel("只读值") { CustomText = "单对象初始值" };
        _customModels = new[] { model };
        FastGrid.SelectedObjects = new[] { model };
        FastTiming.Text = "单对象：编辑自定义文本，按 Enter 提交；只读行应禁用。";
    }

    private void OnCustomMultiTest(object sender, RoutedEventArgs e)
    {
        _measurementVersion++;
        _customModels = new[]
        {
            new CustomEditorModel("只读 A") { CustomText = "一致值" },
            new CustomEditorModel("只读 B") { CustomText = "一致值", RejectCustomText = true }
        };
        FastGrid.SelectedObjects = _customModels;
        FastTiming.Text = "多对象：名称一致、只读行禁用。将 CustomText 改为新值并按 Enter，可验证部分失败报告和实际值回读。";
    }

    private void OnShowCustomResults(object sender, RoutedEventArgs e)
    {
        if (_customModels == null)
        {
            MessageBox.Show("请先运行单对象或多对象自定义控件测试。", "自定义控件测试");
            return;
        }
        FastGrid.Refresh();
        MessageBox.Show(string.Join(Environment.NewLine, _customModels.Select((model, index) =>
            $"对象 {index + 1}：CustomText = {model.CustomText}，ReadOnlyText = {model.ReadOnlyText}")), "自定义控件结果");
    }

    private void OnEditCompleted(object sender, HandyControl.Controls.FastPropertyEditCompletedEventArgs args)
    {
        FastTiming.Text = $"自定义控件提交：成功 {args.SuccessCount} 项，失败 {args.Failures.Count} 项。";
    }

    public class SimpleModel
    {
        [HandyControl.Controls.Property]
        public string Name { get; set; }
        [HandyControl.Controls.Property]
        public int Value { get; set; }
        [HandyControl.Controls.Property]
        public string Description { get; set; }
    }

    public class MultiSelectionModel
    {
        [HandyControl.Controls.Property]
        public string Name { get; set; }
        [HandyControl.Controls.Property]
        public int Count { get; set; }
    }

    public class CustomEditorModel
    {
        public CustomEditorModel(string readOnlyText)
        {
            ReadOnlyText = readOnlyText;
        }

        [HandyControl.Controls.Property(CustomControlType = typeof(CustomTextEditor), TitleVerticalAlignment = VerticalAlignment.Top)]
        public string CustomText
        {
            get => _customText;
            set
            {
                if (RejectCustomText && value != _customText)
                {
                    throw new InvalidOperationException("此对象拒绝该值，用于验证部分失败报告。");
                }
                _customText = value;
            }
        }

        private string _customText;
        public bool RejectCustomText { get; set; }
        public bool BlockMultiEdit { get; set; }

        [HandyControl.Controls.Property(CustomControlType = typeof(CustomTextEditor), TitleVerticalAlignment = VerticalAlignment.Top)]
        public string ReadOnlyText { get; private set; }
    }

    public sealed class CustomTextEditor : TextBox, HandyControl.Controls.IFastPropertyGridEditor,
        HandyControl.Controls.IFastPropertyGridMultiEditor
    {
        public static readonly DependencyProperty EditorValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(string), typeof(CustomTextEditor), new PropertyMetadata(string.Empty, OnValueChanged));

        private bool _synchronizing;

        public CustomTextEditor()
        {
            MinWidth = 140;
            Height = 200;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            AcceptsReturn = true;
            TextWrapping = TextWrapping.Wrap;
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            TextChanged += OnTextChanged;
            KeyDown += (_, args) =>
            {
                if (args.Key == System.Windows.Input.Key.Enter
                    && System.Windows.Input.Keyboard.Modifiers.HasFlag(System.Windows.Input.ModifierKeys.Control))
                {
                    ValueCommitted?.Invoke(this, EventArgs.Empty);
                    args.Handled = true;
                }
            };
        }

        public string Value
        {
            get => (string)GetValue(EditorValueProperty);
            set => SetValue(EditorValueProperty, value);
        }

        public DependencyProperty ValueProperty => EditorValueProperty;

        public bool IsMixedValue
        {
            get => _isMixedValue;
            set
            {
                _isMixedValue = value;
                if (value)
                {
                    _synchronizing = true;
                    Text = "多个值";
                    _synchronizing = false;
                }
            }
        }

        private bool _isMixedValue;
        public event EventHandler ValueCommitted;

        public bool CanEdit(System.Collections.Generic.IReadOnlyList<object> targets, string propertyName, out string reason)
        {
            reason = null;
            if (targets.OfType<CustomEditorModel>().Any(model => model.BlockMultiEdit))
            {
                reason = "当前对象禁止批量编辑此属性。";
                return false;
            }
            return true;
        }

        public void ApplyValue(System.Collections.Generic.IReadOnlyList<object> targets, object target,
            string propertyName, object value)
        {
            ((CustomEditorModel)target).CustomText = (string)value;
        }

        private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var editor = (CustomTextEditor)sender;
            editor._synchronizing = true;
            editor.Text = (string)args.NewValue ?? string.Empty;
            editor._synchronizing = false;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!_synchronizing)
            {
                _isMixedValue = false;
                SetCurrentValue(EditorValueProperty, Text);
            }
        }
    }

}
