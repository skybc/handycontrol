using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Controls;

/// <summary>
/// A container for form items with shared title layout settings.
/// </summary>
public class Form : ItemsControl
{
    private readonly List<FormItem> _observedItems = new List<FormItem>();
    private ItemsControl _itemsHost;
    private SearchBar _searchBar;

    public static readonly DependencyProperty AutoGroupProperty = DependencyProperty.Register(
        nameof(AutoGroup), typeof(bool), typeof(Form), new PropertyMetadata(false, OnOptionsChanged));

    public static readonly DependencyProperty ShowSearchProperty = DependencyProperty.Register(
        nameof(ShowSearch), typeof(bool), typeof(Form), new PropertyMetadata(false, OnOptionsChanged));

    public bool AutoGroup
    {
        get => (bool)GetValue(AutoGroupProperty);
        set => SetValue(AutoGroupProperty, value);
    }

    public bool ShowSearch
    {
        get => (bool)GetValue(ShowSearchProperty);
        set => SetValue(ShowSearchProperty, value);
    }

    public Form()
    {
        Loaded += (sender, args) => ObserveItems();
        Unloaded += (sender, args) => StopObserving();
    }

    public override void OnApplyTemplate()
    {
        if (_searchBar != null)
        {
            _searchBar.TextChanged -= OnSearchChanged;
        }
        if (_itemsHost != null)
        {
            _itemsHost.ItemsSource = null;
        }
        base.OnApplyTemplate();
        _itemsHost = GetTemplateChild("PART_ItemsHost") as ItemsControl;
        _searchBar = GetTemplateChild("PART_SearchBar") as SearchBar;
        if (_searchBar != null)
        {
            _searchBar.TextChanged += OnSearchChanged;
        }
        RefreshItems();
    }

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnItemsChanged(e);
        ObserveItems();
    }

    private static void OnOptionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var form = (Form)d;
        if (!form.ShowSearch && form._searchBar != null)
        {
            form._searchBar.Clear();
        }
        form.RefreshItems();
    }

    private void OnSearchChanged(object sender, TextChangedEventArgs e) => RefreshItems();

    private void OnItemChanged(object sender, EventArgs e) => RefreshItems();

    private void StopObserving()
    {
        foreach (var item in _observedItems)
        {
            DependencyPropertyDescriptor.FromProperty(FormItem.TitleProperty, typeof(FormItem)).RemoveValueChanged(item, OnItemChanged);
            DependencyPropertyDescriptor.FromProperty(FormItem.GroupProperty, typeof(FormItem)).RemoveValueChanged(item, OnItemChanged);
        }
        _observedItems.Clear();
    }

    private void ObserveItems()
    {
        StopObserving();
        foreach (var item in Items.OfType<FormItem>().Distinct())
        {
            _observedItems.Add(item);
            DependencyPropertyDescriptor.FromProperty(FormItem.TitleProperty, typeof(FormItem)).AddValueChanged(item, OnItemChanged);
            DependencyPropertyDescriptor.FromProperty(FormItem.GroupProperty, typeof(FormItem)).AddValueChanged(item, OnItemChanged);
        }
        RefreshItems();
    }

    private void RefreshItems()
    {
        if (_itemsHost == null)
        {
            return;
        }
        var keyword = string.Empty;
        if (ShowSearch && _searchBar != null)
        {
            keyword = _searchBar.Text.Trim();
        }
        var visible = Items.Cast<object>().Where(item => !(item is FormItem formItem) ||
            (formItem.Title?.ToString() ?? string.Empty).IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        var display = new List<object>();
        if (AutoGroup)
        {
            foreach (var group in Items.OfType<FormItem>().Where(item => !string.IsNullOrWhiteSpace(item.Group)).GroupBy(item => item.Group))
            {
                var matches = group.Where(item => visible.Contains(item)).ToList();
                if (matches.Count > 0)
                {
                    display.Add(new Divider { Content = group.Key,Margin=new Thickness(4,8,4,8) });
                    display.AddRange(matches);
                }
            }
            display.AddRange(visible.Where(item => !(item is FormItem formItem) || string.IsNullOrWhiteSpace(formItem.Group)));
        }
        else
        {
            display.AddRange(visible);
        }
        _itemsHost.ItemsSource = display;
    }

    public static readonly DependencyProperty TitleWidthProperty = DependencyProperty.Register(
        nameof(TitleWidth), typeof(GridLength), typeof(Form), new PropertyMetadata(new GridLength(60)));

    public static readonly DependencyProperty TitleAlignmentProperty = DependencyProperty.Register(
        nameof(TitleAlignment), typeof(HorizontalAlignment), typeof(Form), new PropertyMetadata(HorizontalAlignment.Left));

    public static readonly DependencyProperty TitleVerticalAlignmentProperty = DependencyProperty.Register(
        nameof(TitleVerticalAlignment), typeof(VerticalAlignment), typeof(Form), new PropertyMetadata(VerticalAlignment.Center));

    static Form()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Form), new FrameworkPropertyMetadata(typeof(Form)));
    }

    public GridLength TitleWidth
    {
        get => (GridLength)GetValue(TitleWidthProperty);
        set => SetValue(TitleWidthProperty, value);
    }

    public HorizontalAlignment TitleAlignment
    {
        get => (HorizontalAlignment)GetValue(TitleAlignmentProperty);
        set => SetValue(TitleAlignmentProperty, value);
    }

    public VerticalAlignment TitleVerticalAlignment
    {
        get => (VerticalAlignment)GetValue(TitleVerticalAlignmentProperty);
        set => SetValue(TitleVerticalAlignmentProperty, value);
    }

}
