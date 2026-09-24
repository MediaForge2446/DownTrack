using DownTrack.Application.Services;
using DownTrack.Views;

namespace DownTrack.Infrastructure.Windows;

public sealed class WpfTextPromptService : ITextPromptService
{
    public string? Prompt(string title, string message, string initialValue = "")
    {
        var dialog = new TextPromptWindow(title, message, initialValue);
        return dialog.ShowDialog() == true ? dialog.Value : null;
    }

    public bool Confirm(string title, string message)
    {
        var dialog = new ConfirmationWindow(title, message);
        return dialog.ShowDialog() == true;
    }
}
