# DataGrid集合属性编辑器实现说明

## 概述
为PropertyGrid添加了对集合类型属性的DataGrid编辑支持，允许用户在属性面板中直接编辑集合元素。

## 实现的功能

### 1. PropertyAttribute新增属性
在 `PropertyAttribute.cs` 中添加了以下属性：

- **DataGridHeight** (int, 默认值: 150)
  - 设置DataGrid的显示高度（像素）

- **AddCommandProperty** (string)
  - 指定添加行的命令属性名称
  - 如果配置了该属性，会在DataGrid标题栏右侧显示"+"按钮

- **DeleteCommandProperty** (string)
  - 指定删除行的命令属性名称
  - 如果配置了该属性，会在DataGrid标题栏右侧显示"-"按钮

### 2. DataGridPropertyEditor编辑器
创建了新的 `DataGridPropertyEditor.cs` 文件，实现了以下功能：

#### 2.1 列生成规则
- 优先生成带有 `PropertyAttribute` 标注的属性列
- 如果集合元素类型没有任何带 `PropertyAttribute` 的属性，则生成所有公共属性列
- 忽略标记了 `PropertyAttribute.IsIgnore = true` 的属性

#### 2.2 列类型映射
根据属性类型和配置自动选择合适的列类型：

| 属性类型/配置 | DataGrid列类型 |
|-------------|---------------|
| bool | DataGridCheckBoxColumn |
| enum | DataGridComboBoxColumn (自动填充枚举值) |
| 配置了ComboBoxItemsSourceProperty | DataGridComboBoxColumn |
| 数字、文本等其他类型 | DataGridTextColumn |

#### 2.3 属性适配
- **IsIgnore**: 如果设置为true，该属性不会生成列
- **VisibleProperty**: 暂不支持动态可见性（会跳过该列）
- **EnableProperty**: 影响列的IsReadOnly状态
- **DisplayName**: 用作列标题

#### 2.4 增删改功能
- **添加按钮**: 如果PropertyAttribute配置了AddCommandProperty，在标题栏显示"+"按钮
- **删除按钮**: 如果PropertyAttribute配置了DeleteCommandProperty，在标题栏显示"-"按钮
- **编辑**: DataGrid默认支持单元格编辑
- **内置增删**: DataGrid的CanUserAddRows和CanUserDeleteRows默认开启

### 3. PropertyResolver更新
在 `PropertyResolver.cs` 中添加了集合类型检测：

- 添加了 `IsCollectionType()` 方法，判断属性是否为集合类型
- 在 `CreateDefaultEditor()` 方法中，对集合类型使用 `DataGridPropertyEditor`
- 排除字符串类型（虽然实现了IEnumerable<char>）

## 使用示例

```csharp
public class MyModel
{
    // 简单集合，使用默认高度150
    [Property("配置", "项目列表")]
    public ObservableCollection<ProjectItem> Projects { get; set; }

    // 自定义高度和添加/删除命令
    [Property("数据", "数据项", 
        DataGridHeight = 200,
        AddCommandProperty = "AddDataCommand",
        DeleteCommandProperty = "DeleteDataCommand")]
    public ObservableCollection<DataItem> DataItems { get; set; }

    // 对应的命令
    public ICommand AddDataCommand { get; set; }
    public ICommand DeleteDataCommand { get; set; }
}

// 集合元素定义
public class ProjectItem
{
    [Property("基本", "名称")]
    public string Name { get; set; }

    [Property("基本", "启用")]
    public bool IsEnabled { get; set; }

    [Property("基本", "类型")]
    public ProjectType Type { get; set; }

    [Property("基本", "优先级", ComboBoxItemsSourceProperty = "Priorities")]
    public int Priority { get; set; }

    // 不显示此属性
    [Property(IsIgnore = true)]
    public string InternalId { get; set; }
}
```

## 技术细节

### 集合类型检测
支持以下集合类型：
- 数组 (Array)
- 泛型集合 (IList<T>, ObservableCollection<T>, 等)
- 实现IEnumerable接口的类型

### 元素类型推断
- 数组: 使用 `Type.GetElementType()`
- 泛型集合: 使用泛型参数
- IEnumerable<T>: 通过接口反射获取元素类型

### 数据绑定
- DataGrid.ItemsSource 绑定到集合属性
- 列的Binding绑定到元素属性
- 命令按钮绑定到ViewModel的对应命令属性

## 文件变更清单

1. **PropertyAttribute.cs**
   - 添加 DataGridHeight 属性
   - 添加 AddCommandProperty 属性
   - 添加 DeleteCommandProperty 属性

2. **DataGridPropertyEditor.cs** (新建)
   - 实现 PropertyEditorBase
   - 创建DataGrid和列
   - 处理添加/删除按钮

3. **PropertyResolver.cs**
   - 添加 using System.Collections
   - 添加 IsCollectionType() 方法
   - 更新 CreateDefaultEditor() 以支持集合类型

4. **HandyControl_Shared.projitems**
   - 添加 DataGridPropertyEditor.cs 编译项

## 后续可能的改进

1. 支持VisibleProperty动态可见性
2. 支持ComboBoxItemsSourceProperty的动态数据源绑定
3. 支持列的自定义模板
4. 支持行验证
5. 支持排序和筛选配置
