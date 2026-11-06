# FastPropertyGrid 使用与二次开发

本文对应当前仓库实现（2026-11-05 源码核对）。命名空间为 `HandyControl.Controls`，程序集为 `HandyControl.dll`。使用方无需引用示例工程或复制控件源码。

## 1. 引用 DLL 并加载资源

当前 `Net_GE45/HandyControl_Net_GE45` 项目目标为 `net8.0-windows`。请使用包含 FastPropertyGrid 的本仓库构建产物；普通历史版本 DLL 不一定包含该控件。应用应为 Windows WPF 项目。

将 DLL 放入应用的 `lib` 目录，项目文件添加：

```xml
<PropertyGroup>
  <TargetFramework>net8.0-windows</TargetFramework>
  <UseWPF>true</UseWPF>
</PropertyGroup>
<ItemGroup>
  <Reference Include="HandyControl">
    <HintPath>lib\HandyControl.dll</HintPath>
    <Private>true</Private>
  </Reference>
</ItemGroup>
```

按发布产物保留随附依赖及所需语言资源目录。`App.xaml` 的资源中加载主题：

```xml
<Application.Resources>
  <ResourceDictionary>
    <ResourceDictionary.MergedDictionaries>
      <ResourceDictionary Source="pack://application:,,,/HandyControl;component/Themes/Theme.xaml" />
      <ResourceDictionary Source="pack://application:,,,/HandyControl;component/Themes/SkinStandardLight.xaml" />
    </ResourceDictionary.MergedDictionaries>
  </ResourceDictionary>
</Application.Resources>
```

已有 HandyControl 主题的应用只需确认现有 Theme.xaml 包含 FastPropertyGrid 样式，避免重复加载。

## 2. 最小使用示例

窗口添加命名空间和控件，放在有高度约束的 Grid 内：

```xml
<Window x:Class="Demo.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:hc="clr-namespace:HandyControl.Controls;assembly=HandyControl"
        Title="参数" Width="600" Height="650">
  <Grid Margin="12">
    <hc:FastPropertyGrid x:Name="PropertyPanel"
                        ShowSearchBar="True" ShowSortButton="True" />
  </Grid>
</Window>
```

模型定义在使用方项目中：

```csharp
using System.ComponentModel;
using System.Windows;
using HandyControl.Controls;

namespace Demo;

public class DeviceOptions
{
    [Category("基本"), DisplayName("设备名称"), PropertyOrder(0)]
    public string Name { get; set; } = "设备 A";

    [Category("基本"), DisplayName("启用")]
    public bool Enabled { get; set; } = true;

    [Category("排版")]
    public HorizontalAlignment Horizontal { get; set; } = HorizontalAlignment.Left;

    [Category("排版")]
    public VerticalAlignment Vertical { get; set; } = VerticalAlignment.Center;

    [Category("排版")]
    public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;

    [Category("路径"), PropertyFile(Extension = ".txt")]
    public string TextFile { get; set; } = "";

    [Category("路径"), PropertyFile(Extension = ".jpg|.png|.bmp")]
    public string ImageFile { get; set; } = "";

    [Category("路径"), PropertyFile]
    public string AnyFile { get; set; } = "";

    [Category("路径"), PropertyFolder(Description = "请选择输出目录")]
    public string OutputDirectory { get; set; } = "";

    [ReadOnly(true)]
    public string SerialNumber { get; set; } = "SN001";

    [Browsable(false)]
    public string InternalKey { get; set; } = "";
}
```

窗口构造函数中，在 `InitializeComponent()` 后设置：

```csharp
PropertyPanel.SelectedObject = new DeviceOptions();
```

MVVM 中可替换为 `SelectedObject="{Binding CurrentOptions}"`。编辑直接修改模型属性，不自动保存文件或数据库；持久化由应用处理。

## 3. 单对象与多对象行为

```csharp
PropertyPanel.SelectedObject = null;
PropertyPanel.SelectedObjects = new[]
{
    new DeviceOptions { Name = "设备 A" },
    new DeviceOptions { Name = "设备 B" }
};
```

`SelectedObjects` 中的有效对象优先，忽略 null 并按引用去重；没有有效对象时回退到 `SelectedObject`。去重后只有一个对象仍走单对象编辑路径。返回单对象时先清空 `SelectedObjects`。

| 项目 | 单对象 | 多对象 |
| --- | --- | --- |
| 编辑器 | 复用 PropertyGrid 编辑器及自定义注册 | 固定文本、枚举/布尔下拉、路径浏览 |
| 水平/垂直对齐 | 图标选择 | 枚举文字下拉 |
| TextAlignment | 枚举文字下拉 | 枚举文字下拉 |
| 提交 | 由具体编辑器绑定决定 | 文本失焦或 Enter；下拉选择、浏览确认立即提交 |
| 不同初始值 | 显示当前值 | 文本显示“多个值”；下拉不选中 |
| 失败报告 | 普通 WPF 绑定机制 | EditCompleted，逐对象执行，成功项不回滚 |

多对象只保留每个对象均存在、可浏览、属性类型和转换器类型一致的同名属性。第一个对象的只读属性和不能从字符串转换的属性会被排除；后续目标只读等写入错误可能在提交时报告。类别、显示名及顺序取第一个对象。不要将多对象模式理解为所有单对象编辑器功能的合集。

手动文本修改后按 Esc 重新读取模型值。搜索、切换分类、Refresh 和切换对象会提交已登记的多对象待写入内容；卸载不主动提交。需要明确“应用/取消”事务时，应让控件编辑业务对象的副本，再由业务代码确认。

```csharp
PropertyPanel.EditCompleted += (_, e) =>
{
    System.Diagnostics.Debug.WriteLine($"成功：{e.SuccessCount}");
    foreach (var failure in e.Failures)
    {
        System.Diagnostics.Debug.WriteLine(
            $"{failure.PropertyName}: {failure.Exception.Message}");
        // failure.Target 是写入失败的目标对象。
    }
};
```

该事件仅针对多对象提交，不应作为单对象通用保存事件。

### 路径选择

文件和目录均允许手动输入。取消浏览对话框不修改路径。扩展名只影响浏览过滤，不限制手动输入，也不验证业务路径。

文件扩展名支持 `.txt`、`.jpg|.png|.bmp`（实现也兼容分号、逗号）；始终保留“所有文件”。未配置时先尝试当前文本的扩展名，再回退到所有文件。目录描述用于对话框标题，当前目录存在时用于初始化目录。

多对象只有路径特性和对应配置一致才显示浏览按钮，否则使用普通文本编辑；文件特性优先于目录特性。浏览确认使用同一批量提交机制。

## 4. 常用配置、刷新与性能

| 成员 | 用途 |
| --- | --- |
| ShowSearchBar / ShowSortButton | 显示搜索和排序入口 |
| SearchText | 搜索文本；单对象匹配显示名或属性名，多对象匹配显示名 |
| MinTitleWidth / MaxTitleWidth | 标题宽度约束 |
| ToggleCategory(string) | 切换指定类别的展开状态 |
| Refresh() | UI 线程重建属性、选择快照和通知订阅 |
| IsEditorPoolingEnabled | 多对象文本/下拉编辑器池开关，普通 CLR 属性 |
| EditorsCreated / EditorsReused / PooledEditorCount | 编辑器创建、复用和池数量诊断 |

模型可实现 `INotifyPropertyChanged` 以反映外部修改。控件只处理 UI 线程通知，后台通知会忽略；请通过 Dispatcher 更新模型或调用 Refresh。SelectedObjects 集合内部增删不会自动重建选择快照，修改后调用 Refresh 或替换集合引用。

属性元数据常用 `[Category]`、`[DisplayName]`、`[ReadOnly]`、`[Browsable]`、`[PropertyOrder]` 和 HandyControl 的 `[Property]`。`Property` 的非空显示名/类别优先；避免同时给出冲突配置。

保留控件内部滚动与回收虚拟化；避免外套 ScrollViewer 或在纵向 StackPanel 中给予无限高度。池主要用于多对象编辑器，不代表单对象所有自定义控件都进入池。

## 5. 二开：用滑块编辑 double

以下代码放入使用方项目即可，无需修改 DLL。示例范围为 0～100；注册会影响该应用中所有 double 类型的单对象属性。值域应与模型一致，避免显示时被滑块强制约束。

```csharp
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HandyControl.Controls;

namespace Demo;

public class PercentSliderEditor : PropertyEditorBase
{
    public override FrameworkElement CreateElement(PropertyItem propertyItem)
    {
        return new Slider
        {
            Minimum = 0,
            Maximum = 100,
            TickFrequency = 1,
            IsSnapToTickEnabled = true,
            MinWidth = 140,
            IsEnabled = !propertyItem.IsReadOnly
        };
    }

    public override DependencyProperty GetDependencyProperty()
    {
        return RangeBase.ValueProperty;
    }
}
```

在首次显示属性面板前注册（例如窗口 InitializeComponent 之前）：

```csharp
PropertyResolver.RegisterTypeEditor(typeof(double), typeof(PercentSliderEditor));
```

编辑器类需为可实例化的 public 非抽象类，并有 public 无参构造函数。注册表是静态全局映射，同类型后注册覆盖前注册，没有公开注销接口；建议在应用启动时一次性注册。对已生成的面板注册后需调用 Refresh。

基类 `CreateBinding` 已将目标控件依赖属性绑定到 `propertyItem.Value` 对象上的 `propertyItem.PropertyName`，默认可写属性 TwoWay、只读属性 OneWay，更新触发为 PropertyChanged。这里 `Value` 实际承载所选模型对象，不是该属性的标量值。自定义控件应明确禁用只读编辑，不要仅依赖 OneWay 绑定。

## 6. 二开：仅为指定属性替换编辑器

不想全局覆盖所有 double 时，使用自定义解析器，重写 `CreateDefaultEditor`，再通过 FastPropertyGrid 的虚拟只读属性提供解析器：

```csharp
using System.ComponentModel;
using HandyControl.Controls;

namespace Demo;

public class DevicePropertyResolver : PropertyResolver
{
    public override PropertyEditorBase CreateDefaultEditor(PropertyDescriptor property)
    {
        if (property.PropertyType == typeof(double) && property.Name == "Percent")
        {
            return new PercentSliderEditor();
        }
        return base.CreateDefaultEditor(property);
    }
}

public class DevicePropertyGrid : FastPropertyGrid
{
    public override PropertyResolver PropertyResolver { get; } = new DevicePropertyResolver();
}

public class ProcessOptions
{
    public double Percent { get; set; } = 50;
    public double Temperature { get; set; } = 23.5;
}
```

使用方 XAML 添加 `xmlns:local="clr-namespace:Demo"`，将控件替换为：

```xml
<local:DevicePropertyGrid SelectedObject="{Binding CurrentOptions}" />
```

CurrentOptions 应为 ProcessOptions 实例。此方案无需同时执行上一节的全局 double 注册，否则其他 double 仍受全局注册影响。也可在解析器中检查自己定义的 Attribute，按属性元数据选择不同编辑器。

当前 `ResolveEditor` 并非 virtual。`EditorAttribute` 只在默认结果为 `ReadOnlyTextPropertyEditor` 时尝试覆盖，因此不能用它可靠替换已有 string/double 等内置编辑器；请使用上述注册或解析器方式。

## 7. 组合控件、生命周期和扩展边界

路径框等“输入框＋按钮”编辑器可以让 CreateElement 返回 Grid/UserControl，并重写 `CreateBinding(PropertyItem, DependencyObject)`，将绑定设置到内部实际输入控件。GetDependencyProperty 应返回实际值属性；基类默认绑定不会自动寻找内部子控件。

实现时保持以下约定：

- 每次 CreateElement 创建新的可视元素，不跨行共享已有父级的 WPF 控件。
- 绑定源明确使用 propertyItem.Value，路径使用 PropertyName；只读时禁止按钮和输入修改。
- 对话框确认后再赋值，取消不更新；若绑定为显式或失焦更新，可调用内部输入控件的 BindingExpression.UpdateSource。
- 避免编辑器静态持有模型，或长期订阅全局事件。基类没有统一的 Dispose/Detach 扩展接口；订阅外部事件需自行处理 Loaded/Unloaded 的幂等订阅与解绑，容器可能反复加载。
- 模型校验放在 setter 或业务层；转换器用于显示值与模型值转换，不能假设任意类型都支持字符串批量转换。

**多对象模式目前不调用自定义 PropertyEditorBase 或自定义解析器的编辑器选择。** 若要在多对象中增加滑块、颜色选择等控件，需要修改 DLL 中 FastPropertyGrid.PrepareContainerForItemOverride 的多对象分支，并接入现有 Commit、选择快照检查和 cleanup 解绑流程。这部分不是引用现有 DLL 后可注册的公开扩展点。

## 8. 排查与验收

| 现象 | 检查点 |
| --- | --- |
| XAML 找不到 FastPropertyGrid | DLL 是否来自当前分支；程序集名 HandyControl；项目目标兼容 |
| 控件空白或提示资源缺失 | Theme.xaml、皮肤资源是否加载，是否混用了不同版本 DLL/资源 |
| SelectedObject 设置后仍显示批量内容 | SelectedObjects 中仍有有效对象，先将其清空 |
| 集合增删后面板不变 | 调用 UI 线程 Refresh，或替换 SelectedObjects 引用 |
| 自定义编辑器不出现 | 是否为多对象模式；是否注册了精确属性类型；是否已 Refresh |
| 值显示但不能写入 | setter、ReadOnly、依赖属性类型、绑定错误和类型转换器 |
| 多对象部分失败 | 查看 EditCompleted.Failures，成功项已写入，不会自动回滚 |

接入后建议检查：单对象读写与只读、模型通知、多对象不同初始值和部分失败、路径确认与取消、自定义编辑器读写、滚动后切换对象。本文依据源码核对 API 和行为；不将文档示例视为已完成目标应用的运行验收。



## 9. 内置类型与编辑器完整索引

本节的“自动选择”指未注册自定义编辑器的单对象模式。解析使用属性声明类型的精确匹配，不是运行时值类型匹配。例如声明为 object、实际赋值 double，并不会自动使用数字编辑器。

| 属性声明类型 | 默认编辑器 | 说明 |
| --- | --- | --- |
| string | PlainTextPropertyEditor | 可被下拉数据源、文件、目录特性替换 |
| sbyte / byte | NumberPropertyEditor | 对应类型最小值到最大值 |
| short / ushort | NumberPropertyEditor | 对应类型最小值到最大值 |
| int / uint | NumberPropertyEditor | 对应类型最小值到最大值 |
| long / ulong | NumberPropertyEditor | 对应类型范围，但数值控件经 double 表示，大整数需关注精度 |
| float / double | NumberPropertyEditor | 对应类型范围；可使用 NumberRange |
| bool | SwitchPropertyEditor | 开关，绑定 ToggleButton.IsChecked |
| DateTime | DateTimePropertyEditor | 日期时间，绑定 DateTimePicker.SelectedDateTime |
| HorizontalAlignment | HorizontalAlignmentPropertyEditor | 内置图标选择，绑定 SelectedValue |
| VerticalAlignment | VerticalAlignmentPropertyEditor | 内置图标选择，绑定 SelectedValue |
| TextAlignment 及其他 enum | EnumPropertyEditor | 枚举下拉；默认使用枚举字段 Description，否则枚举名称 |
| System.Windows.Media.ImageSource | ImagePropertyEditor | 图像编辑器；Source 绑定由编辑器自行处理 |
| System.Windows.Media.Color | ColorPropertyEditor | ColorPopup，SelectedBrush；检查实际颜色转换绑定 |
| System.Drawing.Color | ColorPropertyEditor | 同上，不要与 WPF Color 混用 |
| System.Windows.Input.ICommand | CommandPropertyEditor | 默认浏览规则会隐藏命令，需 Property 特性显式纳入 |
| CommandGroup 派生类型 | CommandGroupPropertyEditor | 存在实现限制，见后文 |
| 数组、实现非泛型 IEnumerable 的类型（string 除外） | DataGridPropertyEditor | 默认集合表格；Property.IsListBox 改用 ListBoxPropertyEditor |
| decimal、char、Guid、TimeSpan、普通自定义类、object | ReadOnlyTextPropertyEditor | 没有专门自动编辑器，不自动递归展开对象 |
| Nullable<T>（如 int?、DateTime?） | 通常 ReadOnlyTextPropertyEditor | 单对象解析未解包 Nullable；不要套用多对象行为 |

精确类型映射意味着 BitmapImage 等 ImageSource 派生声明类型、具体 ICommand 实现类不会自动命中基类/接口映射。可将属性声明为映射类型，或注册精确类型编辑器。

其他可用编辑器类：`DatePropertyEditor`（DatePicker.SelectedDate）、`TimePropertyEditor`（TimePicker.SelectedTime）、`SliderNumberPropertyEditor`（滑块）、`FileSelectPropertyEditor`、`FolderSelectPropertyEditor`、`CommboxPropertyEditor`（注意源码拼写）、`ListBoxPropertyEditor`。Date/Time 编辑器未由默认类型表自动选择，需要解析器返回。不要为有必需构造参数的内置编辑器直接使用要求无参构造的 RegisterTypeEditor。

### 单对象编辑器决策顺序

1. 精确类型的全局注册映射。
2. CommandGroup 可赋值类型。
3. 集合：IsListBox 为 true 使用 ListBox，否则 DataGrid。
4. 对内置数字类型应用 NumberRange，IsSlider 决定滑块或数字框。
5. 内置映射类型：存在 ComboBoxItemsSourceProperty 时使用 Commbox 编辑器，之后才走类型默认编辑器；所以字符串下拉优先于文件、目录，数字范围又优先于下拉。
6. string 默认分支内：文件特性、目录特性、普通文本，按此顺序。
7. 非映射类型：enum 使用 EnumPropertyEditor，其余只读文本。普通 enum 的数据源在 EnumPropertyEditor 内处理，不支持将任意非映射类型仅靠 ComboBoxItemsSourceProperty 变成下拉。
8. 最终默认结果为 ReadOnlyTextPropertyEditor 时，才尝试 EditorAttribute。

多对象先按第 3 节规则筛选属性；对 Nullable<T> 解包后判断枚举/布尔，提供选择器，其他可转换类型走文本。NumberRange、ConverterType、集合编辑器、动态使能/可见、附加命令和类型注册不参与多对象编辑器构造。不要将单对象特性当作批量输入约束。

## 10. PropertyAttribute 全部成员参考

普通字符串属性可使用多行文本框；`PropertyGrid` 和 `FastPropertyGrid` 均支持：

```csharp
[Property("基本信息", "说明", IsMultiline = true, Height = 100)]
public string Description { get; set; }
```

默认仍为单行；多行时回车插入换行。`FastPropertyGrid` 多对象编辑使用第一个选中对象的属性配置，失焦或刷新等操作提交修改。文件、目录等专用编辑器不受此配置影响。

命名空间：`HandyControl.Controls`；仅用于属性。可写成 `[Property]`，有两个构造形式：`Property()` 和 `Property(string category, string displayName = "")`。

注意后者只传类别时会将 DisplayName 设为**空字符串**，当前解析使用 `??` 而不是空字符串判断，因此显示名可能为空。建议始终显式指定类别和显示名。

| 成员 | 类型 / 默认值 | 实际作用与注意事项 |
| --- | --- | --- |
| Category | string / null | 分组类别；非 null 时覆盖 CategoryAttribute，空字符串也覆盖 |
| DisplayName | string / null | 显示名；非 null 时覆盖 DisplayNameAttribute，空字符串也覆盖 |
| EnableProperty | string / null | 同一模型上的 bool 属性路径，绑定整个 PropertyItem.IsEnabled；需通知更新 |
| VisibleProperty | string / 空串 | 同一模型上的 bool 属性路径，经 BooleanToVisibilityConverter 绑定行可见性 |
| IsIgnore | bool / false | 存在 Property 时浏览规则直接返回 !IsIgnore，会覆盖 Browsable(false) 和命令默认隐藏规则 |
| CommandProperty | string / 空串 | 同一模型 ICommand 属性路径，在普通编辑器右侧追加按钮 |
| CommandContentName | string / 空串 | **当前附加按钮按字面量作为 Content**，没有把它绑定到同名属性；例如填“执行”，不要误填 nameof(ButtonTitle) |
| ButtonWidth | int / 0 | 附加按钮宽度，仅 >10 时明确赋值，否则用最小宽度等默认布局 |
| Description | string / null | 声明存在，但当前 CreatePropertyItemCore 将 Description 设为 DisplayName，未使用该值 |
| DefaultValue | object / null | 写入 PropertyItem.DefaultValue，优先于 DefaultValueAttribute；不代表自动写回模型或提供重置命令 |
| Index | int / 0 | 排序序号，PropertyOrderAttribute 优先；CommandGroup 内也用此序号排序 |
| TitleWidth | GridLength? / null | 运行时元数据可设置；不能直接作为 C# 特性的命名参数，改用 TitleWidthAttribute |
| DisplayMemberPathProperty | string / 空串 | 下拉项的显示成员路径，尽管名称带 Property，内容本身是项上的路径 |
| SelectedValuePathProperty | string / 空串 | 下拉项的取值成员路径；指定后绑定 SelectedValue，否则通常 SelectedItem |
| ComboBoxItemsSourceProperty | string / 空串 | 同一模型的数据源属性路径；普通 Commbox 还支持分号分隔的多个字符串常量 |
| ConverterType | Type / null | PropertyEditorBase.GetConverter 反射创建 IValueConverter；只对实际使用该基类转换绑定的编辑器生效 |
| Height | int / 150 | 集合编辑器或多行文本框高度，单位为设备无关像素；多行文本的非正值回退为 150 |
| IsMultiline | bool / false | 普通字符串文本框启用多行输入，支持回车、自动换行和垂直滚动 |
| DataGridHeight | int / 与 Height 同值 | 已弃用，内部转发 Height，使用 Height |
| IsListBox | bool / false | 集合使用 ListBox 编辑，否则 DataGrid |
| AddCommandProperty | string / 空串 | 集合新增按钮绑定的模型命令属性名，参数是当前选中项 |
| DeleteCommandProperty | string / 空串 | 集合删除按钮绑定的模型命令属性名，参数是当前选中项，可为 null |
| TitleVerticalAlignment | VerticalAlignment / Center | 单对象行标题垂直对齐 |
| TitleTop | int / 0 | 非零时标题 Margin=(0,TitleTop,0,0)；零时默认右边距 4 |
| GridColumnConverter | Type / null | DataGrid 列生成时使用的 IValueConverter 类型，配置在集合元素的列属性上；不是整个集合值转换器 |

`ConverterType` 与 `GridColumnConverter` 指定的类型应实现 IValueConverter 并有 public 无参构造函数。PlainText/File/Folder 等重写绑定的编辑器并未统一调用基类 GetConverter，因此不要假设 ConverterType 对所有编辑器有效。业务验证应放到模型/业务层。

### 其他特性和标准元数据

| 特性 | 用法 | 当前行为 |
| --- | --- | --- |
| PropertyOrderAttribute | `[PropertyOrder(10)]` | Index 可读写，升序排列，优先于 Property.Index |
| TitleWidthAttribute | `[TitleWidth(120, GridUnitType.Pixel)]` | 类或属性均可，构造参数为 int 和 GridUnitType；属性优先于 Property.TitleWidth，再到类，默认 80 像素 |
| NumberRangeAttribute | `[NumberRange(0, 100, 2)]` | Minimum/Maximum 为 double，DecimalPlaces 默认 -1；IsSlider 默认 false，SmallChange 默认 0.1 |
| PropertyFileAttribute | `[PropertyFile(Extension = ".txt|.csv")]` | string 文件浏览过滤，未指定扩展名见第 3 节 |
| PropertyFolderAttribute | `[PropertyFolder(Description = "请选择目录")]` | string 目录浏览标题 |
| CategoryAttribute | `[Category("基本")]` | 无 Property.Category 覆盖时使用 |
| DisplayNameAttribute | `[DisplayName("名称")]` | 无 Property.DisplayName 覆盖时使用，最终名称参与本地化转换 |
| BrowsableAttribute | `[Browsable(false)]` | 没有 Property 特性时控制静态浏览；Property.IsIgnore 优先 |
| ReadOnlyAttribute | `[ReadOnly(true)]` | PropertyDescriptor.IsReadOnly 驱动编辑器绑定/禁用 |
| DefaultValueAttribute | `[DefaultValue(10)]` | 仅解析元数据，不能代替模型初始化 |
| DescriptionAttribute | `[Description("说明")]` | 解析器提供 ResolveDescription，但当前行工厂未用它生成说明；枚举字段上的 Description 由 Enum 编辑器使用 |
| EditorAttribute | `[Editor(typeof(MyEditor), typeof(PropertyEditorBase))]` | 只在默认编辑器回退为只读文本时生效；反射类型解析失败可能抛异常 |
| TypeConverterAttribute | `[TypeConverter(typeof(MyConverter))]` | 多对象筛选/文本转换使用 PropertyDescriptor.Converter；须处理当前文化和失败 |

NumberRange 示例：

```csharp
[Property("采集", "曝光", Index = 1)]
[NumberRange(0.1, 1000, 2)]
public double Exposure { get; set; } = 10;

[Property("采集", "增益", Index = 2)]
[NumberRange(0, 100, 1, IsSlider = true, SmallChange = 0.5)]
public double Gain { get; set; } = 50;
```

范围和小数位是单对象 UI 配置，不能替代模型 setter 校验，多对象不应用此范围。

## 11. 下拉、使能、可见与按钮的组合示例

```csharp
using System.Collections.ObjectModel;
using System.Windows.Input;
using HandyControl.Controls;

namespace Demo;

public class Choice
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class EditorOptions
{
    // 实际应用的动态更新需实现 INotifyPropertyChanged。
    public bool CanEdit { get; set; } = true;
    public bool ShowOutput { get; set; } = true;

    [Property(IsIgnore = true)]
    public ObservableCollection<Choice> Choices { get; } = new()
    {
        new Choice { Id = 1, Name = "快速" },
        new Choice { Id = 2, Name = "精确" }
    };

    [Property("选项", "模式", ComboBoxItemsSourceProperty = nameof(Choices),
        DisplayMemberPathProperty = nameof(Choice.Name),
        SelectedValuePathProperty = nameof(Choice.Id), EnableProperty = nameof(CanEdit))]
    public int ModeId { get; set; } = 1;

    [Property("选项", "格式", ComboBoxItemsSourceProperty = "文本;图像;原始")]
    public string Format { get; set; } = "文本";

    [Property("路径", "输出目录", VisibleProperty = nameof(ShowOutput),
        CommandProperty = nameof(OpenOutputCommand), CommandContentName = "打开", ButtonWidth = 60)]
    [PropertyFolder(Description = "请选择输出目录")]
    public string Output { get; set; } = "";

    [Property(IsIgnore = true)]
    public ICommand OpenOutputCommand { get; set; }
}
```

OpenOutputCommand 由应用注入实现 ICommand 的命令实例；附加按钮未设置 CommandParameter，命令应从当前模型读取 Output。CanExecute 控制按钮能否执行。使能/可见源需为 bool，变更需在 UI 线程发出通知。

普通下拉中 `"文本;图像;原始"` 分割为多个常量；只有一个非空片段时按模型属性路径解析，不是单项常量。枚举编辑器的数据源始终是模型路径，不支持这种分号常量语法。

```csharp
public enum RunMode
{
    [System.ComponentModel.Description("自动")]
    Auto,
    [System.ComponentModel.Description("手动")]
    Manual
}
```

普通 enum 自动显示上述描述。自定义枚举数据源建议是 Enum[] 或枚举值列表，并隐藏数据源辅助属性；指定 SelectedValuePath 时对应项成员必须返回该枚举类型。多对象下拉直接使用枚举值，不保证相同的 Description 显示。Flags 枚举仍是单选枚举列表，不是位组合编辑器。

## 12. 集合编辑器与命令边界

```csharp
using System.Collections.ObjectModel;
using System.Windows.Input;
using HandyControl.Controls;

namespace Demo;

public class Channel
{
    public string Name { get; set; } = "";
    public double Value { get; set; }
}

public class ChannelOptions
{
    [Property("采集", "通道", Height = 220,
        AddCommandProperty = nameof(AddChannel), DeleteCommandProperty = nameof(DeleteChannel))]
    public ObservableCollection<Channel> Channels { get; set; } = new();

    [Property(IsIgnore = true)]
    public ICommand AddChannel { get; set; }

    [Property(IsIgnore = true)]
    public ICommand DeleteChannel { get; set; }
}
```

命令需由应用初始化：新增命令创建 Channel 并加入 Channels；删除命令检查参数是否为 Channel 后删除。两个命令参数均为当前选中项，未选中时可能为 null。ListBox 形式在同一集合 Property 上添加 `IsListBox = true`。ObservableCollection 用于通知增删，元素实现 INotifyPropertyChanged 用于通知值变更。

DataGrid 会根据元素属性生成列，并有自己的列类型、下拉及转换器处理；它不保证执行每一种顶层 PropertyEditorBase 的 CreateElement。元素列的 GridColumnConverter 与顶层 ConverterType 是两条路径。只读集合属性不等同于集合内部元素不可编辑，集合类型也不等同于一定支持 Add/Remove（数组固定长度、IEnumerable 可能只读）；业务命令必须检查实际能力。

`CommandGroupPropertyEditor` 当前创建按钮的代码中，Command 和 Content 的绑定处于注释状态，且命令组属性值为 null 时缺少保护。不要将其作为可直接执行的内置按钮组承诺给业务方；建议使用普通属性附加按钮或自定义编辑器。

## 13. 给后续 AI 的实现契约

读取本文生成代码时，按以下顺序确定方案：

1. 确认单对象还是多对象；自定义 UI 优先单对象解析器方案，多对象自定义控件需改库。
2. 确认属性声明类型是否精确命中第 9 节，不根据实际值猜测支持。
3. 只修改模型元数据即可满足时，优先 Property、NumberRange、文件/目录特性；所有辅助数据源和命令属性显式隐藏。
4. 全局类型编辑器会覆盖同类型所有属性；仅一个字段使用自定义解析器。禁止臆造 RegisterEditor、EditorTemplate、批量自定义编辑器注册或 Unregister 等不存在接口。
5. 保留绑定源为模型对象，避免把 PropertyItem.Value 当作标量。组合控件重写 CreateBinding，并绑定内部真正的值控件。
6. 精确处理只读、取消、转换、UI 线程及外部事件解绑；不得仅靠 UI 范围约束业务值。
7. 动态模型实现通知；替换集合内容后按实际通知规则 Refresh。Refresh 可能提交待编辑内容，不作为无副作用读取操作。
8. 多对象失败是部分成功语义，不假设事务回滚；检查 EditCompleted 的成功数和失败目标。
9. 引用 DLL 的应用先验证主题与程序集版本，再验证业务编辑器。不要复制示例工程专属资源路径。

### 核对源码入口

仓库内相对路径均位于 `Shared/HandyControl_Shared/`：

- `Controls/PropertyGrid/FastPropertyGrid.cs`：选择、筛选、虚拟化、多对象提交。
- `Controls/PropertyGrid/PropertyResolver.cs`：精确类型表、注册、编辑器优先级。
- `Controls/PropertyGrid/PropertyGrid.cs` 的 CreatePropertyItemCore：单对象元数据实际消费。
- `Controls/PropertyGrid/PropertyItem.cs` 的 InitElement：使能、可见、附加按钮和绑定。
- `Controls/PropertyGrid/PropertyAttribute.cs` 及同目录各 Attribute：元数据声明。
- `Controls/PropertyGrid/Editors/`：每个编辑器实际控件和绑定实现。
- `Themes/Styles/FastPropertyGrid.xaml`、`Themes/Styles/Base/FastPropertyGridBaseStyle.xaml`：默认样式和模板。

源码发生变更时优先重新核对这些入口。本文记录了当前实现缺口，不能用类注释中尚未实现的意图替代实际行为。
