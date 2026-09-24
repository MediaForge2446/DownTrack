using System.Windows;
using System.Windows.Input;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.Views;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        ApplyDirection();
        LocalizationManager.Instance.LanguageChanged += OnLanguageChanged;
        Closed += (_, _) => LocalizationManager.Instance.LanguageChanged -= OnLanguageChanged;
    }

    private void OnLanguageChanged(object? sender, EventArgs e) =>
        ApplyDirection();

    private void ApplyDirection() =>
        FlowDirection = LocalizationManager.Instance.IsRightToLeft
            ? FlowDirection.RightToLeft
            : FlowDirection.LeftToRight;

    private void Close_Click(object sender, RoutedEventArgs e) =>
        Close();

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.OriginalSource is System.Windows.Controls.Button)
            return;

        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }
}