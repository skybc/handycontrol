# Themes GridLength 检查记录

检查日期：2026-10-17。

## 修正清单

- [x] `Themes/Basic/Sizes.xaml`：增加 `TabControlRowHeightToken1`，类型为零像素 GridLength；保留 Border.Height 所需的 Double 资源。
- [x] `Themes/Styles/Base/TabControlBaseStyle.xaml`：修正 2 处行高触发器 Setter。
- [x] `Themes/Styles/TabControl.xaml`：修正 6 处行高触发器 Setter。
- [x] `Themes/Styles/Base/ChatBubbleBaseStyle.xaml`：修正 1 处列宽触发器 Setter，复用现有 12 像素 GridLength 资源。
- [x] `Themes/Theme.xaml`：通过控件项目 PreBuild 的 XamlCombine 流程生成。
- [x] `doc/check_style_resources.py`：补充模板内 TargetName 对应行列定义的 Setter 类型检查。
- [x] `doc/check_gridlength_resources.py`：新增独立全 Themes 检查，覆盖静态/动态资源、模板命名目标和以行列定义为目标的样式；输出绑定复核点。

## 验证结果

- `python doc/check_gridlength_resources.py`：202 个 XAML 文件，1112 个行列定义，84 个相关 Setter，72 处直接资源引用，0 个错误。计数包含生成的 Theme.xaml，因此源文件与生成结果会重复计数。
- 149 处绑定/模板绑定复核点：ContentHeight 使用 Double2GridLengthConverter；InfoElement 继承的 TitleWidth 和 PropertyItem.TitleWidth 均为 GridLength；MinContentHeight、MinWidth、MinHeight 均对应 Double。未发现额外类型错配。
- 在内存中恢复原来的 9 处源样式类型错误，独立检查器全部检出；未改写工作区文件。
- Release 控件库编译通过：0 错误，22 个警告。
- PowerShell STA 离屏验证：StandardLight、SGammaLight 两套主题共 306 次布局/渲染调用。覆盖基础、Sliding、InLine、Capsule、CapsuleSolid、Plus 页签样式，四种页签方向，动态添加/选择/移除 TabItem，空/有数据 VirtualDataGrid，SlidingTabContainer，以及四种 ChatBubble 消息类型的 Receiver/Sender 切换。
- 本次修改文件的 `git diff --check` 通过。
- 旧 `check_style_resources.py` 仍报告 118 项 `source differs after resource expansion`。其将资源展开后的源码与当前 HEAD 原文比较，属于源码等价性检查结果，不能作为本次全量类型检查通过的依据；本次运行未报告其他类别错误。

## 验收边界

未运行实际业务程序中的页面，也未进行可见窗口的鼠标键盘交互验收。静态检查不能证明业务运行时自行覆盖资源或绑定值后的类型安全；实际动态加载页面仍需复测。
