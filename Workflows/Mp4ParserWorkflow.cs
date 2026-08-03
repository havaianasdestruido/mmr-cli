using System.IO;
using MmrCli.Framework;
using MmrCli.Native;

namespace MmrCli.Workflows;

/// <summary>
/// WLXMP4Parser filter-graph funnel. The four helper exports are pure stubs
/// (all arguments are UNREFERENCED in the source): AddMP4SourceFilter,
/// BuildMP4FilterGraph and BuildMP4PlayBack return E_NOTIMPL and
/// IsMP4FilePlayable returns FALSE. A missing temp path is used so the
/// helpers can never touch a real file.
/// </summary>
public static class Mp4ParserWorkflow
{
    private const uint E_NOTIMPL = 0x80004001;
    private const int FALSE = 0;

    public static IEnumerable<TestCase> All()
    {
        string missing = Path.Combine(Path.GetTempPath(), $"mmr-cli-missing-{Guid.NewGuid():N}.mp4");

        yield return T.Test("WLXMP4Parser.dll", "workflow", "mp4:wf:add-source-filter",
            "AddMP4SourceFilter(missing path, NULL, out _) -> E_NOTIMPL",
            ctx => (uint)DllApi.AddMP4SourceFilter(missing, IntPtr.Zero, out _) == E_NOTIMPL);

        yield return T.Test("WLXMP4Parser.dll", "workflow", "mp4:wf:build-graph",
            "BuildMP4FilterGraph(missing path, out _) -> E_NOTIMPL",
            ctx => (uint)DllApi.BuildMP4FilterGraph(missing, out _) == E_NOTIMPL);

        yield return T.Test("WLXMP4Parser.dll", "workflow", "mp4:wf:build-playback",
            "BuildMP4PlayBack(missing path, NULL) -> E_NOTIMPL",
            ctx => (uint)DllApi.BuildMP4PlayBack(missing, IntPtr.Zero) == E_NOTIMPL);

        yield return T.Test("WLXMP4Parser.dll", "workflow", "mp4:wf:playable",
            "IsMP4FilePlayable(missing path) == FALSE",
            ctx => DllApi.IsMP4FilePlayable(missing) == FALSE);
    }
}
