namespace DownTrack.Application.Services;

public interface ITextPromptService
{
    string? Prompt(string title, string message, string initialValue = "");
    bool Confirm(string title, string message);
}
