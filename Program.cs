using System.Diagnostics;
using MmrCli.Framework;
using MmrCli.Native;
using MmrCli.Reporting;
using MmrCli.Tests;
using MmrCli.Workflows;

namespace MmrCli;

internal static class Program
{
    private sealed class Options
    {
        public string? BinDir;
        public bool List;
        public string? Filter;
        public string? Skip;
        public string? Dll;
        public string? JUnit;
        public string? Json;
        public int? Retries;
    }

    private static int Main(string[] args)
    {
        Options opts = Parse(args);

        string binDir = opts.BinDir
            ?? Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "bin"));

        var all = BuildSuite();

        if (opts.List)
        {
            ListTests(all);
            return (int)ExitCode.Success;
        }

        if (!Directory.Exists(binDir))
        {
            Console.WriteLine($"[!] DLL directory not found: {binDir}");
            Console.WriteLine("    Pass the path to build_clean/bin/Debug as the first argument.");
            return (int)ExitCode.EnvironmentError;
        }

        var tests = ApplyOptions(all, opts);

        NativeBootstrap.SetDllSearchPath(binDir);

        var coverage = new CoverageTracker();
        var ctx = new TestContext
        {
            BinDir = binDir,
            Coverage = coverage,
            Modules = new NativeModuleCache(),
        };

        using (ctx.Modules)
        {
            Console.WriteLine("=== WMMR Live Usage Tests (mmr-cli) ===");
            Console.WriteLine($"bin: {binDir}");
            Console.WriteLine($"tests: {tests.Count}  (dll filter: {opts.Dll ?? "*"} | filter: {opts.Filter ?? "*"} | skip: {opts.Skip ?? "-"})");
            Console.WriteLine();

            var sw = Stopwatch.StartNew();
            IReadOnlyList<TestResult> results = new TestRunner().Run(ctx, tests);
            sw.Stop();

            foreach (var r in results)
                PrintResult(r);

            CollectCoverage(ctx, coverage);

            Console.WriteLine();
            PrintSummary(results, coverage, sw.Elapsed);

            if (opts.JUnit is not null)
            {
                Reporters.WriteJUnit(opts.JUnit, results);
                Console.WriteLine($"junit: {Path.GetFullPath(opts.JUnit)}");
            }

            if (opts.Json is not null)
            {
                Reporters.WriteJson(opts.Json, results);
                Console.WriteLine($"json:  {Path.GetFullPath(opts.Json)}");
            }

            return results.Any(r => r.Status == TestStatus.Failed)
                ? (int)ExitCode.TestFailure
                : (int)ExitCode.Success;
        }
    }

    private static Options Parse(string[] args)
    {
        var o = new Options();
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--list": o.List = true; break;
                case "--filter": o.Filter = args[++i]; break;
                case "--skip": o.Skip = args[++i]; break;
                case "--dll": o.Dll = args[++i]; break;
                case "--junit": o.JUnit = args[++i]; break;
                case "--json": o.Json = args[++i]; break;
                case "--retries": o.Retries = int.Parse(args[++i]); break;
                default:
                    if (o.BinDir is null && !args[i].StartsWith("--", StringComparison.Ordinal))
                        o.BinDir = args[i];
                    break;
            }
        }
        return o;
    }

    private static List<TestCase> BuildSuite()
    {
        var all = new List<TestCase>();
        all.AddRange(ExportPresenceTests.All());
        all.AddRange(ComSurfaceTests.All());
        all.AddRange(BehaviorTests.All());
        all.AddRange(WlidcliWorkflow.All());
        all.AddRange(MovieMakerCoreWorkflow.All());
        all.AddRange(VideoTrimWorkflow.All());
        all.AddRange(PublishSubscribeWorkflow.All());
        all.AddRange(Mp4ParserWorkflow.All());
        all.AddRange(MetadataSysWorkflow.All());
        all.AddRange(MfReadWriteWorkflow.All());
        return all;
    }

    private static List<TestCase> ApplyOptions(List<TestCase> all, Options o)
    {
        IEnumerable<TestCase> q = all;

        if (o.Dll is not null)
            q = q.Where(t => t.Dll.Equals(o.Dll, StringComparison.OrdinalIgnoreCase));

        if (o.Filter is not null)
        {
            string f = o.Filter;
            q = q.Where(t => t.Id.Contains(f, StringComparison.OrdinalIgnoreCase)
                             || t.Dll.Contains(f, StringComparison.OrdinalIgnoreCase)
                             || t.Group.Contains(f, StringComparison.OrdinalIgnoreCase)
                             || t.Description.Contains(f, StringComparison.OrdinalIgnoreCase));
        }

        var list = q.ToList();

        if (o.Retries is not null)
            list = list.Select(t => WithRetries(t, o.Retries.Value)).ToList();

        if (o.Skip is not null)
        {
            string s = o.Skip;
            list = list.Select(t => t.Id.Contains(s, StringComparison.OrdinalIgnoreCase) ? Skipped(t) : t).ToList();
        }

        return list;
    }

    private static TestCase WithRetries(TestCase t, int retries)
        => T.Test(t.Dll, t.Group, t.Id, t.Description, t.Body, retries);

    private static TestCase Skipped(TestCase t)
        => T.Test(t.Dll, t.Group, t.Id, t.Description, t.Body, 0, skip: true);

    private static void ListTests(List<TestCase> all)
    {
        Console.WriteLine($"=== mmr-cli test suite: {all.Count} cases ===");
        Console.WriteLine();
        foreach (var g in all.GroupBy(t => t.Dll).OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase))
        {
            Console.WriteLine($"[{g.Key}]");
            foreach (var t in g)
                Console.WriteLine($"  {t.Id,-52} {t.Description}");
            Console.WriteLine();
        }
    }

    private static void PrintResult(TestResult r)
    {
        string mark = r.Status switch
        {
            TestStatus.Passed => "PASS",
            TestStatus.Skipped => "SKIP",
            _ => "FAIL",
        };
        string msg = r.Status == TestStatus.Failed ? $" — {r.Message}" : "";
        Console.WriteLine($"  [{mark}] {r.Id} ({r.DurationMs}ms){msg}");
    }

    private static void CollectCoverage(TestContext ctx, CoverageTracker coverage)
    {
        foreach (var (dll, exports) in ExportCatalog.Catalogs)
        {
            coverage.RegisterExpected(dll, exports.Length);
            try
            {
                IntPtr h = ctx.LoadModule(dll);
                int present = exports.Count(e => ctx.Modules.HasProc(h, e));
                coverage.ReportPresent(dll, present);
            }
            catch
            {
                coverage.ReportPresent(dll, 0);
            }
        }
    }

    private static void PrintSummary(IReadOnlyList<TestResult> results, CoverageTracker coverage, TimeSpan elapsed)
    {
        int pass = results.Count(r => r.Status == TestStatus.Passed);
        int fail = results.Count(r => r.Status == TestStatus.Failed);
        int skip = results.Count(r => r.Status == TestStatus.Skipped);
        Console.WriteLine($"=== PASS={pass} FAIL={fail} SKIP={skip}  ({elapsed.TotalSeconds:0.00}s) ===");

        int dllsLoaded = coverage.Data.Count(kv => kv.Value.Present > 0);
        Console.WriteLine($"=== coverage: {dllsLoaded}/{coverage.Data.Count} DLLs loaded, " +
                          $"{coverage.TotalPresent}/{coverage.TotalExpected} exports present ===");
        foreach (var kv in coverage.Data.OrderBy(kv => kv.Key, StringComparer.OrdinalIgnoreCase))
            Console.WriteLine($"    {kv.Key,-35} {kv.Value.Present,3}/{kv.Value.Expected,-3}");

        if (fail > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Failed cases:");
            foreach (var r in results.Where(r => r.Status == TestStatus.Failed))
                Console.WriteLine($"  [FAIL] {r.Id} — {r.Message}");
        }

        var unloadable = TestSkips.UnloadableDlls.Where(kv => !coverage.Data.TryGetValue(kv.Key, out var c) || c.Present == 0).ToList();
        if (unloadable.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Skipped (DLL not loadable):");
            foreach (var (dll, reason) in unloadable)
                Console.WriteLine($"  [!] {dll} — {reason}");
        }
    }
}
