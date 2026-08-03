using System.Runtime.InteropServices;
using MmrCli.Framework;
using MmrCli.Native;

namespace MmrCli.Workflows;

/// <summary>
/// WLXVideoTrim five-factory funnel: every exported factory returns
/// E_NOTIMPL and leaves its out-parameter NULL.
/// </summary>
public static class VideoTrimWorkflow
{
    private const uint E_NOTIMPL = 0x80004001;

    public static IEnumerable<TestCase> All()
    {
        yield return FactoryCase("videotrim:wf:avi-copier", "CreateAVICopierDirect", ctx => DllApi.CreateAVICopierDirect(out _));

        yield return FactoryCase("videotrim:wf:player", "CreateVideoPlayer", ctx => DllApi.CreateVideoPlayer(out _));

        yield return FactoryCase("videotrim:wf:fmt-context", "CreateVideoFormatContextTranscoder", ctx => DllApi.CreateVideoFormatContextTranscoder(out _));

        yield return FactoryCase("videotrim:wf:wmv-transcoder", "CreateVideoWMVTranscoder", ctx => DllApi.CreateVideoWMVTranscoder(out _));

        yield return T.Test("WLXVideoTrim.dll", "workflow", "videotrim:wf:from-mediatype",
            "CreateVideoCopierFromMediaType(valid GUID) -> E_NOTIMPL, out NULL", ctx =>
            {
                IntPtr guid = Marshal.AllocHGlobal(16);
                try
                {
                    Marshal.WriteInt64(guid, 0, 0x0000000000000011);
                    Marshal.WriteInt64(guid, 8, 0);
                    int hr = DllApi.CreateVideoCopierFromMediaType(guid, out IntPtr ppv);
                    return (uint)hr == E_NOTIMPL && ppv == IntPtr.Zero;
                }
                finally
                {
                    Marshal.FreeHGlobal(guid);
                }
            });
    }

    private static TestCase FactoryCase(string id, string func, Func<TestContext, int> invoke)
        => T.Test("WLXVideoTrim.dll", "workflow", id,
            $"{func} -> E_NOTIMPL, out NULL",
            ctx => (uint)invoke(ctx) == E_NOTIMPL);
}
