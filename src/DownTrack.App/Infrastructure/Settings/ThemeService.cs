using Microsoft.Win32;
using System.Windows;
using System.Windows.Media;

namespace DownTrack.Infrastructure.Settings;

public sealed class ThemeService
{
    public static ThemeService Instance { get; } = new();

    public void Apply(AppThemeMode mode)
    {
        var useLight = mode switch
        {
            AppThemeMode.Light => true,
            AppThemeMode.Dark => false,
            _ => DetectWindowsLightMode()
        };

        var r = Application.Current.Resources;

        SetBrush(r, "WindowBrush", useLight ? "#F5F7FB" : "#10131B");
        SetBrush(r, "SurfaceBrush", useLight ? "#FFFFFF" : "#171B24");
        SetBrush(r, "SurfaceRaisedBrush", useLight ? "#FFFFFF" : "#1C212C");
        SetBrush(r, "SurfaceHoverBrush", useLight ? "#F2F4FF" : "#252B38");
        SetBrush(r, "SurfaceSoftBrush", useLight ? "#FAFBFD" : "#141821");
        SetBrush(r, "BorderBrush", useLight ? "#E1E6EF" : "#303746");
        SetBrush(r, "BorderStrongBrush", useLight ? "#CBD3E1" : "#414A5C");
        SetBrush(r, "TextBrush", useLight ? "#182033" : "#F4F7FC");
        SetBrush(r, "MutedTextBrush", useLight ? "#69758B" : "#A5AFC0");

        SetBrush(r, "AccentSoftBrush", useLight ? "#F0EAFF" : "#2B2350");
        SetBrush(r, "BlueSoftBrush", useLight ? "#EBF2FF" : "#1E2C46");
        SetBrush(r, "CyanSoftBrush", useLight ? "#E8F8F5" : "#143A39");
        SetBrush(r, "DangerSoftBrush", useLight ? "#FFF0F3" : "#42212E");

        SetGradient(r, "SoftGradientBrush",
            useLight ? ["#F2ECFF", "#EEF4FF", "#EAFBF7"] : ["#201A33", "#18263B", "#16302E"]);

        SetGradient(r, "CardGradientBrush",
            useLight ? ["#FFFFFF", "#F7F8FF", "#F2FBFA"] : ["#1A1F2A", "#1B2230", "#19302F"]);
    }

    private static bool DetectWindowsLightMode()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            var value = key?.GetValue("AppsUseLightTheme");
            return value is null || Convert.ToInt32(value) != 0;
        }
        catch
        {
            return true;
        }
    }

    private static void SetBrush(
        ResourceDictionary resources,
        string key,
        string color)
    {
        resources[key] = new SolidColorBrush(
            (Color)ColorConverter.ConvertFromString(color)!);
    }

    private static void SetGradient(
        ResourceDictionary resources,
        string key,
        string[] colors)
    {
        var brush = resources[key] as LinearGradientBrush
            ?? new LinearGradientBrush();

        brush = brush.Clone();
        brush.GradientStops.Clear();

        for (var i = 0; i < colors.Length; i++)
        {
            brush.GradientStops.Add(new GradientStop(
                (Color)ColorConverter.ConvertFromString(colors[i])!,
                (double)i / (colors.Length - 1)));
        }

        resources[key] = brush;
    }
}