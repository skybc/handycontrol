using System.ComponentModel;
using System.Collections.Generic;
using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace HandyControlDemo.ViewModel;

public class VirtualRowData
{
    public int Index { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreateTime { get; set; }
}

public class VirtualDataGridDemoViewModel : DemoViewModelBase<Data.DemoDataModel>
{
    private List<VirtualRowData> _virtualDataSource;
    private int _totalVirtualRows;
    private ICommand _generateMillionRowsCommand;
    private ICommand _generate100kRowsCommand;

    public List<VirtualRowData> VirtualDataSource
    {
        get => _virtualDataSource;
#if NET40
        set => Set(nameof(VirtualDataSource), ref _virtualDataSource, value);
#else
        set => Set(ref _virtualDataSource, value);
#endif
    }

    public int TotalVirtualRows
    {
        get => _totalVirtualRows;
#if NET40
        set => Set(nameof(TotalVirtualRows), ref _totalVirtualRows, value);
#else
        set => Set(ref _totalVirtualRows, value);
#endif
    }

    public ICommand GenerateMillionRowsCommand
    {
        get => _generateMillionRowsCommand ?? (_generateMillionRowsCommand = new RelayCommand(() =>
        {
            GenerateVirtualData(1_000_000);
        }));
    }

    public ICommand Generate100kRowsCommand
    {
        get => _generate100kRowsCommand ?? (_generate100kRowsCommand = new RelayCommand(() =>
        {
            GenerateVirtualData(100_000);
        }));
    }

    public VirtualDataGridDemoViewModel()
    {
        DataList = CreateDemoData();
        VirtualDataSource = new List<VirtualRowData>();
        TotalVirtualRows = 0;
    }

    private void GenerateVirtualData(int rowCount)
    {
        var data = new List<VirtualRowData>();
        for (int i = 0; i < rowCount; i++)
        {
            data.Add(new VirtualRowData
            {
                Index = i + 1,
                Name = $"Item {i + 1}",
                Description = $"Description for item {i + 1}",
                CreateTime = DateTime.Now.AddMinutes(-i)
            });
        }
        VirtualDataSource = data;
        TotalVirtualRows = rowCount;
    }

    private System.Collections.ObjectModel.ObservableCollection<Data.DemoDataModel> CreateDemoData()
    {
        var list = new System.Collections.ObjectModel.ObservableCollection<Data.DemoDataModel>();
        
        for (int i = 1; i <= 10; i++)
        {
            list.Add(new Data.DemoDataModel
            {
                Index = i,
                Name = $"Item {i}",
                IsSelected = i % 2 == 0,
                Remark = $"This is remark for item {i}",
                Type = (Data.DemoType)(i % 3),
                ImgPath = $"/HandyControlDemo;component/Resources/Img/Album/1{(i % 10)}.jpg"
            });
        }
        
        return list;
    }
}
