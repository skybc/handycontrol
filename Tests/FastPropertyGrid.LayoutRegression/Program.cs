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
    private static void Main()
    {
        var app = new Application();
        app.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml") });
        app.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/SkinSGammaLight.xaml") });
        var grid = new FastPropertyGrid { IsCategorized = false, SelectedObject = new Model() };
        var window = new System.Windows.Window { Content = grid, Width = 700, Height = 500, ShowInTaskbar = false };
        window.Show();
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

    private class Model
    {
        [Property("General", "Name")]
        public string Name { get; set; } = "sample";
        [Property("General", "Choice", ComboBoxItemsSourceProperty = "One;Two")]
        public string Choice { get; set; } = "One";
        public string Unmarked { get; set; } = "hidden";
    }
}


