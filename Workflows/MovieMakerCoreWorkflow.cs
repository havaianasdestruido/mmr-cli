using MmrCli.Framework;
using MmrCli.Native;

namespace MmrCli.Workflows;

/// <summary>
/// MovieMakerCore entry-point matrix. Only arguments verified to return
/// quickly (no UI, no hang) are exercised: the three help forms return 0,
/// argc==0 and argv==null return 1 (INIT_FAILED). The --import/--export/
/// --bogus paths were probed to hang (app message loop) and are excluded.
/// </summary>
public static class MovieMakerCoreWorkflow
{
    public static IEnumerable<TestCase> All()
    {
        yield return T.Test("MovieMakerCore.dll", "workflow", "moviemaker:wf:help-dash",
            "MovieMakerMain(2, [MovieMaker.exe, --help]) returns 0",
            _ => DllApi.MovieMakerMain(2, new[] { "MovieMaker.exe", "--help" }) == 0);

        yield return T.Test("MovieMakerCore.dll", "workflow", "moviemaker:wf:help-slash",
            "MovieMakerMain(2, [MovieMaker.exe, /?]) returns 0",
            _ => DllApi.MovieMakerMain(2, new[] { "MovieMaker.exe", "/?" }) == 0);

        yield return T.Test("MovieMakerCore.dll", "workflow", "moviemaker:wf:help-h",
            "MovieMakerMain(2, [MovieMaker.exe, -h]) returns 0",
            _ => DllApi.MovieMakerMain(2, new[] { "MovieMaker.exe", "-h" }) == 0);

        yield return T.Test("MovieMakerCore.dll", "workflow", "moviemaker:wf:argc-zero",
            "MovieMakerMain(0, []) returns 1 (INIT_FAILED)",
            _ => DllApi.MovieMakerMain(0, Array.Empty<string>()) == 1);

        yield return T.Test("MovieMakerCore.dll", "workflow", "moviemaker:wf:argv-null",
            "MovieMakerMain(0, null) returns 1 (INIT_FAILED)",
            _ => DllApi.MovieMakerMain(0, null) == 1);
    }
}
