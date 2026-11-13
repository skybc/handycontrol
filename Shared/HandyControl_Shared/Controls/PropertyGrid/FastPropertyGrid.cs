using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

/// <summary>
/// 支持容器虚拟化的属性编辑器，可编辑单个对象或批量修改多个对象的公共属性。
/// </summary>
/// <remarks>
/// 单对象复用 PropertyGrid 的编辑器；多对象使用文本或枚举、布尔选择编辑器。
/// 批量提交逐个写入对象，个别失败不会回滚已成功的修改，结果通过 EditCompleted 通知。
/// </remarks>
public class FastPropertyGrid : ListBox
{
    public static readonly DependencyProperty SelectedObjectsProperty = DependencyProperty.Register(
    nameof(SelectedObjects), typeof(IEnumerable), typeof(FastPropertyGrid), new PropertyMetadata(null, SelectionChanged));
    public static readonly DependencyProperty ShowSortButtonProperty = PropertyGrid.ShowSortButtonProperty.AddOwner(typeof(FastPropertyGrid));
    public bool ShowSortButton
    {
        get => (bool)GetValue(ShowSortButtonProperty);
        set => SetValue(ShowSortButtonProperty, ValueBoxes.BooleanBox(value));
    }
    public static readonly DependencyProperty ShowSearchBarProperty = PropertyGrid.ShowSearchBarProperty.AddOwner(typeof(FastPropertyGrid));
    public bool ShowSearchBar
    {
        get => (bool)GetValue(ShowSearchBarProperty);
        set => SetValue(ShowSearchBarProperty, ValueBoxes.BooleanBox(value));
    }
    public static readonly DependencyProperty DescriptionProperty = PropertyGrid.DescriptionProperty.AddOwner(typeof(FastPropertyGrid));
    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
    public static readonly DependencyProperty MinTitleWidthProperty = PropertyGrid.MinTitleWidthProperty.AddOwner(typeof(FastPropertyGrid));
    public double MinTitleWidth
    {
        get => (double)GetValue(MinTitleWidthProperty);
        set => SetValue(MinTitleWidthProperty, value);
    }
    public static readonly DependencyProperty MaxTitleWidthProperty = PropertyGrid.MaxTitleWidthProperty.AddOwner(typeof(FastPropertyGrid));
    public double MaxTitleWidth
    {
        get => (double)GetValue(MaxTitleWidthProperty);
        set => SetValue(MaxTitleWidthProperty, value);
    }
    public virtual PropertyResolver PropertyResolver
    {
        get;
    }
    = new();
    /// <summary>
    /// 获取或设置是否按分类显示属性；否则按属性的排序索引显示。
    /// </summary>
    public static readonly DependencyProperty IsCategorizedProperty = DependencyProperty.Register(
        nameof(IsCategorized), typeof(bool), typeof(FastPropertyGrid),
        new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            (d, e) => ((FastPropertyGrid)d).UpdateVisibleRows()));
    public bool IsCategorized
    {
        get => (bool)GetValue(IsCategorizedProperty);
        set => SetValue(IsCategorizedProperty, ValueBoxes.BooleanBox(value));
    }
    private readonly Dictionary<ListBoxItem, Action> _detach = new();
    private readonly Dictionary<ListBoxItem, Action> _commit = new();
    private readonly Queue<TextBox> _textPool = new();
    private readonly Queue<ComboBox> _choicePool = new();
    public bool IsEditorPoolingEnabled
    {
        get;
        set;
    }
    = true;
    public int EditorsCreated
    {
        get;
        private set;
    }
    public int EditorsReused
    {
        get;
        private set;
    }
    public int PooledEditorCount => _textPool.Count + _choicePool.Count;
    private List<Row> _rows = new();
    private readonly HashSet<string> _collapsed = new();
    /// <summary>
    /// 获取或设置用于匹配属性名称或显示名称的搜索文本。
    /// </summary>
    public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register(
    nameof(SearchText), typeof(string), typeof(FastPropertyGrid),
    new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
        (d, e) => ((FastPropertyGrid)d).UpdateVisibleRows()));
    public string SearchText
    {
        get => (string)GetValue(SearchTextProperty);
        set => SetValue(SearchTextProperty, value);
    }
    /// <summary>
    /// 切换批量编辑视图中的分类展开状态，并重建可见行。
    /// </summary>
    public void ToggleCategory(string category)
    {
        if (!_collapsed.Add(category))
        {
            _collapsed.Remove(category);
        }
        UpdateVisibleRows();
    }
    /// <summary>
    /// 提交并释放已有编辑器后应用搜索和分类；单对象使用 WPF 分组，多对象使用分类标题行。
    /// </summary>
    private void UpdateVisibleRows()
    {
        foreach (var commit in _commit.Values.ToArray())
        {
            commit();
        }
        ReleaseEditors();
        var filtered = _rows.Where(row => string.IsNullOrEmpty(SearchText)
            || row.Name?.IndexOf(SearchText, StringComparison.CurrentCultureIgnoreCase) >= 0
            || row.Properties[0].Name.IndexOf(SearchText, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();
        {
            var view = new ListCollectionView(filtered);
            if (GroupStyle.Count == 0)
            {
                GroupStyle.Add(new GroupStyle
                {
                    ContainerStyle = (Style)FindResource("PropertyGroupItemBaseStyle")
                }
                );
            }
            if (IsCategorized)
            {
                view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Row.Category)));
            }
            ItemsSource = view;
            return;
        }
    }
    private bool _committing;
    private object[] _selection = Array.Empty<object>();
    private readonly List<INotifyPropertyChanged> _subscriptions = new();
    public IEnumerable SelectedObjects
    {
        get => (IEnumerable)GetValue(SelectedObjectsProperty);
        set => SetValue(SelectedObjectsProperty, value);
    }
    public event EventHandler<FastPropertyEditCompletedEventArgs> EditCompleted;

    public FastPropertyGrid()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.SortByCategory, (_, _) =>
        {
            SetCurrentValue(IsCategorizedProperty, ValueBoxes.TrueBox);
        }
        ));
        CommandBindings.Add(new CommandBinding(ControlCommands.SortByName, (_, _) =>
        {
            SetCurrentValue(IsCategorizedProperty, ValueBoxes.FalseBox);
        }
        ));
        VirtualizingPanel.SetIsVirtualizing(this, true);
        VirtualizingPanel.SetVirtualizationMode(this, VirtualizationMode.Recycling);
        VirtualizingPanel.SetCacheLength(this, new VirtualizationCacheLength(1));
        ScrollViewer.SetCanContentScroll(this, true);
        Loaded += (_, _) => Refresh();
        Unloaded += (_, _) =>
        {
            Unsubscribe();
            ReleaseEditors();
            _textPool.Clear();
            _choicePool.Clear();
            ItemsSource = null;
            _selection = Array.Empty<object>();
        }
        ;
    }

    private static void SelectionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        ((FastPropertyGrid)sender).Refresh();
    }

    /// <summary>
    /// 重建所选对象的公共属性描述及通知订阅，属性值在可见容器准备时按需读取。
    /// </summary>
    /// <remarks>
    /// 必须在 UI 线程调用。使用 SelectedObjects，忽略 null 并按引用去重。
    /// 没有有效对象时清空面板。刷新前提交当前待写入的编辑内容。
    /// </remarks>
    public void Refresh()
    {
        Dispatcher.VerifyAccess();
        foreach (var commit in _commit.Values.ToArray())
        {
            commit();
        }
        ReleaseEditors();
        Unsubscribe();
        var objects = new List<object>();
        if (SelectedObjects != null)
        {
            foreach (var item in SelectedObjects)
            {
                if (item != null && !objects.Any(existing => ReferenceEquals(existing, item)))
                {
                    objects.Add(item);
                }
            }
        }
        _selection = objects.ToArray();
        var rows = new List<Row>();
        if (_selection.Length > 0)
        {
            // 以第一个对象为基准，仅保留所有目标均存在、可浏览且类型和转换器一致的属性。
            var descriptors = _selection.Select(TypeDescriptor.GetProperties).ToArray();
            foreach (PropertyDescriptor first in descriptors[0])
            {
                var metadata = first.Attributes.OfType<PropertyAttribute>().FirstOrDefault();
                var properties = descriptors.Select(set => set[first.Name]).ToArray();
                if (properties.Any(p => p == null || !p.Attributes.OfType<PropertyAttribute>().Any() || !PropertyResolver.ResolveIsBrowsable(p) || p.PropertyType != first.PropertyType))
                {
                    continue;
                }
                string DisplayName(PropertyDescriptor property)
                {
                    var propertyAttribute = property.Attributes.OfType<PropertyAttribute>().FirstOrDefault();
                    var displayName = propertyAttribute?.DisplayName;
                    if (string.IsNullOrEmpty(displayName))
                    {
                        displayName = PropertyResolver.ResolveDisplayName(property);
                    }
                    return displayName.ToLanguage();
                }
                if (properties.Any(property => !string.Equals(DisplayName(property), DisplayName(first), StringComparison.CurrentCulture)))
                {
                    continue;
                }
                var firstAttribute = first.Attributes.OfType<PropertyAttribute>().FirstOrDefault();
                if (_selection.Length > 1 && firstAttribute?.CustomControlType == null && (first.IsReadOnly || !first.Converter.CanConvertFrom(typeof(string))))
                {
                    continue;
                }
                if (metadata?.CustomControlType == null && properties.Any(p => p.Converter.GetType() != first.Converter.GetType()))
                {
                    continue;
                }
                rows.Add(new Row
                {
                    Properties = properties,
                    Name = DisplayName(first),
                    Category = (metadata?.Category ?? PropertyResolver.ResolveCategory(first)).ToLanguage(),
                    SortIndex = first.Attributes.OfType<PropertyOrderAttribute>().FirstOrDefault()?.Index ?? metadata?.Index ?? 0
                }
                );
            }
        }
        _rows = rows.OrderBy(row => row.SortIndex).ToList();
        UpdateVisibleRows();
        foreach (var notification in _selection.OfType<INotifyPropertyChanged>())
        {
            notification.PropertyChanged += OnPropertyChanged;
            _subscriptions.Add(notification);
        }
    }

    /// <summary>
    /// 仅在 UI 线程且非内部提交期间重新准备已生成的容器；后台线程通知在此忽略。
    /// </summary>
    private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        if (!Dispatcher.CheckAccess() || _committing)
        {
            return;
        }
        foreach (var item in Items)
        {
            if (ItemContainerGenerator.ContainerFromItem(item) is ListBoxItem container)
            {
                PrepareContainerForItemOverride(container, item);
            }
        }
    }

    /// <summary>
    /// 解除旧对象的属性变化订阅，避免刷新或卸载后继续持有对象。
    /// </summary>
    private void Unsubscribe()
    {
        foreach (var item in _subscriptions)
        {
            item.PropertyChanged -= OnPropertyChanged;
        }
        _subscriptions.Clear();
    }

    /// <summary>
    /// 为可见行准备内容；回收容器先解绑旧编辑器，再按单对象或批量编辑路径创建内容。
    /// </summary>
    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        var oldContainer = (ListBoxItem)element;
        if (_detach.Remove(oldContainer, out var detach))
        {
            detach();
        }
        _commit.Remove(oldContainer);
        base.PrepareContainerForItemOverride(element, item);
        var container = (ListBoxItem)element;
        var row = (Row)item;
        if (row.Properties == null)
        {
            var header = new Expander
            {
                Header = row.Name,
                IsExpanded = !_collapsed.Contains(row.Category)
            }
            ;
            header.SetResourceReference(StyleProperty, "FastPropertyGridCategoryStyle");
            header.Expanded += (_, _) =>
            {
                if (_collapsed.Contains(row.Category))
                {
                    ToggleCategory(row.Category);
                }
            }
            ;
            header.Collapsed += (_, _) =>
            {
                if (!_collapsed.Contains(row.Category))
                {
                    ToggleCategory(row.Category);
                }
            }
            ;
            container.Content = header;
            return;
        }
        var customAttribute = row.Properties[0].Attributes.OfType<PropertyAttribute>().FirstOrDefault();
        if (customAttribute?.CustomControlType != null)
        {
            PrepareCustomEditor(container, row, customAttribute);
            return;
        }
        if (_selection.Length == 1)
        {
            var width = _selection[0].GetType().GetCustomAttributes(typeof(TitleWidthAttribute), true)
            .OfType<TitleWidthAttribute>().FirstOrDefault() ?? new TitleWidthAttribute(80, GridUnitType.Pixel);
            var propertyItem = PropertyGrid.CreatePropertyItemCore(row.Properties[0], width, _selection[0], PropertyResolver);
            propertyItem.SetResourceReference(StyleProperty, "PropertyItemBaseStyle");
            propertyItem.InitElement();
            container.Content = propertyItem;
            return;
        }
        var panel = new DockPanel();
        TextBox editor;
        if (IsEditorPoolingEnabled && _textPool.Count > 0)
        {
            editor = _textPool.Dequeue();
            EditorsReused++;
        }
        else
        {
            editor = new TextBox
            {
                MinWidth = 140
            }
            ;
            EditorsCreated++;
        }
        var textSettings = row.Properties[0].Attributes.OfType<PropertyAttribute>().FirstOrDefault();
        bool isMultiline = row.Properties[0].PropertyType == typeof(string) && textSettings?.IsMultiline == true;
        editor.AcceptsReturn = isMultiline;
        editor.TextWrapping = TextWrapping.NoWrap;
        editor.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
        editor.Height = double.NaN;
        if (isMultiline)
        {
            editor.TextWrapping = TextWrapping.Wrap;
            editor.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            editor.Height = 150;
            if (textSettings.Height > 0)
            {
                editor.Height = textSettings.Height;
            }
        }
        var cleanup = new List<Action>();
        panel.Children.Add(editor);
        container.HorizontalContentAlignment = HorizontalAlignment.Stretch;
        var titleWidth = _selection[0].GetType().GetCustomAttributes(typeof(TitleWidthAttribute), true)
            .OfType<TitleWidthAttribute>().FirstOrDefault() ?? new TitleWidthAttribute(80, GridUnitType.Pixel);
        var multiItem = PropertyGrid.CreatePropertyItemCore(row.Properties[0], titleWidth, _selection[0], PropertyResolver);
        multiItem.SetResourceReference(StyleProperty, "PropertyItemBaseStyle");
        multiItem.EditorElement = panel;
        container.Content = multiItem;
        // 捕获选择数组引用，避免旧容器把未完成的编辑写入后来选中的对象。
        var snapshot = _selection;
        var dirty = false;
        var reading = false;
        // 读取所有目标值；值不一致时显示占位文字，读取异常通过提示展示。
        void Read()
        {
            reading = true;
            try
            {
                var values = row.Properties.Select((p, i) => p.GetValue(snapshot[i])).ToArray();
                editor.Text = row.Properties[0].Converter.ConvertToString(null, CultureInfo.CurrentCulture, values[0]);
                if (values.Any(value => !Equals(value, values[0])))
                {
                    editor.Text = "多个值";
                }
                editor.ToolTip = null;
            }
            catch (Exception exception)
            {
                editor.ToolTip = exception.Message;
            }
            dirty = false;
            reading = false;
        }
        // 仅提交用户修改过且仍属于当前选择的内容；逐对象转换并写入，收集失败后重新读取实际值。
        void Commit()
        {
            if (!dirty || !ReferenceEquals(snapshot, _selection))
            {
                return;
            }
            var failures = new List<FastPropertyEditFailure>();
            var succeeded = 0;
            _committing = true;
            for (var i = 0; i < snapshot.Length; i++)
            {
                try
                {
                    var value = row.Properties[i].Converter.ConvertFromString(null, CultureInfo.CurrentCulture, editor.Text);
                    row.Properties[i].SetValue(snapshot[i], value);
                    succeeded++;
                }
                catch (Exception exception)
                {
                    failures.Add(new FastPropertyEditFailure(snapshot[i], row.Properties[i].Name, exception));
                }
            }
            _committing = false;
            Read();
            if (failures.Count > 0)
            {
                editor.ToolTip = string.Join(Environment.NewLine, failures.Select(f => f.Exception.Message));
            }
            EditCompleted?.Invoke(this, new FastPropertyEditCompletedEventArgs(succeeded, failures.AsReadOnly()));
        }
        // 枚举和布尔值使用选择器；文本框仍作为统一提交转换的文本载体。
        var type = Nullable.GetUnderlyingType(row.Properties[0].PropertyType) ?? row.Properties[0].PropertyType;
        if (type.IsEnum || type == typeof(bool))
        {
            ComboBox choices;
            if (IsEditorPoolingEnabled && _choicePool.Count > 0)
            {
                choices = _choicePool.Dequeue();
                EditorsReused++;
            }
            else
            {
                choices = new ComboBox
                {
                    MinWidth = 140
                }
                ;
                EditorsCreated++;
            }
            var options = new List<object>();
            if (Nullable.GetUnderlyingType(row.Properties[0].PropertyType) != null)
            {
                options.Add(null);
            }
            if (type.IsEnum)
            {
                options.AddRange(Enum.GetValues(type).Cast<object>());
            }
            else
            {
                options.Add(false);
                options.Add(true);
            }
            choices.ItemsSource = options;
            try
            {
                var values = row.Properties.Select((property, index) => property.GetValue(snapshot[index])).ToArray();
                if (values.All(value => Equals(value, values[0])))
                {
                    choices.SelectedItem = values[0];
                }
                else
                {
                    choices.ToolTip = "多个值";
                }
            }
            catch (Exception exception)
            {
                choices.ToolTip = exception.Message;
            }
            panel.Children.Remove(editor);
            panel.Children.Add(choices);
            SelectionChangedEventHandler selectionHandler = (_, _) =>
            {
                if (choices.SelectedIndex < 0)
                {
                    return;
                }
                editor.Text = row.Properties[0].Converter.ConvertToString(choices.SelectedItem);
                dirty = true;
                Commit();
            }
            ;
            choices.SelectionChanged += selectionHandler;
            cleanup.Add(() =>
            {
                choices.SelectionChanged -= selectionHandler;
                choices.IsDropDownOpen = false;
                choices.ItemsSource = null;
                choices.ToolTip = null;
                panel.Children.Remove(choices);
                if (IsEditorPoolingEnabled && PooledEditorCount < Math.Max(2, _detach.Count * 2))
                {
                    _choicePool.Enqueue(choices);
                }
            }
            );
        }
        // 仅在所有目标的路径类型和配置一致时提供浏览入口，文件特性优先。
        if (type == typeof(string))
        {
            var files = row.Properties.Select(p => p.Attributes.OfType<PropertyFileAttribute>().FirstOrDefault()).ToArray();
            var folders = row.Properties.Select(p => p.Attributes.OfType<PropertyFolderAttribute>().FirstOrDefault()).ToArray();
            var isFile = files.All(a => a != null) && files.All(a => a.Extension == files[0].Extension);
            var isFolder = files.All(a => a == null) && folders.All(a => a != null) &&
                folders.All(a => a.Description == folders[0].Description);
            if (isFile || isFolder)
            {
                var browse = new Button { Content = "...", Width = 30, Margin = new Thickness(4, 0, 0, 0),
                    IsEnabled = row.Properties.All(p => !p.IsReadOnly) };
                DockPanel.SetDock(browse, Dock.Right);
                panel.Children.Insert(1, browse);
                RoutedEventHandler browseHandler = (_, _) =>
                {
                    if (!ReferenceEquals(snapshot, _selection))
                    {
                        return;
                    }
                    string path;
                    if (isFile)
                    {
                        var dialog = new Microsoft.Win32.OpenFileDialog
                        {
                            RestoreDirectory = true,
                            Filter = FileSelectPropertyEditor.GetFileFilter(files[0].Extension, editor.Text)
                        };
                        if (dialog.ShowDialog() != true)
                        {
                            return;
                        }
                        path = dialog.FileName;
                    }
                    else
                    {
                        var dialog = FolderSelectPropertyEditor.CreateDialog(folders[0]?.Description, editor.Text);
                        if (dialog.ShowDialog() != true)
                        {
                            return;
                        }
                        path = dialog.FolderName;
                    }
                    editor.Text = path;
                    dirty = true;
                    Commit();
                };
                browse.Click += browseHandler;
                cleanup.Add(() => browse.Click -= browseHandler);
            }
        }
        Read();
        TextChangedEventHandler textHandler = (_, _) =>
        {
            if (!reading)
            {
                dirty = true;
            }
        }
        ;
        KeyboardFocusChangedEventHandler focusHandler = (_, _) => Commit();
        KeyEventHandler keyHandler = (_, args) =>
        {
            if (args.Key == Key.Enter && !isMultiline)
            {
                Commit();
                args.Handled = true;
            }
            if (args.Key == Key.Escape)
            {
                Read();
                args.Handled = true;
            }
        }
        ;
        editor.TextChanged += textHandler;
        editor.LostKeyboardFocus += focusHandler;
        editor.KeyDown += keyHandler;
        _commit[container] = Commit;
        // 回收前清理事件、文本和父子关系；池大小随当前已登记容器数量限制。
        _detach[container] = () =>
        {
            editor.TextChanged -= textHandler;
            editor.LostKeyboardFocus -= focusHandler;
            editor.KeyDown -= keyHandler;
            editor.Text = string.Empty;
            editor.ToolTip = null;
            panel.Children.Remove(editor);
            foreach (var action in cleanup)
            {
                action();
            }
            if (IsEditorPoolingEnabled && PooledEditorCount < Math.Max(2, _detach.Count * 2))
            {
                _textPool.Enqueue(editor);
            }
        }
        ;
    }

    private void PrepareCustomEditor(ListBoxItem container, Row row, PropertyAttribute attribute)
    {
        var titleWidth = _selection[0].GetType().GetCustomAttributes(typeof(TitleWidthAttribute), true)
            .OfType<TitleWidthAttribute>().FirstOrDefault() ?? new TitleWidthAttribute(80, GridUnitType.Pixel);
        var item = PropertyGrid.CreatePropertyItemCore(row.Properties[0], titleWidth, _selection[0], PropertyResolver);
        item.SetResourceReference(StyleProperty, "PropertyItemBaseStyle");
        var panel = new DockPanel();
        item.EditorElement = panel;
        container.Content = item;
        container.HorizontalContentAlignment = HorizontalAlignment.Stretch;
        var snapshot = _selection;
        var readOnly = row.Properties.Any(property => property.IsReadOnly);
        var configMatches = row.Properties.All(property =>
        {
            var configuration = property.Attributes.OfType<PropertyAttribute>().FirstOrDefault();
            return configuration?.CustomControlType == attribute.CustomControlType
                && configuration?.ConverterType == attribute.ConverterType;
        });
        IFastPropertyGridEditor customEditor = null;
        DependencyProperty valueProperty = null;
        IValueConverter converter = null;
        string error = null;
        try
        {
            var editorType = attribute.CustomControlType;
            if (editorType == null || editorType.IsAbstract || !typeof(FrameworkElement).IsAssignableFrom(editorType)
                || !typeof(IFastPropertyGridEditor).IsAssignableFrom(editorType)
                || editorType.GetConstructor(Type.EmptyTypes) == null)
            {
                throw new InvalidOperationException("自定义控件必须是实现 IFastPropertyGridEditor 的非抽象 FrameworkElement，并具有公共无参构造函数。");
            }
            customEditor = (IFastPropertyGridEditor)Activator.CreateInstance(editorType);
            if (customEditor.ValueProperty == null || customEditor.ValueProperty.ReadOnly)
            {
                throw new InvalidOperationException("自定义控件 ValueProperty 必须是可读写的依赖属性。");
            }
            valueProperty = customEditor.ValueProperty;
            if (attribute.ConverterType != null)
            {
                converter = Activator.CreateInstance(attribute.ConverterType) as IValueConverter;
                if (converter == null)
                {
                    throw new InvalidOperationException("ConverterType 必须实现 IValueConverter。");
                }
            }
            else if (valueProperty.PropertyType != row.Properties[0].PropertyType)
            {
                throw new InvalidOperationException("ValueProperty 类型必须与被编辑属性类型一致，或配置双向 ConverterType。");
            }
        }
        catch (Exception exception)
        {
            error = exception.GetBaseException().Message;
        }
        if (error != null)
        {
            var errorText = new TextBlock { Text = "自定义编辑器配置错误：" + error, ToolTip = error, TextWrapping = TextWrapping.Wrap };
            panel.Children.Add(errorText);
            return;
        }
        var element = (FrameworkElement)customEditor;
        panel.Children.Add(element);
        var multiEditor = customEditor as IFastPropertyGridMultiEditor;
        var targets = Array.AsReadOnly(snapshot);
        var reasonText = new TextBlock { TextWrapping = TextWrapping.Wrap, Visibility = Visibility.Collapsed };
        DockPanel.SetDock(reasonText, Dock.Bottom);
        panel.Children.Insert(0, reasonText);
        bool CheckCanEdit()
        {
            if (snapshot.Length == 1)
            {
                element.IsEnabled = !readOnly;
                return !readOnly;
            }
            string reason = null;
            if (readOnly)
            {
                reason = "所选对象中存在只读属性。";
            }
            else if (!configMatches)
            {
                reason = "所选对象的自定义编辑器配置不一致。";
            }
            else if (multiEditor != null)
            {
                try
                {
                    if (!multiEditor.CanEdit(targets, row.Properties[0].Name, out reason))
                    {
                        if (string.IsNullOrWhiteSpace(reason))
                        {
                            reason = "当前选择不允许编辑。";
                        }
                    }
                    else
                    {
                        reason = null;
                    }
                }
                catch (Exception exception)
                {
                    reason = exception.GetBaseException().Message;
                }
            }
            element.IsEnabled = reason == null;
            reasonText.Text = reason;
            if (reason == null)
            {
                reasonText.Visibility = Visibility.Collapsed;
            }
            else
            {
                reasonText.Visibility = Visibility.Visible;
            }
            return reason == null;
        }
        CheckCanEdit();
        customEditor.IsMixedValue = false;
        var binding = new Binding(row.Properties[0].Name)
        {
            Source = snapshot[0],
            Mode = _selection.Length == 1 && !readOnly ? BindingMode.TwoWay : BindingMode.OneWay,
            Converter = converter,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };
        if (_selection.Length == 1)
        {
            BindingOperations.SetBinding(element, valueProperty, binding);
            _detach[container] = () =>
            {
                BindingOperations.ClearBinding(element, valueProperty);
                panel.Children.Remove(element);
            };
            return;
        }
        var committed = false;
        void ReadValues()
        {
            try
            {
                var values = row.Properties.Select((property, index) => property.GetValue(snapshot[index])).ToArray();
                var mixed = values.Skip(1).Any(value => !Equals(value, values[0]));
                customEditor.IsMixedValue = mixed;
                if (!mixed)
                {
                    var converted = converter == null ? values[0] : converter.Convert(values[0], valueProperty.PropertyType, null, CultureInfo.CurrentCulture);
                    element.SetValue(valueProperty, converted);
                }
            }
            catch (Exception exception)
            {
                element.ToolTip = exception.GetBaseException().Message;
            }
        }
        EventHandler handler = (_, _) =>
        {
            if (committed || !ReferenceEquals(snapshot, _selection) || !CheckCanEdit())
            {
                return;
            }
            committed = true;
            var failures = new List<FastPropertyEditFailure>();
            var succeeded = 0;
            _committing = true;
            foreach (var (property, index) in row.Properties.Select((property, index) => (property, index)))
            {
                try
                {
                    var value = element.GetValue(valueProperty);
                    if (converter != null)
                    {
                        value = converter.ConvertBack(value, property.PropertyType, null, CultureInfo.CurrentCulture);
                    }
                    if (multiEditor != null)
                    {
                        multiEditor.ApplyValue(targets, snapshot[index], property.Name, value);
                    }
                    else
                    {
                        property.SetValue(snapshot[index], value);
                    }
                    succeeded++;
                }
                catch (Exception exception)
                {
                    failures.Add(new FastPropertyEditFailure(snapshot[index], property.Name, exception.GetBaseException()));
                }
            }
            _committing = false;
            committed = false;
            ReadValues();
            if (failures.Count > 0)
            {
                element.ToolTip = string.Join(Environment.NewLine, failures.Select(failure => failure.Exception.Message));
            }
            CheckCanEdit();
            EditCompleted?.Invoke(this, new FastPropertyEditCompletedEventArgs(succeeded, failures.AsReadOnly()));
        };
        customEditor.ValueCommitted += handler;
        ReadValues();
        _detach[container] = () =>
        {
            customEditor.ValueCommitted -= handler;
            BindingOperations.ClearBinding(element, valueProperty);
            customEditor.IsMixedValue = false;
            panel.Children.Remove(element);
        };
    }

    /// <summary>
    /// 释放所有已登记的编辑器和事件处理器；本方法不主动提交，提交由调用方负责。
    /// </summary>
    private void ReleaseEditors()
    {
        foreach (var action in _detach.Values.ToArray())
        {
            action();
        }
        _detach.Clear();
        _commit.Clear();
    }

    /// <summary>
    /// 容器移出可见区域或被回收时，先提交待写入值，再解绑事件并清空内容。
    /// </summary>
    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
        var container = (ListBoxItem)element;
        if (_commit.Remove(container, out var commit))
        {
            commit();
        }
        if (_detach.Remove(container, out var detach))
        {
            detach();
        }
        container.Content = null;
        base.ClearContainerForItemOverride(element, item);
    }

    /// <summary>
    /// 保存行元数据；Properties 为空表示批量编辑视图的分类标题行。
    /// </summary>
    private sealed class Row
    {
        public PropertyDescriptor[] Properties;
        public string Name;
        public string Category
        {
            get;
            set;
        }
        public int SortIndex;
    }
}

/// <summary>
/// 描述一次批量提交中某个目标对象的属性写入失败。
/// </summary>
public sealed class FastPropertyEditFailure
{
    public object Target
    {
        get;
    }
    public string PropertyName
    {
        get;
    }
    public Exception Exception
    {
        get;
    }
    public FastPropertyEditFailure(object target, string propertyName, Exception exception)
    {
        Target = target;
        PropertyName = propertyName;
        Exception = exception;
    }
}

/// <summary>
/// 提供一次批量提交的成功数量和失败明细；失败不会撤销已成功写入的对象。
/// </summary>
public sealed class FastPropertyEditCompletedEventArgs : EventArgs
{
    public int SuccessCount
    {
        get;
    }
    public IReadOnlyList<FastPropertyEditFailure> Failures
    {
        get;
    }
    public FastPropertyEditCompletedEventArgs(int successCount, IReadOnlyList<FastPropertyEditFailure> failures)
    {
        SuccessCount = successCount;
        Failures = failures;
    }
}
