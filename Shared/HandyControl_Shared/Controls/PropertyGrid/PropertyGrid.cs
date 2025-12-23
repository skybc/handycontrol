using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls;

[TemplatePart(Name = ElementItemsControl, Type = typeof(ItemsControl))]
[TemplatePart(Name = ElementSearchBar, Type = typeof(SearchBar))]
public class PropertyGrid : Control
{
    private const string ElementItemsControl = "PART_ItemsControl";

    private const string ElementSearchBar = "PART_SearchBar";

    private ItemsControl _itemsControl;

    private ICollectionView _dataView;

    private SearchBar _searchBar;

    private string _searchKey;

    private Type _lastObjectType;

    private List<PropertyItem> _cachedPropertyItems;

    public PropertyGrid()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.SortByCategory, SortByCategory, (s, e) => e.CanExecute = ShowSortButton));
        CommandBindings.Add(new CommandBinding(ControlCommands.SortByName, SortByName, (s, e) => e.CanExecute = ShowSortButton));
    }

    public virtual PropertyResolver PropertyResolver { get; } = new();

    public static readonly RoutedEvent SelectedObjectChangedEvent =
        EventManager.RegisterRoutedEvent("SelectedObjectChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<object>), typeof(PropertyGrid));

    public event RoutedPropertyChangedEventHandler<object> SelectedObjectChanged
    {
        add => AddHandler(SelectedObjectChangedEvent, value);
        remove => RemoveHandler(SelectedObjectChangedEvent, value);
    }

    public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(
        nameof(SelectedObject), typeof(object), typeof(PropertyGrid), new PropertyMetadata(default, OnSelectedObjectChanged));

    private static void OnSelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (PropertyGrid)d;
        ctl.OnSelectedObjectChanged(e.OldValue, e.NewValue);
    }

    public object SelectedObject
    {
        get => GetValue(SelectedObjectProperty);
        set => SetValue(SelectedObjectProperty, value);
    }

    protected virtual void OnSelectedObjectChanged(object oldValue, object newValue)
    {
        UpdateItems(newValue);
        RaiseEvent(new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, SelectedObjectChangedEvent));
    }

    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description), typeof(string), typeof(PropertyGrid), new PropertyMetadata(default(string)));

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public static readonly DependencyProperty MaxTitleWidthProperty = DependencyProperty.Register(
        nameof(MaxTitleWidth), typeof(double), typeof(PropertyGrid), new PropertyMetadata(ValueBoxes.Double0Box));

    public double MaxTitleWidth
    {
        get => (double)GetValue(MaxTitleWidthProperty);
        set => SetValue(MaxTitleWidthProperty, value);
    }

    public static readonly DependencyProperty MinTitleWidthProperty = DependencyProperty.Register(
        nameof(MinTitleWidth), typeof(double), typeof(PropertyGrid), new PropertyMetadata(ValueBoxes.Double0Box));

    public double MinTitleWidth
    {
        get => (double)GetValue(MinTitleWidthProperty);
        set => SetValue(MinTitleWidthProperty, value);
    }

    public static readonly DependencyProperty ShowSortButtonProperty = DependencyProperty.Register(
        nameof(ShowSortButton), typeof(bool), typeof(PropertyGrid), new PropertyMetadata(ValueBoxes.TrueBox));

    public bool ShowSortButton
    {
        get => (bool)GetValue(ShowSortButtonProperty);
        set => SetValue(ShowSortButtonProperty, ValueBoxes.BooleanBox(value));
    }

    /// <summary>
    /// 获取或设置是否显示搜索栏。
    /// </summary>
    public static readonly DependencyProperty ShowSearchBarProperty = DependencyProperty.Register(
        nameof(ShowSearchBar), typeof(bool), typeof(PropertyGrid), new PropertyMetadata(ValueBoxes.TrueBox));

    /// <summary>
    /// 获取或设置是否显示搜索栏。
    /// </summary>
    public bool ShowSearchBar
    {
        get => (bool)GetValue(ShowSearchBarProperty);
        set => SetValue(ShowSearchBarProperty, ValueBoxes.BooleanBox(value));
    }

    public override void OnApplyTemplate()
    {
        if (_searchBar != null)
        {
            _searchBar.SearchStarted -= SearchBar_SearchStarted;
        }

        base.OnApplyTemplate();

        _itemsControl = GetTemplateChild(ElementItemsControl) as ItemsControl;
        _searchBar = GetTemplateChild(ElementSearchBar) as SearchBar;

        if (_searchBar != null)
        {
            _searchBar.SearchStarted += SearchBar_SearchStarted;
        }

        UpdateItems(SelectedObject);
    }

    private void UpdateItems(object obj)
    {
        if (obj == null || _itemsControl == null)
        {
            if (_cachedPropertyItems != null)
            {
                foreach (var propertyItem in _cachedPropertyItems)
                {
                    try
                    {
                        propertyItem.Value = obj;
                        propertyItem.Editor.CreateBinding(propertyItem, propertyItem.EditorElement);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            if (_itemsControl != null)
            {
                _itemsControl.ItemsSource = _dataView;
            }
            return;
        }

        if (obj is IList<PropertyItem> items)
        {
            int index = 0;
            items.Do(r =>
            {
                if (r.Editor == null && r.EditorElement == null)
                {
                    r.Editor = PropertyResolver.ResolveEditor(TypeDescriptor.GetProperties(r.Value).OfType<PropertyDescriptor>()
                        .FirstOrDefault(item => item.Name == r.PropertyName));
                }
                if (r.SortIndex < 0)
                {
                    r.SortIndex = index;
                }

                index++;

                r.InitElement();
            });
            _dataView = CollectionViewSource.GetDefaultView(items);
            SortByCategory(null, null);
            _itemsControl.ItemsSource = _dataView;
            _lastObjectType = null;
            _cachedPropertyItems = null;
        }
        else
        {
            var currentType = obj.GetType();

            // 如果类型与上次相同，只更新数据不重建界面
            if (_lastObjectType == currentType && _cachedPropertyItems != null)
            {
                foreach (var propertyItem in _cachedPropertyItems)
                {
                    propertyItem.Value = obj;
                    propertyItem.InitElement(); 
                }
                _itemsControl.ItemsSource = _dataView;
                return;
            }

            // 类型不同，重新构建界面
            _lastObjectType = currentType;

            // obj 获取title width
            var titleWidthAttribute = currentType.GetCustomAttributes(typeof(TitleWidthAttribute), true).OfType<TitleWidthAttribute>().FirstOrDefault();
            TitleWidthAttribute titleWidth = new TitleWidthAttribute(80, GridUnitType.Pixel);
            if (titleWidthAttribute != null)
            {
                titleWidth = titleWidthAttribute;
            }

            _cachedPropertyItems = TypeDescriptor.GetProperties(currentType).OfType<PropertyDescriptor>()
                .Where(item => PropertyResolver.ResolveIsBrowsable(item))
                .Select(r => CreatePropertyItem(r, titleWidth))
                .Do(item => item.InitElement())
                .ToList();

            _dataView = CollectionViewSource.GetDefaultView(_cachedPropertyItems);
            SortByCategory(null, null);
            _itemsControl.ItemsSource = _dataView;
        }
    }

    private void SortByCategory(object sender, ExecutedRoutedEventArgs e)
    {
        if (_dataView == null) return;

        using (_dataView.DeferRefresh())
        {
            _dataView.GroupDescriptions.Clear();
            _dataView.SortDescriptions.Clear();
            _dataView.SortDescriptions.Add(new SortDescription(PropertyItem.SortIndexProperty.Name, ListSortDirection.Ascending));
            //_dataView.GroupDescriptions.Add(new PropertyGroupDescription(PropertyItem.GroupIndexProperty.Name));
            _dataView.GroupDescriptions.Add(new PropertyGroupDescription(PropertyItem.CategoryProperty.Name));
        }
    }

    private void SortByName(object sender, ExecutedRoutedEventArgs e)
    {
        if (_dataView == null) return;

        using (_dataView.DeferRefresh())
        {
            _dataView.GroupDescriptions.Clear();
            _dataView.SortDescriptions.Clear();
            _dataView.SortDescriptions.Add(new SortDescription(PropertyItem.SortIndexProperty.Name, ListSortDirection.Ascending));
        }
    }

    private void SearchBar_SearchStarted(object sender, FunctionEventArgs<string> e)
    {
        if (_dataView == null) return;

        _searchKey = e.Info;
        if (string.IsNullOrEmpty(_searchKey))
        {
            foreach (UIElement item in _dataView)
            {
                item.Show();
            }
        }
        else
        {
            foreach (PropertyItem item in _dataView)
            {
                item.Show(item.PropertyName.ToLower().Contains(_searchKey) || item.DisplayName.ToLower().Contains(_searchKey));
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyDescriptor"></param>
    /// <returns></returns>
    protected virtual PropertyItem CreatePropertyItem(PropertyDescriptor propertyDescriptor, TitleWidthAttribute titleWidth)
    {

        PropertyAttribute property = propertyDescriptor.Attributes.OfType<PropertyAttribute>().FirstOrDefault() as PropertyAttribute;

        var editor = PropertyResolver.ResolveEditor(propertyDescriptor);

        var propertyItem = new PropertyItem()
        {
            Property = property,
            Category = (property?.Category ?? PropertyResolver.ResolveCategory(propertyDescriptor)).ToLanguage(),
            DisplayName = (property?.DisplayName ?? PropertyResolver.ResolveDisplayName(propertyDescriptor)).ToLanguage(),

            IsReadOnly = PropertyResolver.ResolveIsReadOnly(propertyDescriptor),
            DefaultValue = property?.DefaultValue ?? PropertyResolver.ResolveDefaultValue(propertyDescriptor),
            Editor = editor,
            Value = SelectedObject,
            EnableName = property?.EnableProperty ?? "",
            VisiableName = property?.VisibleProperty ?? "",
            CommandPropertyName = property?.CommandProperty ?? "",
            CommandContent = property?.CommandContentName ?? "",
            ButtonWidth = PropertyResolver.ResolveButtonWidth(propertyDescriptor),
            PropertyName = propertyDescriptor.Name,
            PropertyType = propertyDescriptor.PropertyType,
            PropertyTypeName = $"{propertyDescriptor.PropertyType.Namespace}.{propertyDescriptor.PropertyType.Name}",
            SortIndex = propertyDescriptor.Attributes.OfType<PropertyOrderAttribute>().FirstOrDefault()?.Index ?? (property?.Index ?? 0),
            TitleWidth = propertyDescriptor.Attributes.OfType<TitleWidthAttribute>().FirstOrDefault()?.Width ?? (property?.TitleWidth ?? titleWidth.Width),
            TitleVerticalAlignment = property?.TitleVerticalAlignment ?? VerticalAlignment.Center,
            TitleMargin = property != null && property?.TitleTop != 0 ? new Thickness(0, property.TitleTop, 0, 0) : new Thickness(0, 0, 4, 0)
        };
        propertyItem.Description = propertyItem.DisplayName;
        // 如果编辑器是ListBoxPropertyEditor或DataGridPropertyEditor，默认换行显示
        if (editor is ListBoxPropertyEditor || editor is DataGridPropertyEditor)
        {
            propertyItem.EditorOnNewLine = true;
        }

        return propertyItem;
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);
        TitleElement.SetTitleWidth(this, new GridLength(Math.Max(MinTitleWidth, Math.Min(MaxTitleWidth, ActualWidth / 3))));
    }
}
