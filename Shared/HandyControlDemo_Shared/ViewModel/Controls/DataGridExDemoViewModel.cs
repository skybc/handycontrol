using System.ComponentModel;

namespace HandyControlDemo.ViewModel;

public class DataGridExDemoViewModel : DemoViewModelBase<Data.DemoDataModel>
{
    public DataGridExDemoViewModel()
    {
        DataList = CreateDemoData();
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
