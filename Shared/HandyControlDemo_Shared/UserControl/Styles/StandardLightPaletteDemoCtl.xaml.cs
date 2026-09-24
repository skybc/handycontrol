using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Controls;

namespace HandyControlDemo.UserControl;

public partial class StandardLightPaletteDemoCtl
{
    private static readonly Dictionary<Color, string> ColorResourceNames = new()
    {
        [Color.FromRgb(248, 250, 252)] = "BackgroundColor",
        [Color.FromRgb(255, 255, 255)] = "RegionColor",
        [Color.FromRgb(241, 245, 249)] = "SecondaryRegionColor",
        [Color.FromRgb(226, 232, 240)] = "BorderColor",
        [Color.FromRgb(203, 213, 225)] = "SecondaryBorderColor",
        [Color.FromRgb(148, 163, 184)] = "DisabledColor",
        [Color.FromRgb(100, 116, 139)] = "SecondaryTextColor",
        [Color.FromRgb(51, 65, 85)] = "PrimaryTextColor",
        [Color.FromRgb(15, 23, 42)] = "TitleTextColor",
        [Color.FromRgb(239, 246, 255)] = "LightPrimaryColor",
        [Color.FromRgb(37, 99, 235)] = "PrimaryColor",
        [Color.FromRgb(29, 78, 216)] = "DarkPrimaryColor",
        [Color.FromRgb(236, 253, 245)] = "LightSuccessColor",
        [Color.FromRgb(5, 150, 105)] = "SuccessColor",
        [Color.FromRgb(4, 120, 87)] = "DarkSuccessColor",
        [Color.FromRgb(255, 251, 235)] = "LightWarningColor",
        [Color.FromRgb(245, 158, 11)] = "WarningColor",
        [Color.FromRgb(180, 83, 9)] = "DarkWarningColor",
        [Color.FromRgb(254, 242, 242)] = "LightDangerColor",
        [Color.FromRgb(220, 38, 38)] = "DangerColor",
        [Color.FromRgb(185, 28, 28)] = "DarkDangerColor",
        [Color.FromRgb(236, 254, 255)] = "LightInfoColor",
        [Color.FromRgb(8, 145, 178)] = "InfoColor",
        [Color.FromRgb(14, 116, 144)] = "DarkInfoColor"
    };

    public StandardLightPaletteDemoCtl()
    {
        InitializeComponent();
        AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(CopyColor_OnMouseLeftButtonUp), true);
    }

    private void CopyColor_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        for (var element = e.OriginalSource as DependencyObject; element != null; element = VisualTreeHelper.GetParent(element))
        {
            if (element is FrameworkElement { Tag: string resourceName } && !string.IsNullOrWhiteSpace(resourceName))
            {
                CopyResourceName(resourceName);
                e.Handled = true;
                return;
            }

            if (element is Border { Background: SolidColorBrush brush } && ColorResourceNames.TryGetValue(brush.Color, out resourceName))
            {
                CopyResourceName(resourceName);
                e.Handled = true;
                return;
            }
        }
    }

    private static void CopyResourceName(string resourceName)
    {
        try
        {
            // Clipboard can temporarily be locked by another process. WPF retries before failing.
            Clipboard.SetDataObject(resourceName);
            Growl.Success($"已复制资源名：{resourceName}");
        }
        catch (ExternalException)
        {
            Growl.Warning("剪贴板正被其他程序占用，请稍后重试。");
        }
    }
}
