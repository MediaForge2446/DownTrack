using System.Windows;

namespace DownTrack.Views;

public partial class TextPromptWindow : Window
{
    public TextPromptWindow(string title, string message, string initialValue)
    {
        InitializeComponent();
        TitleText.Text = title;
        MessageText.Text = message;
        ValueBox.Text = initialValue;
        ValueBox.SelectAll();

        Loaded += (_, _) =>
        {
            Owner ??= Application.Current.MainWindow;
            ValueBox.Focus();
        };
    }

    public string Value => ValueBox.Text.Trim();

    private void Continue_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
