# HandyControl 控件样式更改规范

## 1. 适用范围

本文档适用于 HandyControl 控件库的 WPF 样式、控件模板、主题资源、颜色皮肤及其合并配置的修改。

**凡是涉及控件样式的任务，开始修改前必须先阅读本文档。** 修改完成后应按照本文档的检查项确认源文件、合并清单和生成文件之间保持一致。

## 2. 核心规则

`Theme.xaml` 是由 `XamlCombine.exe` 根据合并清单编译/合并生成的最终资源字典，不是样式的维护源文件。

因此：

- 修改控件样式时，必须修改 `Themes` 目录下除 `Theme.xaml` 以外的源文件，通常是 `Styles/`、`Styles/Base/` 或 `Basic/` 下的 XAML 文件。
- 严禁直接编辑以下生成文件：
  - `Shared/HandyControl_Shared/Themes/Theme.xaml`
  - `Net_40/HandyControl_Net_40/Themes/Theme.xaml`
- 如果生成文件出现变化，应由构建前步骤重新生成，并将变化视为构建产物检查结果，而不是手工维护内容。
- 新增样式文件时，除创建 XAML 文件外，还必须把相对路径加入对应的合并清单：`Theme_GE45.txt` 和/或 `Theme_40.txt`。
- 不要只修改一个平台的源文件，除非该样式明确是平台专属；需要平台差异时，应在对应清单或 `Net_40/.../Themes/Styles/` 中使用现有的兼容结构。

## 3. 目录结构与关联关系

```text
Shared/
└─ HandyControl_Shared/
   └─ Themes/
      ├─ Theme.cs                    主题运行时逻辑、皮肤切换和资源字典加载
      ├─ Theme_GE45.txt              .NET 8+/GE45 主题合并清单
      ├─ Theme_40.txt                .NET 4.0 主题合并清单
      ├─ Theme.xaml                  共享主题生成结果（不要直接编辑）
      ├─ Basic/                      基础资源：颜色、画刷、字体、尺寸、几何等
      │  └─ Colors/                  默认、Dark、Violet 等颜色资源
      ├─ Styles/                     控件对外使用的样式和模板
      │  └─ Base/                    可复用的基础样式，通常由同控件的公开样式继承
      ├─ SkinDefault.xaml            默认皮肤
      ├─ SkinDark.xaml               深色皮肤
      ├─ SkinViolet.xaml             紫色皮肤
      └─ Lang.xaml                   主题相关的语言资源

Net_GE45/
└─ HandyControl_Net_GE45/
   └─ Theme.xaml                     由 Theme_GE45.txt 生成的 GE45 构建产物

Net_40/
└─ HandyControl_Net_40/
   └─ Themes/
      ├─ Theme.xaml                  由 Theme_40.txt 生成并被 Net40 项目编译
      └─ Styles/                     Net40 专属或兼容性覆盖样式
```

### 3.1 资源和样式的依赖层次

通常按以下关系组织：

```text
Colors / Basic resources
        ↓
Styles/Base/*BaseStyle.xaml
        ↓
Styles/*.xaml
        ↓
Theme_*.txt
        ↓  XamlCombine.exe（构建前步骤）
平台项目中的 Theme.xaml
        ↓
Theme.cs / ResourceHelper / Demo 或业务应用
```

- `Basic/` 提供样式使用的颜色、画刷、尺寸、字体、几何和转换器等资源。
- `Styles/Base/` 放置可复用的基础样式和控件模板；公开样式通常通过 `BasedOn` 继承它们。
- `Styles/` 放置控件最终使用的公开样式、模板和控件特定资源。
- `Theme_GE45.txt` 和 `Theme_40.txt` 决定文件进入最终主题的顺序。资源文件必须先于引用它们的样式文件；基础样式必须先于继承它们的公开样式。
- `Theme.cs` 运行时加载皮肤资源和合并后的主题资源。皮肤切换主要替换合并字典中的皮肤资源，不应通过修改生成的 `Theme.xaml` 实现。

### 3.2 两个目标框架的关系

- `Shared/HandyControl_Shared/Themes/` 是通用主题源文件的首选修改位置。
- GE45 项目在构建前执行：

  ```text
  XamlCombine.exe Theme_GE45.txt Theme.xaml
  ```

  输出到 `Net_GE45/HandyControl_Net_GE45/Theme.xaml`（项目命令中的相对路径以项目目录为基准）。该文件由构建流程使用，仍不得手工编辑。

- Net40 项目在构建前执行 `XamlCombine.exe`，使用 `Theme_40.txt` 输出到 `Net_40/HandyControl_Net_40/Themes/Theme.xaml`。
- 两份清单可能因框架兼容性而不同。编辑共享样式后，应检查两份清单是否都引用了该文件；Net40 专属覆盖则只维护 Net40 对应文件和清单。

## 4. 样式修改流程

1. 先阅读本文档，并确认目标控件、目标框架和当前使用的皮肤。
2. 在 `Shared/HandyControl_Shared/Themes/Styles/` 或 `Styles/Base/` 中定位样式源文件；颜色、间距、尺寸等公共资源优先检查 `Basic/`。
3. 使用 `x:Key`、`TargetType`、`BasedOn`、控件模板部件名和资源键确认修改点，避免在生成文件中搜索后直接修改。
4. 修改已有源文件。新增文件时，将路径按正确依赖顺序加入 `Theme_GE45.txt`、`Theme_40.txt`。
5. 如存在 Net40 兼容差异，再检查 `Net_40/HandyControl_Net_40/Themes/Styles/` 中的对应覆盖文件。
6. 构建对应项目，让 `XamlCombine.exe` 重新生成 `Theme.xaml`，确认构建成功且资源键、模板引用没有缺失。
7. 在 Demo 或相关控件示例中验证默认、Dark、Violet 皮肤，以及启用、禁用、悬停、按下、聚焦、校验错误等状态。
8. 查看 Git 差异：源文件和合并清单是主要修改内容；生成的 `Theme.xaml` 只应是构建产生的同步结果，不应包含手工编辑痕迹。

## 5. 修改检查清单

- [ ] 修改前已阅读本文档。
- [ ] 没有直接编辑任何生成的 `Theme.xaml`。
- [ ] 修改位置位于正确的源目录（`Basic/`、`Styles/`、`Styles/Base/` 或明确的平台专属目录）。
- [ ] 新增文件已加入正确的 `Theme_*.txt` 合并清单。
- [ ] 合并顺序满足“资源 → 基础样式 → 公开样式”的依赖关系。
- [ ] GE45 与 Net40 的行为和资源引用已检查，或已说明平台差异。
- [ ] 默认、Dark、Violet 皮肤及主要交互状态已验证。
- [ ] 已执行构建，并确认 `XamlCombine.exe` 生成过程成功。

## 6. 常见错误

### 直接修改 `Theme.xaml`

下次构建会覆盖手工修改，且源文件不会同步，最终导致修改丢失。应根据样式所属控件回到 `Styles/` 或 `Styles/Base/` 的源文件修改。

### 新增文件但没有加入清单

文件存在于磁盘上并不代表会进入最终主题。必须将相对路径加入目标框架使用的 `Theme_*.txt`，否则运行时不会加载该资源。

### 只更新公开样式，遗漏基础样式或公共资源

如果多个控件共用同一 `BaseStyle`、画刷或尺寸资源，应优先修改共享源，并检查所有引用方的影响，避免为单个控件复制一份不一致的模板。

### 忽略清单顺序

合并后的资源字典依赖声明顺序。被 `StaticResource` 引用的资源、被 `BasedOn` 继承的基础样式必须先出现在清单中；否则可能出现 XAML 加载或构建错误。
