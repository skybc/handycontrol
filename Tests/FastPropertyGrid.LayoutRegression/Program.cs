using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HandyControl.Controls;
using TextBox = System.Windows.Controls.TextBox;
using ComboBox = System.Windows.Controls.ComboBox;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        var app = new Application();
        app.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml") });
        app.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/SkinSGammaLight.xaml") });
        var grid = new FastPropertyGrid { IsCategorized = false, SelectedObjects = new[] { new Model() } };
        var window = new System.Windows.Window { Content = grid, Width = 700, Height = 500, ShowInTaskbar = false };
        window.Show();
        grid.UpdateLayout();
        VerifySelection(grid);
        if (args.Contains("--selection-only"))
        {
            window.Close();
            return;
        }
        grid.SelectedObjects = new[] { new Model() };
        grid.UpdateLayout();
        var single = Capture(grid);
        grid.SelectedObjects = new[] { new Model(), new Model() };
        grid.UpdateLayout();
        var multi = Capture(grid);
        if (single.Count != 2 || multi.Count != 2) throw new Exception($"Missing rows: single={single.Count}, multi={multi.Count}");
        foreach (var pair in single)
        {
            if (!multi.TryGetValue(pair.Key, out var actual) || pair.Value != actual)
            {
                throw new Exception($"{pair.Key}: single={pair.Value}; multi={actual}");
            }
        }
        window.Close();
        Console.WriteLine("PASS: single/multiple property layout");
    }

    private static void VerifySelection(FastPropertyGrid grid)
    {
        var first = new Model();
        var second = new Model();
        var next = new Model();
        grid.SelectedObjects = new object[] { null, first, first };
        grid.UpdateLayout();
        Require(grid.Items.Count == 2 && first.SubscriberCount == 1, "Duplicate references must use one target and subscription");
        var singleEditor = Descendants<PropertyItem>(grid).Single(item => item.PropertyName == nameof(Model.Name));
        Descendants<TextBox>(singleEditor).First().Text = "single edit";
        Require(first.Name == "single edit", "Single-element selection must retain single-object editing");

        grid.SelectedObjects = new[] { first, second };
        grid.UpdateLayout();
        Require(first.SubscriberCount == 1 && second.SubscriberCount == 1, "Multiple selection subscriptions");
        var row = Descendants<PropertyItem>(grid).Single(item => item.PropertyName == nameof(Model.Name));
        var pendingEditor = Descendants<TextBox>(row).First();
        pendingEditor.Text = "pending edit";
        Require(first.Name == "single edit" && second.Name == "sample", "Multi-object text should remain pending");
        grid.SelectedObjects = new[] { next };
        grid.UpdateLayout();
        Require(first.Name == "pending edit" && second.Name == "pending edit" && next.Name == "sample",
            "Selection switch must commit to original targets");
        Require(first.SubscriberCount == 0 && second.SubscriberCount == 0 && next.SubscriberCount == 1,
            "Selection switch must release old subscriptions");
        pendingEditor.Text = "stale edit";
        Require(first.Name == "pending edit" && second.Name == "pending edit", "Released editors must not update old targets");

        foreach (var empty in new System.Collections.IEnumerable[] { null, Array.Empty<object>(), new object[] { null, null } })
        {
            grid.SelectedObjects = new[] { next };
            grid.SelectedObjects = empty;
            grid.UpdateLayout();
            Require(grid.Items.Count == 0 && next.SubscriberCount == 0, "Empty selection must clear rows and subscriptions");
        }
        var mutable = new System.Collections.Generic.List<object> { first };
        grid.SelectedObjects = mutable;
        mutable.Clear();
        grid.Refresh();
        Require(grid.Items.Count == 0 && first.SubscriberCount == 0, "Refresh must rebuild mutable collection snapshots");
        Console.WriteLine("PASS: selection normalization, switching, pending commit, editor and subscription release");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
        {
            throw new Exception(message);
        }
    }

    private static System.Collections.Generic.Dictionary<string, string> Capture(FastPropertyGrid grid)
    {
        return Descendants<PropertyItem>(grid).ToDictionary(item => item.PropertyName, item =>
        {
            var title = Descendants<TextBlock>(item).First(x => x.Name == "PART_TitleTextBlock");
            var control = Descendants<Control>(item).First(x => x is TextBox || x is ComboBox);
            return $"row={item.Margin}; title={title.Margin}/{title.Padding}/{title.ActualWidth}; editor={control.GetType().FullName}/{control.Margin}/{control.Padding}/{control.ActualWidth}";
        });
    }

    private static System.Collections.Generic.IEnumerable<T> Descendants<T>(DependencyObject root) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
        {
            var child = VisualTreeHelper.GetChild(root, i);
            if (child is T match) yield return match;
            foreach (var descendant in Descendants<T>(child)) yield return descendant;
        }
    }

    private class Model : System.ComponentModel.INotifyPropertyChanged
    {
        private System.ComponentModel.PropertyChangedEventHandler _propertyChanged;
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }
        public int SubscriberCount => _propertyChanged?.GetInvocationList().Count(handler => handler.Target is FastPropertyGrid) ?? 0;

        [Property("General", "Name")]
        public string Name { get; set; } = "sample";
        [Property("General", "Choice", ComboBoxItemsSourceProperty = "One;Two")]
        public string Choice { get; set; } = "One";
        public string Unmarked { get; set; } = "hidden";
    }
}


