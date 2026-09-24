using System.Windows;
using System.Windows.Input;

namespace DownTrack.Views;

public partial class MainWindow : Window
{
    private Rect _restoreBounds;
    private bool _workAreaMaximized;

    public MainWindow()
    {
        InitializeComponent();
        StateChanged += MainWindow_StateChanged;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        MaximizeToWorkArea();
    }

    private void MainWindow_StateChanged(object? sender, System.EventArgs e)
    {
        if (WindowState == WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
            MaximizeToWorkArea();
            return;
        }

        if (WindowState == WindowState.Normal)
        {
            RootBorder.CornerRadius = _workAreaMaximized
                ? new CornerRadius(0)
                : new CornerRadius(18);
        }
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            ToggleMaximize();
            return;
        }

        if (e.LeftButton == MouseButtonState.Pressed &&
            WindowState == WindowState.Normal &&
            !_workAreaMaximized)
        {
            DragMove();
        }
    }

    private void Minimize_Click(object sender, RoutedEventArgs e) =>
        WindowState = WindowState.Minimized;

    private void Maximize_Click(object sender, RoutedEventArgs e) =>
        ToggleMaximize();

    private void Close_Click(object sender, RoutedEventArgs e) =>
        Close();

    private void ToggleMaximize()
    {
        if (_workAreaMaximized)
            RestoreFromWorkArea();
        else
        {
            SaveRestoreBounds();
            MaximizeToWorkArea();
        }
    }

    private void MaximizeToWorkArea()
    {
        var workArea = SystemParameters.WorkArea;

        if (!_workAreaMaximized)
            SaveRestoreBounds();

        _workAreaMaximized = true;
        WindowState = WindowState.Normal;
        Left = workArea.Left;
        Top = workArea.Top;
        Width = workArea.Width;
        Height = workArea.Height;
        RootBorder.CornerRadius = new CornerRadius(0);
    }

    private void RestoreFromWorkArea()
    {
        _workAreaMaximized = false;
        WindowState = WindowState.Normal;

        if (_restoreBounds.Width > 0 && _restoreBounds.Height > 0)
        {
            Left = _restoreBounds.Left;
            Top = _restoreBounds.Top;
            Width = _restoreBounds.Width;
            Height = _restoreBounds.Height;
        }

        RootBorder.CornerRadius = new CornerRadius(18);
    }

    private void SaveRestoreBounds()
    {
        if (_workAreaMaximized)
            return;

        _restoreBounds = new Rect(Left, Top, Width, Height);

        if (_restoreBounds.Width <= 0 || _restoreBounds.Height <= 0)
            _restoreBounds = new Rect(90, 70, 1200, 760);
    }
}