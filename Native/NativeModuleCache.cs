using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MmrCli.Native;

public sealed class NativeModuleCache : IDisposable
{
    private readonly Dictionary<string, IntPtr> _handles = new(StringComparer.OrdinalIgnoreCase);

    public IntPtr Load(string dllPath)
    {
        string key = Path.GetFileName(dllPath);
        if (_handles.TryGetValue(key, out var existing) && existing != IntPtr.Zero)
            return existing;

        IntPtr h = LoadLibrary(dllPath);
        if (h == IntPtr.Zero)
            throw new Win32Exception(Marshal.GetLastWin32Error());
        _handles[key] = h;
        return h;
    }

    public IntPtr GetProc(IntPtr hModule, string name) => GetProcAddress(hModule, name);

    public bool HasProc(IntPtr hModule, string name) => GetProcAddress(hModule, name) != IntPtr.Zero;

    public void Dispose()
    {
        foreach (var kv in _handles)
            FreeLibrary(kv.Value);
        _handles.Clear();
    }

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool FreeLibrary(IntPtr hModule);
}

public static class NativeBootstrap
{
    public static void SetDllSearchPath(string binDir)
    {
        if (!SetDllDirectoryW(binDir))
            throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool SetDllDirectoryW(string lpPathName);
}
