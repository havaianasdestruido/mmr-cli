using System.Runtime.InteropServices;
using MmrCli.Framework;
using MmrCli.Native;

namespace MmrCli.Workflows;

/// <summary>
/// wlidcli identity lifecycle: environment -> sign-in cycle -> handle
/// issuance -> ticket retrieval. Chained funnels over the C API.
/// </summary>
public static class WlidcliWorkflow
{
    public static IEnumerable<TestCase> All()
    {
        yield return T.Test("wlidcli.dll", "workflow", "wlidcli:wf:env",
            "env bootstrap: WLGetEnvironment -> 'production'", ctx =>
            {
                int hr = DllApi.WLGetEnvironment(out IntPtr envPtr);
                if (hr != 0) return false;
                string? env = envPtr == IntPtr.Zero ? null : Marshal.PtrToStringUni(envPtr);
                bool ok = env == "production";
                if (envPtr != IntPtr.Zero) DllApi.WLFreeMemory(envPtr);
                return ok;
            });

        yield return T.Test("wlidcli.dll", "workflow", "wlidcli:wf:signin-cycle",
            "sign-in cycle: FALSE -> WLClogin S_OK -> TRUE -> WLCheckCredentials S_OK", ctx =>
            {
                if (DllApi.WLIsSignedIn(0) != 0) return false;
                if (DllApi.WLClogin(IntPtr.Zero, null, 0, out _) != 0) return false;
                if (DllApi.WLIsSignedIn(0) == 0) return false;
                return DllApi.WLCheckCredentials(null) == 0;
            });

        yield return T.Test("wlidcli.dll", "workflow", "wlidcli:wf:handles",
            "identity handle issuance: two handles, distinct and increasing", ctx =>
            {
                uint a = DllApi.WLCreateIdentityHandle();
                uint b = DllApi.WLCreateIdentityHandle();
                return a != 0 && b > a && a != b;
            });

        yield return T.Test("wlidcli.dll", "workflow", "wlidcli:wf:ticket",
            "ticket retrieval: WLGetTicket(1) -> S_OK + ticket + free", ctx =>
            {
                int hr = DllApi.WLGetTicket(1, out IntPtr ticketPtr);
                if (hr != 0) return false;
                string? ticket = Marshal.PtrToStringUni(ticketPtr);
                bool ok = !string.IsNullOrEmpty(ticket) && ticket!.Contains("ticket=");
                if (ticketPtr != IntPtr.Zero) DllApi.WLFreeMemory(ticketPtr);
                return ok;
            });
    }
}
