using System.Runtime.InteropServices;
using MmrCli.Framework;
using MmrCli.Native;

namespace MmrCli.Tests;

/// <summary>
/// Behavioral checks for the standard COM DLL entry points shared by the
/// COM DLLs in the set. Values come from the WMMR sources:
///   DllCanUnloadNow     -> S_OK        (WLMFReadWrite: S_FALSE)
///   DllGetClassObject   -> CLASS_E_CLASSNOTAVAILABLE
///   DllRegisterServer   -> S_OK
///   DllUnregisterServer -> S_OK
/// WLXPipeline exports only DllRegisterServer; WLMFReadWrite exports only the
/// DllCanUnloadNow/DllGetClassObject pair. MovieMakerPreviewClient is the one
/// real COM server in the set: its class factory reads rclsid (a valid GUID is
/// passed, never matching CLSID_MovieMakerPreviewClient) and its
/// register/unregister write registry keys, so only lifetime + class factory
/// are exercised behaviorally for it.
/// </summary>
public static class ComSurfaceTests
{
    private const int S_OK = 0;
    private const int S_FALSE = 1;
    private const int CLASS_E_CLASSNOTAVAILABLE = unchecked((int)0x80040111);

    private static TestCase ComCase(string dll, string func, string desc, Func<int> fn, int expected)
        => T.Test(dll, "com", $"com:{dll}:{func}", desc, ctx => fn() == expected);

    private static TestCase GetClassObjectCase(string dll, string func, Func<IntPtr, IntPtr, int> fn)
        => T.Test(dll, "com", $"com:{dll}:{func}",
            $"{func} returns CLASS_E_CLASSNOTAVAILABLE",
            ctx =>
            {
                IntPtr guid = Marshal.AllocHGlobal(16);
                try
                {
                    Marshal.WriteInt64(guid, 0, 0x1111111111111111);
                    Marshal.WriteInt64(guid, 8, 0x2222222222222222);
                    return fn(guid, IntPtr.Zero) == CLASS_E_CLASSNOTAVAILABLE;
                }
                finally
                {
                    Marshal.FreeHGlobal(guid);
                }
            });

    public static IEnumerable<TestCase> All()
    {
        foreach (var (dll, can, co, reg, unreg) in StandardComDlls())
        {
            if (TestSkips.UnloadableDlls.ContainsKey(dll))
                continue;

            yield return ComCase(dll, "DllCanUnloadNow", "DllCanUnloadNow returns S_OK", can, S_OK);
            yield return GetClassObjectCase(dll, "DllGetClassObject", co);
            yield return ComCase(dll, "DllRegisterServer", "DllRegisterServer returns S_OK", reg, S_OK);
            yield return ComCase(dll, "DllUnregisterServer", "DllUnregisterServer returns S_OK", unreg, S_OK);
        }

        // MovieMakerPreviewClient: real COM server; class factory needs a real
        // (non-matching) rclsid, registration is skipped (real registry I/O).
        yield return ComCase("MovieMakerPreviewClient.dll", "DllCanUnloadNow", "DllCanUnloadNow returns S_OK", DllApi.Preview_DllCanUnloadNow, S_OK);
        yield return GetClassObjectCase("MovieMakerPreviewClient.dll", "DllGetClassObject",
            (rclsid, riid) => DllApi.Preview_DllGetClassObject(rclsid, riid, out _));

        // WLMFReadWrite: only the unload/classfactory pair; unload is S_FALSE.
        yield return ComCase("WLMFReadWrite.dll", "DllCanUnloadNow", "DllCanUnloadNow returns S_FALSE", DllApi.Mfrw_DllCanUnloadNow, S_FALSE);
        yield return GetClassObjectCase("WLMFReadWrite.dll", "DllGetClassObject",
            (rclsid, riid) => DllApi.Mfrw_DllGetClassObject(rclsid, riid, out _));

        // WLXPipeline: only DllRegisterServer is exported.
        yield return ComCase("WLXPipeline.dll", "DllRegisterServer", "DllRegisterServer returns S_OK", DllApi.Pipeline_DllRegisterServer, S_OK);
    }

    private static IEnumerable<(string Dll, Func<int> Can, Func<IntPtr, IntPtr, int> Co, Func<int> Reg, Func<int> Unreg)> StandardComDlls()
    {
        yield return ("MetadataSys.dll", DllApi.Mds_DllCanUnloadNow, (r, i) => DllApi.Mds_DllGetClassObject(r, i, out _), DllApi.Mds_DllRegisterServer, DllApi.Mds_DllUnregisterServer);
        yield return ("WLMFDS.dll", DllApi.Mfds_DllCanUnloadNow, (r, i) => DllApi.Mfds_DllGetClassObject(r, i, out _), DllApi.Mfds_DllRegisterServer, DllApi.Mfds_DllUnregisterServer);
        yield return ("WLXPhotoCinematic.dll", DllApi.Cinematic_DllCanUnloadNow, (r, i) => DllApi.Cinematic_DllGetClassObject(r, i, out _), DllApi.Cinematic_DllRegisterServer, DllApi.Cinematic_DllUnregisterServer);
        yield return ("WLXMovieLibrary.dll", DllApi.MovieLibrary_DllCanUnloadNow, (r, i) => DllApi.MovieLibrary_DllGetClassObject(r, i, out _), DllApi.MovieLibrary_DllRegisterServer, DllApi.MovieLibrary_DllUnregisterServer);
        yield return ("WLXSlideshow.dll", DllApi.Slideshow_DllCanUnloadNow, (r, i) => DllApi.Slideshow_DllGetClassObject(r, i, out _), DllApi.Slideshow_DllRegisterServer, DllApi.Slideshow_DllUnregisterServer);
        yield return ("WLXFaceRecognition.dll", DllApi.FaceRecog_DllCanUnloadNow, (r, i) => DllApi.FaceRecog_DllGetClassObject(r, i, out _), DllApi.FaceRecog_DllRegisterServer, DllApi.FaceRecog_DllUnregisterServer);
        yield return ("WLXMediaPublishSubscribe.dll", DllApi.Mps_DllCanUnloadNow, (r, i) => DllApi.Mps_DllGetClassObject(r, i, out _), DllApi.Mps_DllRegisterServer, DllApi.Mps_DllUnregisterServer);
        yield return ("WLXMP4Parser.dll", DllApi.Mp4_DllCanUnloadNow, (r, i) => DllApi.Mp4_DllGetClassObject(r, i, out _), DllApi.Mp4_DllRegisterServer, DllApi.Mp4_DllUnregisterServer);
    }
}
