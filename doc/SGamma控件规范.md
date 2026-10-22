# SGammaLight 控件规范

来源：`SGamma原型(12).html`。以文件中最终生效 CSS 为准；注释仅用于解释意图。CSS px 对应 WPF DIP，不按物理像素固定尺寸。原型文件保持不变。

## 1. 色板

| 用途 | CSS 令牌 | 色值 |
|---|---|---|
| 内容白底 | white | #FFFFFF |
| 画布背景、只读输入 | canvas | #F8FAFC |
| 面板、悬停底色 | panel / sel-hi | #F1F5F9 |
| 下沉区域、普通分隔线 | sunk / line | #E2E8F0 |
| 弱分隔线 | line-soft | #F1F5F9 |
| 控件描边 | line-strong | #CBD5E1 |
| 主要文字 | t1 | #0F172A |
| 次级文字、普通按钮文字 | t2 | #334155 |
| 辅助文字 | t3 | #64748B |
| 占位、禁用文字 | t4 / dis | #94A3B8 |
| 图像深底 | ink | #0F172A |
| 主色、焦点、选中标识 | acc | #2563EB |
| 主色加深、选中文字 | acc-t | #1D4ED8 |
| 浅色选中底 | sel / acc-bg | #EFF6FF |
| 成功文字/描边、浅底、实心 | ok / ok-bg / ok-mid | #047857 / #ECFDF5 / #059669 |
| 警告文字/描边、浅底、实心 | warn / warn-bg / warn-mid | #B45309 / #FFFBEB / #F59E0B |
| 错误文字/描边、浅底、实心 | err / err-bg / err-mid | #B91C1C / #FEF2F2 / #DC2626 |

原型没有独立 Info 色支；兼容 HandyControl 的 Info 样式保留青色语义。原型注释说导航采用中性底，但实际 `.railbtn.on` 为蓝底白字；不能把所有选中场景统一解释成同一种填充。

## 2. 字体、间距和形状

- 正文 14；字重 400 / 500 / 600；大数字展示 24 / 32。正文行高 1.4，紧凑行高 1.15。
- 字体依次 Microsoft YaHei、PingFang SC、Segoe UI；WPF 使用本机可用字体。数字要求等宽数值显示。
- 图标 16 / 20 / 24；常规 margin、padding、gap 使用 4 / 8 / 16 / 24。实际 CSS 存在 12px 等局部例外，不能用注释否认这些现有规则。
- 圆角：密集单元/小徽标 2、普通控件 4、浮层和弹窗按钮 6、卡片与新版弹窗 10。
- 默认边框 1；树/列表选中强调条 3；键盘焦点轮廓 2，偏移 1。输入焦点为蓝色边框加 2px 浅蓝外环。
- 阴影：卡片 `0 1px 3px rgba(15,23,42,.24)`；浮窗 `0 4px 14px rgba(15,23,42,.28)`；菜单层 `0 10px 26px rgba(15,23,42,.16)`；模态层 `0 24px 60px rgba(15,23,42,.32)`。新版弹窗实际透明度 .30。

## 3. 控件与状态

| 控件 / CSS | 尺寸与布局 | 状态 |
|---|---|---|
| 普通按钮 `.btn` | 高 32、水平 padding 8、圆角 4、图标 16、图文 gap 4 | 白底、t2 文字、line-strong 边框；hover 为 panel 底、acc 边框、acc-t 字 |
| 主按钮 `.btn.pri` | 同普通按钮 | acc 底/边框、白字；hover acc-t 底 |
| 禁用按钮 `.btn.dis` | 同普通按钮 | dis 文字、line 边框；原型各类工具按钮还存在 .35～.5 的不同透明度 |
| 输入框/下拉 `input.fi,select.fi` | 高 32、圆角 4、水平 padding 16、14 字号 | 白底、t1 文字、line-strong 边框；readonly 为 canvas 底/t2 字 |
| 表格内输入 | 高 32、水平 padding 8 | 数字右对齐，普通表单数字仍左对齐 |
| 下拉箭头 | 16×16，距右 8，文本右侧预留 32 | 正常 t3，禁用 dis；HTML 展开列表为系统列表 |
| 勾选 `.chk` | 16×16、边框 1、圆角 2 | 白底；选中 acc 底/边框、白色勾 |
| 开关 `.sw` | 40×20、圆角 10、滑块 16×16、边距 2 | 默认 line-strong、开启 acc；禁用 opacity .5 |
| 树节点 `.tnode` | 高 32、文字 14 | hover panel；选中 sel 底/acc-t 字/左侧 3px acc 条 |
| 通用列表 `table.ltab` | 表头 32，内容行 40，水平 padding 8 | 表头 t3；行 hover canvas；选中 sel 底/左侧 3px acc 条 |
| 密集表格 `table.otab` | 表头和内容行 32 | 行 hover sel-hi；数字列右对齐 |
| 页签 | 文字 14，按所在页面布局 | 浅选中底、强调色标识；新主题标准页签使用底部 3px 条 |
| 标签 `.pill` | 高 24、水平 padding 8、圆角 4、gap 4 | 语义浅底配对应深色文字 |
| 菜单勾选 `.camop .cbx` | 16×16、圆角 2、边框 1.5 | 选中 acc 底/边框、白色图标 |

原型没有为通用 `.btn` 明确定义按下颜色；新主题保留现有深一档按下反馈，属于兼容补充。禁用交互由 WPF IsEnabled 控制，优先于 hover/pressed。

## 4. 页面骨架与弹窗

| 项目 | 尺寸 |
|---|---|
| 原型舞台 | 1920×1040 |
| 全局条 / 顶部工具条 | 40 / 68 |
| 面板标题 / 功能行 / 状态栏 | 40 / 36 / 36 |
| 导航按钮 / 导航栏宽 | 64×64 / 72 |
| 树栏 / 节点栏 / 属性栏 | 280 / 280 / 320 |
| 新版弹窗 S / M / L | 480×300 / 640×480 / 960×680 |
| 新版弹窗标题 / 页脚 | 32 / 48 |
| 弹窗内边距 / 按钮间距 | 16 / 8 |
| 弹窗按钮 | 高 32、水平 padding 16、圆角 6 |

新版弹窗规则覆盖早期 `.adlg.sm` 的 440 宽和旧标题栏尺寸。页面骨架、弹窗壳和业务布局由调用页面实现，主题提供配色、基础控件样式及弹窗尺寸资源，不强制改写所有 Window。

## 5. WPF 使用

控件库 `Theme` 和 Demo 首次启动默认采用 SGammaLight。跟随系统时，浅色使用 SGammaLight，深色继续使用 Dark。现有保存的皮肤选择继续生效；枚举旧数值保持不变。显式用法：

```xml
<hc:Theme Skin="SGammaLight" />
```

入口 `Themes/SkinSGammaLight.xaml` 独立合并通用基础资源、新色板、新样式和按钮字典。原有 StandardLight 文件未修改。

| 场景 | 样式 / 资源键 |
|---|---|
| 密集表格 | SGammaLightDenseDataGrid |
| 表格输入 / 数字输入 / 下拉 | SGammaLightTableTextBox / SGammaLightTableNumberTextBox / SGammaLightTableComboBox |
| 弹窗普通 / 主按钮 | SGammaLightDialogButton / SGammaLightDialogPrimaryButton |
| 开关 | SGammaLightSwitch（兼容 ToggleButtonSwitch） |
| 弹窗三档尺寸 | SGammaLightDialogSmallWidth/Height、SGammaLightDialogMediumWidth/Height、SGammaLightDialogLargeWidth/Height |

默认样式覆盖普通 Button、TextBox、PasswordBox、ComboBox、NumericUpDown、CheckBox、RadioButton、DataGrid、TreeView、TabControl、菜单、卡片和 PropertyGrid；按钮家族保持统一高 32 和字号 14。应用显式设置本地属性或自定义 Style 时遵循 WPF 原有优先级。

## 6. 验收边界

编译、WPF 资源加载与离屏布局检查分别记录；它们不代替人工视觉验收。需要在实际桌面确认不同 DPI、中文长文本、键盘焦点、下拉弹层、悬停/按下、业务弹窗与真实页面布局。


### 本次验证记录

- .NET 8 控件库与 Demo 构建通过（0 错误；现有源码产生 22 条警告）。
- 临时 STA WPF 验证程序检查默认主题、附加属性默认值、32 高按钮、输入内边距、40 高表格、40×20 开关、模拟 hover、禁用优先级、只读背景、树/表格选中条、场景样式与主题切换，共 20 项通过。
- 新主题资源解析和 Style.Seal 检查通过；15 类控件完成离屏模板实例化与布局，并检查渲染图。
- 未执行真实鼠标/键盘和多 DPI 人工验收；原型业务页面与弹窗布局由调用方按本规范使用。
