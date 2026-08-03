using MmrCli.Framework;
using MmrCli.Native;

namespace MmrCli.Tests;

/// <summary>
/// One test case per expected export, driven by the dumpbin-verified
/// ExportCatalog. Each checks the export resolves via GetProcAddress on the
/// freshly loaded DLL (contract/presence layer).
/// </summary>
public static class ExportPresenceTests
{
    public static IEnumerable<TestCase> All()
    {
        foreach (var (dll, exports) in ExportCatalog.Catalogs)
        {
            if (TestSkips.UnloadableDlls.ContainsKey(dll))
                continue;

            string dllName = dll;
            foreach (var name in exports)
            {
                string export = name;
                yield return T.Test(
                    dll: dllName,
                    group: "exports",
                    id: $"presence:{dllName}:{export}",
                    desc: $"export resolves: {export}",
                    body: ctx =>
                    {
                        IntPtr h = ctx.LoadModule(dllName);
                        return ctx.Modules.HasProc(h, export);
                    });
            }
        }
    }
}
