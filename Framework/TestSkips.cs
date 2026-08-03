namespace MmrCli.Framework;

/// <summary>
/// DLLs that cannot be loaded at all in the current build. Tests that require
/// the DLL to load (presence, COM surface) are skipped and a warning is
/// surfaced so the suite stays actionable without failing on a pre-existing
/// build defect.
/// </summary>
public static class TestSkips
{
    public static readonly IReadOnlyDictionary<string, string> UnloadableDlls =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["WLMFDS.dll"] =
                "LoadLibrary fails with ERROR_DLL_INIT_FAILED (DllMain runs CoInitializeEx/MFStartup under the loader lock); native LoadLibrary fails identically",
        };
}
