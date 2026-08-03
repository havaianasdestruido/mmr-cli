using System.Text;
using MmrCli.Framework;
using MmrCli.Native;

namespace MmrCli.Workflows;

/// <summary>
/// WLXMediaPublishSubscribe publish lifecycle funnel over the PublishManager
/// C API: create manager, enumerate targets, default-target policy, service
/// status, then the E_NOTIMPL session stubs and global cleanup.
/// All expected values are pinned from the WLXMediaPublishSubscribe.cpp source.
/// </summary>
public static class PublishSubscribeWorkflow
{
    private const int S_OK = 0;
    private const uint E_INVALIDARG = 0x80070057;
    private const uint E_NOTIMPL = 0x80004001;
    private const int PublishTarget_Facebook = 0;

    public static IEnumerable<TestCase> All()
    {
        yield return T.Test("WLXMediaPublishSubscribe.dll", "workflow", "mps:wf:manager",
            "manager lifecycle: Create -> EnumerateTargets(5) -> GetTargetName(Facebook) -> Destroy", ctx =>
            {
                IntPtr mgr = DllApi.PublishManager_Create();
                try
                {
                    if (mgr == IntPtr.Zero) return false;

                    var targets = new int[5];
                    uint count = 5;
                    if (DllApi.PublishManager_EnumerateTargets(mgr, targets, ref count) != S_OK) return false;
                    if (count != 5) return false;
                    var sorted = targets.Distinct().OrderBy(x => x).ToArray();
                    if (sorted.Length != 5 || sorted[0] != 0 || sorted[4] != 4) return false;

                    var name = new StringBuilder(64);
                    if (DllApi.PublishManager_GetTargetName(PublishTarget_Facebook, name, (uint)name.Capacity) != S_OK) return false;
                    return name.ToString() == "Facebook";
                }
                finally
                {
                    if (mgr != IntPtr.Zero) DllApi.PublishManager_Destroy(mgr);
                }
            });

        yield return T.Test("WLXMediaPublishSubscribe.dll", "workflow", "mps:wf:invalid-arg",
            "EnumerateTargets(NULL, ...) returns E_INVALIDARG", ctx =>
            {
                uint count = 1;
                return (uint)DllApi.PublishManager_EnumerateTargets(IntPtr.Zero, Array.Empty<int>(), ref count) == E_INVALIDARG;
            });

        yield return T.Test("WLXMediaPublishSubscribe.dll", "workflow", "mps:wf:default-target",
            "default-target policy: SetDefaultTarget S_OK, GetDefaultTarget -> Facebook (0)", ctx =>
            {
                if (DllApi.PublishManager_SetDefaultTarget(IntPtr.Zero, 2) != S_OK) return false;
                int target = -1;
                if (DllApi.PublishManager_GetDefaultTarget(IntPtr.Zero, out target) != S_OK) return false;
                return target == PublishTarget_Facebook;
            });

        yield return T.Test("WLXMediaPublishSubscribe.dll", "workflow", "mps:wf:service-status",
            "GetServiceStatus(Facebook) -> S_OK, available TRUE", ctx =>
            {
                int available = 0;
                if (DllApi.PublishManager_GetServiceStatus(PublishTarget_Facebook, out available) != S_OK) return false;
                return available != 0;
            });

        yield return T.Test("WLXMediaPublishSubscribe.dll", "workflow", "mps:wf:session-stubs",
            "session stubs: GetStatus/Cancel/GetResult/RefreshToken E_NOTIMPL, StartSubscribe NULL", ctx =>
            {
                if ((uint)DllApi.PublishManager_GetStatus(IntPtr.Zero, out _, out _) != E_NOTIMPL) return false;
                if ((uint)DllApi.PublishManager_Cancel(IntPtr.Zero) != E_NOTIMPL) return false;
                if ((uint)DllApi.PublishManager_GetResult(IntPtr.Zero, IntPtr.Zero) != E_NOTIMPL) return false;
                if ((uint)DllApi.PublishManager_RefreshToken(IntPtr.Zero, 0) != E_NOTIMPL) return false;
                if (DllApi.PublishManager_StartSubscribe(IntPtr.Zero, 0, "x") != IntPtr.Zero) return false;

                uint status = 99, percent = 99;
                bool subOk = (uint)DllApi.PublishManager_GetSubscribeStatus(IntPtr.Zero, out status, out percent) == E_NOTIMPL
                             && status == 0 && percent == 0;
                return subOk;
            });

        yield return T.Test("WLXMediaPublishSubscribe.dll", "workflow", "mps:wf:cleanup",
            "global cleanup: PublishManager_Cleanup returns S_OK", _ => DllApi.PublishManager_Cleanup() == S_OK);
    }
}
