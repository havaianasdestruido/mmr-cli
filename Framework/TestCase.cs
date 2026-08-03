namespace MmrCli.Framework;

public sealed class TestCase
{
    public required string Id { get; init; }
    public required string Dll { get; init; }
    public required string Group { get; init; }
    public required string Description { get; init; }
    public int Retries { get; init; }
    public bool Skip { get; init; }
    public required Func<TestContext, bool> Body { get; init; }
}

public static class T
{
    public static TestCase Test(string dll, string group, string id, string desc, Func<TestContext, bool> body, int retries = 0, bool skip = false)
        => new()
        {
            Id = id,
            Dll = dll,
            Group = group,
            Description = desc,
            Body = body,
            Retries = retries,
            Skip = skip,
        };
}
