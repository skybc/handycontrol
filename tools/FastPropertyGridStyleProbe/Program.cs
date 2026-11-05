using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using HandyControl.Controls;
class Program
{
 [STAThread] static void Main()
 {
  var app = new Application();
  app.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml") });
  app.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/SkinSGammaLight.xaml") });
  var grid = new FastPropertyGrid { SelectedObject = new Model() };
  var window = new System.Windows.Window { Content = grid, Width = 700, Height = 500, ShowInTaskbar = false };
  window.Show(); grid.UpdateLayout();
  if (grid.Items.Count != 3) throw new Exception("Missing properties");
  if (Count<PropertyItem>(grid) != 3) throw new Exception("Shared property templates missing");
  grid.SearchText = "Name"; grid.UpdateLayout();
  if (grid.Items.Count != 1) throw new Exception("Search failed");
  Console.WriteLine("PASS: shared templates, read-only properties, grouping and search");
  window.Close();
 }
 static int Count<T>(DependencyObject root) { int count = 0; for(int i=0;i<VisualTreeHelper.GetChildrenCount(root);i++) { var child=VisualTreeHelper.GetChild(root,i); if(child is T) count++; count += Count<T>(child); } return count; }
 class Model { [Category("Basic")] public string Name {get;set;} = "demo"; [Category("Basic")] public bool Enabled {get;set;} = true; [Category("Other")] public int ReadOnly => 12; }
}
