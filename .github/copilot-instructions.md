# HandyControl Codebase Guide

## Project Overview
HandyControl is a WPF control library providing reusable, styled UI controls for .NET applications. This codebase uses a **Shared Project architecture** to support multiple .NET Framework versions (NET40, NET45+) from a single source.

## Architecture

### Shared Project Structure
- **`Shared/HandyControl_Shared/`**: Core control library source (shared .projitems)
  - `Controls/`: Custom WPF controls (organized by category: Attach, Panel, Input, etc.)
  - `Tools/`: Utilities, converters, helpers, extensions
  - `Themes/`: XAML resource dictionaries for styling
  - `Data/`: Data models and value objects
  - `Interactivity/`: Behavior base classes
  - `Properties/Langs/`: Localized resource files (.resx)
  
- **`Net_GE45/HandyControl_Net_GE45/`**: .NET 8+ specific project
- **`Net_40/HandyControl_Net_40/`**: .NET 4.0 specific project (legacy)

These platform-specific projects **import** the shared `.projitems` file. DO NOT duplicate code between platforms - always edit the Shared projects.

### Demo Application Structure
- **`Shared/HandyControlDemo_Shared/`**: Demo application source (shared)
  - `UserControl/`: Demo pages showcasing each control
  - `ViewModel/`: Uses MVVMLight messaging for navigation
  - `Data/Model/`: Demo data models

The demo uses **GalaSoft.MvvmLight.Messaging** for loose-coupled component communication.

## Key Patterns

### Control Development
Controls follow WPF's **lookless control pattern**:

```csharp
[TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
public class MyControl : Control
{
    private const string ElementPanel = "PART_Panel";
    private Panel _panel;
    
    public override void OnApplyTemplate()
    {
        if (_panel != null)
        {
            // Unsubscribe old events
        }
        base.OnApplyTemplate();
        
        _panel = GetTemplateChild(ElementPanel) as Panel;
        if (_panel != null)
        {
            // Subscribe new events
        }
    }
}
```

Always declare `[TemplatePart]` attributes and handle template reapplication correctly.

### Attached Properties
The library uses **attached properties** extensively for opt-in behavior (see `Controls/Attach/` folder):

```csharp
public class MyAttach
{
    public static readonly DependencyProperty MyProperty = 
        DependencyProperty.RegisterAttached(
            "My", typeof(bool), typeof(MyAttach), 
            new PropertyMetadata(ValueBoxes.FalseBox, OnMyChanged));
    
    public static void SetMy(DependencyObject element, bool value) 
        => element.SetValue(MyProperty, ValueBoxes.BooleanBox(value));
    
    public static bool GetMy(DependencyObject element) 
        => (bool)element.GetValue(MyProperty);
}
```

**Always use `ValueBoxes` for boxing bool/int values** to avoid allocations (see `Data/ValueBoxes.cs`).

### Theme System
Themes are built using a **custom XAML combiner**:
- Source files: `Themes/Theme_GE45.txt` (lists XAML files to merge)
- Build target runs `XamlCombine.exe` to generate `Theme.xaml`
- Skins: `SkinDefault.xaml`, `SkinDark.xaml`, `SkinViolet.xaml`

Custom themes extend `Theme` class and override `GetSkin()` and `GetTheme()` (see `Themes/Theme.cs`).

### Localization
Uses .resx files with custom `Lang` provider:
- `Properties/Langs/Lang.resx` (default)
- `Lang.[culture].resx` for translations (en, zh-CN, ja, etc.)
- `LangProvider.cs` is T4-generated - don't edit manually
- Use `Lang.PropertyName` in code, `{x:Static langs:Lang.PropertyName}` in XAML

### PropertyGrid System
The PropertyGrid uses reflection + attributes to auto-generate editors:

```csharp
public class MyModel
{
    [Property("Category", "Display Name", 
        EnableProperty = "IsFieldEnabled",
        VisibleProperty = "IsFieldVisible")]
    public string MyProperty { get; set; }
}
```

Recent addition: **`DataGridPropertyEditor`** for collection properties (see `DATAGRID_IMPLEMENTATION.md`):
- Supports `DataGridHeight`, `AddCommandProperty`, `DeleteCommandProperty`
- Auto-generates columns from element type with `[Property]` attributes
- Register custom editors: `PropertyResolver.RegisterTypeEditor(typeof(T), typeof(MyEditor))`

## Build & Run

### Build Solution
Open `HandyControl.sln` in Visual Studio 2022+. The solution has multiple configurations:
- `Debug|Any CPU` - Builds .NET 8+ projects
- `Debug-Net40|Any CPU` - Builds legacy .NET 4.0 projects

**Pre-build step**: `XamlCombine.exe` generates theme XAML (see project file `PreBuild` target).

### Run Demo
Set `HandyControlDemo_Net_GE45` as startup project. The demo showcases all controls with live examples.

### NuGet Package
`HandyControl_Net_GE45.csproj` has `<GeneratePackageOnBuild>True</GeneratePackageOnBuild>` for Release builds.

## Common Tasks

### Adding a New Control
1. Create in `Shared/HandyControl_Shared/Controls/[Category]/`
2. Add to `.projitems` file (both HandyControl_Shared and target .csproj)
3. Create style in `Themes/Styles/` folder
4. Add style path to `Theme_GE45.txt`
5. Create demo in `HandyControlDemo_Shared/UserControl/Controls/`

### Modifying Themes
Edit individual XAML files in `Themes/Styles/` or `Themes/Basic/`, then rebuild. DO NOT directly edit the generated `Theme.xaml`.

### Adding Localization
Add key to `Lang.resx`, then add translations to culture-specific `.resx` files. Run T4 template if needed.

## Dependencies
- **Core library**: Only Microsoft.NETFramework.ReferenceAssemblies (build-time)
- **Demo**: AvalonEdit (code editor), MVVMLight (messaging)
- **Embedded**: Microsoft.Expression.* and System.Windows.Interactivity (in Shared projects)

## XAML Namespace
Controls use unified namespace URI: `https://handyorg.github.io/handycontrol` (see `Properties/AssemblyInfo.cs`).

```xaml
xmlns:hc="https://handyorg.github.io/handycontrol"
<hc:Button />
```

## Coding Conventions
- **Namespaces**: Use file-scoped namespaces where supported
- **Null handling**: Use null-conditional operators (`?.`, `??`)
- **Event unsubscribe**: Always unsubscribe in `OnApplyTemplate()` before getting new template parts
- **DependencyProperty**: Use static readonly fields, follow naming `[PropertyName]Property`
- **Comments**: XML docs on public APIs; implementation comments in Chinese are acceptable

## Recent Changes
Check `DATAGRID_IMPLEMENTATION.md` for details on PropertyGrid DataGrid editor feature added recently.
