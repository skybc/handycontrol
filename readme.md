# HandyControl 🛠️

HandyControl 是一个为 WPF 提供大量美观、可复用控件与主题的开源控件库，采用 Shared Project 架构以同时支持 .NET Framework（Net 4.0）与 .NET 8+ 平台。

**基于 / Based on**: https://github.com/HandyOrg/HandyControl（感谢原项目和所有贡献者）

**本地优化与开发仓库 / Local development fork**: https://github.com/skybc/handycontrol

---

## 🚀 主要特性

- 丰富的 UI 控件集合（Button、Dialog、PropertyGrid、DataGridEx、TreeEditor、ColorPicker 等）
- 可插拔的皮肤/主题系统（多个内置皮肤：Default / Dark / Violet）
- 支持本地化（.resx + 自动 Lang 提供器）
- Demo 应用展示所有控件用法与样例
- PropertyGrid：支持集合类型的 DataGrid 编辑器（参见 DATAGRID_IMPLEMENTATION.md）
- 支持通过 NuGet 打包（`HandyControl_Net_GE45.csproj` 在 Release 时会生成包）

---

## 目录结构（简要）

- `Shared/HandyControl_Shared/`：共享源码（核心控件、工具类、主题、资源）
- `Shared/HandyControlDemo_Shared/`：Demo 应用源码（展示控件使用示例）
- `Net_GE45/`：.NET 8+ 平台项目（引用 Shared）
- `Net_40/`：.NET 4.0 兼容项目（引用 Shared）
- `DATAGRID_IMPLEMENTATION.md`：PropertyGrid 的 DataGrid 编辑器实现说明
- `.github/`：仓库内部开发说明（包含 codebase guide）

> 详细的代码组织与开发约定请参见仓库内的 `/.github/copilot-instructions.md`，该文档包含控件开发、主题、国际化与常见任务的说明。

---

## 架构要点 🔧

- Shared Project 架构：所有平台共用 `Shared/HandyControl_Shared` 源码，通过各平台项目导入 `.projitems` 以实现跨目标平台编译。
- 主题合并：`Themes/Theme_GE45.txt` 列出要合并的 XAML 文件，构建时通过 `XamlCombine.exe` 生成最终 `Theme.xaml`（这是一个预构建步骤）。
- 控件遵循 WPF lookless 模式，使用 `TemplatePart` 声明模板部件并在 `OnApplyTemplate()` 中正确解绑/订阅事件。
- 国际化：使用 `.resx` 文件与 LangProvider，代码中直接使用 `Lang.PropertyName` 或 XAML 中 `{x:Static langs:Lang.PropertyName}`。
- XAML 命名空间：

```xaml
xmlns:hc="https://handyorg.github.io/handycontrol"
<hc:Button />
```

---

## 本地开发 — 快速开始 🧭

先决条件：
- Windows
- Visual Studio 2022+ 或等效的 dotnet SDK（支持 `net8.0-windows`）

构建与运行 Demo：

1. 打开解决方案 `HandyControl.sln`，选择 `Debug|Any CPU`（或需要构建 Net40 使用 `Debug-Net40|Any CPU`）。
2. 在 Visual Studio 中将 `HandyControlDemo_Net_GE45` 设为启动项目，然后运行（或在命令行中：）

```powershell
cd Net_GE45\HandyControlDemo_Net_GE45
dotnet run --framework net8.0-windows --project "HandyControlDemo_Net_GE45.csproj"
```

提示：项目有预构建步骤会运行 `XamlCombine.exe` 来生成主题资源，构建时请确保预构建步骤可以成功执行。

打包（NuGet）：`HandyControl_Net_GE45.csproj` 在 Release 构建时配置了自动生成包，使用 `dotnet pack`/`dotnet build -c Release` 即可。

---

## 如何添加新控件 / 示例 🌱

遵循仓库的约定：

1. 在 `Shared/HandyControl_Shared/Controls/[Category]/` 新增控件类（遵循 lookless 模式）。
2. 为控件添加样式文件到 `Shared/HandyControl_Shared/Themes/Styles/`，并在 `Theme_GE45.txt` 中注册样式文件路径（不要手动修改生成的 Theme.xaml）。
3. 在 `.projitems` 中添加控件源文件引用（Shared 项目会被各平台导入）。
4. 在 Demo 应用中新增对应的演示页面：`Shared/HandyControlDemo_Shared/UserControl/Controls/`。
5. 如需本地化，向 `Properties/Langs/Lang.resx` 添加键并为其他文化添加翻译文件。

---

## 重要说明与开发提示 ⚠️

- 使用 `ValueBoxes` 对常用的 bool/int 值进行装箱以减少分配。
- 控件 `OnApplyTemplate()` 中必须在获取新模板部件前先解绑旧订阅，避免内存泄露。
- 不要直接编辑生成的 `Theme.xaml`，应修改组合源文件并通过构建生成。
- PropertyGrid 的 DataGrid 编辑器实现参考 `DATAGRID_IMPLEMENTATION.md`，其中包含列生成规则与自定义命令（添加/删除）配置说明。

---

## 贡献与社区 🤝

欢迎贡献代码与文档：

- Fork -> 新分支 -> 提交 -> Pull Request
- 在 PR 描述中说明改动、测试步骤与兼容性影响
- 如果要提交大改动（例如公共 API 修改或重构），请先打开 issue 与维护者讨论。

目前仓库未包含明确的 `CONTRIBUTING.md` 与 `LICENSE`。建议尽快添加 LICENSE（例如 MIT）以明确代码使用条款。

---

## 参考/文档 📚

- 仓库代码指南：`.github/copilot-instructions.md`
- PropertyGrid DataGrid 编辑器说明：`DATAGRID_IMPLEMENTATION.md`
- 主题文件索引：`Shared/HandyControl_Shared/Themes/Theme_GE45.txt`

--- 