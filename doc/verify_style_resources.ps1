param(
    [string]$AssemblyPath = 'Net_GE45/HandyControl_Net_GE45/bin/Debug/net8.0-windows/HandyControl.dll'
)

$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName PresentationFramework
$assembly = [System.Reflection.Assembly]::LoadFrom((Resolve-Path $AssemblyPath))
$application = [System.Windows.Application]::new()
$failures = [System.Collections.Generic.List[string]]::new()
$styleCount = 0
$templateCount = 0

foreach ($skinName in @('Default', 'Dark', 'Violet', 'StandardLight')) {
    $application.Resources.MergedDictionaries.Clear()
    $theme = [System.Windows.Application]::LoadComponent([Uri]::new('/HandyControl;component/Themes/Theme.xaml', [UriKind]::Relative))
    $skin = [System.Windows.Application]::LoadComponent([Uri]::new("/HandyControl;component/Themes/Skin$skinName.xaml", [UriKind]::Relative))
    $application.Resources.MergedDictionaries.Add($theme)
    $application.Resources.MergedDictionaries.Add($skin)
    # Materialize the deferred resources before template loading. This makes
    # the check independent of ResourceDictionary hash enumeration order.
    $keys = @($theme.Keys) | Sort-Object { $_.ToString() }
    foreach ($key in $keys) {
        $null = $theme[$key]
    }
    foreach ($key in $keys) {
        try {
            $value = $theme[$key]
            if ($value -is [System.Windows.Style]) {
                $value.Seal()
                $styleCount++
                foreach ($setter in $value.Setters) {
                    if ($setter -is [System.Windows.Setter] -and $setter.Value -is [System.Windows.FrameworkTemplate]) {
                        $null = $setter.Value.LoadContent()
                        $templateCount++
                    }
                }
            }
            elseif ($value -is [System.Windows.FrameworkTemplate]) {
                $null = $value.LoadContent()
                $templateCount++
            }
        }
        catch {
            $failures.Add("$skinName / $key : $($_.Exception.ToString())")
        }
    }
}
"Styles sealed: $styleCount; templates loaded: $templateCount; failures: $($failures.Count)"
$failures | ForEach-Object { Write-Output $_ }
if ($failures.Count -gt 0) { exit 1 }
