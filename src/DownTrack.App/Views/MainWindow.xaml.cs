using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace DownTrack.Views;

public partial class MainWindow : Window
{
    private const uint MonitorDefaultToNearest = 2;
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

        RootBorder.CornerRadius = _workAreaMaximized
            ? new CornerRadius(0)
            : new CornerRadius(20);
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            ToggleMaximize();
            return;
        }

        if (e.LeftButton == MouseButtonState.Pressed &&
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
        if (!_workAreaMaximized)
            SaveRestoreBounds();

        var workArea = GetCurrentMonitorWorkArea();

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

        RootBorder.CornerRadius = new CornerRadius(20);
    }

    private void SaveRestoreBounds()
    {
        if (_workAreaMaximized)
            return;

        var width = Width > 0 ? Width : 1320;
        var height = Height > 0 ? Height : 820;
        var left = double.IsNaN(Left) ? 90 : Left;
        var top = double.IsNaN(Top) ? 70 : Top;

        _restoreBounds = new Rect(left, top, width, height);
    }

    private Rect GetCurrentMonitorWorkArea()
    {
        var fallback = SystemParameters.WorkArea;

        try
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            if (hwnd == IntPtr.Zero)
                return fallback;

            var monitor = MonitorFromWindow(hwnd, MonitorDefaultToNearest);
            if (monitor == IntPtr.Zero)
                return fallback;

            var info = new MONITORINFO
            {
                cbSize = (uint)Marshal.SizeOf<MONITORINFO>()
            };

            if (!GetMonitorInfo(monitor, ref info))
                return fallback;

            var source = PresentationSource.FromVisual(this);
            var transform = source?.CompositionTarget?.TransformFromDevice
                ?? new System.Windows.Media.Matrix();

            var left = info.rcWork.Left * transform.M11;
            var top = info.rcWork.Top * transform.M22;
            var width = (info.rcWork.Right - info.rcWork.Left) * transform.M11;
            var height = (info.rcWork.Bottom - info.rcWork.Top) * transform.M22;

            return new Rect(left, top, width, height);
        }
        catch
        {
            return fallback;
        }
    }

    [DllImport("user32.dll")]
    private static extern IntPtr MonitorFromWindow(IntPtr hWnd, uint dwFlags);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetMonitorInfo(
        IntPtr hMonitor,
        ref MONITORINFO lpmi);

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MONITORINFO
    {
        public uint cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public uint dwFlags;
    }
}