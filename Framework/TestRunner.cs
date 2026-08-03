using System.Diagnostics;

namespace MmrCli.Framework;

public sealed class TestRunner
{
    public IReadOnlyList<TestResult> Run(TestContext ctx, IEnumerable<TestCase> tests)
    {
        var results = new List<TestResult>();
        foreach (var test in tests)
            results.Add(RunOne(ctx, test));
        return results;
    }

    private static TestResult RunOne(TestContext ctx, TestCase test)
    {
        if (test.Skip)
        {
            return new TestResult
            {
                Id = test.Id,
                Dll = test.Dll,
                Group = test.Group,
                Description = test.Description,
                Status = TestStatus.Skipped,
                DurationMs = 0,
            };
        }

        int attempts = Math.Max(1, test.Retries + 1);
        string? lastMessage = null;
        long totalMs = 0;

        for (int i = 0; i < attempts; i++)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                bool ok = test.Body(ctx);
                sw.Stop();
                totalMs += sw.ElapsedMilliseconds;
                return new TestResult
                {
                    Id = test.Id,
                    Dll = test.Dll,
                    Group = test.Group,
                    Description = test.Description,
                    Status = TestStatus.Passed,
                    DurationMs = totalMs,
                };
            }
            catch (Exception ex)
            {
                sw.Stop();
                totalMs += sw.ElapsedMilliseconds;
                lastMessage = $"{ex.GetType().Name}: {ex.Message}";
                if (i < attempts - 1)
                {
                    Thread.Sleep(50);
                    continue;
                }
                return new TestResult
                {
                    Id = test.Id,
                    Dll = test.Dll,
                    Group = test.Group,
                    Description = test.Description,
                    Status = TestStatus.Failed,
                    DurationMs = totalMs,
                    Message = lastMessage,
                };
            }
        }

        return new TestResult
        {
            Id = test.Id,
            Dll = test.Dll,
            Group = test.Group,
            Description = test.Description,
            Status = TestStatus.Failed,
            DurationMs = totalMs,
            Message = lastMessage,
        };
    }
}
