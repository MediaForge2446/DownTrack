using System.Runtime.InteropServices;
using DownTrack.Application.Services;

namespace DownTrack.Infrastructure.Windows;

public sealed class WindowsFolderPicker : IFolderPicker
{
    public string? PickFolder(string? initialPath = null)
    {
        var dialog = (IFileOpenDialog)new FileOpenDialog();

        try
        {
            dialog.SetOptions(FOS.PICKFOLDERS | FOS.FORCEFILESYSTEM | FOS.PATHMUSTEXIST);

            if (!string.IsNullOrWhiteSpace(initialPath) && Directory.Exists(initialPath))
            {
                var shellItem = ShellItemFromPath(initialPath);
                if (shellItem is not null)
                {
                    try
                    {
                        dialog.SetFolder(shellItem);
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(shellItem);
                    }
                }
            }

            if (dialog.Show(GetActiveWindow()) != 0)
                return null;

            dialog.GetResult(out var result);
            try
            {
                result.GetDisplayName(SIGDN.FILESYSPATH, out var pathPtr);
                try
                {
                    return Marshal.PtrToStringUni(pathPtr);
                }
                finally
                {
                    Marshal.FreeCoTaskMem(pathPtr);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(result);
            }
        }
        finally
        {
            Marshal.ReleaseComObject(dialog);
        }
    }

    private static IShellItem? ShellItemFromPath(string path)
    {
        var iid = typeof(IShellItem).GUID;

        return SHCreateItemFromParsingName(
            path,
            IntPtr.Zero,
            ref iid,
            out var item) == 0
            ? item
            : null;
    }

    private static IntPtr GetActiveWindow() =>
        System.Windows.Application.Current?.MainWindow is { } window
            ? new System.Windows.Interop.WindowInteropHelper(window).Handle
            : IntPtr.Zero;

    [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
    private static extern int SHCreateItemFromParsingName(
        string pszPath,
        IntPtr pbc,
        [In] ref Guid riid,
        [MarshalAs(UnmanagedType.Interface)] out IShellItem ppv);

    [ComImport]
    [Guid("DC1C5A9C-E88A-4DDE-A5A1-60F82A20AEF7")]
    private sealed class FileOpenDialog
    {
    }

    [ComImport]
    [Guid("D57C7288-D4AD-4768-BE02-9D969532D960")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IFileOpenDialog
    {
        [PreserveSig] int Show(IntPtr parent);

        void SetFileTypes(uint cFileTypes, IntPtr rgFilterSpec);
        void SetFileTypeIndex(uint iFileType);
        void GetFileTypeIndex(out uint piFileType);
        void Advise(IntPtr pfde, out uint pdwCookie);
        void Unadvise(uint dwCookie);
        void SetOptions(FOS fos);
        void GetOptions(out FOS fos);
        void SetDefaultFolder(IShellItem psi);
        void SetFolder(IShellItem psi);
        void GetFolder(out IShellItem ppsi);
        void GetCurrentSelection(out IShellItem ppsi);
        void SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);
        void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);
        void SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);
        void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);
        void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);
        void GetResult(out IShellItem ppsi);
        void AddPlace(IShellItem psi, int fdap);
        void RemovePlace(IShellItem psi);
        void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);
        void Close(int hr);
        void SetClientGuid(in Guid guid);
        void ClearClientData();
        void SetFilter(IntPtr pFilter);
    }

    [ComImport]
    [Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IShellItem
    {
        void BindToHandler(IntPtr pbc, in Guid bhid, in Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);
        void GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
        void GetDisplayName(SIGDN sigdnName, out IntPtr ppszName);
        void GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);
        void Compare([MarshalAs(UnmanagedType.Interface)] IShellItem psi, uint hint, out int piOrder);
    }

    [Flags]
    private enum FOS : uint
    {
        PICKFOLDERS = 0x00000020,
        FORCEFILESYSTEM = 0x00000040,
        PATHMUSTEXIST = 0x00000800
    }

    private enum SIGDN : uint
    {
        FILESYSPATH = 0x80058000
    }
}
