using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using HandyControl.Controls;
using DataGrid = System.Windows.Controls.DataGrid;
using Button = System.Windows.Controls.Button;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        TestCollections();
        TestCopy();
        TestColumns();
        TestCommands();
        Console.WriteLine("DataGridPropertyEditor regression tests passed.");
    }

    private static void TestCollections()
    {
        var owner = new CollectionOwner();
        var editor = new DataGridPropertyEditor();
        var array = Item(owner, nameof(owner.Array));
        Invoke(editor, "AddItemToCollection", array, new Row { Name = "new" });
        Check(owner.Array.Length == 3 && owner.Array[2].Name == "new", "array add");
        Invoke(editor, "RemoveItemFromCollection", array, owner.Array[1], 1);
        Check(owner.Array.Length == 2 && owner.Array[1].Name == "new", "array remove by index");
        Invoke(editor, "RemoveItemFromCollection", array, owner.Array[1], 0);
        Check(owner.Array.Length == 1 && owner.Array[0].Name == null, "array remove with reordered view");
        var readOnly = Item(owner, nameof(owner.ReadOnlyArray));
        Check(!(bool)Invoke(editor, "CanChangeCollection", readOnly, true), "read-only array");

        var set = Item(owner, nameof(owner.Set));
        var row = new Row { Name = "set" };
        Invoke(editor, "AddItemToCollection", set, row);
        Check(owner.Set.Contains(row), "generic collection add");
        Invoke(editor, "RemoveItemFromCollection", set, row, 0);
        Check(!owner.Set.Contains(row), "generic collection remove");

        var empty = Item(owner, nameof(owner.Empty));
        Invoke(editor, "AddItemToCollection", empty, row);
        Check(owner.Empty != null && owner.Empty.Single() == row, "null collection initialization");
        var nullArray = Item(owner, nameof(owner.EmptyArray));
        Invoke(editor, "AddItemToCollection", nullArray, row);
        Check(owner.EmptyArray.Length == 1 && owner.EmptyArray[0] == row, "null array initialization");
        var unsupported = Item(owner, nameof(owner.ReadOnlyList));
        Check(!(bool)Invoke(editor, "CanChangeCollection", unsupported, true), "fixed collection disabled");
        var enumerable = Item(owner, nameof(owner.EnumerableOnly));
        Check(!(bool)Invoke(editor, "CanChangeCollection", enumerable, true), "null enumerable-only collection disabled");
        Check((Type)Invoke(editor, "GetCollectionElementType", typeof(UnusualCollection<int>)) == typeof(string), "generic element type");

        var noConstructor = Item(owner, nameof(owner.NoConstructor));
        var view = (Grid)editor.CreateElement(noConstructor);
        editor.CreateBinding(noConstructor, view);
        var buttons = (StackPanel)((Grid)view.Children[0]).Children[0];
        Check(!((Button)buttons.Children[0]).IsEnabled, "missing constructor disables add");
    }

    private static void TestCopy()
    {
        var source = new IndexedRow { Name = "after" };
        var target = new IndexedRow { Name = "before" };
        DataGridPropertyEditor.CopySameType(source, target);
        Check(target.Name == "after", "copy with indexer");
    }

    private static void TestColumns()
    {
        var first = new ColumnOwner { Options = new[] { "first" } };
        var item = Item(first, nameof(first.Rows));
        var editor = new DataGridPropertyEditor(new PropertyAttribute { AddCommandProperty = nameof(first.AddCommand) });
        var container = (Grid)editor.CreateElement(item);
        editor.CreateBinding(item, container);
        var grid = (DataGrid)container.Children[1];
        var visibleColumn = grid.Columns.Single(c => Equals(c.Header, "Conditional"));
        Check(visibleColumn.CellStyle != null, "conditional column exists");
        var row = new Row { IsShown = false, IsEditable = false };
        var cell = new DataGridCell { DataContext = row, Style = visibleColumn.CellStyle };
        Pump();
        Check(cell.Visibility == Visibility.Collapsed && !cell.IsEnabled, "conditional cell state");
        row.IsShown = true;
        row.IsEditable = true;
        Pump();
        Check(cell.Visibility == Visibility.Visible && cell.IsEnabled, "conditional cell updates");
        var readOnlyColumn = grid.Columns.Single(c => Equals(c.Header, "ReadOnly"));
        Check(readOnlyColumn.IsReadOnly, "read-only column with enable condition");
        var numberColumn = (DataGridTextColumn)grid.Columns.Single(c => Equals(c.Header, "Amount"));
        Check(((Binding)numberColumn.Binding).StringFormat != "N0", "decimal precision");

        var combo = (DataGridComboBoxColumn)grid.Columns.Single(c => Equals(c.Header, "Choice"));
        Pump();
        Check(ReferenceEquals(combo.ItemsSource, first.Options), "initial combo source");
        var second = new ColumnOwner { Options = new[] { "second" } };
        item.Value = second;
        editor.CreateBinding(item, container);
        Pump();
        Check(ReferenceEquals(combo.ItemsSource, second.Options), "combo source after object switch");

        var plain = Item(new PlainOwner(), nameof(PlainOwner.Rows));
        var plainEditor = new DataGridPropertyEditor();
        var plainView = (Grid)plainEditor.CreateElement(plain);
        Check(((DataGrid)plainView.Children[1]).Columns.Count > 0, "columns without PropertyAttribute");
    }

    private static void TestCommands()
    {
        var owner = new ColumnOwner();
        var item = Item(owner, nameof(owner.Rows));
        var editor = new DataGridPropertyEditor(new PropertyAttribute
        {
            AddCommandProperty = nameof(owner.AddCommand),
            DeleteCommandProperty = nameof(owner.DeleteCommand)
        });
        var container = (Grid)editor.CreateElement(item);
        editor.CreateBinding(item, container);
        var buttons = (StackPanel)((Grid)container.Children[0]).Children[0];
        Check(ReferenceEquals(((Button)buttons.Children[0]).Command, owner.AddCommand), "add command binding");
        Check(ReferenceEquals(((Button)buttons.Children[1]).Command, owner.DeleteCommand), "delete command binding");
    }

    private static PropertyItem Item(object owner, string name)
    {
        var descriptor = TypeDescriptor.GetProperties(owner)[name];
        return new PropertyItem { Value = owner, PropertyName = name, PropertyType = descriptor.PropertyType, IsReadOnly = descriptor.IsReadOnly };
    }

    private static object Invoke(object target, string name, params object[] args)
    {
        var method = target.GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        return method.Invoke(target, args);
    }

    private static void Pump() => Dispatcher.CurrentDispatcher.Invoke(() => { }, DispatcherPriority.ApplicationIdle);
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class CollectionOwner
    {
        public Row[] Array { get; set; } = { new Row(), new Row() };
        public Row[] ReadOnlyArray { get; } = { new Row() };
        public HashSet<Row> Set { get; set; } = new();
        public ICollection<Row> Empty { get; set; }
        public Row[] EmptyArray { get; set; }
        public IList<Row> ReadOnlyList { get; } = new ReadOnlyCollection<Row>(new List<Row>());
        public EnumerableOnly EnumerableOnly { get; set; }
        public List<NoDefaultConstructor> NoConstructor { get; } = new();
    }

    private sealed class UnusualCollection<T> : List<string> { }
    private sealed class EnumerableOnly : IEnumerable<Row>
    {
        public EnumerableOnly() { }
        public IEnumerator<Row> GetEnumerator() => Enumerable.Empty<Row>().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    private sealed class NoDefaultConstructor { public NoDefaultConstructor(string value) { } }
    private sealed class IndexedRow
    {
        public string Name { get; set; }
        public string this[int index] { get => Name; set => Name = value; }
    }

    private sealed class PlainOwner { public List<IndexedRow> Rows { get; } = new(); }
    private sealed class ColumnOwner
    {
        public List<Row> Rows { get; } = new();
        public string[] Options { get; set; }
        public ICommand AddCommand { get; } = new TestCommand();
        public ICommand DeleteCommand { get; } = new TestCommand();
    }

    private sealed class Row : INotifyPropertyChanged
    {
        private bool _isShown;
        private bool _isEditable;
        [Property(DisplayName = "Conditional", VisibleProperty = nameof(IsShown), EnableProperty = nameof(IsEditable))]
        public string Name { get; set; }
        [Property(DisplayName = "ReadOnly", EnableProperty = nameof(IsEditable))]
        public string ReadOnly => Name;
        [Property(DisplayName = "Amount")]
        public decimal Amount { get; set; }
        [Property(DisplayName = "Choice", ComboBoxItemsSourceProperty = nameof(ColumnOwner.Options))]
        public string Choice { get; set; }
        [Property(IsIgnore = true)]
        public bool IsShown { get => _isShown; set { _isShown = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsShown))); } }
        [Property(IsIgnore = true)]
        public bool IsEditable { get => _isEditable; set { _isEditable = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEditable))); } }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    private sealed class TestCommand : ICommand
    {
        public event EventHandler CanExecuteChanged { add { } remove { } }
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) { }
    }
}
