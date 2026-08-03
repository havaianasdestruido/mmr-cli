// mmr-cli — real-world usage tests against the WMMR built DLLs.
// Exercises the C export surface of wlidcli, uxctl, WLXVideoTrim,
// WLXPipetran and MovieMakerCore using P/Invoke.
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MmrCli;

internal static class Program
{
    private static int _pass;
    private static int _fail;

    private static int Main(string[] args)
    {
        Console.WriteLine("=== WMMR Live Usage Tests (mmr-cli) ===");
        Console.WriteLine();

        string bin = args.Length > 0
            ? Path.GetFullPath(args[0])
            : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "bin"));

        if (!Directory.Exists(bin))
        {
            Console.WriteLine($"[!] DLL directory not found: {bin}");
            Console.WriteLine("    Pass the path to build_clean/bin/Debug as the first argument.");
            return 2;
        }

        // Set DLL search path before loading any native DLLs.
        Native.SetDllDirectoryW(bin);

        RunWlidcli(bin);
        RunUxctl(bin);
        RunVideoTrim(bin);
        RunPipetran(bin);
        RunMovieMakerCore(bin);

        Console.WriteLine();
        Console.WriteLine($"=== PASS={_pass} FAIL={_fail} ===");
        return _fail == 0 ? 0 : 1;
    }

    private static void Check(string name, Func<bool> fn)
    {
        try
        {
            if (fn())
            {
                _pass++;
                Console.WriteLine($"  [PASS] {name}");
            }
            else
            {
                _fail++;
                Console.WriteLine($"  [FAIL] {name}");
            }
        }
        catch (Exception ex)
        {
            _fail++;
            Console.WriteLine($"  [FAIL] {name} — {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static void RunWlidcli(string bin)
    {
        Console.WriteLine();
        Console.WriteLine("[wlidcli.dll]");

        IntPtr h = IntPtr.Zero;
        try
        {
            h = Native.LoadLibrary(Path.Combine(bin, "wlidcli.dll"));
            if (h == IntPtr.Zero)
            {
                Console.WriteLine("  [FAIL] LoadLibrary wlidcli.dll — " + new Win32Exception(Marshal.GetLastWin32Error()).Message);
                _fail++;
                return;
            }

            Check("WLIsSignedIn(0) is FALSE before any sign-in call", () => Native.WLIsSignedIn(0) == 0);

            Check("WLCheckCredentials(null) returns S_OK", () => Native.WLCheckCredentials(null) == 0);

            Check("WLClogin(0, null, 0, out _) returns S_OK", () =>
            {
                int hr = Native.WLClogin(IntPtr.Zero, null, 0, out _);
                return hr == 0;
            });

            Check("WLIsSignedIn(0) is TRUE after WLClogin", () => Native.WLIsSignedIn(0) != 0);

            Check("WLCreateIdentityHandle() returns increasing nonzero handles", () =>
            {
                uint a = Native.WLCreateIdentityHandle();
                uint b = Native.WLCreateIdentityHandle();
                return a != 0 && b > a;
            });

            Check("WLGetEnvironment returns S_OK and 'production'", () =>
            {
                int hr = Native.WLGetEnvironment(out IntPtr envPtr);
                string? env = envPtr == IntPtr.Zero ? null : Marshal.PtrToStringUni(envPtr);
                bool ok = hr == 0 && env == "production";
                if (envPtr != IntPtr.Zero) Native.WLFreeMemory(envPtr);
                return ok;
            });

            Check("WLGetTicket(1) returns S_OK and a ticket", () =>
            {
                int hr = Native.WLGetTicket(1, out IntPtr ticketPtr);
                if (hr != 0) return false;
                string? ticket = Marshal.PtrToStringUni(ticketPtr);
                bool ok = !string.IsNullOrEmpty(ticket) && ticket!.Contains("ticket=");
                if (ticketPtr != IntPtr.Zero) Native.WLFreeMemory(ticketPtr);
                return ok;
            });
        }
        finally
        {
            if (h != IntPtr.Zero) Native.FreeLibrary(h);
        }
    }

    private static void RunUxctl(string bin)
    {
        Console.WriteLine();
        Console.WriteLine("[uxctl.dll]");

        IntPtr h = IntPtr.Zero;
        try
        {
            h = Native.LoadLibrary(Path.Combine(bin, "uxctl.dll"));
            if (h == IntPtr.Zero)
            {
                Console.WriteLine("  [FAIL] LoadLibrary uxctl.dll — " + new Win32Exception(Marshal.GetLastWin32Error()).Message);
                _fail++;
                return;
            }

            Check("UxControlsInitProcess() returns S_OK", () => Native.UxControlsInitProcess() == 0);

            Check("UxControlsCreateObject returns CLASS_E_CLASSNOTAVAILABLE", () =>
                (uint)Native.UxControlsCreateObject(out _) == 0x80040111);

            Check("UxControlsUninitProcess() is callable", () =>
            {
                Native.UxControlsUninitProcess();
                return true;
            });
        }
        finally
        {
            if (h != IntPtr.Zero) Native.FreeLibrary(h);
        }
    }

    private static void RunVideoTrim(string bin)
    {
        Console.WriteLine();
        Console.WriteLine("[WLXVideoTrim.dll]");

        IntPtr h = IntPtr.Zero;
        try
        {
            h = Native.LoadLibrary(Path.Combine(bin, "WLXVideoTrim.dll"));
            if (h == IntPtr.Zero)
            {
                Console.WriteLine("  [FAIL] LoadLibrary WLXVideoTrim.dll — " + new Win32Exception(Marshal.GetLastWin32Error()).Message);
                _fail++;
                return;
            }

            Check("CreateVideoPlayer returns E_NOTIMPL", () => (uint)Native.CreateVideoPlayer(out _) == 0x80004001);

            Check("CreateVideoFormatContextTranscoder returns E_NOTIMPL", () => (uint)Native.CreateVideoFormatContextTranscoder(out _) == 0x80004001);

            Check("CreateVideoWMVTranscoder returns E_NOTIMPL", () => (uint)Native.CreateVideoWMVTranscoder(out _) == 0x80004001);

            Check("CreateAVICopierDirect returns E_NOTIMPL", () => (uint)Native.CreateAVICopierDirect(out _) == 0x80004001);
        }
        finally
        {
            if (h != IntPtr.Zero) Native.FreeLibrary(h);
        }
    }

    private static void RunPipetran(string bin)
    {
        Console.WriteLine();
        Console.WriteLine("[WLXPipetran.dll]");

        IntPtr h = IntPtr.Zero;
        try
        {
            h = Native.LoadLibrary(Path.Combine(bin, "WLXPipetran.dll"));
            if (h == IntPtr.Zero)
            {
                Console.WriteLine("  [FAIL] LoadLibrary WLXPipetran.dll — " + new Win32Exception(Marshal.GetLastWin32Error()).Message);
                _fail++;
                return;
            }

            Check("GetTFXCreateFunctions returns E_NOTIMPL with count==0", () =>
            {
                int hr = Native.GetTFXCreateFunctions(out _, out uint count);
                return (uint)hr == 0x80004001 && count == 0;
            });
        }
        finally
        {
            if (h != IntPtr.Zero) Native.FreeLibrary(h);
        }
    }

    private static void RunMovieMakerCore(string bin)
    {
        Console.WriteLine();
        Console.WriteLine("[MovieMakerCore.dll]");

        IntPtr h = IntPtr.Zero;
        try
        {
            h = Native.LoadLibrary(Path.Combine(bin, "MovieMakerCore.dll"));
            if (h == IntPtr.Zero)
            {
                Console.WriteLine("  [FAIL] LoadLibrary MovieMakerCore.dll — " + new Win32Exception(Marshal.GetLastWin32Error()).Message);
                _fail++;
                return;
            }

            Check("MovieMakerMain(2, [MovieMaker.exe, --help]) returns 0", () =>
                Native.MovieMakerMain(2, new[] { "MovieMaker.exe", "--help" }) == 0);
        }
        finally
        {
            if (h != IntPtr.Zero) Native.FreeLibrary(h);
        }
    }
}

internal static partial class Native
{
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool FreeLibrary(IntPtr hModule);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool SetDllDirectoryW(string lpPathName);

    // ---- wlidcli.dll (all __stdcall) ----
    [DllImport("wlidcli.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int WLGetEnvironment(out IntPtr env);

    [DllImport("wlidcli.dll", SetLastError = true)]
    internal static extern uint WLCreateIdentityHandle();

    [DllImport("wlidcli.dll", SetLastError = true)]
    internal static extern int WLIsSignedIn(uint handle);

    [DllImport("wlidcli.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int WLClogin(IntPtr hwndParent, string? cred, uint flags, out IntPtr authState);

    [DllImport("wlidcli.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int WLCheckCredentials(string? cred);

    [DllImport("wlidcli.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int WLGetTicket(uint handle, out IntPtr ticket);

    [DllImport("wlidcli.dll", SetLastError = true)]
    internal static extern void WLFreeMemory(IntPtr pv);

    // ---- uxctl.dll (all __stdcall) ----
    [DllImport("uxctl.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern int UxControlsInitProcess();

    [DllImport("uxctl.dll", SetLastError = true)]
    internal static extern void UxControlsUninitProcess();

    [DllImport("uxctl.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern int UxControlsCreateObject(out IntPtr ppObject);

    // ---- WLXVideoTrim.dll (all __stdcall) ----
    [LibraryImport("WLXVideoTrim.dll")]
    internal static partial int CreateVideoPlayer(out IntPtr ppUnknown);

    [LibraryImport("WLXVideoTrim.dll")]
    internal static partial int CreateVideoFormatContextTranscoder(out IntPtr ppUnknown);

    [LibraryImport("WLXVideoTrim.dll")]
    internal static partial int CreateVideoWMVTranscoder(out IntPtr ppUnknown);

    [LibraryImport("WLXVideoTrim.dll")]
    internal static partial int CreateAVICopierDirect(out IntPtr ppUnknown);

    // ---- WLXPipetran.dll (__stdcall) ----
    [LibraryImport("WLXPipetran.dll")]
    internal static partial int GetTFXCreateFunctions(out IntPtr ppFunctions, out uint pCount);

    // ---- MovieMakerCore.dll (__cdecl) ----
    [DllImport("MovieMakerCore.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    internal static extern int MovieMakerMain(int argc, string[] argv);
}
