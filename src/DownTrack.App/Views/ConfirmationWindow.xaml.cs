using System.Windows;

namespace DownTrack.Views;

public partial class ConfirmationWindow : Window
{
    public ConfirmationWindow(string title, string message)
    {
        InitializeComponent();
        TitleText.Text = title;
        MessageText.Text = message;
        Loaded += (_, _) => Owner ??= Application.Current.MainWindow;
    }

    private void Delete_Click(object sender, RoutedEventArgs e) => DialogResult = true;
    private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
