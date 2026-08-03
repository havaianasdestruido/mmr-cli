using System.Runtime.InteropServices;
using MmrCli.Framework;
using MmrCli.Native;

namespace MmrCli.Tests;

/// <summary>
/// Atomic behavioral checks (baseline contract). All expected values are
/// pinned against the WMMR sources / live probes:
///   wlidcli / uxctl / WLXVideoTrim / WLXPipetran / MovieMakerCore --help,
///   plus the stub DLLs (WLXPhotoSqm, DmxBici, UXCore resource helpers).
/// </summary>
public static class BehaviorTests
{
    private const uint E_NOTIMPL = 0x80004001;
    private const uint CLASS_E_CLASSNOTAVAILABLE = 0x80040111;

    public static IEnumerable<TestCase> All()
    {
        foreach (var t in Wlidcli()) yield return t;
        foreach (var t in Uxctl()) yield return t;
        foreach (var t in VideoTrim()) yield return t;
        foreach (var t in Pipetran()) yield return t;
        foreach (var t in Sqm()) yield return t;
        foreach (var t in Bici()) yield return t;
        foreach (var t in UxCore()) yield return t;
        foreach (var t in MovieMakerCoreBaseline()) yield return t;
    }

    private static IEnumerable<TestCase> Wlidcli()
    {
        yield return T.Test("wlidcli.dll", "behavior", "wlidcli:not-signed-in",
            "WLIsSignedIn(0) is FALSE before any sign-in call", ctx => DllApi.WLIsSignedIn(0) == 0);

        yield return T.Test("wlidcli.dll", "behavior", "wlidcli:check-creds",
            "WLCheckCredentials(null) returns S_OK", ctx => DllApi.WLCheckCredentials(null) == 0);

        yield return T.Test("wlidcli.dll", "behavior", "wlidcli:login",
            "WLClogin(0, null, 0, out _) returns S_OK", ctx => DllApi.WLClogin(IntPtr.Zero, null, 0, out _) == 0);

        yield return T.Test("wlidcli.dll", "behavior", "wlidcli:signed-in-after-login",
            "WLIsSignedIn(0) is TRUE after WLClogin", ctx => DllApi.WLIsSignedIn(0) != 0);

        yield return T.Test("wlidcli.dll", "behavior", "wlidcli:identity-handles",
            "WLCreateIdentityHandle() returns increasing nonzero handles", ctx =>
            {
                uint a = DllApi.WLCreateIdentityHandle();
                uint b = DllApi.WLCreateIdentityHandle();
                return a != 0 && b > a;
            });

        yield return T.Test("wlidcli.dll", "behavior", "wlidcli:environment",
            "WLGetEnvironment returns S_OK and 'production'", ctx =>
            {
                int hr = DllApi.WLGetEnvironment(out IntPtr envPtr);
                string? env = envPtr == IntPtr.Zero ? null : Marshal.PtrToStringUni(envPtr);
                bool ok = hr == 0 && env == "production";
                if (envPtr != IntPtr.Zero) DllApi.WLFreeMemory(envPtr);
                return ok;
            });

        yield return T.Test("wlidcli.dll", "behavior", "wlidcli:ticket",
            "WLGetTicket(1) returns S_OK and a ticket", ctx =>
            {
                int hr = DllApi.WLGetTicket(1, out IntPtr ticketPtr);
                if (hr != 0) return false;
                string? ticket = Marshal.PtrToStringUni(ticketPtr);
                bool ok = !string.IsNullOrEmpty(ticket) && ticket!.Contains("ticket=");
                if (ticketPtr != IntPtr.Zero) DllApi.WLFreeMemory(ticketPtr);
                return ok;
            });
    }

    private static IEnumerable<TestCase> Uxctl()
    {
        yield return T.Test("uxctl.dll", "behavior", "uxctl:init",
            "UxControlsInitProcess() returns S_OK", ctx => DllApi.UxControlsInitProcess() == 0);

        yield return T.Test("uxctl.dll", "behavior", "uxctl:create-object",
            "UxControlsCreateObject returns CLASS_E_CLASSNOTAVAILABLE",
            ctx => (uint)DllApi.UxControlsCreateObject(out _) == CLASS_E_CLASSNOTAVAILABLE);

        yield return T.Test("uxctl.dll", "behavior", "uxctl:uninit",
            "UxControlsUninitProcess() is callable", ctx => { DllApi.UxControlsUninitProcess(); return true; });
    }

    private static IEnumerable<TestCase> VideoTrim()
    {
        yield return T.Test("WLXVideoTrim.dll", "behavior", "videotrim:avi-copier",
            "CreateAVICopierDirect returns E_NOTIMPL", ctx => (uint)DllApi.CreateAVICopierDirect(out _) == E_NOTIMPL);

        yield return T.Test("WLXVideoTrim.dll", "behavior", "videotrim:player",
            "CreateVideoPlayer returns E_NOTIMPL", ctx => (uint)DllApi.CreateVideoPlayer(out _) == E_NOTIMPL);

        yield return T.Test("WLXVideoTrim.dll", "behavior", "videotrim:fmt-context",
            "CreateVideoFormatContextTranscoder returns E_NOTIMPL",
            ctx => (uint)DllApi.CreateVideoFormatContextTranscoder(out _) == E_NOTIMPL);

        yield return T.Test("WLXVideoTrim.dll", "behavior", "videotrim:wmv-transcoder",
            "CreateVideoWMVTranscoder returns E_NOTIMPL", ctx => (uint)DllApi.CreateVideoWMVTranscoder(out _) == E_NOTIMPL);

        yield return T.Test("WLXVideoTrim.dll", "behavior", "videotrim:from-mediatype",
            "CreateVideoCopierFromMediaType(valid GUID) returns E_NOTIMPL", ctx =>
            {
                IntPtr guid = Marshal.AllocHGlobal(16);
                try
                {
                    Marshal.WriteInt64(guid, 0, 0x0000000000000011);
                    Marshal.WriteInt64(guid, 8, 0);
                    return (uint)DllApi.CreateVideoCopierFromMediaType(guid, out _) == E_NOTIMPL;
                }
                finally
                {
                    Marshal.FreeHGlobal(guid);
                }
            });
    }

    private static IEnumerable<TestCase> Pipetran()
    {
        yield return T.Test("WLXPipetran.dll", "behavior", "pipetran:create-functions",
            "GetTFXCreateFunctions returns E_NOTIMPL with count==0", ctx =>
            {
                int hr = DllApi.GetTFXCreateFunctions(out _, out uint count);
                return (uint)hr == E_NOTIMPL && count == 0;
            });
    }

    private static IEnumerable<TestCase> Sqm()
    {
        yield return T.Test("WLXPhotoSqm.dll", "behavior", "sqm:startup-shutdown",
            "Startup/Shutdown are callable no-ops", ctx => { DllApi.Sqm_Startup(); DllApi.Sqm_Shutdown(); return true; });

        yield return T.Test("WLXPhotoSqm.dll", "behavior", "sqm:isenabled",
            "IsEnabled returns FALSE", ctx => DllApi.Sqm_IsEnabled() == false);

        yield return T.Test("WLXPhotoSqm.dll", "behavior", "sqm:optin-state",
            "GetOptInState returns NotSet (0)", ctx => DllApi.Sqm_GetOptInState() == 0);

        yield return T.Test("WLXPhotoSqm.dll", "behavior", "sqm:set",
            "Sqm_Set is callable", ctx => { DllApi.Sqm_Set(1, 2); return true; });
    }

    private static IEnumerable<TestCase> Bici()
    {
        yield return T.Test("DmxBici.dll", "behavior", "bici:start-experience",
            "StartExperience returns increasing ids", ctx =>
            {
                int a = DllApi.Bici_StartExperience();
                int b = DllApi.Bici_StartExperience();
                return a >= 0 && b > a;
            });

        yield return T.Test("DmxBici.dll", "behavior", "bici:start-with-id",
            "StartExperienceWithId(7) returns 7", ctx => DllApi.Bici_StartExperienceWithId(7) == 7);

        yield return T.Test("DmxBici.dll", "behavior", "bici:timers",
            "TimerStart/TimerAccumulate/TimerRecord return TRUE", ctx =>
            {
                bool s = DllApi.Bici_TimerStart(9);
                bool a = DllApi.Bici_TimerAccumulate(9);
                bool r = DllApi.Bici_TimerRecord(9);
                return s && a && r;
            });

        yield return T.Test("DmxBici.dll", "behavior", "bici:transfer-to-app",
            "TransferExperienceToApp returns TRUE with NULL out", ctx =>
            {
                bool ok = DllApi.Bici_TransferExperienceToApp(out IntPtr names);
                return ok && names == IntPtr.Zero;
            });

        yield return T.Test("DmxBici.dll", "behavior", "bici:transfer-to-appid",
            "TransferExperienceToAppId returns TRUE", ctx => DllApi.Bici_TransferExperienceToAppId(0) == true);
    }

    private static IEnumerable<TestCase> UxCore()
    {
        yield return T.Test("UXCore.dll", "behavior", "uxcore:strid",
            "StrToID(\"123\") == 123", ctx => DllApi.StrToID("123") == 123);

        yield return T.Test("UXCore.dll", "behavior", "uxcore:strid-null",
            "StrToID(null) == 0", ctx => DllApi.StrToID(null) == 0);

        yield return T.Test("UXCore.dll", "behavior", "uxcore:rmfind-module",
            "RMFindModule(0, \"x\") returns NULL", ctx => DllApi.RMFindModule(IntPtr.Zero, "x") == IntPtr.Zero);

        yield return T.Test("UXCore.dll", "behavior", "uxcore:rmfind-resource",
            "RMFindModuleForResource(0, 0) returns NULL", ctx => DllApi.RMFindModuleForResource(IntPtr.Zero, 0) == IntPtr.Zero);

        yield return T.Test("UXCore.dll", "behavior", "uxcore:rmupdate-set",
            "RMUpdateResourceSet is callable", ctx => { DllApi.RMUpdateResourceSet(IntPtr.Zero); return true; });
    }

    private static IEnumerable<TestCase> MovieMakerCoreBaseline()
    {
        yield return T.Test("MovieMakerCore.dll", "behavior", "moviemaker:help",
            "MovieMakerMain(2, [MovieMaker.exe, --help]) returns 0",
            ctx => DllApi.MovieMakerMain(2, new[] { "MovieMaker.exe", "--help" }) == 0);
    }
}
