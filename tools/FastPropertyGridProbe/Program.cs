using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using HandyControl.Controls;
class Program
{
    [STAThread]
    static void Main()
    {
        var application = new Application();
        application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml") });
        application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/SkinSGammaLight.xaml") });
        var grid = new FastPropertyGrid();
        var window = new System.Windows.Window { Content = grid, Width = 700, Height = 500, ShowInTaskbar = false };
        var times = new System.Collections.Generic.List<double>();
        window.Show();
        for (int i = 0; i < 200; i++)
        {
            var watch = Stopwatch.StartNew();
            grid.SelectedObjects = new[] { new Model() };
            grid.UpdateLayout();
            var frame = new DispatcherFrame();
            grid.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => frame.Continue = false));
            Dispatcher.PushFrame(frame);
            times.Add(watch.Elapsed.TotalMilliseconds);
        }
        if (grid.EditorsCreated >= 1000) { throw new Exception("Virtualization failed"); }
        grid.SearchText = "P0001";
        grid.UpdateLayout();
        if (grid.Items.Count != 2) { throw new Exception("Search failed"); }
        grid.ToggleCategory("Misc");
        grid.SearchText = "";
        grid.SelectedObjects = null;
        window.Close();
        times.Sort();
        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(new { Samples = times.Count, P50 = times[100], P95 = times[190], grid.EditorsCreated, grid.EditorsReused, grid.PooledEditorCount, Machine = Environment.MachineName }));
    }
}


