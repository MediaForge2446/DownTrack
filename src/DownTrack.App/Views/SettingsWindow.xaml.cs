using System.Windows;
using System.Windows.Input;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.Views;

public partial class SettingsWindow : Window
{
    private readonly LocalizationService _localization;

    public SettingsWindow()
    {
        InitializeComponent();
        _localization = LocalizationService.Instance;
        LanguageBox.ItemsSource = LocalizationService.SupportedLanguages;
        LanguageBox.SelectedValue = _localization.SelectedCode;
        Loaded += (_, _) => LanguageBox.Focus();
    }

    private void Apply_Click(object sender, RoutedEventArgs e)
    {
        _localization.SetLanguage(LanguageBox.SelectedValue as string ?? "en");
        DialogResult = true;
    }

    private void Close_Click(object sender, RoutedEventArgs e) =>
        DialogResult = false;

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.OriginalSource is System.Windows.Controls.Button)
            return;

        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }
}