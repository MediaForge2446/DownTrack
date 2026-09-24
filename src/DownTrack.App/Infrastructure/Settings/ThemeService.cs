using Microsoft.Win32;
using System.Windows.Media;

namespace DownTrack.Infrastructure.Settings;

public sealed class ThemeService
{
    private ThemeService()
    {
    }

    public static ThemeService Instance { get; } = new();

    public string ActiveTheme { get; private set; } = "System";

    public event EventHandler? ThemeChanged;

    public void Apply(string theme)
    {
        ActiveTheme = theme switch
        {
            "Dark" => "Dark",
            "Light" => "Light",
            _ => "System"
        };

        var isDark = ActiveTheme switch
        {
            "Dark" => true,
            "Light" => false,
            _ => IsWindowsDarkMode()
        };

        var resources = System.Windows.Application.Current.Resources;

        SetSolid(resources, "WindowBrush", isDark ? "#11131A" : "#F7F8FC");
        SetSolid(resources, "SurfaceBrush", isDark ? "#181B24" : "#FFFFFF");
        SetSolid(resources, "SurfaceRaisedBrush", isDark ? "#202430" : "#FFFFFF");
        SetSolid(resources, "SurfaceHoverBrush", isDark ? "#292E3D" : "#F1F3FF");
        SetSolid(resources, "SurfaceSoftBrush", isDark ? "#151821" : "#F8FAFD");
        SetSolid(resources, "BorderBrush", isDark ? "#303646" : "#E1E6F0");
        SetSolid(resources, "BorderStrongBrush", isDark ? "#454C60" : "#C7D1E2");
        SetSolid(resources, "TextBrush", isDark ? "#F5F7FB" : "#151A2C");
        SetSolid(resources, "MutedTextBrush", isDark ? "#A9B1C2" : "#6B758B");
        SetSolid(resources, "AccentSoftBrush", isDark ? "#302450" : "#EFE8FF");
        SetSolid(resources, "BlueSoftBrush", isDark ? "#203A62" : "#EAF1FF");
        SetSolid(resources, "CyanSoftBrush", isDark ? "#123934" : "#E9F8F5");
        SetSolid(resources, "DangerSoftBrush", isDark ? "#432532" : "#FFF0F3");

        SetGradient(resources, "PrimaryGradientBrush",
            isDark
                ? ["#7B61FF", "#B66FF5", "#4F86FF"]
                : ["#6F4DFF", "#9B63FF", "#3C8DFF"]);

        SetGradient(resources, "AuroraGradientBrush",
            isDark
                ? ["#7B61FF", "#4F86FF", "#18B9A5"]
                : ["#7657FF", "#4B86FF", "#18B9A5"]);

        SetGradient(resources, "SoftGradientBrush",
            isDark
                ? ["#292041", "#203050", "#173C3A"]
                : ["#F2EEFF", "#EEF4FF", "#EAFBF7"]);

        SetGradient(resources, "CardGradientBrush",
            isDark
                ? ["#1D202B", "#202C43", "#193936"]
                : ["#FFFFFF", "#F7F8FF", "#F1FBF8"]);

        ThemeChanged?.Invoke(this, EventArgs.Empty);
    }

    public static bool IsWindowsDarkMode()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(
                @"SoftwareMicrosoftWindowsCurrentVersionThemesPersonalize");

            var value = key?.GetValue("AppsUseLightTheme");
            return value is int intValue && intValue == 0;
        }
        catch
        {
            return false;
        }
    }

    private static void SetSolid(System.Collections.IDictionary resources, string key, string hex)
    {
        if (resources[key] is SolidColorBrush brush)
            brush.Color = (Color)ColorConverter.ConvertFromString(hex);
        else
            resources[key] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
    }

    private static void SetGradient(
        System.Collections.IDictionary resources,
        string key,
        string[] colors)
    {
        if (resources[key] is not LinearGradientBrush brush ||
            brush.GradientStops.Count != colors.Length)
        {
            var gradient = new LinearGradientBrush
            {
                StartPoint = new System.Windows.Point(0, 0),
                EndPoint = new System.Windows.Point(1, 1)
            };

            for (var index = 0; index < colors.Length; index++)
                gradient.GradientStops.Add(
                    new GradientStop(
                        (Color)ColorConverter.ConvertFromString(colors[index]),
                        index / (double)(colors.Length - 1)));

            resources[key] = gradient;
            return;
        }

        for (var index = 0; index < colors.Length; index++)
            brush.GradientStops[index].Color =
                (Color)ColorConverter.ConvertFromString(colors[index]);
    }
}